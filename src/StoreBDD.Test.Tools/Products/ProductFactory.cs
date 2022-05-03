using StoreBDD.Entities;
using StoreBDD.Services.Products.Contracts;

namespace StoreBDD.Test.Tools.Products
{
    public static class ProductFactory
    {
        public static Product GenerateProduct(string name, int categoryId
            , int minimumCount = 5, int count = 20)
        {
            return new Product
            {
                Name = name,
                Count = count,
                MinimumCount = minimumCount,
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

        public static SellProductDto GenerateSellProductDto(int count)
        {
            return new SellProductDto
            {
                SoldCount = count
            };
        }

        public static BuyProductDto GenerateBuyProductDto(int count)
        {
            return new BuyProductDto
            {
                BoughtCount = count
            };
        }
    }
}
