using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductReviews.Models
{
    public class UpLoadFiles
    {
        [Key]
        public int file_id { get; set; }
        public string file_name { get; set; }
        public string file_path { get; set; }
        public string file_type { get; set; }
        public string simple_name { get; set; }
        public string simple_path { get; set; }
        public string simple_type { get; set; }
        public int create_user { get; set; }
        public DateTime create_date { get; set; }
    }
    public class UpLoadHsty
    {
        [Key]
        public int hsty_id { get; set; }
        public string old_name { get; set; }
        public string new_name { get; set; }
        public DateTime create_date { get; set; }
    }
}