using Common.DbTools;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UI_KontrolerSistema
{
    public class Statistics
    {
        private DbGenerator dbg = new DbGenerator();

        public double AverageLoad(DateTime date)
        {
            return GetSystemCalculations(date).Values.Average();
        }

        public double MinimumLoad(DateTime date)
        {
            return GetSystemCalculations(date).Values.Min();
        }
        public double MaximumLoad(DateTime date)
        {
            return GetSystemCalculations(date).Values.Max();
        }

        public double GetTotalCost(DateTime date)
        {
            double result = 0;
            List<Generator> generators = dbg.ReadAll();

            foreach(var gen in generators)
            {   
                var measurements = gen.MeasurementHistory.Where(x => x.Date.Date == date.Date);
                double totalPowerProduced = 0;

                foreach (Measurement m in measurements)
                {
                    totalPowerProduced += m.Value;  //ukupna proizvodnja za svaki generator tog dana
                }

                result += totalPowerProduced * gen.WorkPrice * 3 / 1000; //podelicemo sa 1000 jer je cena u megawatima   | mnozimo sa 3 jer svako merenje vazi 3 sekunde
            }

            return result;
        }

        public double TotalGeneratorsActive(DateTime date)
        {
            var result = 0;
            List<Generator> generators = dbg.ReadAll();

            foreach(Generator gen in generators)
            {
               foreach (Measurement m in gen.MeasurementHistory)
               {
                    if (m.Date.Date == date.Date)
                    {
                        result++;
                        break;
                    }
               }
            }
            return result;
        }

        public Dictionary<int, double> GetSystemCalculations(DateTime date)
        {
             List<Generator> generators = dbg.ReadAll(); //svi generatori u sistemu

             List<DateTime> dates = new List<DateTime>(); //lista svih mogucih trenutaka kada su merenja belezena

             foreach (Generator g in generators)
             {
                 foreach (Measurement m in g.MeasurementHistory)
                 {
                     if (m.Date.Date == date.Date)
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
                         if (m.Date.Date == date.Date)
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
