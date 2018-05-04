using System;
using System.Linq;
using LinqToDB;
using NUnit.Framework;
using Northwind.LinqToDb_Task1_.Entities;
using System.Collections.Generic;
using LinqToDB.Data;

namespace Northwind.LinqToDb_Task1_
{
    [TestFixture]
    public class Subtask3
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
        public void Add_new_Employee_with_Territories()
        {
            Employee newEmployee = new Employee { FirstName = "Aliaksey", LastName = "Karmaz" };
            try
            {
                connection.BeginTransaction();
                newEmployee.EmployeeId = Convert.ToInt32(connection.InsertWithIdentity(newEmployee));
                connection.Territories.Where(t => t.TerritoryDescription.Length <= 5)
                    .Insert(connection.EmployeeTerritories, t => new EmployeeTerritory { EmployeeId = newEmployee.EmployeeId, TerritoryId = t.TerritoryId });
                connection.CommitTransaction();
            }
            catch
            {
                connection.RollbackTransaction();
            }
        }

        [Test]
        public void Move_Products_to_another_Category()
        {
            int updatedCount = connection.Products.Update(p => p.CategoryId == 2, pr => new Product
            {
                CategoryId = 1
            });

            Console.WriteLine(updatedCount);
        }

        [Test]
        public void Insert_list_of_Products_with_Suppliers_and_Categories()
        {
            var products = new List<Product>
            {
                new Product
                {
                    ProductName = "Car",
                    Category = new Category {CategoryName = "Vehicles"},
                    Supplier = new Supplier {CompanyName = "Stark industries"}
                },
                new Product
                {
                    ProductName = "Reactive car",
                    Category = new Category {CategoryName = "Vehicles"},
                    Supplier = new Supplier {CompanyName = "Stark industries"}
                }
            };

            try
            {
                connection.BeginTransaction();
                //pass ids to products list
                foreach (var product in products)
                {
                    var category = connection.Categories.FirstOrDefault(c => c.CategoryName == product.Category.CategoryName);
                    product.CategoryId = category?.CategoryId ?? Convert.ToInt32(connection.InsertWithIdentity(
                                             new Category
                                             {
                                                 CategoryName = product.Category.CategoryName
                                             }));
                    var supplier = connection.Suppliers.FirstOrDefault(s => s.CompanyName == product.Supplier.CompanyName);
                    product.SupplierId = supplier?.SupplierId ?? Convert.ToInt32(connection.InsertWithIdentity(
                                             new Supplier
                                             {
                                                 CompanyName = product.Supplier.CompanyName
                                             }));
                }

                connection.BulkCopy(products);
                connection.CommitTransaction();
            }
            catch
            {
                connection.RollbackTransaction();
            }
        }

        [Test]
        public void Replace_Product_with_the_same_in_NotShippedOrder()
        {
            var updatedRows = connection.OrderDetails.LoadWith(od => od.Order).LoadWith(od => od.Product)
                .Where(od => od.Order.ShippedDate == null).Update(
                    od => new OrderDetail
                    {
                        ProductId = connection.Products.First(p => p.CategoryId == od.Product.CategoryId && p.ProductId > od.ProductId) != null
                            ? connection.Products.First(p => p.CategoryId == od.Product.CategoryId && p.ProductId > od.ProductId).ProductId
                            : connection.Products.First(p => p.CategoryId == od.Product.CategoryId).ProductId
                    });
            Console.WriteLine($"{updatedRows} rows updated");
        }
    }
}
