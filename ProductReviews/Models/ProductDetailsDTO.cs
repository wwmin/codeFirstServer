using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductReviews.Models
{
    public class ProductDetailsDTO
    {
        public int ProductId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int Price { get; set; }
        public List<Review> Reviews { get; set; }
    }
}