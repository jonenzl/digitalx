﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace WcfDigitalX
{
    [DataContract]
    public class OrderModel
    {
        [DataMember]
        public int OrderID { get; set; }

        [DataMember]
        public int CustomerID { get; set; }

        [DataMember]
        public DateTime OrderDate { get; set; }

        [DataMember]
        public bool Complete { get; set; }

        [DataMember]
        public decimal TotalAmount { get; set; }
    }
}
