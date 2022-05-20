using Microsoft.EntityFrameworkCore;
using StoreBDD.Entities;
using StoreBDD.Services.Products.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace StoreBDD.Persistence.EF.Products
{
    public class EFProductRepository : ProductRepository
    {
        private readonly EFDataContext _dataContext;

        public EFProductRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public async Task Add(Product product)
        {
           await _dataContext.Products.AddAsync(product);
        }

        public async Task<bool> CheckCategory(int categoryId)
        {
            return await _dataContext.Categories
                .AnyAsync(_ => _.Id == categoryId);
        }

        public async Task<bool> CheckId(int productId)
        {
            return await _dataContext.Products.AnyAsync(_ => _.Id == productId);
        }

        public async Task<bool> CheckName(int categoryId, 
            string name, int ignoreId)
        {
            return await _dataContext.Products
                .Where(_ => _.CategoryId == categoryId && _.Id != ignoreId)
                .AnyAsync(_ => _.Name == name);
        }

        public async Task Delete(Product product)
        {
            _dataContext.Products.Remove(product);
        }

        public async Task<GetProductDto> Get(int id)
        {
            return await _dataContext.Products
                .Where(_ => _.Id == id)
                .Select(_ => new GetProductDto
                {
                    Name = _.Name,
                    CategoryId = _.CategoryId,
                    Count = _.Count,
                    MinimumCount = _.MinimumCount,
                    Price = _.Price
                }).FirstOrDefaultAsync();
        }

        public async Task<Product> GetById(int id)
        {
            return await _dataContext.Products
                .FirstOrDefaultAsync(p => p.Id == id);
        }
    }
}
