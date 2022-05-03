using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Categories;
using StoreBDD.Services.Categories;
using StoreBDD.Services.Categories.Contracts;
using StoreBDD.Services.Categories.Exceptions;
using StoreBDD.Specs.Infrastructure;
using StoreBDD.Test.Tools.Categories;
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
    public class AddCategoryWithDuplicateTitle : EFDataContextDatabaseFixture
    {
        private readonly CategoryService _sut;
        private readonly EFDataContext _dataContext;
        private Category _category;
        private AddCategoryDto _dto;
        Action expected;
        public AddCategoryWithDuplicateTitle(ConfigurationFixture
            configuration) : base(configuration)
        {
            _dataContext = CreateDataContext();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            var _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Given("دسته بندی با عنوان 'لبنیات'در فهرست دسته بندی کالا وجود دارد")]
        public void Given()
        {
            _category = CategoryFactory.GenerateCategory("لبنیات");

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [When("دسته بندی با عنوان 'لبنیات' تعریف میکنم")]
        public void When()
        {
            _dto = CategoryFactory.GenerateAddCategoryDto("لبنیات");

            expected = () => _sut.Add(_dto);
        }

        [Then("تنها یک دسته بندی با عنوان ' لبنیات' باید در فهرست دسته بندی کالا وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories.Count().Should().Be(1);
            _dataContext.Categories
                .Should().Contain(_ => _.Title == _dto.Title);
        }

        [And(": خطایی با عنوان 'عنوان دسته بندی کالا تکراریست ' باید رخ دهد")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<DuplicateCategoryTitleException>();
        }

        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
            ThenAnd();
        }

    }
}
