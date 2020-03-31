using Common.Enums;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IDbGenerator
    {
        void Create(Generator generator);
        void ResetRemoteGenerators();
        List<Generator> ReadAll();
        List<Measurement> GetMeasurementsForGenerator(int generatorId);
        Generator GetGenerator(int id);
        List<Generator> ReadAllRemote();
        List<Generator> ReadAllLocal();
        List<Generator> ReadAllForLC(string code);
        List<int> ReadAllIds(string code);
        void UpdatePower(List<Generator> generators);
        void ChangeType(int generatorId, int newActivePower, EControl control);
    }
}
