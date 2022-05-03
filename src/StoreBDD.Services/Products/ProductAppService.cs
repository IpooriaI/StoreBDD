using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.BuyFactors.Contracts;
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
        private readonly BuyFactorRepository _buyFactorRepository;
        private readonly UnitOfWork _unitOfWork;

        public ProductAppService(ProductRepository repository
            , UnitOfWork unitOfWork,SellFactorRepository sellFactorRepository,
            BuyFactorRepository buyFactorRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _sellFactorRepository = sellFactorRepository;
            _buyFactorRepository = buyFactorRepository;
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
          
            CheckIfNameIsDuplicate(product.CategoryId,product.Name);

            _repository.Add(product);
            _unitOfWork.Commit();
        }

        public void Delete(int id)
        {
            var product = GetProduct(id);

            _repository.Delete(product);
            _unitOfWork.Commit();
        }

        public GetProductDto Get(int id)
        {
            return _repository.Get(id);
        }

        public CountCheckerDto Sell(int id, SellProductDto dto)
        {
            var product = GetProduct(id);

            CheckIfProductCountIsEnough(product.Count, dto.SoldCount);
            product.Count -= dto.SoldCount;
            CreateSellFactor(dto.SoldCount,product.Id);

            _unitOfWork.Commit();

            if(product.Count <= product.MinimumCount)
            {
                return new CountCheckerDto
                {
                    MinimumCountReached = true
                };
            }
            else
            {
                return new CountCheckerDto
                {
                    MinimumCountReached = false
                };
            }
        }

        public void Buy(int id, BuyProductDto dto)
        {
            var product = GetProduct(id);
            product.Count += dto.BoughtCount;
            CreateBuyFactor(dto.BoughtCount, product.Id);

            _unitOfWork.Commit();
        }

        public void Update(int id, UpdateProductDto dto)
        {
            var product = GetProduct(id);

            CheckIfNameIsDuplicate(product.CategoryId, dto.Name ,product.Id);

            product.Name = dto.Name;
            product.MinimumCount = dto.MinimumCount;
            product.Count = dto.Count;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;

            _unitOfWork.Commit();
        }

        private Product GetProduct(int id)
        {
            var product = _repository.GetById(id);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            return product;
        }

        private void CreateSellFactor(int soldCount,int productId)
        {
            var sellFactor = new SellFactor
            {
                ProductId = productId,
                Count = soldCount,
                DateSold = DateTime.Now.Date,
            };

            _sellFactorRepository.Add(sellFactor);
        }

        private void CreateBuyFactor(int boughtCount, int productId)
        {
            var buyFactor = new BuyFactor
            {
                ProductId = productId,
                Count = boughtCount,
                DateBought = DateTime.Now.Date,
            };

            _buyFactorRepository.Add(buyFactor);
        }

        private void CheckIfNameIsDuplicate(int categoryId, string productName
            ,int ignoreId = 0)
        {
            var checkName = _repository.CheckName(categoryId, productName
                ,ignoreId);

            if (checkName)
            {
                throw new DuplicateProductNameInSameCategoryException();
            }
        }

        private static void CheckIfProductCountIsEnough(int productCount, int soldCount)
        {
            if (productCount - soldCount < 0)
            {
                throw new NotEnoughProductException();
            }
        }


    }
}
