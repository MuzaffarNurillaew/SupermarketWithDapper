using SupperMarket.Data.IRepositories;
using SupperMarket.Data.Repositories;
using SupperMarket.Domain.Entities;
using SupperMarket.Service.Helpers;
using SupperMarket.Service.Interfaces;

namespace SupperMarket.Service.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService()
        {
            try
            {
                productRepository = new ProductRepository();
            }
            catch (Exception)
            {
                Console.WriteLine("Unable to access with database!");
                Environment.Exit(0);
            }
        }
        public async Task<Response<Product>> CreateAsync(Product product)
        {
            if (product.Amount == 0 || product.Name == string.Empty || product.Price == 0)
            {
                return new Response<Product>()
                {
                    Message = "Provided info about product is not enough!"
                };
            }
            try
            {
                Product createdEntity = await this.productRepository.InsertAsync(product);
                return new Response<Product>()
                {
                    StatusCode = 200,
                    Message = "Successfully added",
                    Result = createdEntity
                };
            }
            catch (Exception)
            {
                return new Response<Product>()
                {
                    Message = "Something went wrong!"
                };
            }
        }

        public async Task<Response<bool>> DeleteAsync(long id)
        {
            bool hasBeenDeleted = await productRepository.DeleteAsync(id);

            if (hasBeenDeleted)
            {
                return new Response<bool>()
                {
                    StatusCode = 200,
                    Message = "Successfully deleted.",
                    Result = true
                };
            }

            return new Response<bool>()
            {
                Message = "Unable to delete."
            };
        }

        public async Task<Response<List<Product>>> GetAllAsync()
        {
            return new Response<List<Product>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = await productRepository.SelectAllAsync()
            };
        }

        public async Task<Response<Product>> GetByIdAsync(long id)
        {
            Product product = await productRepository.SelectByIdAsync(id);

            if (product is null)
            {
                return new Response<Product>();
            }

            return new Response<Product>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = product
            };
        }

        public async Task<Response<List<Product>>> GetAllByNameAsync(string name)
        {
            var result = await productRepository.SelectByNameAsync(name);

            return new Response<List<Product>>()
            {
                StatusCode = 200,
                Message = "Ok",
                Result = result
            };
        }

        public async Task<Response<Product>> UpdateAsync(long id, Product product)
        {
            Product productToUpdate = await productRepository.SelectByIdAsync(id);

            if (productToUpdate is null)
            {
                return new Response<Product>();
            }

            await productRepository.UpdateAsync(id, product);

            return new Response<Product>()
            {
                StatusCode = 200,
                Message = "Successfully updated!",
                Result = productToUpdate
            };
        }
    }
}
