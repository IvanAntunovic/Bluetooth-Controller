using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InTheHand;
using InTheHand.Net.Bluetooth;
using InTheHand.Net.Ports;
using InTheHand.Net.Sockets;

namespace BluetoothApplication
{
    class BluetoothDevice
    {
        public string DeviceName { get; private set; }
        public bool IsAuthenticated { get; set; }
        public bool IsConnected { get; set; }
        public ushort Nap { get; set; }
        public uint Sap { get; set; }
        public DateTime LastSeen { get; set; }
        public DateTime LastUsed { get; set; }
        public bool Remembered { get; set; }
        public BluetoothDeviceInfo DeviceInfo { get; set; }
        public override string ToString() { return DeviceName; }

        public BluetoothDevice(BluetoothDeviceInfo deviceInfo)
        {
            if (deviceInfo != null)
            {
                DeviceInfo = deviceInfo;
                IsAuthenticated = deviceInfo.Authenticated;
                IsConnected = deviceInfo.Connected;
                DeviceName = deviceInfo.DeviceName;
                LastSeen = deviceInfo.LastSeen;
                LastUsed = deviceInfo.LastUsed;
                Nap = deviceInfo.DeviceAddress.Nap;
                Sap = deviceInfo.DeviceAddress.Sap;
                Remembered = deviceInfo.Remembered;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj == null)
            {
                return false;
            }

            if (this.GetType() != obj.GetType())
            {
                return false;
            }

            BluetoothDevice bluetoothDevice = (BluetoothDevice)obj;

            if (this.DeviceName != bluetoothDevice.DeviceName || 
                this.DeviceInfo.DeviceAddress != bluetoothDevice.DeviceInfo.DeviceAddress ||
                !this.DeviceInfo.ClassOfDevice.Equals(bluetoothDevice.DeviceInfo.ClassOfDevice)
                )
            {
                return false;
            }

            return true;
        }
    }
}