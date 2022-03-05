namespace EmpAPI.Models
{
    public class Employee
    {
        private int? employeeId;

        public int? EmployeeId
        {
            get { return employeeId; }
            set { employeeId = value; }
        }

        private string employeeName;

        public string EmployeeName
        {
            get { return employeeName; }
            set { employeeName = value; }
        }

        private string department;

        public string Department
        {
            get { return department; }
            set { department = value; }
        }

        private string dateOfJoining;

        public string DateOfJoining
        {
            get { return dateOfJoining; }
            set { dateOfJoining = value; }
        }

        private string photoFileName;

        public string PhotoFileName
        {
            get { return photoFileName; }
            set { photoFileName = value; }
        }
    }
}
