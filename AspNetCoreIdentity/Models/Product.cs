using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreIdentity.Models
{
    public class Product
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public string ImageUrl { get; set; }

        public string CreatedBy { get; set; }
        public string UpdatedBy { get; set; }

        [ForeignKey("CreatedBy")]
        public User CreatedUser { get; set; }
        [ForeignKey("UpdatedBy")]
        public User UpdatedByUser { get; set; }
    }
}
