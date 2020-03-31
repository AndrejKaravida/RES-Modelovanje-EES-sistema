using Common.Contracts;
using Common.Enums;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Common.DbTools
{
    public class DbGenerator : IDbGenerator
    {
        public DbGenerator()
        {
        }

        public void Create(Generator generator)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Group group = context.Groups.Include("Generators").FirstOrDefault(g => g.Name == generator.GroupName);

                if (group != null)
                {
                    group.Generators.Add(generator);
                    group.NumOfUnits++;
                    context.SaveChanges();
                }
            }
        }

        public void ResetRemoteGenerators()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var generators = context.Generators.Where(g => g.Control == EControl.REMOTE && g.State == EState.ONLINE).ToList();

                foreach (Generator generator in generators)
                {
                    generator.CurrentPower = 0;
                }
            }
        }

        public List<Generator> ReadAll()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Generators.Include(m => m.MeasurementHistory).ToList();
            }
        }

        public List<Measurement> GetMeasurementsForGenerator(int generatorId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                Generator gen = context.Generators.Include(g => g.MeasurementHistory).FirstOrDefault(g => g.Id == generatorId);
                return gen.MeasurementHistory.ToList();
            }
        }

        public Generator GetGenerator(int generatorId)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Generators.Where(g => g.Id == generatorId)
                        .Include(g => g.MeasurementHistory)
                        .Include(g => g.Setpoints).FirstOrDefault();
            }
        }

        public List<Generator> ReadAllRemote()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Generators.Where(g => g.Control == EControl.REMOTE).ToList();
            }
        }

        public List<Generator> ReadAllLocal()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Generators.Where(g => g.Control == EControl.LOCAL).ToList();
            }
        }

        public List<Generator> ReadAllForLC(string code)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Generators.Where(g => g.LCCode == code)
                    .Include(g => g.MeasurementHistory)
                    .Include(g => g.Setpoints)
                    .ToList();
            }
        }

        public List<int> ReadAllIds(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var codes = new List<int>();

                var generators = context.Generators.Where(g => g.LCCode == code).ToList();

                foreach (var gen in generators)
                {
                    codes.Add(gen.Id);
                }

                return codes;
            }
        }

        public void UpdatePower(List<Generator> generators)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (Generator g in generators)
                {
                    var genFromDb = context.Generators.FirstOrDefault(gen => gen.Id == g.Id);
                    genFromDb.CurrentPower = g.CurrentPower;
                }

                context.SaveChanges();
            }
        }

        public void ChangeType(int id, int newActivePower, EControl control)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var genFromDb = context.Generators.FirstOrDefault(gen => gen.Id == id);

                if (genFromDb != null && (genFromDb.Control != control || (genFromDb.Control == EControl.LOCAL && genFromDb.CurrentPower != newActivePower)))
                {
                    genFromDb.Control = control;

                    if (genFromDb.Control == 0)
                    {
                        genFromDb.CurrentPower = newActivePower;
                    }

                    context.SaveChanges();
                }
            }
        }
    }
}
