using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Categories;
using StoreBDD.Services.Categories;
using StoreBDD.Services.Categories.Contracts;
using StoreBDD.Services.Categories.Exceptions;
using StoreBDD.Test.Tools.Categories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace StoreBDD.Services.Test.Unit.Categories
{
    public class CategoryServiceTests 
    {
        private readonly EFDataContext _dataContext;
        private readonly UnitOfWork _unitOfWork;
        private readonly CategoryService _sut;
        private readonly CategoryRepository _repository;

        public CategoryServiceTests()
        {
            _dataContext =
                new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFCategoryRepository(_dataContext);
            _sut = new CategoryAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void Add_adds_category_properly()
        {
            AddCategoryDto dto = CategoryFactory
                .GenerateAddCategoryDto("Dummy");

            _sut.Add(dto);

            _dataContext.Categories.Should().HaveCount(1);
            _dataContext.Categories.Should().Contain(_ => _.Title == dto.Title);
        }

        [Fact]
        public void Add_throws_DuplicateCategoryTitle_when_category_with_the_same_title_already_exists()
        {
            var category = new Category
            {
                Title = "DummyTitle"
            };
            _dataContext.Manipulate(_ => _.Categories.Add(category));
            var dto = CategoryFactory.GenerateAddCategoryDto(category.Title);

            Action expected =()=> _sut.Add(dto);

            expected.Should().ThrowExactly<DuplicateCategoryTitleException>();
        }

    }
}
