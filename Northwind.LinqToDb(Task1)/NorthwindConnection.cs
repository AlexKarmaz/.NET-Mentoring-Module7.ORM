using System;
using LinqToDB;
using LinqToDB.Data;
using Northwind.LinqToDb_Task1_.Entities;

namespace Northwind.LinqToDb_Task1_
{
    public class NorthwindConnection : DataConnection
    {
        public NorthwindConnection(string configurationString) : base(configurationString)
        {
        }

        public ITable<Category> Categories => GetTable<Category>();
        public ITable<Product> Products => GetTable<Product>();
        public ITable<Supplier> Suppliers => GetTable<Supplier>();
        public ITable<Region> Regions => GetTable<Region>();
        public ITable<Territory> Territories => GetTable<Territory>();
        public ITable<EmployeeTerritory> EmployeeTerritories => GetTable<EmployeeTerritory>();
        public ITable<Employee> Employees => GetTable<Employee>();
        public ITable<Shipper> Shippers => GetTable<Shipper>();
        public ITable<Order> Orders => GetTable<Order>();
        public ITable<OrderDetail> OrderDetails => GetTable<OrderDetail>();
    }
}
