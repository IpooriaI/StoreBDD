using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;

namespace StoreBDD.Services.Products.Contracts
{
    public interface ProductRepository : Repository
    {
        void Add(Product product);
        bool CheckName(int categoryId, string name,int ignoreId);
        Product GetById(int id);
        void Delete(Product product);
        GetProductDto Get(int id);
    }
}
