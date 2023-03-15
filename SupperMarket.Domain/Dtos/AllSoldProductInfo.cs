using SupperMarket.Domain.Commons;

namespace SupperMarket.Domain.Dtos
{
    public class AllSoldProductInfo : Auditable
    {
        public long ProductId { get; set; }
        public long SoldProductCount { get; set; }
        public decimal SoldProductMoney { get; set; }
    }
}
