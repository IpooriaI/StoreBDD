using Microsoft.AspNetCore.Mvc;
using StoreBDD.Services.BuyFactors.Contracts;
using System.Collections.Generic;

namespace StoreBDD.RestAPI.Controllers
{
    [Route("api/buy-factors")]
    [ApiController]
    public class BuyFactorsController : Controller
    {
        private readonly BuyFactorService _service;

        public BuyFactorsController(BuyFactorService service)
        {
            _service = service;
        }

        [HttpGet]
        public List<GetBuyFactorDto> GetAll()
        {
            return _service.GetAll();
        }
    }
}
