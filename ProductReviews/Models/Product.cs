using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace ProductReviews.Models
{
    public class Product
    {
        public int ProductId { get; set; }
        
        [Required]
        public string Name { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        //navigation property
        public ICollection<Review> Reviews { get; set; }
    }
}