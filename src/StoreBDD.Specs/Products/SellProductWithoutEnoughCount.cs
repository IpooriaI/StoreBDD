using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.BuyFactors;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Persistence.EF.SellFactors;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Services.Products.Exceptions;
using StoreBDD.Specs.Infrastructure;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
using System;
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
    public class SellProductWithoutEnoughCount : EFDataContextDatabaseFixture
    {
        private readonly ProductService _sut;
        private readonly EFDataContext _dataContext;
        private SellProductDto _dto;
        private Category _category;
        private Product _product;
        private Action expected;
        private int _count;

        public SellProductWithoutEnoughCount(ConfigurationFixture
            configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            var _repository = new EFProductRepository(_dataContext);
            var _sellRepository = new EFSellFactorRepository(_dataContext);
            var _buyRepository = new EFBuyFactorRepository(_dataContext);
            _sut = new ProductAppService(_repository,
                _unitOfWork, _sellRepository, _buyRepository);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.GenerateCategory("لبنیات");

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '1' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _product = ProductFactory
                .GenerateProduct("ماست کاله", _category.Id, 1);
            _count = _product.Count;
            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }

        [When("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '1' در دسته بندی 'لبنیات' 2 عدد ان را میفروشیم")]
        public void When()
        {

            _dto = new SellProductDto
            {
                SoldCount = 22
            };

            expected = () => _sut.Sell(_product.Id, _dto);

        }

        [Then("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '1' در دسته بندی 'لبنیات' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Products.Should()
                .Contain(_ => _.Count == _count);
        }

        [And("خطایی با عنوان 'کالا به اندازه کافی موجود نیست' باید ارسال شود")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<NotEnoughProductException>();
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
