using ClassLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace WcfDigitalX
{
    /// NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IService1" in both code and config file together.
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]
        List<Product> GetAllProducts();

        [OperationContract]
        List<Product> GetProducts(int PageSize, int PageNumber);

        [OperationContract]
        List<Product> GetTopSellers();

        [OperationContract]
        Product ProductDetail(int id);

        [OperationContract]
        List<OrderModel> Orders(int id);

        [OperationContract]
        Customer CurrentCustomer(string id);

        [OperationContract]
        Address GetAddress(string UserName);

        [OperationContract]
        void AddCustomer(Customer customer);

        [OperationContract]
        void AddAddress(Address address, string UserName);
    }
}
