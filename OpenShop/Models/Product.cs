using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OpenShop.Models
{
    public partial class Product
    {
        [Key]
        public int Id { get; set; }
        [Display(Name = "Product Name")]
        public string product_name { get; set; }
        [Display(Name  = "Price")]
        public double price { get; set; }

        public virtual ProductQuantities ProductQuantities { get; set; }
    }
}