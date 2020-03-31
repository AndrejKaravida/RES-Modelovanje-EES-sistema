using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IDbSystemController
    {
        double GetRequiredPower();
        void Create();
        void SetRequiredPower(double power);
        void SetTotalPower(double power);
        void ShutDownController();
        double GetTotalPower();
        SystemController GetSystemController();
        void SaveSetpoints(Generator g);
    }
}
