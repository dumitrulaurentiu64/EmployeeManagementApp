using AspNetCore.Reporting;
using EmpAPI.Models;
using EmpAPI.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using System.Reflection;
using System.Security.Permissions;

namespace EmpAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlyerController : Controller
    {
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IEmployeeRepository _employeeRepository;
        public FlyerController(IEmployeeRepository employeeRepository, IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
            _employeeRepository = employeeRepository;
        }
        [HttpGet]
        public IActionResult Flyer()
        {
            var dt = new DataTable();
            List<Employee> employeeList = _employeeRepository.GetAll();
            dt = ToDataTable(employeeList);

            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.ContentRootPath}Reports\\AllEmployeesFlyers.rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("EmployeeDataset", dt);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

            return File(result.MainStream, "application/pdf");
        }
        [HttpGet("{id}")]
        public IActionResult Flyer(int id)
        {
            var dt = new DataTable();
            List<Employee> emp = new List<Employee> { _employeeRepository.Find(id) };
            dt = ToDataTable(emp);

            string mimetype = "";
            int extension = 1;
            var path = $"{this._webHostEnvironment.ContentRootPath}Reports\\EmployeeFlyer.rdlc";

            Dictionary<string, string> parameters = new Dictionary<string, string>();
            LocalReport localReport = new LocalReport(path);
            localReport.AddDataSource("EmployeeDataset", dt);
            var result = localReport.Execute(RenderType.Pdf, extension, parameters, mimetype);

            return File(result.MainStream, "application/pdf");
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }
}
