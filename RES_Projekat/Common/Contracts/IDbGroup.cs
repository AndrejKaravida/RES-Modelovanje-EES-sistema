using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Contracts
{
    public interface IDbGroup
    {
        Group GetGroup(string name);
        void Create(Group group);
        void IncreaseNumber(string code);
        List<string> ReadAll(string code);
    }
}
