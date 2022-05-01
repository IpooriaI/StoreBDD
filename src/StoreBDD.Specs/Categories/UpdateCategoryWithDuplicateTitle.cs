using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Categories;
using StoreBDD.Specs.Infrastructure;
using FluentAssertions;
using Xunit;
using static StoreBDD.Specs.BDDHelper;
using StoreBDD.Services.Categories.Contracts;
using StoreBDD.Services.Categories;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Services.Categories.Exceptions;

namespace StoreBDD.Specs.Categories
{
    public class UpdateCategoryWithDuplicateTitle : EFDataContextDatabaseFixture
    {
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _repository;
        private readonly EFDataContext _dataContext;
        private UpdateCategoryDto _dto;
        private Category _category;
        private Category _secondCategory;
        Action expected;
        public UpdateCategoryWithDuplicateTitle(ConfigurationFixture 
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

        [Given("دسته بندی با عنوان 'خشکبار'در فهرست دسته بندی کالا وجود دارد")]
        public void GivenAnd()
        {
            _secondCategory = new Category
            {
                Title = "خشکبار"
            };

            _dataContext.Manipulate(_ => _.Categories.Add(_secondCategory));
        }

        [When("دسته بندی با عنوان 'لبنیات' را به 'خشکبار'ویرایش میکنم")]
        public void When()
        {
            _dto = new UpdateCategoryDto
            {
                Title = "خشکبار"
            };

            expected =()=> _sut.Update(_category.Id, _dto);
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
