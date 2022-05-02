using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Categories;
using StoreBDD.Services.Categories;
using StoreBDD.Services.Categories.Contracts;
using StoreBDD.Services.Categories.Exceptions;
using StoreBDD.Specs.Infrastructure;
using System;
using System.Linq;
using Xunit;
using static StoreBDD.Specs.BDDHelper;

namespace StoreBDD.Specs.Categories
{
    [Scenario("مدیریت دسته بندی")]
    [Feature("",
        AsA = "فروشنده ",
        IWantTo = "دسته بندی کالا مدیریت کنم",
        InOrderTo = "کالا های خود را دسته بندی کنم"
        )]
    public class DeleteCategoryWithProductsInside : EFDataContextDatabaseFixture
    {
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _repository;
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;
        Action expected;
        public DeleteCategoryWithProductsInside(ConfigurationFixture
            configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = new Category
            {
                Title = "لبنیات"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }
        [And("کالایی با عنوان 'ماست کاله'و قیمت'5000' و تعداد '5' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _product = new Product
            {
                Name = "ماست کاله",
                CategoryId = _category.Id,
                Count = 5,
                Price = 5000,
                MinimumCount = 3,
            };

            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }

        [When("دسته بندی با عنوان 'لبنیات' را حذف میکنیم")]
        public void When()
        {
            expected = () => _sut.Delete(_category.Id);
        }

        [Then("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالا باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Count().Should().Be(1);
            _dataContext.Categories.Should().Contain(_ => _.Id == _category.Id);
        }

        [And("خطایی با عنوان 'کالا در این دسته بندی وجود دارد' باید ارسال شود")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<CategoryHasProductsException>();
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
