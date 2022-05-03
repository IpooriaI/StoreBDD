using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.BuyFactors;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Persistence.EF.SellFactors;
using StoreBDD.Services.BuyFactors.Contracts;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Services.SellFactors;
using StoreBDD.Services.SellFactors.Contracts;
using StoreBDD.Specs.Infrastructure;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using static StoreBDD.Specs.BDDHelper;

namespace StoreBDD.Specs.SellFactors
{
    [Scenario("مدیریت کالا")]
    [Feature("",
       AsA = "فروشنده ",
       IWantTo = " کالا های خود را مدیریت کنم",
       InOrderTo = "کالا های خود را بفروشم"
       )]
    public class GetSellFactor : EFDataContextDatabaseFixture
    {
        private readonly EFDataContext _dataContext;
        private readonly SellFactorService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly SellFactorRepository _repository;
        private readonly BuyFactorRepository _buyFactorRepository;
        private readonly ProductRepository _productRepository;
        private readonly ProductService _productSut;
        private Category _category;
        private Product _product;
        private SellProductDto _dto;
        private List<GetSellFactorDto> _expected;

        public GetSellFactor(ConfigurationFixture configuration) : base(
            configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFSellFactorRepository(_dataContext);
            _productRepository = new EFProductRepository(_dataContext);
            _buyFactorRepository = new EFBuyFactorRepository(_dataContext);
            _sut = new SellFactorAppService(_repository,_unitOfWork);
            _productSut = new ProductAppService(_productRepository,_unitOfWork,
                _repository,_buyFactorRepository);
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

        [And("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' در دسته بندی 'لبنیات' 2 عدد ان را میفروشیم")]
        public void GivenSecondAnd()
        {
            _dto = ProductFactory.GenerateSellProductDto(2);
            _productSut.Sell(_product.Id, _dto);
        }

        [When("تاریخچه فروش را مشاهده میکنیم")]
        public void When()
        {
             _expected = _sut.GetAll();
        }

        [Then("تاریخچه فروشی با عنوان 'ماست کاله' و تعداد '2' و تاریخ 'امروز' باید وجود داشته باشد")]
        public void Then()
        {
            _expected.Should().Contain(_ => _.ProductId == _product.Id);
            _expected.Should().Contain(_ => _.DateSold == DateTime.Now.Date);
            _expected.Should().Contain(_ => _.Count == _dto.SoldCount);
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
