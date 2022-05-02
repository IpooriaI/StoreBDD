using StoreBDD.Infrastructure.Application;

namespace StoreBDD.Services.Products.Contracts
{
    public interface ProductService : Service
    {
        void Add(AddProductDto dto);
        void Update(int id, UpdateProductDto dto);
    }
}
