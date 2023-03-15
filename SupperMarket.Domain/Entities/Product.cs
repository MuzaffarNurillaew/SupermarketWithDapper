using SupperMarket.Domain.Commons;

namespace SupperMarket.Domain.Entities
{
    public class Product : Auditable
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public long Amount { get; set; }
    }
}
