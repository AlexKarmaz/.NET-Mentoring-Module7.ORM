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
    }
}
