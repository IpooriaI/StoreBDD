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
                Count = 5,
                MinimumCount = 3,
                Price = 3000,
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
    }
}
