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
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Persistence.EF.Products;
using StoreBDD.Services.Products;

namespace StoreBDD.Specs.Products
{        
        [Scenario("مدیریت کالا")]
        [Feature("",
        AsA = "فروشنده ",
        IWantTo = " کالا های خود را مدیریت کنم",
        InOrderTo = "کالا های خود را بفروشم"
        )]
    public class AddProduct : EFDataContextDatabaseFixture
    {
        private readonly ProductService _sut;
        private readonly UnitOfWork _unitOfWork;
        private readonly ProductRepository _repository;
        private readonly EFDataContext _dataContext;
        private AddCategoryDto _dto;
        public AddProduct(ConfigurationFixture configuration) : base(
            configuration)
        {
            _dataContext = CreateDataContext();
            _unitOfWork = new EFUnitOfWork(_dataContext);
            _repository = new EFProductRepository(_dataContext);
            _sut = new ProductAppService(_repository, _unitOfWork);
        }



    }
}
