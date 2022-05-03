using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.BuyFactors;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Persistence.EF.SellFactors;
using StoreBDD.Services.BuyFactors;
using StoreBDD.Services.BuyFactors.Contracts;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Specs.Infrastructure;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
using System;
using System.Collections.Generic;
using Xunit;
using static StoreBDD.Specs.BDDHelper;

namespace StoreBDD.Specs.BuyFactors
{
    [Scenario("مدیریت کالا")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " کالا های خود را مدیریت کنم",
       InOrderTo = "کالا های خود را بفروشم"
       )]
    public class GetBuyFactor : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly BuyFactorService _sut;
        private readonly ProductService _productSut;
        private Category _category;
        private Product _product;
        private BuyProductDto _dto;
        private List<GetBuyFactorDto> _expected;

        public GetBuyFactor(ConfigurationFixture configuration) : base(
            configuration)
        {
            _dataContext = CreateDataContext();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            var _repository = new EFBuyFactorRepository(_dataContext);
            var _productRepository = new EFProductRepository(_dataContext);
            var _sellFactorRepository = new EFSellFactorRepository(_dataContext);
            _sut = new BuyFactorAppService(_repository, _unitOfWork);
            _productSut = new ProductAppService(_productRepository, _unitOfWork
                , _sellFactorRepository, _repository);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.GenerateCategory("لبنیات");
            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _product = ProductFactory
                .GenerateProduct("ماست کاله", _category.Id);
            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }

        [And("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' در دسته بندی 'لبنیات' 2 عدد به ان اضافه میکنیم")]
        public void GivenSecondAnd()
        {
            _dto = ProductFactory.GenerateBuyProductDto(2);
            _productSut.Buy(_product.Id, _dto);
        }

        [When("تاریخچه خرید را مشاهده میکنیم")]
        public void When()
        {
            _expected = _sut.GetAll();
        }

        [Then("تاریخچه خریدی با عنوان 'ماست کاله' و تعداد '2' و تاریخ 'امروز' باید وجود داشته باشد")]
        public void Then()
        {
            _expected.Should().Contain(_ => _.ProductId == _product.Id);
            _expected.Should().Contain(_ => _.DateBought == DateTime.Now.Date);
            _expected.Should().Contain(_ => _.Count == _dto.BoughtCount);
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            GivenSecondAnd();
            When();
            Then();
        }
    }
}
