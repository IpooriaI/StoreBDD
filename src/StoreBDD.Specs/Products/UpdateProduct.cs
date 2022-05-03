using FluentAssertions;
using StoreBDD.Entities;
using StoreBDD.Infrastructure.Application;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Services.Products;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Specs.Infrastructure;
using StoreBDD.Test.Tools.Categories;
using StoreBDD.Test.Tools.Products;
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
    public class UpdateProduct : EFDataContextDatabaseFixture
    {
        private readonly ProductService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _repository;
        private readonly EFDataContext _dataContext;
        private UpdateProductDto _dto;
        private Category _category;
        private Product _product;

        public UpdateProduct(ConfigurationFixture configuration) : base(
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
            _category = CategoryFactory.GenerateCategory("لبنیات");

            _dataContext.Manipulate(_ => _.Categories.Add(_category));
        }

        [And("کالایی با عنوان 'ماست کالکه' و قیمت '5000' و تعداد '20' و حداقل موجودی '5' در دسته بندی 'لبنیات' وجود دارد")]
        public void GivenAnd()
        {
            _product = ProductFactory
                .GenerateProduct("ماست کالکه", _category.Id); 
            _dataContext.Manipulate(_ => _.Products.Add(_product));
        }

        [When("کالایی با عنوان 'ماست کالکه' و قیمت '5000' و تعداد '20' در دسته بندی 'لبنیات' را به 'ماست کاله' و قیمت'4500' ویرایش میکنم")]
        public void When()
        {
            _dto = ProductFactory
                .GenerateUpdateProductDto("ماست کاله", _product.CategoryId); 

            _sut.Update(_product.Id,_dto);
        }

        [Then("کالایی با عنوان 'ماست کاله' و قیمت '4500' و تعداد '20' و حداقل موجودی '5' در دسته بندی 'لبنیات' باید وجود داشته باشد")]
        public void Then()
        {
            _dataContext.Products.Count().Should().Be(1);
            _dataContext.Products
                .Should().Contain(_ => _.Name == _dto.Name);
            _dataContext.Products
                .Should().Contain(_ => _.CategoryId == _dto.CategoryId);
            _dataContext.Products
                .Should().Contain(_ => _.Price == _dto.Price);
            _dataContext.Products
                .Should().Contain(_ => _.MinimumCount == _dto.MinimumCount);

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
