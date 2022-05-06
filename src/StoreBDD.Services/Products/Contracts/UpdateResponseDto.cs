namespace StoreBDD.Services.Products.Contracts
{
    public class UpdateResponseDto
    {
        public bool MinimumCountReached { get; set; }
        public int RemainingSupply { get; set; }
    }
}
