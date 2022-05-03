using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Services.BuyFactors.Contracts
{
    public class GetBuyFactorDto
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        public DateTime DateBought { get; set; }
    }
}
