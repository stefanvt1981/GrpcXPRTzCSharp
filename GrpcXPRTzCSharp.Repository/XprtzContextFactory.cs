using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GrpcXPRTzCSharp.Repository
{
    public class XprtzContextFactory : IXprtzContextFactory
    {
        public XprtzContext CreateXprtzContext()
        {
            var builder = new DbContextOptionsBuilder<XprtzContext>()
                .UseInMemoryDatabase("Xprtz");

            return new XprtzContext(builder.Options);
        }
    }
}
