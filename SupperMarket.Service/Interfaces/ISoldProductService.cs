using SupperMarket.Domain.Dtos;
using SupperMarket.Domain.Entities;
using SupperMarket.Service.Helpers;

namespace SupperMarket.Service.Interfaces
{
    public interface ISoldProductService
    {
        Task<Response<SoldProduct>> CreateAsync(SoldProduct soldProduct);
        Task<Response<bool>> DeleteAsync(long id);
        Task<Response<SoldProduct>> GetByIdAsync(long id);
        Task<Response<List<SoldProduct>>> GetAllByProductIdAsync(long id);
        Task<Response<SoldProduct>> UpdateAsync(long id, SoldProduct soldProduct);
        Task<Response<List<SoldProduct>>> GetAllAsync();
        Task<Response<List<AllSoldProductInfo>>> GetOverallStatsAsync();
    }
}
