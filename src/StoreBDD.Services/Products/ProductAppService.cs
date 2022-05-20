using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.BuyFactors.Contracts;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Services.Products.Exceptions;
using StoreBDD.Services.SellFactors.Contracts;
using System;
using System.Threading.Tasks;

namespace StoreBDD.Services.Products
{
    public class ProductAppService : ProductService
    {
        private readonly ProductRepository _repository;
        private readonly SellFactorRepository _sellFactorRepository;
        private readonly BuyFactorRepository _buyFactorRepository;
        private readonly UnitOfWork _unitOfWork;

        public ProductAppService(ProductRepository repository
            , UnitOfWork unitOfWork, SellFactorRepository sellFactorRepository,
            BuyFactorRepository buyFactorRepository)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _sellFactorRepository = sellFactorRepository;
            _buyFactorRepository = buyFactorRepository;
        }

        public async Task Add(AddProductDto dto)
        {
            var product = new Product
            {
                Id = dto.Id,
                Name = dto.Name,
                MinimumCount = dto.MinimumCount,
                Count = 0,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
            };

            CheckIfNameIsDuplicate(product.CategoryId, product.Name);
            CheckIfIdIsDuplicate(product.Id);
            CheckIfCategoryExists(product.CategoryId);

            await _repository.Add(product);
            await _unitOfWork.Commit();
        }

        public async Task Delete(int id)
        {
            var product = await GetProduct(id);

            await _repository.Delete(product);
            await _unitOfWork.Commit();
        }

        public async Task<GetProductDto> Get(int id)
        {
            return await _repository.Get(id);
        }

        public async Task<UpdateResponseDto> Sell(int id, SellProductDto dto)
        {
            var product = await GetProduct(id);

            await CheckIfProductCountIsEnough(product.Count, dto.SoldCount);
            product.Count -= dto.SoldCount;
            await CreateSellFactor(dto.SoldCount, product.Id);
            var updateResponse =
                CreateUpdateResponse(product.Count, product.MinimumCount);

            await _unitOfWork.Commit();
            return updateResponse;
        }

        public async Task Buy(int id, BuyProductDto dto)
        {
            var product = await GetProduct(id);
            product.Count += dto.BoughtCount;
            CreateBuyFactor(dto.BoughtCount, product.Id);

            await _unitOfWork.Commit();
        }

        public async Task Update(int id, UpdateProductDto dto)
        {
            var product = await GetProduct(id);

            await CheckIfNameIsDuplicate(product.CategoryId, dto.Name, product.Id);
            CheckIfCategoryExists(dto.CategoryId);

            product.Name = dto.Name;
            product.MinimumCount = dto.MinimumCount;
            product.Count = dto.Count;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;

            await _unitOfWork.Commit();
        }

        private async Task<Product> GetProduct(int id)
        {
            var product = await _repository.GetById(id);

            if (product == null)
            {
                throw new ProductNotFoundException();
            }

            return product;
        }

        private async Task CreateSellFactor(int soldCount, int productId)
        {
            var sellFactor = new SellFactor
            {
                ProductId = productId,
                Count = soldCount,
                DateSold = DateTime.Now.Date,
            };

            await _sellFactorRepository.Add(sellFactor);
        }

        private async void CreateBuyFactor(int boughtCount, int productId)
        {
            var buyFactor = new BuyFactor
            {
                ProductId = productId,
                Count = boughtCount,
                DateBought = DateTime.Now.Date,
            };

            await _buyFactorRepository.Add(buyFactor);
        }

        private async Task CheckIfNameIsDuplicate(int categoryId, 
            string productName, int ignoreId = 0)
        {
            if (await _repository.CheckName(categoryId, productName, ignoreId))
            {
                throw new DuplicateProductNameInSameCategoryException();
            }
        }

        private async void CheckIfIdIsDuplicate(int productId)
        {
            if (await _repository.CheckId(productId))
            {
                throw new DuplicateProductIdException();
            }
        }

        private static void CheckIfProductCountIsEnough(int productCount, int soldCount)
        {
            if (productCount - soldCount < 0)
            {
                throw new NotEnoughProductException();
            }
        }

        private async void CheckIfCategoryExists(int categoryId)
        {
            var checkCategory = await _repository.CheckCategory(categoryId);

            if (!checkCategory)
            {
                throw new CategoryNotFoundException();
            }
        }

        private static UpdateResponseDto CreateUpdateResponse(int count
            , int minimumCount)
        {
            var response = new UpdateResponseDto
            {
                MinimumCountReached = false,
                RemainingSupply = count
            };

            if (count <= minimumCount)
            {
                response.MinimumCountReached = true;
            }

            return response;
        }

    }
}
