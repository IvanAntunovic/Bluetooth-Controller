using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Forms;
using InTheHand;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using System.IO;
using System.Net.Sockets;

namespace BluetoothApplication
{
    public partial class BluetoothForm : Form
    {
        BluetoothClient         bluetoothClient;
        BluetoothListener       bluetoothListener;
        BluetoothTransmitter    bluetoothTransmitter;
        BluetoothReceiver       bluetoothReceiver;
        BluetoothDeviceScanner  bluetoothDeviceScanner;
        List<BluetoothDevice>   discoveredBluetoothDevices;
        BluetoothDevice         selectedBluetoothDevice;
        
        readonly Guid mUUID = new Guid("00001101-0000-1000-8000-00805F9B34FB");
        bool serverStarted;
        string myPin;

        public BluetoothForm()
        {
            this.bluetoothClient            = null;
            this.bluetoothListener          = null;
            this.bluetoothTransmitter       = null;
            this.bluetoothReceiver          = null;
            this.bluetoothDeviceScanner     = new BluetoothDeviceScanner();

            this.serverStarted              = false;
            this.selectedBluetoothDevice    = null;
            this.discoveredBluetoothDevices = new List<BluetoothDevice>();
            this.myPin                      = "1234";
            InitializeComponent();
        }

        private void bGo_Click(object sender, EventArgs e)
        {
            if (serverStarted)
            {
                this.updateUI("Server already started!");
                return;
            }
            if (rbClient.Checked)
            {
                this.startDeviceScan();
            }
            else
            {
                this.connectAsServer();
            }
        }

        private void startDeviceScan()
        {
            Thread bluetoothScanThread;

            try
            {
                availableDevicesListBox.DataSource = null;
                availableDevicesListBox.Items.Clear();
                this.discoveredBluetoothDevices.Clear();

                updateUI("Starting Scan...");
                bluetoothScanThread = new Thread(new ThreadStart(this.scan));
                bluetoothScanThread.Start();
            }
            catch (Exception ex)
            {
                updateUI("Error while scanning for devices " + ex);
            }   
        }

        private void scan()
        {
            BluetoothDeviceInfo[] devices;
            BluetoothClient client;

            client = new BluetoothClient();
            devices = client.DiscoverDevicesInRange();
            foreach (BluetoothDeviceInfo device in devices)
            {
                this.discoveredBluetoothDevices.Add(new BluetoothDevice(device));
            }

            updateUI("Scan has completed");
            updateUI(this.discoveredBluetoothDevices.Count.ToString() + " devices discovered");
            updateAvailableDeviceList(this.discoveredBluetoothDevices);
        }

        private void connectAsServer()
        {
            Thread bluetoothServerThread;

            bluetoothServerThread = new Thread(new ThreadStart(ServerConnectThread));
            bluetoothServerThread.Start();
        }

        public void ServerConnectThread()
        {
            BluetoothClient bluetoothClient;
            string receivedMessage; 

            serverStarted = true;

            updateUI("Server started, waiting for clients");
            this.bluetoothListener = new BluetoothListener(mUUID);
            this.bluetoothListener.Start();
            bluetoothClient = this.bluetoothListener.AcceptBluetoothClient();

            updateUI("Client has connected");
            this.bluetoothReceiver = new BluetoothReceiver(this.bluetoothClient.GetStream());
            this.bluetoothTransmitter = new BluetoothTransmitter(this.bluetoothClient.GetStream());

            while (true)
            {
                try
                {
                    //handle server connection
                    if (bluetoothReceiver.DataAvailable)
                    {
                        receivedMessage = bluetoothReceiver.readString();
                        updateUI("Received: " + receivedMessage);
                        this.bluetoothTransmitter.writeString(receivedMessage);
                    }
                }
                catch (IOException exception)
                {
                    updateUI("Client has disconnected. " + exception);
                    break;
                }
                finally
                {
                    this.bluetoothListener.Stop();
                    this.bluetoothClient.GetStream().Close();
                    this.bluetoothReceiver.close();
                    this.bluetoothTransmitter.close();

                    this.bluetoothClient = null;
                    this.bluetoothListener = null;

                    serverStarted = false;
                }
            }
        }

        private void updateUI(string message)
        {
            Func<int> del = delegate ()
            {
                communicationMonitorTextBox.AppendText(string.Format("{0}\n", message));
                return 0;
            };
            try
            {
                Invoke(del);
            }
            catch(Exception ex)
            {
                throw (ex);
            }
        }

