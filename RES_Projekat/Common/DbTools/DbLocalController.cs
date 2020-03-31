using Common.Contracts;
using Common.Enums;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DbTools
{
    public class DbLocalController : IDbLocalController
    {
        public DbLocalController()
        {
        }

        public void Connect(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (!context.LocalControllers.Any(lc => lc.Code == code))
                {
                    Create(code);
                }
                else
                {
                    LocalController localController = context.LocalControllers.First(lc => lc.Code == code);
                    localController.IsOnline = true;
                    context.SaveChanges();
                }
            }
        }

        public void Create(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                LocalController localController = new LocalController(code);
                localController.IsOnline = true;

                SystemController sc = context.SystemControllers.Include("LocalControllers").FirstOrDefault();

                if (sc != null)
                {
                    sc.LocalControllers.Add(localController);
                    context.SaveChanges();
                }
            }
        }

        public LocalController GetLocalController(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.LocalControllers.Include("Groups").Include("Generators").FirstOrDefault(lc => lc.Code == code);
            }
        }

        public void TurnOnLCGenerators(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (Generator generator in context.Generators.Where(g => g.LCCode == code))
                {
                    generator.State = EState.ONLINE;
                    context.SaveChanges();
                }
            }
        }

        public void ShutDownGenerators(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                foreach (Generator generator in context.Generators.Where(g => g.LCCode == code))
                {
                    generator.State = EState.OFFLINE;
                }

                context.SaveChanges();
            }
        }

        public List<LocalController> ReadAll()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.LocalControllers.Include("Generators").Where(lc => lc.IsOnline).ToList();
            }
        }

        public bool IsCodeFree(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                if (context.LocalControllers.Any(lc => lc.Code.Equals(code)))
                {
                    return false;
                }

                return true;
            }
        }

        public bool IsOnline(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                LocalController lc = context.LocalControllers.FirstOrDefault(l => l.Code.Equals(code));

                return lc != null && lc.IsOnline;
            }
        }

        public void ShutDown(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                LocalController lc = context.LocalControllers.FirstOrDefault(l => l.Code.Equals(code));

                if (lc != null)
                {
                    lc.IsOnline = false;
                    context.SaveChanges();
                }
            }
        }

        public void SaveChanges(Generator generator)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                Generator genFromDb = context.Generators.FirstOrDefault(g => g.Id == generator.Id);
                
                if (genFromDb != null)
                {
                    genFromDb.CurrentPower = generator.CurrentPower;
                    genFromDb.State = generator.State;
                    context.SaveChanges();
                }
            }
        }
    }
}
