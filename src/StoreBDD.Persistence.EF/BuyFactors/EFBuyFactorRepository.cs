using StoreBDD.Entities;
using StoreBDD.Services.BuyFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Persistence.EF.BuyFactors
{
    public class EFBuyFactorRepository : BuyFactorRepository
    {
        private readonly EFDataContext _dataContext;
        public EFBuyFactorRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(BuyFactor buyFactor)
        {
            _dataContext.BuyFactors.Add(buyFactor);
        }
    }
}
