using EmpAPI.Models;

namespace EmpAPI.Repository
{
    public interface IDepartmentRepository
    {
        public List<Department> GetAll();
        public Department Find();

        public void Delete(int id);
        public Department Update(Department department);
        public Department Insert(Department department);
    }
}
