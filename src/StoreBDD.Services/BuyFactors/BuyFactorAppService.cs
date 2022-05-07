using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.BuyFactors.Contracts;
using StoreBDD.Services.BuyFactors.Exceptions;
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

        public void Delete(int id)
        {
            var factor = GetBuyFactor(id);

            _repository.Delete(factor);
            _unitOfWork.Commit();
        }

        public GetBuyFactorDto Get(int id)
        {
            return _repository.Get(id);
        }

        public List<GetBuyFactorDto> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(int id, UpdateBuyFactorDto dto)
        {
            var factor = GetBuyFactor(id);

            factor.DateBought = dto.DateBought;
            factor.Count = dto.Count;

            _unitOfWork.Commit();
        }

        private BuyFactor GetBuyFactor(int id)
        {
            var factor = _repository.GetById(id);

            if (factor == null)
            {
                throw new BuyFactorNotFoundException();
            }

            return factor;
        }
    }
}
