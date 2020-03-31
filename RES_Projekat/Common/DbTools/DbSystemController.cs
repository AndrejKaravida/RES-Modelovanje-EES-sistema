using Common.Contracts;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.DbTools
{
    public class DbSystemController : IDbSystemController
    {
        public double GetRequiredPower()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.SystemControllers.First().RequiredPower;
            }
        }

        public void Create()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                context.SystemControllers.Add(new SystemController());
                context.SaveChanges();
            }
        }

        public void SetRequiredPower(double power)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.SystemControllers.First().RequiredPower = power;
                context.SaveChanges();
            }
        }

        public void SetTotalPower(double power)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.SystemControllers.First().TotalPower = power;
                context.SaveChanges();
            }
        }

        public void ShutDownController()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.SystemControllers.First().TotalPower = 0;
                context.SystemControllers.First().RequiredPower = 0;
                context.SaveChanges();
            }
        }

        public double GetTotalPower()
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.SystemControllers.First().TotalPower;
            }
        }

        public SystemController GetSystemController()
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.SystemControllers.FirstOrDefault();
            }
        }

        public void SaveSetpoints(Generator generator)
        {
            using(ApplicationDbContext context = new ApplicationDbContext())
            {
                Generator genFromDb = context.Generators.Include("Setpoints").FirstOrDefault(g => g.Id == generator.Id);

                if (genFromDb != null)
                {
                    genFromDb.Setpoints.ForEach(sp =>
                    {
                        sp.Value = generator.Setpoints.FirstOrDefault(s => s.Id == sp.Id).Value;
                        sp.Date = generator.Setpoints.FirstOrDefault(s => s.Id == sp.Id).Date;
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
