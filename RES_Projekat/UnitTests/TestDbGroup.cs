using Common;
using Common.Contracts;
using Common.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.TestCommon.TestDbTools
{
    [TestFixture]
    public class TestDbGroup
    {
        private static Mock<IDbGroup> MockReturnAll(List<string> returnValue)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGroup> mockedDB = new Mock<IDbGroup>();
            mockedDB.Setup(x => x.ReadAll("12345")).Returns(returnValue);
            return mockedDB;
        }

        private static Mock<IDbGroup> MockCreate(Group gr)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGroup> mockedDB = new Mock<IDbGroup>();
            mockedDB.Setup(x => x.Create(gr));
            return mockedDB;
        }

        private static Mock<IDbGroup> MockIncreaseNumber(string code)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGroup> mockedDB = new Mock<IDbGroup>();
            mockedDB.Setup(x => x.IncreaseNumber(code));
            return mockedDB;
        }

        [Test]
        public void TestMockIncreaseNumber()
        {
            IDbGroup mockedDb = MockIncreaseNumber("first").Object;
            Assert.DoesNotThrow(() => mockedDb.IncreaseNumber("first"));
        }

        [Test]
        public void TestMockCreate()
        {
            Group g = new Group();
            IDbGroup mockedDb = MockCreate(g).Object;
            
            Assert.DoesNotThrow(() => mockedDb.Create(g));
        }

        [Test]
        public void TestGroupReadAll()
        {
            List<string> lista = new List<string>()
            {
                "123456",
                "1234567",
                "12345678"
            };

            IDbGroup mockedDb = MockReturnAll(lista).Object;
            var result = mockedDb.ReadAll("1234");

            //Ne postoji takva lista
            Assert.IsNull(result);
        }

     
    }
}
