using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfDigitalX
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in both code and config file together.
    public class Service1 : IService1
    {
        DigitalXDBEntities db = new DigitalXDBEntities();

        // Return a list of all products
        public List<Product> GetAllProducts()
        {
            return db.Products.ToList();
        }

        // Return a list of a certain number of products on a certain page
        public List<Product> GetProducts(int PageSize, int PageNumber)
        {
            var data = (from p in db.Products
                        orderby p.ProductID ascending
                        select p).Skip((PageNumber - 1) * PageSize).Take(PageSize);

            return data.ToList();
        }

        // Return the top five products based on their price
        public List<Product> GetTopSellers()
        {
            var data = (from p in db.Products
                        orderby p.Price descending
                        select p).Take(5);

            return data.ToList();
        }

        // Return the details for the corresponding product
        public Product ProductDetail(int id)
        {
            var data = db.Products.Find(id);

            return data;
        }

        // Return a list of orders for the currently logged in customer
        public List<OrderModel> Orders(int id)
        {
            var data = (from o in db.Orders
                        join od in db.OrderDetails on o.OrderID equals od.OrderID
                        join p in db.Products on od.ProductID equals p.ProductID
                        where o.CustomerID == id
                        select new { orderId = o.OrderID, orderDate = o.OrderDate, complete = o.Complete, totalAmount = (od.Quantity * p.Price) });

            List<OrderModel> myOrder = new List<OrderModel>();

            foreach (var item in data)
            {
                OrderModel order = new OrderModel();
                order.OrderID = item.orderId;
                order.OrderDate = item.orderDate;
                order.Complete = item.complete;
                order.TotalAmount = item.totalAmount;
                myOrder.Add(order);
            }

            return myOrder;
        }

        // Return the details for current customer
        public Customer CurrentCustomer(string UserName)
        {
            Customer data = (from c in db.Customers
                             where c.UserName == UserName
                             select c).SingleOrDefault(); 

            return data;
        }

        // Return the address details for current customer
        public Address GetAddress(string UserName)
        {
            var address = db.Customers.Where(p => p.UserName == UserName)
                .Select(a => a.Addresses.FirstOrDefault()).FirstOrDefault();

            return address;
        }

        // Return the address details for current customer
        //public Address GetAddress(string UserName)
        //{
        //    //var customerID = CurrentCustomer(UserName).CustomerID;
        //    Customer customer = CurrentCustomer(UserName);
        //    //Address data = db.Customers.Find(customerID).Addresses.FirstOrDefault();
        //    Address address = (from a in db.Addresses
        //                       where a.Customers == customer
        //                       select a).FirstOrDefault();

        //    //Address data = (from a in db.Addresses
        //    //                where a.AddressID == addressID
        //    //                select a).SingleOrDefault();

        //    return address;
        //}

        // Add a new customer to the DigitalX database when they register on the website
        public void AddCustomer(Customer customer)
        {
            db.Customers.Add(customer);
            db.SaveChanges();
        }

        // Add a new address for the customer to the DigitalX database
        public void AddAddress(Address address, string UserName)
        {
            db.Addresses.Add(address);
            var customerID = CurrentCustomer(UserName).CustomerID; 
            db.Customers.Find(customerID).Addresses.Add(address); 
            db.SaveChanges();
        }
    }
}
