using Models.EF;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WatchShop.Areas.Client.Models
{
    [Serializable]
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}