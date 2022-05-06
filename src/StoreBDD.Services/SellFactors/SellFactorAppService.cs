using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.SellFactors.Contracts;
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
    }
}
