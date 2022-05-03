using System;

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
