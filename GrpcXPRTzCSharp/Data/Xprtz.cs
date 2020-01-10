using Messages;
using Messages.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GrpcXPRTzCSharp.Server.Data
{
    public static class Xprtz
    {
        public static List<Xprt> XprtList = new List<Xprt>()
        {
            new Xprt
            {                
                BadgeNumber = 2080,
                FirstName = "Sander",
                LastName = "Obdijn",
                GeboorteDatum = Utils.ToUnixTimestamp(new DateTime(1990, 1, 2)),
                Skills = { "DevOps", ".Net Core"},
            },
            new Xprt {                
                BadgeNumber= 7538,
                FirstName= "Joeri",
                LastName= "Lieuw",
                GeboorteDatum = Utils.ToUnixTimestamp(new DateTime(1990, 2, 3)),
                Skills = { "Asp.Net", ".Net Core"},
            },
            new Xprt {                
                BadgeNumber= 5144,
                FirstName= "Dick",
                LastName= "van Hirtum",
                GeboorteDatum = Utils.ToUnixTimestamp(new DateTime(1980, 3, 4)),
                Skills = { "Entity Framework", ".Net Core"},
            }
        };
    }
}