using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BluetoothController
{
    class BluetoothReceiver
    {
        Stream clientStream;

        public BluetoothReceiver(Stream clientStream)
        {
            this.clientStream = clientStream;
        }

        public bool canRead()
        {
            return this.clientStream.CanRead;
        }

        public void readBytes(byte[] buffer, int offset, int count)
        {
            try
            {
                this.clientStream.Read(buffer, offset, count);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Client has disconnected!!!!");
            }
        }

        public String readString()
        {
            byte[] rxBuffer = new byte[1024];

            try
            {
                this.clientStream.Read(rxBuffer, 0, 1024);
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Client has disconnected!!!!");
            }

            return Encoding.ASCII.GetString(rxBuffer);
        }
    }
}
