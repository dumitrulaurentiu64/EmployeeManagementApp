namespace EmpAPI.Models
{
    public class Department
    {
        private int? departmentId;

        public int? DepartmentId
        {
            get { return departmentId; }
            set { departmentId = value; }
        }

        private string departmentName;

        public string DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; }
        }


    }
}
