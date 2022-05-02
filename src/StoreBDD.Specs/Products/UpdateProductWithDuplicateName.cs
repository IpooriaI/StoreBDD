﻿using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Services.Products.Exceptions;
using StoreBDD.Specs.Infrastructure;
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
    public class UpdateProductWithDuplicateName : EFDataContextDatabaseFixture
    {
        private readonly ProductService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _repository;
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

        [And("کالایی با عنوان 'ماست کالکه' و قیمت '5000' و تعداد '20' و حداقل موجودی '5' در دسته بندی 'لبنیات' وجود دارد")]
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

        [And("کالایی با عنوان 'ماست شیرازی' و قیمت '6000' و تعداد '40' و حداقل موجودی '10' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenSecondAnd()
        {
            _secondProduct = new Product
            {
                Name = "ماست شیرازی",
                Price = 6000,
                Count = 40,
                MinimumCount = 10,
                CategoryId = _category.Id,
            };
            _dataContext.Manipulate(_ => _.Products.Add(_secondProduct));
        }

        [When("کالایی با عنوان 'ماست شیرازی' و قیمت '6000' و تعداد '40' در دسته بندی 'لبنیات' را به 'ماست کاله' و قیمت'4500' ویرایش میکنیم")]
        public void When()
        {
            _dto = new UpdateProductDto
            {
                Name = _product.Name,
                MinimumCount = _product.MinimumCount,
                Price = 4500,
                CategoryId = _product.CategoryId,
                Count = _product.Count,
            };

            expected =()=> _sut.Update(_secondProduct.Id, _dto);
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
