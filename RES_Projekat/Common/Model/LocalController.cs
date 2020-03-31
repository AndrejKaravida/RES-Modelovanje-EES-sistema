using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.Model
{
    public class LocalController
    {
        private int id;
        private string code;
        private ICollection<Generator> generators;
        private ICollection<Group> groups;
        private bool isOnline;
        private double totalPower;

        public int Id { get => id; set => id = value; }
        public string Code { get => code; set => code = value; }
        public ICollection<Generator> Generators { get => generators; set => generators = value; }
        public ICollection<Group> Groups { get => groups; set => groups = value; }
        public bool IsOnline { get => isOnline; set => isOnline = value; }

        public double TotalPower
        {
            get
            {
                totalPower = 0;
                
                if(Generators != null)
                {
                    foreach (Generator g in Generators)
                    {
                        totalPower += g.CurrentPower;
                    }
                }

                return totalPower;
            }
            set => totalPower = value;
        }

        public LocalController(string code)
        {
            Code = code;
            IsOnline = true;
        }

        public LocalController()
        {
            IsOnline = true;
        }
    }
}