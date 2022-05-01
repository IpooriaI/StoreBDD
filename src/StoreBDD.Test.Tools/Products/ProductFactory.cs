using StoreBDD.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Test.Tools.Products
{
    public static class ProductFactory
    {
        public static Product GenerateProduct(string name,int categoryId)
        {
            return new Product
            {
                Name = "test",
                Count = 5,
                MinimumCount = 3,
                Price = 3000,
                CategoryId = categoryId,
            };
        }
    }
}
