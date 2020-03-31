using Autofac.Extras.Moq;
using Common;
using Common.Contracts;
using Common.DbTools;
using Common.Enums;
using Common.Model;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace UnitTests.TestCommon.TestDbTools
{
    [TestFixture]
    public class TestDbGenerator
    {
      
        private static Mock<IDbGenerator> MockReturnAll(List<Generator> returnValue)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.ReadAll()).Returns(returnValue);
            return mockedDB;
        }

        private static Mock<IDbGenerator> MockResetRemoteGenerators()
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.ResetRemoteGenerators());
            return mockedDB;
        }

        private static Mock<IDbGenerator> MockCreate(Generator gen)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.Create(gen));
            return mockedDB;
        }

        private static Mock<IDbGenerator> MockGetMeasurementsForGenerator(int id)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.GetMeasurementsForGenerator(id));
            return mockedDB;
        }

        private static Mock<IDbGenerator> MockGetGenerator(int id)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.GetGenerator(id));
            return mockedDB;
        }

        private static Mock<IDbGenerator> MockReadAllRemote()
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.ReadAllRemote());
            return mockedDB;
        }

        private static Mock<IDbGenerator> MockReadAllLocal()
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.ReadAllLocal());
            return mockedDB;
        }

        private static Mock<IDbGenerator> ReadAllForLC(string code)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.ReadAllForLC(code));
            return mockedDB;
        }

        private static Mock<IDbGenerator> MockReadAllIds(string code)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.ReadAllIds(code));
            return mockedDB;
        }

        private static Mock<IDbGenerator> MockUpdatePower(List<Generator> generators)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.UpdatePower(generators));
            return mockedDB;
        }

        private static Mock<IDbGenerator> MockChangeType(int generatorId, int newActivePower, EControl control)
        {
            Mock<ApplicationDbContext> mockedDbFactory = new Mock<ApplicationDbContext>();
            Mock<IDbGenerator> mockedDB = new Mock<IDbGenerator>();
            mockedDB.Setup(x => x.ChangeType(generatorId, newActivePower, control));
            return mockedDB;
        }

        [Test]
        public void TestGetGenerator()
        {
            Generator g = new Generator();
            IDbGenerator mockedDb = MockGetGenerator(g.Id).Object;
            Assert.IsNull(mockedDb.GetGenerator(g.Id));
        }

        [Test]
        public void TestGetMeasurementsForGenerator()
        {
            //ovaj generator ne postoji u bazi
            IDbGenerator mockedDb = MockGetMeasurementsForGenerator(-1).Object;
            Assert.IsNull(mockedDb.GetMeasurementsForGenerator(-1));
        }

        [Test]
        public void TestGeneratorReturnAll()
        {
            IDbGenerator mockedDb = MockReturnAll(GetSampleGenerators()).Object;
            var result = mockedDb.ReadAll().Count;
            Assert.NotZero(mockedDb.ReadAll().Count);
        }


        private List<Generator> GetSampleGenerators()
        {
            List<Generator> output = new List<Generator>
            {
                new Generator { },

                new Generator { },

                new Generator { }

            };

            return output;
        }
        
    }
}
