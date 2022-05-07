using StoreBDD.Infrastructure.Application;
using System.Collections.Generic;

namespace StoreBDD.Services.SellFactors.Contracts
{
    public interface SellFactorService : Service
    {
        List<GetSellFactorDto> GetAll();
        GetSellFactorDto Get(int id);
        GetProfitDto GetProfit();
        void Delete(int id);
        void Update(int id, UpdateSellFactorDto dto);
    }
}
