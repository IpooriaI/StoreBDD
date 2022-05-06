using System.Collections.Generic;

namespace StoreBDD.Services.SellFactors.Contracts
{
    public class GetProfitDto
    {
        public int Profit { get; set; }
        public List<GetSellFactorDto> SellFactors { get; set; }
    }
}
