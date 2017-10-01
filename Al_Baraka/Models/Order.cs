using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Al_Baraka.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int ProductId{ get; set; }
        public Product ProductItem { get; set; }
        public int UserId { get; set; }
        public DateTime TimeOfBye { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Contact { get; set; }
        public bool IsDone { get; set; }
        public string Note { get; set; }

    }
}
