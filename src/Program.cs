using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluetoothController
{
    class Program
    {
        static void Main(string[] args)
        {
            BluetoothController bluetooth = new BluetoothController();
            string keyboardInput;
            int paireddeviceInfo = -1;
            string deviceInfoSelected ="";

            Console.WriteLine("Enter 'bt -commands' for all available commands.");
            Console.WriteLine("In order to connect to a specific deviceInfo, " +
                                "deviceInfos need to be paired first!");

            while (true)
            {
                Console.Write(">> ");
                keyboardInput = Console.ReadLine();
                if (keyboardInput.Equals("bt -device -discover"))
                {
                    bluetooth.discoverDevices();
                }
                else if (keyboardInput.StartsWith("bt -device -select"))
                {
                    keyboardInput = keyboardInput.TrimStart("bt -deviceInfo -select".ToCharArray());
                    if (!bluetooth.isDeviceFound(keyboardInput))
                    {
                        Console.WriteLine("<< Invalid device selected.");
                    }
                    else
                    {
                        Console.WriteLine("<< deviceInfo {0} slected.", keyboardInput);
                        deviceInfoSelected = keyboardInput;
                    }
                }
                else if (keyboardInput.Equals("bt -connect"))
                {
                    bluetooth.connect();
                }
                else if (keyboardInput.Equals("bt -pair"))
                {
                    paireddeviceInfo = bluetooth.pairDevice(deviceInfoSelected);
                }
                else if (keyboardInput.StartsWith("bt -transmit "))
                {
                    keyboardInput = keyboardInput.TrimStart("bt -input ".ToCharArray());
                    if (keyboardInput.Length > 0)
                    {
                        bluetooth.writeString(keyboardInput);
                        Console.WriteLine("Text has been sent.");
                    }
                    else
                    { 
                        Console.WriteLine("No text to be sent!");
                    }
                }
                else if (keyboardInput.Equals("bt -commands"))
                {
                    Console.WriteLine("<< {0, -3}", "Available commands:" );
                    Console.WriteLine("bt -device -discover");
                    Console.WriteLine("bt -device -select [deviceInfo Name]");
                    Console.WriteLine("bt -pair");
                    Console.WriteLine("bt -connect");
                    Console.WriteLine("bt -transmit [Text]");
                }
                else
                {
                    Console.WriteLine("Invalid Command");
                } 
            }
        }

 
    }
}
