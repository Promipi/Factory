﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Core.Contracts.Sales
{
    public sealed class SaleProductCreateDto
    {
        public string ProductId { get; set; }

        public int Quantity { get; set; }
    }
}
