using System;
using System.Threading;
using nanoFramework.Device.Bluetooth;
using nanoFramework.Device.Bluetooth.Advertisement;
using nanoFramework.Device.Bluetooth.GenericAttributeProfile;

namespace LegoBluetoothHub
{
    public class Program
    {
        // LEGO Hub Service UUID
        private static readonly Guid LegoHubServiceUuid = new Guid("00001623-1212-EFDE-1623-785FEABCD123");

        // LEGO Hub Characteristic UUID
        private static readonly Guid LegoHubCharacteristicUuid = new Guid("00001624-1212-EFDE-1623-785FEABCD123");

        public static void Main()
        {
            // BluetoothLEServer is a singleton object so gets its instance. The Object is created when you first access it
            // and can be disposed to free up memory.
            BluetoothLEServer server = BluetoothLEServer.Instance;

            // Give device a name
            server.DeviceName = "LegoESP32";

            //The GattServiceProvider is used to create and advertise the primary service definition.
            //An extra device information service will be automatically created.
            GattServiceProviderResult result = GattServiceProvider.Create(LegoHubServiceUuid);

            // Create the GATT characteristic
            var legoHubCharacteristic = new GattLocalCharacteristicParameters
            {
                CharacteristicProperties = GattCharacteristicProperties.Read | GattCharacteristicProperties.WriteWithoutResponse | GattCharacteristicProperties.Write | GattCharacteristicProperties.Notify,
                WriteProtectionLevel = GattProtectionLevel.Plain,
                UserDescription = "LEGO Hub Characteristic"
            };


            GattServiceProvider serviceProvider = result.ServiceProvider;

            GattLocalService service = serviceProvider.Service;
            var legoHubService = serviceProvider.Service;

            // Add the characteristic to the service
            var charres = legoHubService.CreateCharacteristic(LegoHubCharacteristicUuid, legoHubCharacteristic);
            var legoCharacteristic = charres.Characteristic;
            legoCharacteristic.ReadRequested += (o, e) =>
            {
                Console.WriteLine("Received message from LEGO Hub");
                Console.WriteLine($"Data: {e.Session.DeviceId.Id}");
            };

            // Once all the Characteristics have been created you need to advertise the Service so 
            // other devices can see it. Here we also say the device can be connected too and other
            // devices can see it. 
            serviceProvider.StartAdvertising(new GattServiceProviderAdvertisingParameters()
            {
                IsConnectable = true,
                IsDiscoverable = true
            });

            Console.WriteLine("Advertising LEGO Hub...");

            // Keep the application running
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
