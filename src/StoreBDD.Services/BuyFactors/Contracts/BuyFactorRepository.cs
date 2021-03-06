using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using System.Collections.Generic;

namespace StoreBDD.Services.BuyFactors.Contracts
{
    public interface BuyFactorRepository : Repository
    {
        void Add(BuyFactor buyFactor);
        List<GetBuyFactorDto> GetAll();
        GetBuyFactorDto Get(int id);
        BuyFactor GetById(int id);
        void Delete(BuyFactor factor);
    }
}
