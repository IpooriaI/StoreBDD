using StoreBDD.Infrastructure.Application;
using System.Collections.Generic;

namespace StoreBDD.Services.SellFactors.Contracts
{
    public interface SellFactorService : Service
    {
        List<GetSellFactorDto> GetAll();
    }
}
