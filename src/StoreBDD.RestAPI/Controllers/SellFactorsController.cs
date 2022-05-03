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

    }
}
