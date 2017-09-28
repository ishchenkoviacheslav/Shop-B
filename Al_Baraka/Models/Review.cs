using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Al_Baraka.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string ReviewText { get; set; }
        public string Name { get; set; }
        public string Contact { get; set; }
        public int UserId { get; set; }
    }
}
