using System;
using System.Linq;
using LinqToDB;
using NUnit.Framework;

namespace Northwind.LinqToDb_Task1_
{
    [TestFixture]
    public class Subtask2
    {
        private NorthwindConnection connection;

        [SetUp]
        public void SetUp()
        {
            connection = new NorthwindConnection("Northwind");
        }

        [TearDown]
        public void CleanUp()
        {
            connection.Dispose();
        }

        [Test]
        public void List_of_products_with_category_and_supplier()
        {
            foreach (var product in connection.Products.LoadWith(p => p.Category).LoadWith(p => p.Supplier).ToList())
            {
                Console.WriteLine($"Product name: {product.ProductName}; Category: {product.Category?.CategoryName}; Supplier: {product.Supplier?.ContactName}");
            }
        }

        [Test]
        public void List_of_employees_with_region()
        {
            var query = from e in connection.Employees
                        join et in connection.EmployeeTerritories on e.EmployeeId equals et.EmployeeId into el
                        from w in el.DefaultIfEmpty()
                        join t in connection.Territories on w.TerritoryId equals t.TerritoryId into zl
                        from z in zl.DefaultIfEmpty()
                        join r in connection.Regions on z.RegionId equals r.RegionId into kl
                        from k in kl.DefaultIfEmpty()
                        select new { e.FirstName, e.LastName, Region = k };
            query = query.Distinct();

            foreach (var record in query.ToList())
            {
                Console.WriteLine($"Employee: {record.FirstName} {record.LastName}; Region: {record.Region?.RegionDescription}");
            }
        }

        [Test]
        public void Count_of_employees_by_regions()
        {
            var query = from r in connection.Regions
                        join t in connection.Territories on r.RegionId equals t.RegionId into kl
                        from k in kl.DefaultIfEmpty()
                        join et in connection.EmployeeTerritories on k.TerritoryId equals et.TerritoryId into zl
                        from z in zl.DefaultIfEmpty()
                        join e in connection.Employees on z.EmployeeId equals e.EmployeeId into dl
                        from d in dl.DefaultIfEmpty()
                        select new { Region = r, d.EmployeeId };
            var result = from row in query.Distinct()
                         group row by row.Region into ger
                         select new { RegionDescription = ger.Key.RegionDescription, EmployeesCount = ger.Count(e => e.EmployeeId != 0) };

            foreach (var record in result.ToList())
            {
                Console.WriteLine($"Region: {record.RegionDescription}; Employees count: {record.EmployeesCount}");
            }
        }

        [Test]
        public void Employees_with_Shippers_according_to_Orders()
        {
            var query = (from e in connection.Employees
                         join o in connection.Orders on e.EmployeeId equals o.EmployeeId into el
                         from w in el.DefaultIfEmpty()
                         join s in connection.Shippers on w.ShipperId equals s.ShipperId into zl
                         from z in zl.DefaultIfEmpty()
                         select new { e.EmployeeId, e.FirstName, e.LastName, z.CompanyName }).Distinct().OrderBy(t => t.EmployeeId);

            foreach (var record in query.ToList())
            {
                Console.WriteLine($"Employee: {record.FirstName} {record.LastName} Shipper: {record.CompanyName}");
            }
        }
    }
}
