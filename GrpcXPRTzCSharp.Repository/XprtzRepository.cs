using GrpcXPRTzCSharp.Repository.Mappers;
using Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcXPRTzCSharp.Repository
{
    public class XprtzRepository : IXprtzRepository
    {
        private readonly IXprtzContextFactory _contextFactory;

        public XprtzRepository(IXprtzContextFactory contextFactory)
        {
            _contextFactory = contextFactory;
        }

        public async Task<Xprt> AddXprt(Xprt xprt)
        {
            using (var ctx = _contextFactory.CreateXprtzContext())
            {
                var newXprt = ctx.Xprtz.Add(xprt.ToEntity());
                await ctx.SaveChangesAsync();
                return newXprt.Entity.ToDTO();
            }
        }

        public Task<Xprt> GetXprtByBadge(int badgeNumber)
        {
            return Task.Run<Xprt>(() =>
            {
                using (var ctx = _contextFactory.CreateXprtzContext())
                {
                    var xprt = ctx.Xprtz.Where(x => x.BadgeNumber == badgeNumber).FirstOrDefault();

                    return xprt == null ? null : xprt.ToDTO();
                }
            });
        }

        public Task<Xprt> GetXprtById(int id)
        {
            return Task.Run<Xprt>(() =>
            {
                using (var ctx = _contextFactory.CreateXprtzContext())
                {
                    var xprt = ctx.Xprtz.Where(x => x.Id == id).FirstOrDefault();

                    return xprt == null ? null : xprt.ToDTO();
                }
            });
        }

        public Task<List<Xprt>> GetAllXprtz()
        {
            return Task.Run<List<Xprt>>(() =>
            {
                using (var ctx = _contextFactory.CreateXprtzContext())
                {
                    return ctx.Xprtz.Select(x => x.ToDTO()).ToList();
                }
            });
        }

        public async Task AddAllXprtz(List<Xprt> xprtz)
        {
            using (var ctx = _contextFactory.CreateXprtzContext())
            {
                ctx.Xprtz.AddRange(xprtz.Select(x => x.ToEntity()));
                await ctx.SaveChangesAsync();
            }
        }

        public async Task Seed(List<Xprt> xprtz)
        {
            if(!(await GetAllXprtz()).Any())
            {
                await AddAllXprtz(xprtz);
            }
        }

        public async Task AddPhoto(byte[] photo, int badgeNumber)
        {
            using (var ctx = _contextFactory.CreateXprtzContext())
            {
                if (ctx.Xprtz.Any(x => x.BadgeNumber == badgeNumber))
                {
                    var xprt = ctx.Xprtz.Single(x => x.BadgeNumber == badgeNumber);
                    xprt.Foto = photo;
                    await ctx.SaveChangesAsync();
                }
            }
        }
    }
}
