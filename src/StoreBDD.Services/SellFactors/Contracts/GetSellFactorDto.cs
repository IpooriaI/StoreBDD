using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Services.SellFactors.Contracts
{
    public class GetSellFactorDto
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        public DateTime DateSold { get; set; }
    }
}
