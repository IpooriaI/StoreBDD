﻿using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Specs.Infrastructure;
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
    public class DeleteProduct : EFDataContextDatabaseFixture
    {
        private readonly ProductService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _repository;
        private readonly EFDataContext _dataContext;
        private Category _category;
        private Product _product;

        public DeleteProduct(ConfigurationFixture configuration) : base(
            configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_repository, _unitOfWork);
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

        [And("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' و حداقل موجودی '5' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _product = new Product
            {
                Name = "ماست کاله",
                Price = 5000,
                Count = 20,
                MinimumCount = 5,
                CategoryId = _category.Id,
            };
            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }

        [When("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '20' و حداقل موجودی '5' از دسته بندی 'لبنیات' حذف میکنیم")]
        public void When()
        {
            _sut.Delete(_product.Id);
        }

        [Then("کالایی با عنوان 'ماست کاله' و قیمت '5000' و تعداد '0' در دسته بندی 'لبنیات' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Products.Count().Should().Be(0);
            _dataContext.Products
                .Should().NotContain(_ => _.Name == _product.Name);
        }

        [Fact]
        public void Run()
        {
            Given();
            GivenAnd();
            When();
            Then();
        }
    }
}