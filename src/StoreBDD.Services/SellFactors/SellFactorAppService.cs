using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.SellFactors.Contracts;
using StoreBDD.Services.SellFactors.Exceptions;
using System.Collections.Generic;

namespace StoreBDD.Services.SellFactors
{
    public class SellFactorAppService : SellFactorService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly SellFactorRepository _repository;
        public SellFactorAppService(SellFactorRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public void Delete(int id)
        {
            var factor = GetSellFactor(id);

            _repository.Delete(factor);
            _unitOfWork.Commit();
        }


        public GetSellFactorDto Get(int id)
        {
            return _repository.Get(id);
        }

        public List<GetSellFactorDto> GetAll()
        {
            return _repository.GetAll();
        }

        public GetProfitDto GetProfit()
        {
            return new GetProfitDto
            {
                Profit = CalculateProfit(),
                SellFactors = _repository.GetAll()
            };
        }

        public void Update(int id, UpdateSellFactorDto dto)
        {
            var factor = GetSellFactor(id);

            factor.DateSold = dto.DateSold;
            factor.Count = dto.Count;

            _unitOfWork.Commit();
        }

        private int CalculateProfit()
        {
            int profit = 0;
            var factors = _repository.GetFactorPrice();
            foreach (var factor in factors)
            {
                profit += factor.Count * factor.Price;
            }

            return profit;
        }

        private SellFactor GetSellFactor(int id)
        {
            var factor = _repository.GetById(id);

            if (factor == null)
            {
                throw new SellFactorNotFoundException();
            }

            return factor;
        }
    }
}
