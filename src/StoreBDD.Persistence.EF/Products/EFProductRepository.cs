using StoreBDD.Entities;
using StoreBDD.Services.Products.Contracts;
using System.Linq;

namespace StoreBDD.Persistence.EF.Products
{
    public class EFProductRepository : ProductRepository
    {
        private readonly EFDataContext _dataContext;

        public EFProductRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(Product product)
        {
            _dataContext.Products.Add(product);
        }

        public bool CheckCategory(int categoryId)
        {
            return _dataContext.Categories.Any(_ => _.Id == categoryId);
        }

        public bool CheckId(int productId)
        {
            return _dataContext.Products.Any(_ => _.Id == productId);
        }

        public bool CheckName(int categoryId, string name, int ignoreId)
        {
            return _dataContext.Products
                .Where(_ => _.CategoryId == categoryId && _.Id != ignoreId)
                .Any(_ => _.Name == name);
        }

        public void Delete(Product product)
        {
            _dataContext.Products.Remove(product);
        }

        public GetProductDto Get(int id)
        {
            return _dataContext.Products
                .Where(_ => _.Id == id)
                .Select(_ => new GetProductDto
                {
                    Name = _.Name,
                    CategoryId = _.CategoryId,
                    Count = _.Count,
                    MinimumCount = _.MinimumCount,
                    Price = _.Price
                }).FirstOrDefault();
        }

        public Product GetById(int id)
        {
            return _dataContext.Products.FirstOrDefault(p => p.Id == id);
        }
    }
}
