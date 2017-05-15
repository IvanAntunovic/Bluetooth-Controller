using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;
using System.IO;
using System.Threading;
using InTheHand.Net;

namespace BluetoothController
{
    class BluetoothController
    {
        BluetoothDevice         device;
        BluetoothClient         client;
        BluetoothListener       bluetoothListener;
        BluetoothDeviceInfo[]   deviceList;
        BluetoothDeviceInfo     pairedDevice;
        BluetoothReceiver       bluetoothReceiver;
        BluetoothTransmitter    bluetoothTransmitter;
        BluetoothEndPoint bluetoothEndPoint;

        static readonly Guid MyServiceUuid
            = new Guid("{00112233-4455-6677-8899-aabbccddeeff}");

        public BluetoothController()
        {
            this.bluetoothListener      = new BluetoothListener(MyServiceUuid);
            this.client                 = new BluetoothClient();
            this.bluetoothListener      = null;
            this.pairedDevice           = null;
            this.bluetoothReceiver      = null;
            this.bluetoothTransmitter   = null;
            this.bluetoothEndPoint      = null;
            this.device                 = null;
        }

        public void discoverDevices()
        {
            Thread bluetoothScanThread = new Thread(new ThreadStart(scanDevicesInRange));
            bluetoothScanThread.Start();
        }

        private void scanDevicesInRange()
        {
            Console.WriteLine("<< Starting Device Scan...");
            this.deviceList = client.DiscoverDevicesInRange();
            Console.WriteLine("<< Scan complete");
            Console.WriteLine("<< " + this.deviceList.Length.ToString() + " devices discovered");
            this.printDiscoveredDevices();
        }

        public bool isDeviceFound(String device)
        {
            for (int i = 0; i < this.deviceList.Length; ++i)
            {
                if (device.Contains(this.deviceList[i].DeviceName.ToString()))
                {
                    return true;
                }
            }
            return false;
        }

        public void printDiscoveredDevices()
        {
            if (deviceList == null)
            {
                Console.WriteLine("<< No devices discovered.");
                return;
            }

            BluetoothAddress deviceAddress;
            string tempDeviceAddress;
            string outputText = "";
            Console.WriteLine("<< {0, -12} {1, 35}", "Device Name", "Device Address");
            for (int index = 0; index < this.deviceList.Length; ++index)
            {
                deviceAddress = this.deviceList[index].DeviceAddress;
                tempDeviceAddress = deviceAddress.ToString();
                tempDeviceAddress = String.Format(  "{0}-{1}-{2}-{3}-{4}-{5}",
                                                    tempDeviceAddress.Substring(0, 2),
                                                    tempDeviceAddress.Substring(2, 2),
                                                    tempDeviceAddress.Substring(4, 2),
                                                    tempDeviceAddress.Substring(6, 2),
                                                    tempDeviceAddress.Substring(8, 2),
                                                    tempDeviceAddress.Substring(10, 2)
                                                    );
                outputText += String.Format("{0, -12} {1, 30:N0}\n", 
                                            deviceList[index].DeviceName, tempDeviceAddress
                                            );
            }
            Console.Write(outputText);
            Console.Write(">> ");
        }

        public bool isClientConnected()
        {
            return this.client.Connected;
        }

        public int pairDevice(String selectedDevice)
        {
            const int NO_DEVICE_SELECTED = -1;
            BluetoothDeviceInfo deviceInfo;
            int deviceIndexElement = NO_DEVICE_SELECTED;

            deviceIndexElement = this.deviceIndex(selectedDevice);
            if (deviceIndexElement == NO_DEVICE_SELECTED)
            {
                Console.WriteLine("<< No device selected");
                return -1;
            }

            deviceInfo = this.deviceList.ElementAt(deviceIndexElement);
            if (this.ispairedDevice(deviceInfo))
            {
                Console.WriteLine("<< Device paired. Name: " + deviceInfo.DeviceName);
                this.device = new BluetoothDevice(deviceInfo);
                return 0;
            }
            else
            {
                Console.WriteLine("<< Pair failed.");
                return -2;
            }
        }

        private int deviceIndex(String selectedDevice)
        {
            for (int index = 0; index < this.deviceList.Length; ++index)
            {
                if (this.deviceList[index].DeviceName.Equals(selectedDevice))
                {
                    return index;
                }
            }
            return -1;
        }

