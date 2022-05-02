﻿using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Categories;
using StoreBDD.Services.Categories;
using StoreBDD.Services.Categories.Contracts;
using StoreBDD.Specs.Infrastructure;
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
    public class UpdateCategory : EFDataContextDatabaseFixture
    {
        private readonly CategoryService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryRepository _repository;
        private readonly EFDataContext _dataContext;
        private UpdateCategoryDto _dto;
        private Category _category;
        public UpdateCategory(ConfigurationFixture configuration) : base(
            configuration)
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

        [When("دسته بندی با عنوان 'لبنیات' را به 'خشکبار'ویرایش میکنم")]
        public void When()
        {
            _dto = new UpdateCategoryDto
            {
                Title = "خشکبار"
            };

            _sut.Update(_category.Id, _dto);
        }


        [Then("یک دسته بندی با عنوان 'خشکبار' باید در فهرست دسته بندی ها وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Categories
                .Should().Contain(_ => _.Title == _dto.Title);
        }


        [Fact]
        public void Run()
        {
            Given();
            When();
            Then();
        }
    }
}
