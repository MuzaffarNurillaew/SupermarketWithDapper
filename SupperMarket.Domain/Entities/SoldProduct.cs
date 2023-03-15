using SupperMarket.Domain.Commons;

namespace SupperMarket.Domain.Entities
{
    public class SoldProduct : Auditable
    {
        public long Productid { get; set; }
        public long Amount { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
