using SupperMarket.Domain.Dtos;
using SupperMarket.Domain.Entities;

namespace SupperMarket.Data.IRepositories
{
    public interface ISoldProductRepository
    {
        Task<SoldProduct> InsertAsync(SoldProduct soldProduct);
        Task<SoldProduct> UpdateAsync(long id, SoldProduct SoldProduct);
        Task<bool> DeleteAsync(long id);
        Task<List<SoldProduct>> SelectByProductIdAsync(long id);
        Task<SoldProduct> SelectByIdAsync(long id);
        Task<List<AllSoldProductInfo>> SelectOverallStatsAsync();
        Task<List<SoldProduct>> SelectAllAsync();
    }
}
