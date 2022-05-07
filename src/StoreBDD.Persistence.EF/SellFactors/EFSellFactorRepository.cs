using StoreBDD.Entities;
using StoreBDD.Services.SellFactors.Contracts;
using System.Collections.Generic;
using System.Linq;

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

        public List<GetFactorPriceDto> GetFactorPrice()
        {
            return _dataContext.SellFactors
                .Join(_dataContext.Products, a => a.ProductId,
                b => b.Id, (a, b) => new GetFactorPriceDto
                {
                    Price = b.Price,
                    Count = a.Count,
                }).ToList();
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

        public GetSellFactorDto Get(int id)
        {
            return _dataContext.SellFactors
               .Select(_ => new GetSellFactorDto
               {
                   Id = _.Id,
                   Count = _.Count,
                   DateSold = _.DateSold,
                   ProductId = _.ProductId,
               }).FirstOrDefault(_ => _.Id == id);
        }

        public SellFactor GetById(int id)
        {
            return _dataContext.SellFactors.FirstOrDefault(_ => _.Id ==id);
        }

        public void Delete(SellFactor factor)
        {
            _dataContext.SellFactors.Remove(factor);
        }
    }
}
