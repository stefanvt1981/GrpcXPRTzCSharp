using System.Collections.Generic;
using System.Threading.Tasks;
using Messages;

namespace GrpcXPRTzCSharp.Repository
{
    public interface IXprtzRepository
    {
        Task AddAllXprtz(List<Xprt> xprtz);
        Task Seed(List<Xprt> xprtz);
        Task<Xprt> AddXprt(Xprt xprt);
        Task<List<Xprt>> GetAllXprtz();
        Task<Xprt> GetXprtByBadge(int badgeNumber);
        Task<Xprt> GetXprtById(int id);
        Task AddPhoto(byte[] photo, int badgeNumber);
    }
}