        string myPin = "1234";
        private bool ispairedDevice(BluetoothDeviceInfo deviceInfo)
        {
            if (deviceInfo == null)
            {
                Console.WriteLine("<< Device not paired!");
                return false;
            }

            if (!deviceInfo.Authenticated)
            {
                if (!BluetoothSecurity.PairRequest(deviceInfo.DeviceAddress, myPin))
                {
                    return false;
                }
            }
            return true;
        }

        public void connect()
        {
            if (!this.ispairedDevice(this.pairedDevice))
            {
                Console.WriteLine("<< Devices not paired");
                return;
            }

            if (this.pairedDevice.Authenticated)
            {
                Console.WriteLine("<< Attempting to connect to a device {0}", 
                                   this.pairedDevice.DeviceName.ToString()
                                  );
                //this.client.SetPin(DEVICE_PIN);
                this.client.BeginConnect(this.pairedDevice.DeviceAddress,
                                         MyServiceUuid,
                                         new AsyncCallback(BluetoothClientConnectCallback),
                                         this.pairedDevice
                                         );
            }
        }

        void BluetoothClientConnectCallback(IAsyncResult result)
        {
            if (result.IsCompleted)
            {
                Stream tempClientStream = null;
                BluetoothClient localClient;
                try
                {
                    this.bluetoothEndPoint =  new BluetoothEndPoint(this.pairedDevice.DeviceAddress, MyServiceUuid);
                    
                    try
                    {
                        localClient = new BluetoothClient(this.bluetoothEndPoint);
                    }
                    catch 
                    {
                        throw new Exception("Error while connecting to client");
                    }
                    Console.WriteLine("<< Client has connected");
                    tempClientStream = this.client.GetStream();

                    this.bluetoothReceiver      = new BluetoothReceiver(tempClientStream);
                    this.bluetoothTransmitter   = new BluetoothTransmitter(tempClientStream);
                }
                catch(ObjectDisposedException ex)
                {
                    Console.WriteLine(ex);
                }
                catch(InvalidOperationException ex)
                {
                    Console.WriteLine(ex);
                }
                
            }
            else
            {
                Console.WriteLine("<< Client has not connected");
            }
            //Console.Write(">> ");
        }

        public void connectAsServer()
        {
            Console.WriteLine("attempting connect");
            this.bluetoothListener.Start();
            this.client = this.bluetoothListener.AcceptBluetoothClient();
            Console.WriteLine("Client has connected");

            Stream stream = this.client.GetStream();
        }

        public void writeBytes(byte[] buffer, int offset, int count)
        {
            if (this.device == null)
            {
                throw new ArgumentNullException("device");
            }

            if (Byte.ReferenceEquals(buffer, null))
            {
                throw new ArgumentNullException("buffer");
            }

            BluetoothClient bluetoothClient = new BluetoothClient();
            BluetoothEndPoint bluetoothEndPoint = new BluetoothEndPoint(this.device.DeviceInfo.DeviceAddress, MyServiceUuid);
            Stream bluetoothStream = bluetoothClient.GetStream();
            this.bluetoothTransmitter = new BluetoothTransmitter(bluetoothStream);

            if (bluetoothClient.Connected && bluetoothStream != null)
            {
                this.bluetoothTransmitter.writeBytes(buffer, offset, count);
            }

            
        }

        public void writeString(string content)
        {
            if (this.device == null)
            {
                throw new ArgumentNullException("device");
            }

            if (string.IsNullOrEmpty(content))
            {
                throw new ArgumentNullException("content");
            }

            BluetoothClient bluetoothClient = new BluetoothClient();
            BluetoothEndPoint bluetoothEndPoint = new BluetoothEndPoint(this.device.DeviceInfo.DeviceAddress, MyServiceUuid);
            Stream bluetoothStream = bluetoothClient.GetStream();
            this.bluetoothTransmitter = new BluetoothTransmitter(bluetoothStream);

            if (bluetoothClient.Connected && bluetoothStream != null)
            {
                this.bluetoothTransmitter.writeString(content);
            }
      
        }

        public void readBytes(byte[] buffer, int offset, int count)
        {
            this.bluetoothReceiver.readBytes(buffer, offset, count);
        }

        public String readString()
        {
            return this.bluetoothReceiver.readString();
        }
        
    }
}
