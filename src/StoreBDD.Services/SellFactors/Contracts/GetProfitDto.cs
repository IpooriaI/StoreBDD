using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Services.SellFactors.Contracts
{
    public class GetProfitDto
    {
        public int Profit { get; set; }
        public List<GetSellFactorDto> SellFactors { get; set; }
    }
}
