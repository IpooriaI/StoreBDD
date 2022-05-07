using StoreBDD.Infrastructure.Application;
using System.Collections.Generic;

namespace StoreBDD.Services.BuyFactors.Contracts
{
    public interface BuyFactorService : Service
    {
        List<GetBuyFactorDto> GetAll();
        GetBuyFactorDto Get(int id);
        void Delete(int id);
        void Update(int id, UpdateBuyFactorDto dto);
    }
}
