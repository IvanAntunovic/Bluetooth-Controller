using InTheHand.Net.Sockets;
using System;
using System.Collections.Generic;
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
    class BluetoothDeviceScanner
    {
        List<BluetoothDevice> discoveredBluetoothDevices;

        public BluetoothDeviceScanner()
        {
            this.discoveredBluetoothDevices = new List<BluetoothDevice>();
        }

        public void start()
        {
            Thread bluetoothScanThread;

            this.discoveredBluetoothDevices.Clear();
            bluetoothScanThread = new Thread(new ThreadStart(this.scan));
            bluetoothScanThread.Start();
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
        }

        public void stop()
        {
            discoveredBluetoothDevices.Clear();
        }

        public List<BluetoothDevice> getDiscoveredDevices()
        {
            return this.discoveredBluetoothDevices;
        }
    }
}
