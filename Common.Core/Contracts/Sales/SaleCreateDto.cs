using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Contracts.Sales
{
    public sealed class SaleCreateDto
    {
        public string CustomerId { get; set; } = string.Empty;

        public List<SaleProductCreateDto> Items { get; set; } = new List<SaleProductCreateDto>();
    }
}
