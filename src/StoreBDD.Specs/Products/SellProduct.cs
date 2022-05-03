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
using StoreBDD.Services.SellFactors.Contracts;
using StoreBDD.Specs.Infrastructure;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
using System;
using System.Linq;
using Xunit;
using static StoreBDD.Specs.BDDHelper;

namespace StoreBDD.Specs.Products
{
    [Scenario("مدیریت کالا")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا های خود را مدیریت کنم",
        InOrderTo = "کالا های خود را بفروشم"
        )]
    public class SellProduct : EFDataContextDatabaseFixture
    {
        private readonly ProductService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _repository;
        private readonly SellFactorRepository _sellRepository;
        private readonly BuyFactorRepository _buyRepository;
        private readonly EFDataContext _dataContext;
        private SellProductDto _dto;
        private Category _category;
        private Product _product;
        private int _count;

        public SellProduct(ConfigurationFixture configuration) : base(
            configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sellRepository = new EFSellFactorRepository(_dataContext);
            _buyRepository = new EFBuyFactorRepository(_dataContext);
            _sut = new ProductAppService(_repository,
                _unitOfWork, _sellRepository, _buyRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.GenerateCategory("لبنیات");

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' و حداقل موجودی '5' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _product = ProductFactory
                .GenerateProduct("ماست کاله", _category.Id);
            _count = _product.Count;
            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }

        [When("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' در دسته بندی 'لبنیات' 2 عدد ان را میفروشیم")]
        public void When()
        {

            _dto = new SellProductDto
            {
                SoldCount = 2
            };

            _sut.Sell(_product.Id,_dto);

        }

        [Then("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '18' در دسته بندی 'لبنیات' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Products.Should()
                .Contain(_ => _.Count == _count-_dto.SoldCount);
        }

        [And("فاکتور فروشی با عنوان 'ماست کاله' و تعداد '2' و تاریخ 'امروز' باید وجود داشته باشد")]
        public void ThenAnd()
        {
            _dataContext.SellFactors.Should().HaveCount(1);
            _dataContext.SellFactors.Should()
                .Contain(_ => _.DateSold == DateTime.Now.Date);            
            _dataContext.SellFactors.Should()
                .Contain(_ => _.Count == _dto.SoldCount);
        }


        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            When();
            Then();
            ThenAnd();
        }
    }
}
