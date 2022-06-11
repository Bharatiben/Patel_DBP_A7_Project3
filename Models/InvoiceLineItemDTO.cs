using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Patel_DBP_A7_Project3.Models
{
    public class InvoiceLineItemDTO
    {
        public InvoiceLineItem invoiceLine1 { set; get; }
        public List<Invoice> invoice1 { set; get; }
        public List<Product> product1 { set; get; }
    }
}