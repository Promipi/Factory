using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Domain
{
    public class Sale
    {
        [Required]
        public required string Id { get; set; } = Guid.NewGuid().ToString();

        public DateTime Date { get; set; }

        public decimal Total { get; set; }

        public string? CustomerId { get; set; }

        public required Customer Customer { get; set; }

        public ICollection<ProductSale> Items { get; set; }
    }
}
