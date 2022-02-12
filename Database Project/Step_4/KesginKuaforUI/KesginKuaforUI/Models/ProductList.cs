using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KesginKuaforUI.Models
{
    public class ProductList
    {

        public int productID { get; set; }
        public string productName { get; set; }
        public string productType { get; set; }
        public int price { get; set; }
        public string quantity { get; set; }
    }
}