﻿namespace StoreBDD.Services.Products.Contracts
{
    public class AddProductDto
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public int MinimumCount { get; set; }
        public int CategoryId { get; set; }
    }
}
