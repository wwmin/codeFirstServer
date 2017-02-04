using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ProductReviews.Models
{
    public class Review
    {
        public int ReviewId{get;set;}
        public int ProductId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }
        //navigation Property
        public Product Product { get; set; }
    }
}