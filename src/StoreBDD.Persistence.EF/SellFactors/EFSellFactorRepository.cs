using StoreBDD.Entities;
using StoreBDD.Services.SellFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreBDD.Persistence.EF.SellFactors
{
    public class EFSellFactorRepository : SellFactorRepository
    {
        private readonly EFDataContext _dataContext;

        public EFSellFactorRepository(EFDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public void Add(SellFactor sellFactor)
        {
            _dataContext.SellFactors.Add(sellFactor);
        }

        public List<GetSellFactorDto> GetAll()
        {
            return _dataContext.SellFactors
                .Select(_ => new GetSellFactorDto
            {
                Id = _.Id,
                Count = _.Count,
                DateSold = _.DateSold,
                ProductId = _.ProductId,
            }).ToList();
        }
    }
}
