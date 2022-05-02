using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Services.Products.Exceptions;

namespace StoreBDD.Services.Products
{
    public class ProductAppService : ProductService
    {
        private readonly ProductRepository _repository;
        private readonly UnitOfWork _unitOfWork;

        public ProductAppService(ProductRepository repository
            , UnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
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
    }
}
