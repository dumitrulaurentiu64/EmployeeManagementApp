namespace EmpAPI.Models
{
    public class Employee
    {
        public int? EmployeeId { get; set; }
        public string? Name { get; set; }
        public string? Lastname { get; set; }
        public string? Position { get; set; }
        public string? Department { get; set; }
        public string? DateOfJoining { get; set; }
        public string? PhotoFileName { get; set; }
        public int? BaseSalary { get; set; }
        public int? Increase { get; set; }
        public int? GrossPrizes { get; set; }
        public int? GrossTotal { get; set; }
        public int? TaxableGross { get; set; }
        public int? CAS { get; set; }
        public int? CASS { get; set; }
        public int? Tax { get; set; }
        public int? Deductions { get; set; }
        public int? NetSalary { get; set; }

    }
}
