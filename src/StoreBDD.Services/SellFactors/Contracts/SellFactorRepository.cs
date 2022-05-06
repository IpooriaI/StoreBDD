using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using System.Collections.Generic;

namespace StoreBDD.Services.SellFactors.Contracts
{
    public interface SellFactorRepository : Repository
    {
        void Add(SellFactor sellFactor);
        List<GetSellFactorDto> GetAll();
        List<GetFactorPriceDto> GetFactorPrice();
        GetSellFactorDto Get(int id);
    }
}
