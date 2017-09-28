    using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Al_Baraka.Models
{
    public class ViewProduct
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        [Required]
        public int Price { get; set; }
        public string Country { get; set; }
        public IFormFile Image { get; set; }
        public ProductGroups Groups { get; set; }
        public measure Measure { get; set; }

    }
}