        private void ClientConnectThread()
        {
            BluetoothClient client = new BluetoothClient();
            updateUI("attempting to connect to the device");
            client.BeginConnect(this.selectedBluetoothDevice.DeviceInfo.DeviceAddress, BluetoothService.SerialPort, new AsyncCallback(BluetoothClientConnectCallback), client);
        }

        byte[] message = null;
        void BluetoothClientConnectCallback(IAsyncResult result)
        {
            NetworkStream bluetoothStream;
            
            try
            {
                this.bluetoothClient = result.AsyncState as BluetoothClient;
                this.bluetoothClient.EndConnect(result);
                updateUI("Connected");

                discoveredBluetoothDevices = discoveredBluetoothDevices.Where(device => device.DeviceName != selectedBluetoothDevice.DeviceName).ToList();
                updateAvailableDeviceList(this.discoveredBluetoothDevices);
                updateConnectedDeviceList(this.selectedBluetoothDevice);

                
                bluetoothStream = bluetoothClient.GetStream();
                bluetoothStream.ReadTimeout = 1000;
                
                while (true)
                {
                    if (this.message != null)
                    {
                        bluetoothStream.Write(message, 0, message.Length);
                        this.message = null;
                    }
                }
            }
            catch (Exception ex)
            {
                updateUI(ex.ToString());
                this.bluetoothClient.GetStream().Close();

                this.bluetoothClient = null;
            }
        }

        public eDevicePair pairDevice()
        {
            if (!this.selectedBluetoothDevice.IsAuthenticated)
            {
                return eDevicePair.DEVICE_ALREADY_PAIRED;
            }
       
            if (!BluetoothSecurity.PairRequest(this.selectedBluetoothDevice.DeviceInfo.DeviceAddress, this.myPin))
            {
                return eDevicePair.PAIR_NOK;
            }

            return eDevicePair.PAIR_OK;
        }

        private void tbText_KeyPress(object sender, System.Windows.Forms.KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Enter)
            {
                this.message = Encoding.ASCII.GetBytes(tbText.Text);
                tbText.Clear();
            }
        }

        private void sendButton_Click(object sender, EventArgs e)
        {
            this.message = Encoding.ASCII.GetBytes(tbText.Text);
            tbText.Clear();
        }


        private void availableDevicesListBox_DoubleClick(object sender, EventArgs e)
        {
            BluetoothDeviceInfo selectedDevice;

            try
            {
                // Need to implement error hadnling for clicking on the already connected device
                selectedDevice = this.discoveredBluetoothDevices[availableDevicesListBox.SelectedIndex].DeviceInfo;
                this.selectedBluetoothDevice = new BluetoothDevice(selectedDevice);
                updateUI(this.selectedBluetoothDevice.DeviceName + " was selected, attempting to connect");
            }
            catch (NullReferenceException ex)
            {
                updateUI("Device selection error" + ex);
            }
            catch (ArgumentOutOfRangeException ex)
            {
                updateUI("Device selection error" + ex);
            }

            eDevicePair retValueDevicePair;

            retValueDevicePair = pairDevice();
            if (retValueDevicePair == eDevicePair.PAIR_OK)
            {
                updateUI("device paired..");
                updateUI("starting connect thread");
                Thread bluetoothClientThread = new Thread(new ThreadStart(ClientConnectThread));
                bluetoothClientThread.Start();
            }
            else if (retValueDevicePair == eDevicePair.DEVICE_ALREADY_PAIRED)
            {
                updateUI("Device already paired");
            }
            else
            {
                updateUI("Pair failed");
            }
        }

        private void updateAvailableDeviceList(List<BluetoothDevice> discoveredBluetoothDevices)
        {
            Func<int> del = delegate ()
            {
                List<string> deviceNames = discoveredBluetoothDevices.Select(o => o.DeviceName).ToList();
                availableDevicesListBox.DataSource = deviceNames;
                return 0;
            };
            Invoke(del);
        }

        private void updateConnectedDeviceList(BluetoothDevice connectedBluetoothDevice)
        {
            Func<int> del = delegate ()
            {
                connectedBluetoothDevices.Items.Add(connectedBluetoothDevice.DeviceName);
                return 0;
            };
            Invoke(del);
        }
    }
}
