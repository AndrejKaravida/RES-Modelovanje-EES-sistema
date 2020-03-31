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
    public class TestDbSystemController
    {
        private static Mock<IDbSystemController> MockCreate()
        {
            Mock<IDbSystemController> mockedDB = new Mock<IDbSystemController>();
            mockedDB.Setup(x => x.Create());
            return mockedDB;
        }

        [Test]
        public void TestSystemControllerCreate()
        {
            IDbSystemController mockedDb = MockCreate().Object;
            Assert.DoesNotThrow(() => mockedDb.Create());
        }

 
        [Test]
        public void TestSystemControllerGetRequiredPower()
        {
            double requiredPower = 1234;
            Mock<IDbSystemController> mockedDB = new Mock<IDbSystemController>();
            mockedDB.Setup(x => x.GetRequiredPower()).Returns(requiredPower);

            double testPower = mockedDB.Object.GetRequiredPower();
            Assert.AreEqual(testPower, requiredPower);
            Assert.DoesNotThrow(() => mockedDB.Object.GetRequiredPower());
            Assert.IsNotNull(testPower);
        }

        [Test]
        public void TestSystemControllerSetRequiredPower()
        {
            double requiredPower = 1234;
            Mock<IDbSystemController> mockedDB = new Mock<IDbSystemController>();
            mockedDB.Setup(x => x.SetRequiredPower(requiredPower));

            Assert.DoesNotThrow(() => mockedDB.Object.SetRequiredPower(requiredPower));
        }

        [Test]
        public void TestSystemControllerSetTotalPower()
        {
            double totalPower = 1234;
            Mock<IDbSystemController> mockedDB = new Mock<IDbSystemController>();
            mockedDB.Setup(x => x.SetTotalPower(totalPower));

            Assert.DoesNotThrow(() => mockedDB.Object.SetRequiredPower(totalPower));
        }

        [Test]
        public void TestSystemControllerGetTotalPower()
        {
            double totalPower = 1234;
            Mock<IDbSystemController> mockedDB = new Mock<IDbSystemController>();
            mockedDB.Setup(x => x.GetTotalPower()).Returns(totalPower);

            double testPower = mockedDB.Object.GetTotalPower();
            Assert.AreEqual(testPower, totalPower);
            Assert.DoesNotThrow(() => mockedDB.Object.GetTotalPower());
            Assert.IsNotNull(testPower);
        }
    }
}
