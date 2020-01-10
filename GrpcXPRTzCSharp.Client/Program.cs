using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GrpcXPRTzCSharp.Client
{
    class Program
    {
        const int Port = 9000;

        static async Task Main(string[] args)
        {
            // The port number(5001) must match the port of the gRPC server.

            //var channel = GrpcChannel.ForAddress("https://localhost:5001");

            //var client = new Greeter.GreeterClient(channel);

            //var reply = await client.SayHelloAsync(

            //                  new HelloRequest { Name = "Stefan" });

            //Console.WriteLine("Greeting: " + reply.Message);

            //Console.WriteLine("Press any key to exit...");

            //Console.ReadKey();

            //var option = int.Parse(args[0]);

            //var cacert = File.ReadAllText(@"ca.crt");
            //var cert = File.ReadAllText(@"client.crt");
            //var key = File.ReadAllText(@"client.key");
            //var keypair = new KeyCertificatePair(cert, key);
            //SslCredentials creds = new SslCredentials(cacert, keypair);

            var channel = GrpcChannel.ForAddress("https://localhost:9000");
            var client = new XprtService.XprtServiceClient(channel);

            
            while (true)
            {
                Console.Clear();
                Console.WriteLine("Kies een actie, gevolgd door enter:");
                Console.WriteLine("1: SendMetadataAsync");
                Console.WriteLine("2: GetByBadgeNumber");
                Console.WriteLine("3: GetAll");
                Console.WriteLine("4: AddPhoto");
                Console.WriteLine("5: SaveAll");
                Console.WriteLine("6: EXIT");

                var input = Console.ReadLine();
                try
                {
                    int option = int.Parse(input);

                    switch (option)
                    {
                        case 1:
                            SendMetadataAsync(client).Wait();
                            break;
                        case 2:
                            GetByBadgeNumber(client).Wait();
                            break;
                        case 3:
                            GetAll(client).Wait();
                            break;
                        case 4:
                            AddPhoto(client).Wait();
                            break;
                        case 5:
                            SaveAll(client).Wait();
                            break;
                        case 6:
                            
                            return;
                        default:
                            Console.WriteLine($"Onbekende optie: {option}");
                            break;
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }

                Console.WriteLine("");
                Console.WriteLine("Geef enter om door te gaan.");
                Console.ReadKey();
            }
        }

        public static async Task SendMetadataAsync(XprtService.XprtServiceClient client)
        {
            Metadata md = new Metadata();
            md.Add("username", "svantilborg");
            md.Add("password", "password1");
            try
            {
                await client.GetByBadgeNumberAsync(new Messages.GetByBadgeNumberRequest(), md);
            }
            catch (Exception e)
            {
                // Just swallow the expected exception
            }

        }
        public static async Task GetByBadgeNumber(XprtService.XprtServiceClient client)
        {
            var res = await client.GetByBadgeNumberAsync(new Messages.GetByBadgeNumberRequest() { BadgeNumber = 2080 });
            Console.WriteLine(res.Xprt);
        }

        public static async Task GetAll(XprtService.XprtServiceClient client)
        {
            using (var call = client.GetAll(new Messages.GetAllRequest()))
            {
                var responseStream = call.ResponseStream;
                while (await responseStream.MoveNext())
                {
                    Console.WriteLine(responseStream.Current.Xprt);
                }
            }
        }

        public static async Task AddPhoto(XprtService.XprtServiceClient client)
        {
            Metadata md = new Metadata();
            md.Add("badgenumber", "2080");

            FileStream fs = File.OpenRead("Penguins.jpg");
            using (var call = client.AddPhoto())
            {
                var stream = call.RequestStream;
                while (true)
                {
                    byte[] buffer = new byte[64 * 1024];
                    int numRead = await fs.ReadAsync(buffer, 0, buffer.Length);
                    if (numRead == 0)
                    {
                        break;
                    }
                    if (numRead < buffer.Length)
                    {
                        Array.Resize(ref buffer, numRead);
                    }

                    await stream.WriteAsync(new Messages.AddPhotoRequest() { Data = ByteString.CopyFrom(buffer) });
                }
                await stream.CompleteAsync();

                var res = await call.ResponseAsync;

                Console.WriteLine(res.IsOk);
            }

        }

        private static async Task SaveAll(XprtService.XprtServiceClient client)
        {
            var xprtz = new List<Xprt>()
            {
                new Xprt{
                    BadgeNumber= 123,
                    FirstName= "Stefan",
                    LastName= "van Tilborg",
                    GeboorteDatum = ToUnixTimestamp(new DateTime(1981, 8, 8)),
                    Skills = { "gRPC", ".Net Core"},
                },
                new Xprt{
                    BadgeNumber= 234,
                    FirstName= "Jasper",
                    LastName= "Jak",
                    GeboorteDatum = ToUnixTimestamp(new DateTime(1960, 1, 1)),
                    Skills = { "Slap ouwehoeren", "Salaris uitbetalen"},
                }
            };
            using (var call = client.SaveAll())
            {
                var requestStream = call.RequestStream;
                var responseStream = call.ResponseStream;

                var responseTask = Task.Run(async () =>
                {
                    while (await responseStream.MoveNext())
                    {
                        Console.WriteLine("Saved: " + responseStream.Current.Xprt);
                    }
                });

                foreach (var x in xprtz)
                {
                    await requestStream.WriteAsync(new XprtRequest() { Xprt = x });
                }
                await call.RequestStream.CompleteAsync();
                await responseTask;
            }
        }

        private static Int64 ToUnixTimestamp(DateTime dateTime)
        {
            try
            {
                return (Int64)(dateTime.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            }
            catch
            {
                return 0;
            }
        }
    }
}
