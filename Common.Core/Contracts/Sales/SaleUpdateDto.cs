using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Contracts.Sales
{
    public sealed class SaleUpdateDto
    {
        public string Id { get; set; } = string.Empty;
        public string CustomerId { get; set; } = string.Empty;

        public List<SaleProductCreateDto> Items = new List<SaleProductCreateDto>();
    }
}
