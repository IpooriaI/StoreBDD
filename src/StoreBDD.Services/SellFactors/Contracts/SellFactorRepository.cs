using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Services.SellFactors.Contracts
{
    public interface SellFactorRepository : Repository
    {
        void Add(SellFactor sellFactor);
        List<GetSellFactorDto> GetAll();
    }
}
