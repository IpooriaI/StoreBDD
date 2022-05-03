using StoreBDD.Entities;
using StoreBDD.Services.BuyFactors.Contracts;
using System.Collections.Generic;
using System.Linq;

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

        public List<GetBuyFactorDto> GetAll()
        {
            return _dataContext.BuyFactors.Select(x => new GetBuyFactorDto
            {
                Count = x.Count,
                DateBought = x.DateBought,
                Id = x.Id,
                ProductId = x.ProductId,
            }).ToList();
        }
    }
}
