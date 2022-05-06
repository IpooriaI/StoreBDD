using StoreBDD.Entities;
using StoreBDD.Services.Products.Contracts;

namespace StoreBDD.Test.Tools.Products
{
    public static class ProductFactory
    {
        public static Product GenerateProduct(string name, int categoryId
             ,int id, int minimumCount = 5, int count = 20)
        {
            return new Product
            {
                Id = id,
                Name = name,
                Count = count,
                MinimumCount = minimumCount,
                Price = 5000,
                CategoryId = categoryId,
            };
        }

        public static Product GenerateProductWithCategory(string name
            ,int id, int minimumCount = 5, int count = 20)
        {
            var product = new Product
            {
                Id = id,
                Name = name,
                Count = count,
                MinimumCount = minimumCount,
                Price = 5000,
                Category = new Category
                {
                    Title = "TestTitle"
                }
            };
            product.CategoryId = product.Category.Id;

            return product;
        }

        public static AddProductDto GenerateAddProductDto
            (string name, int categoryId,int id=50)
        {
            return new AddProductDto
            {
                Id = id,
                Name = name,
                MinimumCount = 3,
                Price = 3000,
                CategoryId = categoryId,
            };
        }

        public static UpdateProductDto GenerateUpdateProductDto
            (string name, int categoryId,int id=40)
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
