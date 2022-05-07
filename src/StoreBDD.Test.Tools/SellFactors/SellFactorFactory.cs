using StoreBDD.Entities;
using StoreBDD.Services.SellFactors.Contracts;
using System;

namespace StoreBDD.Test.Tools.SellFactors
{
    public static class SellFactorFactory
    {
        public static SellFactor GenerateSellFactor()
        {
            var sellFactor = new SellFactor
            {
                DateSold = DateTime.Now.Date,
                Count = 3,
                Product = new Product
                {
                    Name = "Dummy Name",
                    Count = 6,
                    MinimumCount = 1,
                    Price = 6000,
                    Category = new Category
                    {
                        Title = "Dummy CategoryTitle"
                    },
                },
            };

            sellFactor.Product.CategoryId = sellFactor.Product.Category.Id;
            sellFactor.ProductId = sellFactor.Product.Id;

            return sellFactor;
        }

        public static UpdateSellFactorDto GenerateUpdateSellFactorDto(int count
            , DateTime date)
        {
            return new UpdateSellFactorDto
            {
                DateSold= date.Date,
                Count = count
            };
        }
    }
}
