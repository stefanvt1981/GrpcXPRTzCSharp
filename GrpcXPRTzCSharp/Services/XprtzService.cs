using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using GrpcXPRTzCSharp.Repository;
using Messages;

namespace GrpcXPRTzCSharp.Server.Services
{
    public class XprtzService : XprtService.XprtServiceBase
    {

        private readonly IXprtzRepository _repository;

        public XprtzService(IXprtzRepository repository) : base()
        {
            _repository = repository;

            // Seed
            _repository.Seed(Data.Xprtz.XprtList);
        }

        public override async Task<XprtResponse> Save(XprtRequest request, ServerCallContext context)
        {
            var newXprt = await _repository.AddXprt(request.Xprt);

            return new XprtResponse { Xprt = newXprt };
        }

        public override async Task SaveAll(IAsyncStreamReader<XprtRequest> requestStream, IServerStreamWriter<XprtResponse> responseStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                var xprt = requestStream.Current.Xprt;
                
                var newXprt = await _repository.AddXprt(xprt);                

                await responseStream.WriteAsync(new XprtResponse()
                {
                    Xprt = newXprt
                });
            }

            Console.WriteLine("Xprtz:");
            foreach (var x in Data.Xprtz.XprtList)
            {
                Console.WriteLine(x);
            }
        }

        public override async Task<XprtResponse> GetByBadgeNumber(GetByBadgeNumberRequest request, ServerCallContext context)
        {
            Metadata md = context.RequestHeaders;
            foreach (var entry in md)
            {
                Console.WriteLine(entry.Key + ": " + entry.Value);
            }

            var xprt = await _repository.GetXprtByBadge(request.BadgeNumber);
            
            return new XprtResponse()
            {
                Xprt = xprt
            };
            
            throw new Exception("Xprt not found with Badge Number: " +
                request.BadgeNumber);
        }

        public override async Task GetAll(GetAllRequest request, IServerStreamWriter<XprtResponse> responseStream, ServerCallContext context)
        {
            foreach (var x in await _repository.GetAllXprtz())
            {
                await responseStream.WriteAsync(new XprtResponse()
                {
                    Xprt = x
                });
            }
        }

        public override async Task<AddPhotoResponse> AddPhoto(IAsyncStreamReader<AddPhotoRequest> requestStream, ServerCallContext context)
        {
            int badgeNumber = 0;

            Metadata md = context.RequestHeaders;
            foreach (var entry in md)
            {
                if (entry.Key.Equals("badgenumber", StringComparison.CurrentCultureIgnoreCase))
                {
                    Console.WriteLine("Receiving photo for badgenumber: " + entry.Value);
                    try
                    {
                        badgeNumber = int.Parse(entry.Value);
                    }
                    catch
                    {
                        badgeNumber = 0;
                    }

                    break;
                }
            }

            var data = new List<byte>();
            while (await requestStream.MoveNext())
            {
                Console.WriteLine("Received " +
                    requestStream.Current.Data.Length + " bytes");
                data.AddRange(requestStream.Current.Data);
            }
            Console.WriteLine("Received file with " + data.Count + " bytes");

            if (badgeNumber > 0)
            {
                await _repository.AddPhoto(data.ToArray(), badgeNumber);
            }

            return new AddPhotoResponse()
            {
                IsOk = true
            };
        } 
    }
}
