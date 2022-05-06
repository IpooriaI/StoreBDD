namespace StoreBDD.Services.Products.Contracts
{
    public class AddProductDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public int MinimumCount { get; set; }
        public int CategoryId { get; set; }
    }
}
