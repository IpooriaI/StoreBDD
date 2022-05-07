using Microsoft.AspNetCore.Mvc;
using StoreBDD.Services.SellFactors.Contracts;
using System.Collections.Generic;

namespace StoreBDD.RestAPI.Controllers
{
    [Route("api/sell-factors")]
    [ApiController]
    public class SellFactorsController : Controller
    {
        private readonly SellFactorService _service;

        public SellFactorsController(SellFactorService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<GetSellFactorDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public GetSellFactorDto Get(int id)
        {
            return _service.Get(id);
        }

        [HttpGet("profit")]
        public GetProfitDto GetProfit()
        {
            return _service.GetProfit();
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }

        [HttpPut("{id}")]
        public void Update(int id, UpdateSellFactorDto dto)
        {
            _service.Update(id, dto);
        }
    }
}
