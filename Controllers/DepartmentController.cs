using EmpAPI.Models;
using EmpAPI.Repository;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Data.SqlClient;

namespace EmpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : ControllerBase
    {
        private readonly IDepartmentRepository _departmentRepository;

        public DepartmentController(IDepartmentRepository _departmentRepository)
        {
            this._departmentRepository = _departmentRepository;
        }

        [HttpGet]
        public JsonResult Get()
        {
            List<Department> table = _departmentRepository.GetAll();
            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(Department dep)
        {
            _departmentRepository.Insert(dep);
            return new JsonResult(dep);
        }

        [HttpPut]
        public JsonResult Put(Department dep)
        {
            _departmentRepository.Update(dep);
            return new JsonResult(dep);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            _departmentRepository.Delete(id);
            return new JsonResult("Deleted succesfully");
        }
    }
}
