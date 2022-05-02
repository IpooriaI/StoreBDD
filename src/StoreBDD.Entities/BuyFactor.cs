using System;

namespace StoreBDD.Entities
{
    public class BuyFactor
    {
        public int Id { get; set; }
        public int Count { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        public DateTime DateBought { get; set; }
    }
}
