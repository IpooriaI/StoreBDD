using StoreBDD.Entities;
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
    }
}
