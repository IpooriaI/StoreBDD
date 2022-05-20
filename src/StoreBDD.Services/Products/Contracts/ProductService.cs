using StoreBDD.Infrastructure.Application;
using System.Threading.Tasks;

namespace StoreBDD.Services.Products.Contracts
{
    public interface ProductService : Service
    {
        Task Add(AddProductDto dto);
        Task Update(int id, UpdateProductDto dto);
        Task Delete(int id);
        Task<GetProductDto> Get(int id);
        Task<UpdateResponseDto> Sell(int id, SellProductDto dto);
        Task Buy(int id, BuyProductDto dto);
    }
}
