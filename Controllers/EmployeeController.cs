using EmpAPI.Helpers;
using EmpAPI.Models;
using EmpAPI.Repository;
using EmpAPI.Services;
using Microsoft.AspNetCore.Mvc;


namespace EmpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IEmailService _emailService;

        public EmployeeController(IWebHostEnvironment env, IEmployeeRepository employeeRepository, IEmailService emailService)
        {
            _env = env;
            _employeeRepository = employeeRepository;
            _emailService = emailService;

        }

        [HttpGet]
        public JsonResult Get()
        {
            List<Employee> table = _employeeRepository.GetAll();
            return new JsonResult(table);
        }

        [HttpGet("{id}")]
        public JsonResult Find(int id)
        {
            Employee emp = _employeeRepository.Find(id);
            return new JsonResult(emp);
        }

        [HttpPost("{email}")]
        public JsonResult Post(Employee emp, string email)
        {
            emp = _employeeRepository.Insert(emp);
            _emailService.CreateAccount(email, emp.Firstname, (int)emp.EmployeeId);
            return new JsonResult(emp);
        }

        [HttpPut]
        public JsonResult Put(Employee emp)
        { 
            _employeeRepository.Update(emp);
            return new JsonResult(emp);
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            _employeeRepository.Delete(id);
            return new JsonResult("Deteleted Succesfully");
        }

        [Route("SaveFile")]
        [HttpPost]
        public JsonResult SaveFile()
        {
            try
            {
                var httpRequest = Request.Form;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = _env.ContentRootPath + "/Photos/" + filename;


                using(var stream = new FileStream(physicalPath, FileMode.Create))
                {
                    postedFile.CopyTo(stream);
                }

                return new JsonResult(filename);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
