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

        [Route("FirstStartup")]
        [HttpGet]
        public JsonResult FirstStartup()
        {
            List<Employee> table = _employeeRepository.GetAll();
            if (table.Count == 0)
                return new JsonResult(true);
            return new JsonResult(false);
        }

        [HttpGet("{id}")]
        public JsonResult Find(int id)
        {
            Employee emp = _employeeRepository.Find(id);
            return new JsonResult(emp);
        }

        [HttpPost("{email}/{role}")]
        public JsonResult Post(Employee emp, string email, string role)
        {
            emp = _employeeRepository.Insert(emp);
            _emailService.CreateAccount(email, emp.Firstname, (int)emp.EmployeeId, role);
            return new JsonResult(emp);
        }

        [HttpPut("{role}")]
        public JsonResult Put(Employee emp, string role)
        { 
            _employeeRepository.Update(emp);
            _emailService.UpdateAccount((int)emp.EmployeeId, role);
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
