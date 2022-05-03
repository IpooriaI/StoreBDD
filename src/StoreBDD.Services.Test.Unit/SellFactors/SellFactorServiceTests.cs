using FluentAssertions;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.BuyFactors;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Persistence.EF.SellFactors;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Services.SellFactors;
using StoreBDD.Services.SellFactors.Contracts;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
using System;
using Xunit;

namespace StoreBDD.Services.Test.Unit.SellFactors
{
    public class SellFactorServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly SellFactorService _sut;
        private readonly ProductService _productSut;

        public SellFactorServiceTests()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            var _repository = new EFSellFactorRepository(_dataContext);
            var _productRepository = new EFProductRepository(_dataContext);
            var _buyFactorRepository = new EFBuyFactorRepository(_dataContext);
            _sut = new SellFactorAppService(_repository, _unitOfWork);
            _productSut = new ProductAppService(_productRepository, _unitOfWork,
                _repository, _buyFactorRepository);
        }

        [Fact]
        public void GetAll_Returns_all_GetSellFactorDto_properly()
        {
            var category = CategoryFactory.GenerateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory
                .GenerateProduct("ماست کاله", category.Id);
            _dataContext.Manipulate(_ => _.Products.Add(product));
            var dto = ProductFactory.GenerateSellProductDto(2);
            _productSut.Sell(product.Id, dto);

            var expected = _sut.GetAll();

            expected.Should().Contain(_ => _.ProductId == product.Id);
            expected.Should().Contain(_ => _.DateSold == DateTime.Now.Date);
            expected.Should().Contain(_ => _.Count == dto.SoldCount);
        }
    }
}
