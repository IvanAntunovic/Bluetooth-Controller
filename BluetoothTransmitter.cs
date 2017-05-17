using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothApplication
{
    class BluetoothTransmitter
    {
        NetworkStream bluetoothStream;

        public BluetoothTransmitter(NetworkStream bluetoothStream)
        {
            this.bluetoothStream = bluetoothStream;
            CanWrite = this.bluetoothStream.CanWrite;
        }

        public byte readByte()
        {
            return (byte)this.bluetoothStream.ReadByte();
        }

        public void writeBytes(byte[] buffer, int offset, int size)
        {
            this.bluetoothStream.Write(buffer, offset, size);
            this.bluetoothStream.Flush();
        }

        public void writeString(string message)
        {
            this.bluetoothStream.Write(Encoding.ASCII.GetBytes(message), 0, message.Length);
            this.bluetoothStream.Flush();
        }

        public void close()
        {
            this.bluetoothStream.Close();
        }

        public bool CanWrite { get; set; }
    }
}
