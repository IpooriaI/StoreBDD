using Microsoft.AspNetCore.Mvc;
using StoreBDD.Services.Products.Contracts;

namespace StoreBDD.RestAPI.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsController : Controller
    {
        private readonly ProductService _service;

        public ProductsController(ProductService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddProductDto dto)
        {
            _service.Add(dto);
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }

        [HttpPut("{id}/update")]
        public void Update(int id, UpdateProductDto dto)
        {
            _service.Update(id, dto);
        }

        [HttpPut("{id}/sell")]
        public CountCheckerDto Sell(int id, SellProductDto dto)
        {
            return _service.Sell(id, dto);
        }

        [HttpPut("{id}/buy")]
        public void Buy(int id, BuyProductDto dto)
        {
            _service.Buy(id, dto);
        }
    }
}
