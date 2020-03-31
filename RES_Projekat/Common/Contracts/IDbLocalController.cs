using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IDbLocalController
    {
        void Connect(string code);
        void Create(string code);
        LocalController GetLocalController(string code);
        void TurnOnLCGenerators(string code);
        void ShutDownGenerators(string code);
        List<LocalController> ReadAll();
        bool IsCodeFree(string code);
        bool IsOnline(string code);
        void ShutDown(string code);
        void SaveChanges(Generator generator);
    }
}
