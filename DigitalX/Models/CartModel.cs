using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DigitalX.ServiceReference1;

namespace DigitalX.Models
{
    public class CartModel
    {
        public Product product { get; set; }

        public int quantity { get; set; }
    }
}