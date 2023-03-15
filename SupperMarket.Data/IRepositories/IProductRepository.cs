using SupperMarket.Domain.Entities;

namespace SupperMarket.Data.IRepositories
{
    public interface IProductRepository
    {
        Task<Product> InsertAsync(Product product);
        Task<Product> UpdateAsync(long id, Product product);
        Task<bool> DeleteAsync(long id);
        Task<Product> SelectByIdAsync(long id);
        Task<List<Product>> SelectByNameAsync(string name);
        Task<List<Product>> SelectAllAsync();
    }
}
