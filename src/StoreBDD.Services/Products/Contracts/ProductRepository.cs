using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using System.Threading.Tasks;

namespace StoreBDD.Services.Products.Contracts
{
    public interface ProductRepository : Repository
    {
        Task Add(Product product);
        Task<bool> CheckName(int categoryId, string name, int ignoreId);
        Task<bool> CheckCategory(int categoryId);
        Task<bool> CheckId(int productId);
        Task<Product> GetById(int id);
        Task Delete(Product product);
        Task<GetProductDto> Get(int id);
    }
}
