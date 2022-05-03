using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Services.Products.Exceptions;
using StoreBDD.Services.SellFactors.Contracts;
using System;

namespace StoreBDD.Services.Products
{
    public class ProductAppService : ProductService
    {
        private readonly ProductRepository _repository;
        private readonly SellFactorRepository _sellFactorRepository;
        private readonly UnitOfWork _unitOfWork;

        public ProductAppService(ProductRepository repository
            , UnitOfWork unitOfWork,SellFactorRepository sellFactorRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _sellFactorRepository = sellFactorRepository;
        }

        public void Add(AddProductDto dto)
        {
            var product = new Product
            {
                Name = dto.Name,
                MinimumCount = dto.MinimumCount,
                Count = 0,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
            };
            var checkName = _repository
                .CheckName(product.CategoryId, product.Name);

            if(checkName)
            {
                throw new DuplicateProductNameInSameCategoryException();
            }


            _repository.Add(product);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var product = _repository.GetById(id);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            _repository.Delete(product);
            _unitOfWork.Commit();
        }

        public GetProductDto Get(int id)
        {
            return _repository.Get(id);
        }

        public void Sell(int id, SellProductDto dto)
        {
            var product = _repository.GetById(id);
            product.Count = product.Count - dto.SoldCount;

            var sellFactor = new SellFactor
            {
                ProductId = product.Id,
                Count = dto.SoldCount,
                DateSold = DateTime.Now.Date,
            };

            _sellFactorRepository.Add(sellFactor);

            _unitOfWork.Commit();
        }

        public void Update(int id, UpdateProductDto dto)
        {
            var product = _repository.GetById(id);

            if(product==null)
            {
                throw new ProductNotFoundException();
            }

            var checkName = _repository.CheckName(product.CategoryId, dto.Name);


            if (checkName)
            {
                throw new DuplicateProductNameInSameCategoryException();
            }

            product.Name = dto.Name;
            product.MinimumCount = dto.MinimumCount;
            product.Count = dto.Count;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;

            _unitOfWork.Commit();
        }
    }
}
