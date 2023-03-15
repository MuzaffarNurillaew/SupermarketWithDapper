using SupperMarket.Data.DapperDb;
using SupperMarket.Data.IRepositories;
using SupperMarket.Domain.Dtos;
using SupperMarket.Domain.Entities;
using System.Data;

namespace SupperMarket.Data.Repositories
{
    public class SoldProductRepository : ISoldProductRepository
    {
        private IDapper<SoldProduct> dapper = new Dapperr<SoldProduct>();
        public SoldProductRepository()
        {
            dapper.Check();
        }
        public async Task<bool> DeleteAsync(long id)
        {
            SoldProduct soldProduct = await SelectByIdAsync(id);

            if (soldProduct is null)
            {
                return false;
            }

            string query = $"DELETE FROM SoldProducts " +
                           $"WHERE Id = {id}";

            await dapper.DeleteAsync(query);
            return true;
        }

        public async Task<SoldProduct> InsertAsync(SoldProduct soldProduct)
        {

            string query = $"INSERT INTO SoldProducts (ProductId, Amount, TotalPrice, createdAt) " +
                $"VALUES " +
                $"({soldProduct.Productid}, {soldProduct.Amount}, {soldProduct.TotalPrice}, now())";

            await dapper.InsertAsync(query);

            return await dapper.SelectAsync("SELECT * FROM SoldProducts " +
                "WHERE Id = (SELECT MAX(Id) FROM SoldProducts)");
        }

        public async Task<List<SoldProduct>> SelectAllAsync()
        {
            string query = "SELECT * FROM SoldProducts";
            return await dapper.SelectAllAsync(query);
        }

        public async Task<List<SoldProduct>> SelectByProductIdAsync(long id)
        {
            string query = $"SELECT * FROM SoldProducts " +
                $"WHERE ProductId = {id}";

            return await dapper.SelectAllAsync(query);
        }

        public async Task<SoldProduct> SelectByIdAsync(long id)
        {
            string query = $"SELECT * FROM SoldProducts " +
                           $"WHERE Id = {id}";

            return await dapper.SelectAsync(query);
        }

        public async Task<List<AllSoldProductInfo>> SelectOverallStatsAsync()
        {
            IDapper<AllSoldProductInfo> dapperForAll = new Dapperr<AllSoldProductInfo>();

            var records = await dapperForAll.SelectAllAsync("SELECT * FROM SelectOverallStatsAsync()");

            return records;
        }

        public async Task<SoldProduct> UpdateAsync(long id, SoldProduct soldProduct)
        {
            SoldProduct entity = await SelectByIdAsync(id);

            if (entity is null)
            {
                return null;
            }

            string query = $"UPDATE SoldProducts " +
                $"SET ProductId = {soldProduct.Productid}, Amount = {soldProduct.Amount}, " +
                $"TotalPrice = {soldProduct.TotalPrice}, UpdatedAt = NOW() " +
                $"WHERE Id = {id}";
            await dapper.UpdateAsync(query);

            return await SelectByIdAsync(id);
        }
    }
}
