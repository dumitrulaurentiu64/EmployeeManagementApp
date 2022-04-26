using EmpAPI.Models;

namespace EmpAPI.Repository
{
    public interface IEmployeeRepository
    {
        public List<Employee> GetAll();
        public Employee Find(int id);

        public void Delete(int id);
        public Employee Update(Employee employee);
        public Employee Insert(Employee employee);
    }
}
