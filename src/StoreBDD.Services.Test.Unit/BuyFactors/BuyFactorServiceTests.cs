using FluentAssertions;
using StoreBDD.Infrastructure.Test;
using StoreBDD.Persistence.EF;
using StoreBDD.Persistence.EF.BuyFactors;
using StoreBDD.Services.BuyFactors;
using StoreBDD.Services.BuyFactors.Contracts;
using StoreBDD.Services.Products.Contracts;
using StoreBDD.Test.Tools.BuyFactors;
using System;
using Xunit;

namespace StoreBDD.Services.Test.Unit.BuyFactors
{
    public class BuyFactorServiceTests
    {
        private readonly EFDataContext _dataContext;
        private readonly BuyFactorService _sut;
        private readonly ProductService _productSut;

        public BuyFactorServiceTests()
        {
            _dataContext = new EFInMemoryDatabase()
                .CreateDataContext<EFDataContext>();
            var _unitOfWork = new EFUnitOfWork(_dataContext);
            var _repository = new EFBuyFactorRepository(_dataContext);
            _sut = new BuyFactorAppService(_repository, _unitOfWork);
        }

        [Fact]
        public void GetAll_Returns_all_GetBuyFactorDto_properly()
        {
            var buyFactor = BuyFactorFactory.GenerateBuyFactor();
            _dataContext.Manipulate(_ => _.BuyFactors.Add(buyFactor));

            var expected = _sut.GetAll();

            expected.Should().Contain(_ => _.ProductId == buyFactor.ProductId);
            expected.Should().Contain(_ => _.DateBought == DateTime.Now.Date);
            expected.Should().Contain(_ => _.Count == buyFactor.Count);
        }
    }
}
