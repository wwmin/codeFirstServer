using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProductReviews.JGZX.Entities.Common
{
    public class SysConfigBean
    {
        [Key]
        public int sys_id { get; set; }
        public string filepath { get; set; }
        public string systype { get; set; }
    }
}