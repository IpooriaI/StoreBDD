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
    public class UpdateCategoryWithDuplicateTitle : EFDataContextDatabaseFixture
    {
        private readonly CategoryService _sut;
        private readonly EFDataContext _dataContext;
        private UpdateCategoryDto _dto;
        private Category _category;
        private Category _secondCategory;
        Action expected;
        public UpdateCategoryWithDuplicateTitle(ConfigurationFixture
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

        [Given("دسته بندی با عنوان 'خشکبار'در فهرست دسته بندی کالا وجود دارد")]
        public void GivenAnd()
        {
            _secondCategory = CategoryFactory.GenerateCategory("خشکبار");

            _dataContext.Manipulate(_ => _.Categories.Add(_secondCategory));
        }

        [When("دسته بندی با عنوان 'لبنیات' را به 'خشکبار'ویرایش میکنم")]
        public void When()
        {
            _dto = CategoryFactory.GenerateUpdateCategoryDto("خشکبار");

            expected = () => _sut.Update(_category.Id, _dto);
        }


        [Then("یک دسته بندی با عنوان 'لبنیات' باید در فهرست دسته بندی ها وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories
                .Should().Contain(_ => _.Title == _secondCategory.Title);
        }

        [And("خطایی با عنوان 'عنوان دسته بندی تکراری است' باید ارسال شود")]
        public void ThenAnd()
        {
            expected.Should().ThrowExactly<DuplicateCategoryTitleException>();
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
