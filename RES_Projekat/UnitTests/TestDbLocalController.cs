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
    public class TestDbLocalController
    {
        private static Mock<IDbLocalController> MockReadAll(List<LocalController> returnValue)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbLocalController> mockedDB = new Mock<IDbLocalController>();
            mockedDB.Setup(x => x.ReadAll()).Returns(returnValue);
            return mockedDB;
        }

        private static Mock<IDbLocalController> MockIsCodeFree(string code)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbLocalController> mockedDB = new Mock<IDbLocalController>();
            mockedDB.Setup(x => x.IsCodeFree(code)).Returns(true);
            return mockedDB;
        }

        [Test]
        public void TestLocalControllerIsCodeFree()
        {
            IDbLocalController mockedDb = MockIsCodeFree("aa").Object;
            Assert.IsTrue(mockedDb.IsCodeFree("aa"));
        }

        [Test]
        public void TestLocalControllerReturnAll()
        {
            IDbLocalController mockedDb = MockReadAll(GetLocalControllers()).Object;
            Assert.NotZero(mockedDb.ReadAll().Count);
        }


        private List<LocalController> GetLocalControllers()
        {
            List<LocalController> output = new List<LocalController>
            {
                new LocalController { },

                new LocalController { },

                new LocalController { }

            };

            return output;
        }
    }
}
