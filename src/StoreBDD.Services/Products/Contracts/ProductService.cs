using StoreBDD.Infrastructure.Application;

namespace StoreBDD.Services.Products.Contracts
{
    public interface ProductService : Service
    {
        void Add(AddProductDto dto);
        void Update(int id, UpdateProductDto dto);
        void Delete(int id);
        GetProductDto Get(int id);
        CountCheckerDto Sell(int id, SellProductDto dto);
        void Buy(int id, BuyProductDto dto);
    }
}
