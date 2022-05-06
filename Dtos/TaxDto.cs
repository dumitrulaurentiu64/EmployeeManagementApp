using Newtonsoft.Json;

namespace EmpAPI.Dtos
{
    public class TaxDto
    {
        public TaxDto(int tax, int cas, int cass, string pass = "none")
        {
            Tax = tax;
            CAS = cas;
            CASS = cass;
            Pass = pass;
        }


        public int Tax { get; set; }
        public int CAS { get; set; }
        public int CASS { get; set; }
        public string Pass { get; set; }
    }
}
