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
    public class EmployeeController : ControllerBase
    {
        private readonly IWebHostEnvironment _env;
        private readonly IEmployeeRepository _employeeRepository;

        public EmployeeController(IWebHostEnvironment env, IEmployeeRepository employeeRepository)
        {
            _env = env;
            this._employeeRepository = employeeRepository;
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

        [HttpPost]
        public JsonResult Post(Employee emp)
        {

            _employeeRepository.Insert(emp);
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

        [Route("GetAllDepartmentNames")]
        public JsonResult GetAllDepartmentNames()
        {
            //string query = @"select DepartmentName from dbo.Department";
            //DataTable table = new DataTable();
            //string sqlDataSource = _configuration.GetConnectionString("EmployeeAppCon");
            //SqlDataReader myReader;

            //using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            //{
            //    myCon.Open();
            //    using (SqlCommand myCommand = new SqlCommand(query, myCon))
            //    {
            //        myReader = myCommand.ExecuteReader();
            //        table.Load(myReader);

            //        myReader.Close();
            //        myCon.Close();
            //    }
            //}

            return new JsonResult("asdf");
        }
    }
}
