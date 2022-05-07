using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Services.SellFactors.Contracts
{
    public class UpdateSellFactorDto
    {
        public int Count { get; set; }
        public DateTime DateSold { get; set; }
    }
}
