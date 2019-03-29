using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using IoT.Common;
using Microsoft.Azure.Devices.Client;
using Microsoft.Azure.Devices.Shared;
using Newtonsoft.Json;

namespace IoT.BandAgent
{
    class Program
    {
        private const string DeviceConnectionString =
            "HostName=XXX;DeviceId=XX;SharedAccessKey=XX";

        private static async Task UpdateTwin(DeviceClient device)
        {
            var twinProperties = new TwinCollection();
            twinProperties["connectionType"] = "wf";
            twinProperties["connectionStrength"] = "w";

            await device.UpdateReportedPropertiesAsync(twinProperties);
        }

        static async Task Main(string[] args)
        {
            Console.WriteLine("Initializing Device...");

            var device = DeviceClient.CreateFromConnectionString(DeviceConnectionString);

            await device.OpenAsync();

            Console.WriteLine("Device is connected!");

            await UpdateTwin(device);

            Console.WriteLine("Press a key to perform an action:");
            Console.WriteLine("q: quits");
            Console.WriteLine("h: send weather is too hot inside the car");
            Console.WriteLine("u: send weather is too cold inside the car");
            Console.WriteLine("e: request emergency help");

            var random = new Random();
            var quitRequested = false;
            while (!quitRequested)
            {
                Console.Write("Action? ");
                var input = Console.ReadKey().KeyChar;
                Console.WriteLine();

                var status = StatusType.NotSpecified;
                var latitude = random.Next(0, 100);
                var longitude = random.Next(0, 100);
                var humidity = random.Next(0, 100);

                switch (Char.ToLower(input))
                {
                    case 'q':
                        quitRequested = true;
                        break;
                    case 'h':
                        status = StatusType.Hot;
                        break;
                    case 'c':
                        status = StatusType.Cold;
                        break;
                    case 'e':
                        status = StatusType.Emergency;
                        break;
                }

                var telemetry = new Telemetry
                {
                    Latitude = latitude,
                    Longitude = longitude,
                    Humidity = humidity,
                    Status = status
                };

                var payload = JsonConvert.SerializeObject(telemetry);

                var message = new Message(Encoding.ASCII.GetBytes(payload));

                await device.SendEventAsync(message);

                Console.WriteLine("Message sent!");
            }

            Console.WriteLine("Disconnecting...");

        }

    }
}
