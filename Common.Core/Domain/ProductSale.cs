using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Domain
{
    public class ProductSale
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ProductId { get; set; }

        public Product Product { get; set; }

        public string SaleId { get; set; }


        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }
    }
}
