using Common.Contracts;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Common.DbTools
{
    public class DbGroup : IDbGroup
    {
        public DbGroup()
        {
        }

        public Group GetGroup(string name)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                return context.Groups.FirstOrDefault(g => g.Name == name);
            }
        }

        public void Create(Group group)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                LocalController lc = context.LocalControllers.Include("Groups").FirstOrDefault(l => l.Code == group.LCCode);

                if (lc != null)
                {
                    if (!lc.Groups.Any(g => g.Name == group.Name))
                    {
                        lc.Groups.Add(group);

                        context.SaveChanges();
                    }
                    else
                    {
                        MessageBox.Show("The group with this name already exists in local system.", "Name taken", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Can't find Local Controller.", "Local Controller error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public void IncreaseNumber(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var group = context.Groups.FirstOrDefault(g => g.Name == code);

                if (group.Id > 0)
                {
                    group.NumOfUnits++;
                    context.SaveChanges();
                }
            }
        }

        public List<string> ReadAll(string code)
        {
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                var codes = new List<string>();

                var groups = context.Groups.Where(g => g.LCCode == code).ToList();

                foreach (var group in groups)
                {
                    codes.Add(group.Name);
                }

                return codes;
            }
        }
    }
}
