using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace BluetoothController
{
    class BluetoothTransmitter
    {
        Stream clientStream;

        public BluetoothTransmitter(Stream clientStream)
        {
            this.clientStream = clientStream;
        }

        public bool canWrite()
        {
            return this.clientStream.CanWrite;
        }

        public void writeBytes(byte[] buffer, int offset, int count)
        {
            try
            {
                this.clientStream.Write(buffer, offset, count);
                this.clientStream.Flush();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Client has disconnected!!!!");
            }
        }

        public void writeString(string nString)
        {
            byte[] txBuffer = new byte[1024];
            txBuffer = Encoding.ASCII.GetBytes(nString);

            try
            {
                this.clientStream.Write(txBuffer, 0, txBuffer.Length);
                this.clientStream.Flush();
            }
            catch (IOException ex)
            {
                Console.WriteLine(ex);
                Console.WriteLine("Client has disconnected!!!!");
            }
        }
    }
}
