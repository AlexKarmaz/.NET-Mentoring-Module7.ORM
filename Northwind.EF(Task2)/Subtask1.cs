using System;
using NUnit.Framework;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace Northwind.EF_Task2_
{
    [TestFixture]
    public class Subtask1
    {
        private NorthwindDB context;

        [SetUp]
        public void SetUp()
        {
            context = new NorthwindDB();
        }

        [TearDown]
        public void CleanUp()
        {
            context.Dispose();
        }

        [Test]
        public void Orders_with_products_of_concrete_category()
        {
            int selectedCategoryId = 3;
            var result = context.Orders.Include(o => o.Order_Details.Select(od => od.Product)).Include(o => o.Customer)
                .Where(o => o.Order_Details.Any(od => od.Product.CategoryID == selectedCategoryId))
                .Select(o => new
                {
                    o.Customer.ContactName,
                    Order_Details = o.Order_Details.Select(od => new
                    {
                        od.Product.ProductName,
                        od.OrderID,
                        od.Discount,
                        od.Quantity,
                        od.UnitPrice,
                        od.ProductID
                    })
                }).ToList();

            foreach (var row in result)
            {
                Console.WriteLine($"Customer: {row.ContactName} Products: {string.Join(", ", row.Order_Details.Select(od => od.ProductName))}");
            }
        }
    }
}
