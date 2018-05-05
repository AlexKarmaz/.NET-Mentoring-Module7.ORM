namespace Northwind.EF_Task2_.Migrations
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Northwind.EF_Task2_.NorthwindDB>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Northwind.EF_Task2_.NorthwindDB context)
        {
            context.Categories.AddOrUpdate(c => c.CategoryName,
                new Category { CategoryName = "Vehicles" },
                new Category { CategoryName = "Drinks" });

            context.Regions.AddOrUpdate(r => r.RegionID,
                new Region { RegionDescription = "Minsk", RegionID = 5 });

            context.Territories.AddOrUpdate(t => t.TerritoryID,
                new Territory { TerritoryID = "199766", TerritoryDescription = "Railway station", RegionID = 5 },
                new Territory { TerritoryID = "343241232", TerritoryDescription = "Independent square", RegionID = 5 });
        }
    }
}
