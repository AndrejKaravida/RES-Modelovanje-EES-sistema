using Common.DbTools;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UI_LokalniKontroler
{
	public class Calculations
    {
		private DbGenerator dbg = new DbGenerator();

        public double MeanGeneratorPower(int generatorId, DateTime fromDate, DateTime toDate)
		{
            return dbg.GetGenerator(generatorId).MeasurementHistory.Where(x => x.Date.Date >= fromDate.Date && x.Date.Date <= toDate.Date).ToList().Average(m => m.Value);
        }

		public double MinGeneratorPower(int generatorId, DateTime fromDate, DateTime toDate)
		{
            return dbg.GetGenerator(generatorId).MeasurementHistory.Where(x => x.Date.Date >= fromDate.Date && x.Date.Date <= toDate.Date).ToList().Min(m => m.Value);
		}

		public double MaxGeneratorPower(int generatorId, DateTime fromDate, DateTime toDate)
        {
            return dbg.GetGenerator(generatorId).MeasurementHistory.Where(x => x.Date.Date >= fromDate.Date && x.Date.Date <= toDate.Date).ToList().Max(m => m.Value);
        }

        public double MeanGroupPower(string groupCode, DateTime fromDate, DateTime toDate)
		{
            return GetGroupCalculations(groupCode, fromDate, toDate).Values.Average();
		}

		public double MinGroupPower(string groupCode, DateTime fromDate, DateTime toDate)
		{
            return GetGroupCalculations(groupCode, fromDate, toDate).Values.Min();
        }
		public double MaxGroupPower(string groupCode, DateTime fromDate, DateTime toDate)
		{
            return GetGroupCalculations(groupCode, fromDate, toDate).Values.Max();
        }

        public Dictionary<int, double> GetGroupCalculations(string groupCode, DateTime fromDate, DateTime toDate)
        {
            List<Generator> generators = dbg.ReadAllForLC(Data.Code).Where(g => g.GroupName == groupCode).ToList(); //svi generatori za ovaj lokalni kontroler

            List<DateTime> dates = new List<DateTime>(); //lista svih mogucih trenutaka kada su merenja belezena

            foreach (Generator g in generators)
            {
                foreach (Measurement m in g.MeasurementHistory)
                {
                    if (m.Date.Date >= fromDate.Date && m.Date.Date <= toDate.Date)
                    {
                        if (!dates.Contains(m.Date.AddTicks(-m.Date.Ticks % TimeSpan.TicksPerSecond))) // dodajemo trenutak u listu ukoliko vec ne postoji i ako ispunjava prosledjen datum
                            dates.Add(m.Date.AddTicks(-m.Date.Ticks % TimeSpan.TicksPerSecond));
                    }
                }
            }

            int numOfdates = dates.Count;
            Dictionary<int, double> keyValuePairs = new Dictionary<int, double>();   //int - redni broj trenutka iz dates, double - ukupna vrednost merenja svih gen za taj trenutak

            for (int i = 0; i < dates.Count; i++)
            {
                keyValuePairs.Add(i, 0);                     //inicijalizujemo vrednost merenja za svkai trenutak na 0
            }


            for (int i = 0; i < numOfdates; i++)
            {
                foreach (Generator g in generators)
                {
                    foreach (Measurement m in g.MeasurementHistory)
                    {
                        if (m.Date.Date >= fromDate.Date && m.Date.Date <= toDate.Date)
                        {
                            if (m.Date.AddTicks(-m.Date.Ticks % TimeSpan.TicksPerSecond) == dates[i]) //ako je ovo merenje bas u i-tom trenutku
                            {
                                keyValuePairs[i] += m.Value;

                            }
                        }
                    }
                }
            }

            //dobili smo zbir svih merenja za svaki trenutak koji postoji <vremenski_trenutak, ukupna_snage>, u funkcijama dalje izvlacimo min, max, average

            return keyValuePairs;
        }


	}
}
