using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.BuyFactors.Contracts;
using StoreBDD.Services.SellFactors.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public List<GetBuyFactorDto> GetAll()
        {
            return _repository.GetAll();
        }
    }
}
