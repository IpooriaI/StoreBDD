using StoreBDD.Infrastructure.Application;
using System.Collections.Generic;

namespace StoreBDD.Services.BuyFactors.Contracts
{
    public interface BuyFactorService : Service
    {
        List<GetBuyFactorDto> GetAll();
    }
}
