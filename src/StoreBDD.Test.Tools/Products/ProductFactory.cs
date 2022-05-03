using StoreBDD.Entities;
using StoreBDD.Services.Products.Contracts;

namespace StoreBDD.Test.Tools.Products
{
    public static class ProductFactory
    {
        public static Product GenerateProduct(string name, int categoryId)
        {
            return new Product
            {
                Name = name,
                Count = 20,
                MinimumCount = 5,
                Price = 5000,
                CategoryId = categoryId,
            };
        }

        public static AddProductDto GenerateAddProductDto
            (string name, int categoryId)
        {
            return new AddProductDto
            {
                Name = name,
                MinimumCount = 3,
                Price = 3000,
                CategoryId = categoryId,
            };
        }

        public static UpdateProductDto GenerateUpdateProductDto
            (string name, int categoryId)
        {
            return new UpdateProductDto
            {
                Name = name,
                MinimumCount = 5,
                Price = 5400,
                CategoryId = categoryId,
                Count = 40,
            };
        }
    }
}
