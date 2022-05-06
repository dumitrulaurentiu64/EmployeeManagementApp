using EmpAPI.Dtos;
using EmpAPI.Repository;
using Microsoft.AspNetCore.Mvc;

namespace EmpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ConfigController : Controller
    {
        private readonly ITaxConfigRepository _taxConfigRepository;

        public ConfigController(ITaxConfigRepository _taxConfigRepository)
        {
            this._taxConfigRepository = _taxConfigRepository;
        }

        [HttpGet]
        public JsonResult Get()
        {
            TaxDto taxDto = _taxConfigRepository.Get();
            return new JsonResult(taxDto);
        }

        [HttpPut]
        public JsonResult Put(TaxDto taxDto)
        {
            _taxConfigRepository.UpdateTaxes(taxDto);
            return new JsonResult(taxDto);
        }
    }
}
