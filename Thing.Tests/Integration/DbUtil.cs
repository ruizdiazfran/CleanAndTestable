using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Thing.Core.Infrastructure.Persistence;

namespace Thing.Tests.Integration
{
    public class DbUtil
    {
        internal static ThingDbContext CreateDbContext()
        {
            var dbContext = new ThingDbContext();
            dbContext.Database.Log = Console.WriteLine;
            return dbContext;
        }        
    }
}
