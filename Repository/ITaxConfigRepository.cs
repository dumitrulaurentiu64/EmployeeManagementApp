using EmpAPI.Dtos;

namespace EmpAPI.Repository
{
    public interface ITaxConfigRepository
    {
        public TaxDto Get();
        public void UpdateTaxes(TaxDto taxDto);

        public void InsertTaxes(TaxDto taxDto);
    }
}
