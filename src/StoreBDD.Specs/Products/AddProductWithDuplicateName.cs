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
using System.Linq;
using Xunit;
using static StoreBDD.Specs.BDDHelper;

namespace StoreBDD.Specs.Products
{
    public class AddProductWithDuplicateName : EFDataContextDatabaseFixture
    {
        private readonly ProductService _sut;
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        private AddProductDto _dto;
        Action expected;

        public AddProductWithDuplicateName(ConfigurationFixture
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

        [And("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' و حداقل موجودی '5' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _product = ProductFactory
                .GenerateProduct("ماست کاله", _category.Id, 3);

            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }

        [When("کالایی با عنوان 'ماست کاله' و قیمت '3000' و تعداد '43' به دسته بندی 'لبنیات' اضافه میکنیم")]
        public void When()
        {
            _dto = ProductFactory.GenerateAddProductDto("ماست کاله", _category.Id);


            expected = () => _sut.Add(_dto);
        }

        [Then("فقط کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' در دسته بندی 'لبنیات' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Products.Count().Should().Be(1);
            _dataContext.Products
                .Should().Contain(_ => _.Name == _product.Name);
            _dataContext.Products
                .Should().Contain(_ => _.CategoryId == _product.CategoryId);
            _dataContext.Products
                .Should().Contain(_ => _.Price == _product.Price);
            _dataContext.Products
                .Should().Contain(_ => _.MinimumCount == _product.MinimumCount);

        }

        [And("خطایی با عنوان 'کالایی با این نام در این دسته بندی وجود دارد' باید ارسال شود")]
        public void ThenAnd()
        {
            expected.Should()
                .ThrowExactly<DuplicateProductNameInSameCategoryException>();
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
