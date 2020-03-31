using System.Collections.Generic;

namespace Common.Model
{
    public class Group
    {
        private int id;
        private string name;
        private int numOfUnits;
        private double currentProduction;
        private ICollection<Generator> generators;
        private string lcCode;
        private double maxProduction;

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public int NumOfUnits { get => numOfUnits; set => numOfUnits = value; }
        public ICollection<Generator> Generators { get => generators; set => generators = value; }
        public string LCCode { get => lcCode; set => lcCode = value; }
        public double CurrentProduction { get => currentProduction; set => currentProduction = value; }
        public double MaxProduction { get => maxProduction; set => maxProduction = value; }

        public Group()
        {
            Generators = new List<Generator>();
        }
    }
}
