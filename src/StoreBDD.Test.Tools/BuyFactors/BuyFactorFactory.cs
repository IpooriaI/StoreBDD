using StoreBDD.Entities;
using StoreBDD.Services.BuyFactors.Contracts;
using System;

namespace StoreBDD.Test.Tools.BuyFactors
{
    public static class BuyFactorFactory
    {
        public static BuyFactor GenerateBuyFactor()
        {
            var buyFactor = new BuyFactor
            {
                DateBought = DateTime.Now.Date,
                Count = 2,
                Product = new Product
                {
                    Name = "Dummy Name",
                    Count = 4,
                    MinimumCount = 2,
                    Price = 5000,
                    Category = new Category
                    {
                        Title = "Dummy CategoryTitle"
                    },
                },
            };

            buyFactor.Product.CategoryId = buyFactor.Product.Category.Id;
            buyFactor.ProductId = buyFactor.Product.Id;

            return buyFactor;
        }

        public static UpdateBuyFactorDto GenerateUpdateBuyFactorDto(int count
            , DateTime date)
        {
            return new UpdateBuyFactorDto
            {
                Count = count,
                DateBought = date.Date
            };
        }
    }
}
