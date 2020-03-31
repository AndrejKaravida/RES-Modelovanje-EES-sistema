using Common.Enums;
using Common.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTests.TestCommon
{
    [TestFixture]
    public class TestModel
    {
        [Test]
        public void TestGeneratorConstructors()
        {
            Generator gen1 = new Generator();
            
            Assert.IsNotNull(gen1);
            Assert.IsInstanceOf<Generator>(gen1);
        }

        [Test]
        public void TestGroupConstructors()
        {
            Group g1 = new Group();
       //     Group g2 = new Group("123");

            Assert.IsNotNull(g1);
            Assert.IsInstanceOf<Group>(g1);

          //  Assert.IsNotNull(g2);
         //   Assert.IsInstanceOf<Group>(g2);
        }

        [Test]
        public void TestSystemControllerConstructor()
        {
            SystemController sc1 = new SystemController();

            Assert.IsNotNull(sc1);
            Assert.IsInstanceOf<SystemController>(sc1);
        }

        [Test]
        public void TestMeasurementConstructors()
        {
            Measurement m1 = new Measurement();
            Measurement m2 = new Measurement(123);

            Assert.IsNotNull(m1);
            Assert.IsInstanceOf<Measurement>(m1);

            Assert.IsNotNull(m2);
            Assert.IsInstanceOf<Measurement>(m2);

        }

        [Test]
        public void TestLocalControllerConstructors()
        {
            LocalController lc1 = new LocalController();
            LocalController lc2 = new LocalController("123");

            Assert.IsNotNull(lc1);
            Assert.IsInstanceOf<LocalController>(lc1);

            Assert.IsNotNull(lc2);                               
            Assert.IsInstanceOf<LocalController>(lc2);
        }

        [Test]
        public void TestAddMeasurementsToHistory()
        {
            Generator gen = new Generator();

            Assert.DoesNotThrow(() => gen.AddMeasurementToHistory());
            Assert.IsInstanceOf<Generator>(gen);
        }
    }
}
