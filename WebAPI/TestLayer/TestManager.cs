using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLayer;
using DataLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory;


namespace TestLayer
{
    [TestFixture]
    public class TestManager
    {
        internal static GiftShopDbContext dbContext;

        static TestManager()
        {
            DbContextOptionsBuilder builder = new DbContextOptionsBuilder();
            builder.UseInMemoryDatabase("TestDb");
            dbContext = new GiftShopDbContext(builder.Options);
        }

        [OneTimeTearDown]
        public void Dispose()
        {
            dbContext.Dispose();
        }

    }
}
