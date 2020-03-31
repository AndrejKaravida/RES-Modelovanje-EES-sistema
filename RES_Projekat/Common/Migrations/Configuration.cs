using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Data.SQLite.EF6.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal sealed class Configuration : DbMigrationsConfiguration<Common.ApplicationDbContext>
{
    public Configuration()
    {
        SetSqlGenerator("System.Data.SQLite", new SQLiteMigrationSqlGenerator());
        AutomaticMigrationsEnabled = true;
        AutomaticMigrationDataLossAllowed = true;
    }

}
