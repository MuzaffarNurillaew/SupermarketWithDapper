using SupperMarket.Domain.Entities;
using SupperMarket.Service.Helpers;

namespace SupperMarket.Service.Interfaces
{
    public interface IProductService
    {
        Task<Response<Product>> CreateAsync(Product product);
        Task<Response<bool>> DeleteAsync(long id);
        Task<Response<Product>> GetByIdAsync(long id);
        Task<Response<List<Product>>> GetAllByNameAsync(string name);
        Task<Response<Product>> UpdateAsync(long id, Product product);
        Task<Response<List<Product>>> GetAllAsync();
    }
}
