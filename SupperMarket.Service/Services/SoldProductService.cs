using SupperMarket.Data.IRepositories;
using SupperMarket.Data.Repositories;
using SupperMarket.Domain.Dtos;
using SupperMarket.Domain.Entities;
using SupperMarket.Service.Helpers;
using SupperMarket.Service.Interfaces;

namespace SupperMarket.Service.Services
{
    public class SoldProductService : ISoldProductService
    {
        private readonly ISoldProductRepository soldProductRepository;
        private readonly IProductRepository productRepository;
        public SoldProductService()
        {
            try
            {
                soldProductRepository = new SoldProductRepository();
                productRepository = new ProductRepository();
            }
            catch
            {
                Console.WriteLine("Cannot access with database");
                Environment.Exit(1);
            }
        }
        public async Task<Response<SoldProduct>> CreateAsync(SoldProduct soldProduct)
        {
            if (soldProduct.Amount == 0)
            {
                return new Response<SoldProduct>()
                {
                    Message = "You cannot sell if amount is zero!"
                };
            }

            Product product = await productRepository.SelectByIdAsync(soldProduct.Productid);
            if (product is null)
            {
                return new Response<SoldProduct>()
                {
                    Message = "No such product"
                };
            }
            else if (product.Amount < soldProduct.Amount)
            {
                return new Response<SoldProduct>()
                {
                    Message = $"Not enough amount to sell. We have only {product.Amount}"
                };
            }
            try
            {
                soldProduct.TotalPrice = (await productRepository.SelectByIdAsync(soldProduct.Productid)).Price * soldProduct.Amount;
                SoldProduct result = await soldProductRepository.InsertAsync(soldProduct);

                return new Response<SoldProduct>
                {
                    StatusCode = 200,
                    Message = "Successfully added",
                    Result = result
                };
            }
            catch (Exception)
            {
                return new Response<SoldProduct>()
                {
                    Message = "Something went wrong"
                };
            }
        }

        public async Task<Response<bool>> DeleteAsync(long id)
        {
            SoldProduct entityToDelete = await soldProductRepository.SelectByIdAsync(id);

            if (entityToDelete is null) 
            {
                return new Response<bool>();
            }

            await soldProductRepository.DeleteAsync(id);
            return new Response<bool>()
            {
                StatusCode = 200,
                Message = "Successfully deleted",
                Result = true
            };
        }

        public async Task<Response<List<SoldProduct>>> GetAllAsync()
        {
            return new Response<List<SoldProduct>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = await soldProductRepository.SelectAllAsync()
            };
        }

        public async Task<Response<SoldProduct>> GetByIdAsync(long id)
        {
            SoldProduct entity = await soldProductRepository.SelectByIdAsync(id);

            if (entity is null)
            {
                return new Response<SoldProduct>();
            }

            return new Response<SoldProduct>
            {
                StatusCode = 200,
                Message = "Ok",
                Result = entity
            };
        }

        public async Task<Response<List<SoldProduct>>> GetAllByProductIdAsync(long id)
        {
            return new Response<List<SoldProduct>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = await soldProductRepository.SelectByProductIdAsync(id)
            };
        }

        public async Task<Response<List<AllSoldProductInfo>>> GetOverallStatsAsync()
        {
            return new Response<List<AllSoldProductInfo>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = await soldProductRepository.SelectOverallStatsAsync()
            };
        }

        public async Task<Response<SoldProduct>> UpdateAsync(long id, SoldProduct soldProduct)
        {
            SoldProduct entityToUpdate = await soldProductRepository.SelectByIdAsync(id);

            if (entityToUpdate is null)
            {
                return new Response<SoldProduct>();
            }

            SoldProduct updatedEntity = await soldProductRepository.UpdateAsync(id, soldProduct);

            return new Response<SoldProduct>
            {
                StatusCode = 200,
                Message = "Successfully updated",
                Result = updatedEntity
            };
        }
    }
}
