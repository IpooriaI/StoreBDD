using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.BuyFactors.Contracts;
using System.Collections.Generic;

namespace StoreBDD.Services.BuyFactors
{
    public class BuyFactorAppService : BuyFactorService
    {
        private readonly UnitOfWork _unitOfWork;
        private readonly BuyFactorRepository _repository;
        public BuyFactorAppService(BuyFactorRepository repository,
            UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public GetBuyFactorDto Get(int id)
        {
            return _repository.Get(id);
        }

        public List<GetBuyFactorDto> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
