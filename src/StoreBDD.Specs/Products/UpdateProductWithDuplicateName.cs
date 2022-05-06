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
    public class UpdateProductWithDuplicateName : EFDataContextDatabaseFixture
    {
        private readonly ProductService _sut;
        private readonly EFDataContext _dataContext;
        private UpdateProductDto _dto;
        private Category _category;
        private Product _product;
        private Product _secondProduct;
        private Action expected;

        public UpdateProductWithDuplicateName
            (ConfigurationFixture configuration) : base(configuration)
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

        [And("کالایی با عنوان 'ماست کالکه' و قیمت '5000' و تعداد '20' و حداقل موجودی '5' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _product = ProductFactory
                .GenerateProduct("ماست کاله", _category.Id,3);

            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }

        [And("کالایی با عنوان 'ماست شیرازی' و قیمت '6000' و تعداد '40' و حداقل موجودی '10' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenSecondAnd()
        {
            _secondProduct = ProductFactory
                .GenerateProduct("ماست شیرازی", _category.Id,4);

            _dataContext.Manipulate(_ => _.Products.Add(_secondProduct));
        }

        [When("کالایی با عنوان 'ماست شیرازی' و قیمت '6000' و تعداد '40' در دسته بندی 'لبنیات' را به 'ماست کاله' و قیمت'4500' ویرایش میکنیم")]
        public void When()
        {
            _dto = ProductFactory
                .GenerateUpdateProductDto(_product.Name, _product.CategoryId);

            expected = () => _sut.Update(_secondProduct.Id, _dto);
        }

        [Then("کالایی با عنوان 'ماست شیرازی' و قیمت '6000' و تعداد '40' و حداقل موجودی '10' در دسته بندی 'لبنیات' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Products.Should()
                .Contain(_ => _.Name == _secondProduct.Name);
            _dataContext.Products.Should()
                .Contain(_ => _.CategoryId == _secondProduct.CategoryId);
            _dataContext.Products.Should()
                .Contain(_ => _.Price == _secondProduct.Price);
            _dataContext.Products.Should()
                .Contain(_ => _.MinimumCount == _secondProduct.MinimumCount);

        }

        [And("خطایی با عنوان 'کالایی با این عنوان در این دسته بندی وجود دارد' باید ارسال شود")]
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
            GivenSecondAnd();
            When();
            Then();
            ThenAnd();
        }
    }
}
