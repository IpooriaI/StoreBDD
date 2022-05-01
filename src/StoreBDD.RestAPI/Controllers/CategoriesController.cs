using Microsoft.AspNetCore.Mvc;
using StoreBDD.Services.Categories.Contracts;
using System.Collections.Generic;

namespace StoreBDD.RestAPI.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private readonly CategoryService _service;

        public CategoriesController(CategoryService service)
        {
            _service = service;
        }

        [HttpPost]
        public void Add(AddCategoryDto dto)
        {
            _service.Add(dto);
        }

        [HttpGet]
        public List<GetCategoryDto> GetAll()
        {
            return _service.GetAll();
        }

        [HttpGet("{id}")]
        public GetCategoryDto Get(int id)
        {
            return _service.Get(id);
        }

        [HttpPut("{id}")]
        public void Update(int id,UpdateCategoryDto dto)
        {
            _service.Update(id, dto);
        }
        
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            _service.Delete(id);
        }
    }
}
