﻿using FluentAssertions;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Services.Products.Exceptions;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
using System;
using Xunit;

namespace StoreBDD.Services.Test.Unit.Products
{
    public class ProductServiceTest
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductService _sut;
        private readonly ProductRepository _repository;
        public ProductServiceTest()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_Product_properly()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            AddProductDto dto = ProductFactory
                .GenerateAddProductDto("TestName", category.Id);

            _sut.Add(dto);

            _dataContext.Products.Should().HaveCount(1);
            _dataContext.Products.Should().Contain(_ => _.Price == dto.Price);
            _dataContext.Products.Should().Contain(_ => _.Name == dto.Name);
            _dataContext.Products.Should()
                .Contain(_ => _.CategoryId == dto.CategoryId);
            _dataContext.Products.Should()
                .Contain(_ => _.MinimumCount == dto.MinimumCount);
        }

        [Fact]
        public void Add_throws_exception_DuplicateProductNameInSameCategoryException_if_product_with_same_name_already_exists_in_this_category()
        {
            var category = CategoryFactory.GenerateCategory("DummyTitle");
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var product = ProductFactory.GenerateProduct("Test", category.Id);
            _dataContext.Manipulate(_ => _.Products.Add(product));
            AddProductDto dto = ProductFactory
                .GenerateAddProductDto(product.Name, category.Id);

            Action expected =()=>  _sut.Add(dto);

            expected.Should()
                .ThrowExactly<DuplicateProductNameInSameCategoryException>();
        }
    }
}
