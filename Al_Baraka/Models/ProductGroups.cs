using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace Al_Baraka.Models
{
    public class ProductGroups
    {

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public bool Sweets { get; set; }
        public bool DriedFruits { get; set; }
        public bool Spice { get; set; }
        public bool Nuts { get; set; }
        public bool Oils { get; set; }
        public bool Sauces { get; set; }
        public bool Italian { get; set; }
        public bool EasternMed { get; set; }
        public bool Grocery { get; set; }

        public int ProductId { get; set; }
        public Product ProductForGroups { get; set; }
        public override string ToString()
        {
            if (Sweets)
                return "Sweets";
            if (DriedFruits)
                return "DriedFruits";
            if (Spice)
                return "Spice";
            if (Nuts)
                return "Nuts";
            if (Oils)
                return "Oils";
            if (Sauces)
                return "Sauces";
            if (Italian)
                return "Italian";
            if (EasternMed)
                return "EasternMed";
            if (Grocery)
                return "Grocery";
            return base.ToString();
        }
    }
}

