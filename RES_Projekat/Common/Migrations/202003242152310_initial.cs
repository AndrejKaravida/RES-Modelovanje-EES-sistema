using System;
using System.Data.Entity.Migrations;

public partial class initial : DbMigration
{
    public override void Up()
    {
        CreateTable(
            "dbo.Generators",
            c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    LCCode = c.String(maxLength: 2147483647),
                    Type = c.Int(nullable: false),
                    MinPower = c.Double(nullable: false),
                    MaxPower = c.Double(nullable: false),
                    WorkPrice = c.Double(nullable: false),
                    GroupName = c.String(maxLength: 2147483647),
                    Control = c.Int(nullable: false),
                    CurrentPower = c.Double(nullable: false),
                    State = c.Int(nullable: false),
                    Group_Id = c.Int(),
                    LocalController_Id = c.Int(),
                })
            .PrimaryKey(t => t.Id)
            .ForeignKey("dbo.Groups", t => t.Group_Id)
            .ForeignKey("dbo.LocalControllers", t => t.LocalController_Id)
            .Index(t => t.Group_Id)
            .Index(t => t.LocalController_Id);
        
        CreateTable(
            "dbo.Measurements",
            c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Date = c.DateTime(nullable: false),
                    Value = c.Double(nullable: false),
                    Generator_Id = c.Int(),
                    Generator_Id1 = c.Int(),
                })
            .PrimaryKey(t => t.Id)
            .ForeignKey("dbo.Generators", t => t.Generator_Id)
            .ForeignKey("dbo.Generators", t => t.Generator_Id1)
            .Index(t => t.Generator_Id)
            .Index(t => t.Generator_Id1);
        
        CreateTable(
            "dbo.Groups",
            c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Name = c.String(maxLength: 2147483647),
                    NumOfUnits = c.Int(nullable: false),
                    LCCode = c.String(maxLength: 2147483647),
                    CurrentProduction = c.Double(nullable: false),
                    MaxProduction = c.Double(nullable: false),
                    LocalController_Id = c.Int(),
                })
            .PrimaryKey(t => t.Id)
            .ForeignKey("dbo.LocalControllers", t => t.LocalController_Id)
            .Index(t => t.LocalController_Id);
        
        CreateTable(
            "dbo.LocalControllers",
            c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    Code = c.String(maxLength: 2147483647),
                    IsOnline = c.Boolean(nullable: false),
                    TotalPower = c.Double(nullable: false),
                    SystemController_Id = c.Int(),
                })
            .PrimaryKey(t => t.Id)
            .ForeignKey("dbo.SystemControllers", t => t.SystemController_Id)
            .Index(t => t.SystemController_Id);
        
        CreateTable(
            "dbo.SystemControllers",
            c => new
                {
                    Id = c.Int(nullable: false, identity: true),
                    RequiredPower = c.Double(nullable: false),
                    TotalPower = c.Double(nullable: false),
                })
            .PrimaryKey(t => t.Id);
        
    }
    
    public override void Down()
    {
        DropForeignKey("dbo.LocalControllers", "SystemController_Id", "dbo.SystemControllers");
        DropForeignKey("dbo.Groups", "LocalController_Id", "dbo.LocalControllers");
        DropForeignKey("dbo.Generators", "LocalController_Id", "dbo.LocalControllers");
        DropForeignKey("dbo.Generators", "Group_Id", "dbo.Groups");
        DropForeignKey("dbo.Measurements", "Generator_Id1", "dbo.Generators");
        DropForeignKey("dbo.Measurements", "Generator_Id", "dbo.Generators");
        DropIndex("dbo.LocalControllers", new[] { "SystemController_Id" });
        DropIndex("dbo.Groups", new[] { "LocalController_Id" });
        DropIndex("dbo.Measurements", new[] { "Generator_Id1" });
        DropIndex("dbo.Measurements", new[] { "Generator_Id" });
        DropIndex("dbo.Generators", new[] { "LocalController_Id" });
        DropIndex("dbo.Generators", new[] { "Group_Id" });
        DropTable("dbo.SystemControllers");
        DropTable("dbo.LocalControllers");
        DropTable("dbo.Groups");
        DropTable("dbo.Measurements");
        DropTable("dbo.Generators");
    }
}
