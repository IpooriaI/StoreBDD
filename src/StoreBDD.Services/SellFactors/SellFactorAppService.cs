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

        public List<GetSellFactorDto> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
