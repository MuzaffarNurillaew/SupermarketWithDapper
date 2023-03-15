using SupperMarket.Data.DapperDb;
using SupperMarket.Data.IRepositories;
using SupperMarket.Domain.Entities;

namespace SupperMarket.Data.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly IDapper<Product> dapper = new Dapperr<Product>();

        public ProductRepository()
        {
            dapper.Check();
        }
        public async Task<bool> DeleteAsync(long id)
        {
            Product product = await SelectByIdAsync(id);

            if (product is null) 
            {
                return false;
            }
            
            string query = $"DELETE FROM Products " +
                $"WHERE Id = {id}";
            await dapper.DeleteAsync(query);
            return true;
        }

        public async Task<List<Product>> SelectAllAsync()
        {
            string query = "SELECT * FROM Products";
            return await dapper.SelectAllAsync(query);
        }

        public async Task<Product> InsertAsync(Product product)
        {
            product.Name = product.Name.Replace("'", "''");
            string query = $"INSERT INTO Products (Name, Price, Amount) " +
                $"VALUES " +
                $"('{product.Name}', {product.Price}, {product.Amount})";

            await dapper.InsertAsync(query);

            return await dapper.SelectAsync("SELECT * FROM Products " +
                "WHERE Id = (SELECT MAX(Id) FROM Products)");
        }

        public async Task<Product> SelectByIdAsync(long id)
        {
            string query = $"SELECT * FROM Products " +
                $"WHERE Id = {id}";

            return await dapper.SelectAsync(query);
        }

        public async Task<Product> UpdateAsync(long id, Product product)
        {
            Product entity = await SelectByIdAsync(id);

            if (entity is null)
            {
                return null;
            }

            product.Name = product.Name.Replace("'", "''");
            string query = $"UPDATE Products " +
                $"SET Name = '{product.Name}', Price = {product.Price}, " +
                $"Amount = {product.Amount}, UpdatedAt = NOW() " +
                $"WHERE Id = {id}";
            await dapper.UpdateAsync(query);

            return await SelectByIdAsync(id);
        }

        public async Task<List<Product>> SelectByNameAsync(string name)
        {
            name = name.Replace("'", "''");
            string query = "SELECT * FROM products " +
                $"WHERE LOWER(name) LIKE LOWER('%{name}%')";

            return await dapper.SelectAllAsync(query);
        }
    }
}
