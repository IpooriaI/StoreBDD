using FluentAssertions;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.BuyFactors;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Persistence.EF.SellFactors;
using StoreBDD.Services.BuyFactors;
using StoreBDD.Services.BuyFactors.Contracts;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
using System;
using Xunit;

namespace StoreBDD.Services.Test.Unit.BuyFactors
{
    public class BuyFactorServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly BuyFactorService _sut;
        private readonly ProductService _productSut;

        public BuyFactorServiceTests()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            var _repository = new EFBuyFactorRepository(_dataContext);
            var _productRepository = new EFProductRepository(_dataContext);
            var _sellFactorRepository = new EFSellFactorRepository(_dataContext);
            _sut = new BuyFactorAppService(_repository, _unitOfWork);
            _productSut = new ProductAppService(_productRepository, _unitOfWork
                , _sellFactorRepository, _repository);
        }

        [Fact]
        public void GetAll_Returns_all_GetBuyFactorDto_properly()
        {
            var category = CategoryFactory.GenerateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory
                .GenerateProduct("ماست کاله", category.Id);
            _dataContext.Manipulate(_ => _.Products.Add(product));
            var dto = ProductFactory.GenerateBuyProductDto(2);
            _productSut.Buy(product.Id, dto);

            var expected = _sut.GetAll();

            expected.Should().Contain(_ => _.ProductId == product.Id);
            expected.Should().Contain(_ => _.DateBought == DateTime.Now.Date);
            expected.Should().Contain(_ => _.Count == dto.BoughtCount);
        }
    }
}
