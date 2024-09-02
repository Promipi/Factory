using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Domain
{
    public class Product
    {
        
        public string Id { get; set; } = Guid.NewGuid().ToString();
 
        public required string Name { get; set; }

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public string? Category {get;set; }

        
    }
}
