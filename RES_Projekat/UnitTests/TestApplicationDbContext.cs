using Common;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.TestCommon
{
    [TestFixture]
    public class TestApplicationDbContext
    {
        [Test]
        public void TestConstructor()
        {
            ApplicationDbContext db = new ApplicationDbContext();

            Assert.IsNotNull(db);
            Assert.IsInstanceOf<ApplicationDbContext>(db);
        }


    }
}
