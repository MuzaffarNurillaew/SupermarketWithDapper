using SupperMarket.Data.AppSettings;
using SupperMarket.Domain.Entities;
using SupperMarket.Service.Helpers;
using SupperMarket.Service.Interfaces;
using SupperMarket.Service.Services;

namespace SupperMarket.Presentation
{
    class Program
    {
        private static readonly IProductService productService = new ProductService();
        private static readonly ISoldProductService soldProductService = new SoldProductService();
        static async Task Main()
        {
            Validate();

            while (true)
            {
                Console.Clear();

                Console.WriteLine($"Buyruqlar\n" +
                    $"1. Mahsulot qo'shish.\n" +
                    $"2. Mahsulot qidirish.\n" +
                    $"3. Mahsulot ma'lumotlarini o'zgartirish.\n" +
                    $"4. Mahsulotni o'chirish.\n" +
                    $"5. Mahsulot sotish\n" +
                    $"6. Hamma mahsulotlar ro'yxatini chiqarish.\n" +
                    $"7. Barcha sotilgan mahsulotlar ro'yxatini chiqarish.\n" +
                    $"8. Sotilgan mahsulotlar bo'yicha umumiy statistika\n" +
                    $"~ Chiqish.\n");

                int choice = 0;
                try
                {
                    choice = int.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    return;
                }
                Console.Clear();
                if (choice == 1)
                {
                    await PrintCreateWindow();
                }
                else if (choice == 2)
                {
                    await PrintSearchWindow();
                }
                else if (choice == 3)
                {
                    await PrintUpdateWindow();
                }
                else if (choice == 4)
                {
                    await PrintDeleteWindow();
                }
                else if (choice == 5)
                {
                    await PrintSellWindow();
                }
                else if (choice == 6)
                {
                    await PrintGetAllWindow();
                }
                else if (choice == 7)
                {
                    await PrintGetAllSoldWindow();
                }
                else if (choice == 8)
                {
                    await PrintGetAllStatisticsAsync();
                }
                else
                {
                    return;
                }
                Console.Write("Asosiy menyuga qaytish uchun xohlagan tugmani bosing.");
                Console.ReadKey();
            }
        }
        private static void Validate()
        {
            Console.WriteLine("configure database settings:");
            Console.Write("Enter the database name (default: SupperMarket): ");
            string dbName = Console.ReadLine();
            if (dbName == string.Empty)
            {
                dbName = "SupperMarket";
            }
            Console.Write("Enter the user id (default postgres) : ");
            string userName = Console.ReadLine();
            if (userName == string.Empty)
            {
                userName = "postgres";
            }
            Console.Write("Enter the password (no default value): ");
            string password = Console.ReadLine();
            Configurations.CONNECTION_STRING = $"host=localhost; Database={dbName}; User Id={userName}; password={password}";
        }

        private static async Task PrintGetAllStatisticsAsync()
        {
            var stats = (await soldProductService.GetOverallStatsAsync()).Result;

            foreach (var item in stats)
            {
                Product product = (await productService.GetByIdAsync(item.ProductId)).Result;
                Console.WriteLine("|------------------------------------------------------|");
                Console.WriteLine($"| id: {item.ProductId} name: {product.Name} | count: {item.SoldProductCount} | total income: {item.SoldProductMoney} |");
                Console.WriteLine("|------------------------------------------------------|");
            }
        }

        private static async Task PrintGetAllSoldWindow()
        {
            Console.WriteLine("Sotuvlar: ");

            List<SoldProduct> soldProducts = (await soldProductService.GetAllAsync()).Result;
            if (soldProducts is null)
            {
                Console.WriteLine("No any sold products");
            }

            foreach (var soldProduct in soldProducts)
            {
                Product product = (await productService.GetByIdAsync(soldProduct.Productid)).Result;
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine($"id: {soldProduct.Id} | name: {product.Name} | amount: {soldProduct.Amount} | total Price: {soldProduct.TotalPrice} | sold at: {soldProduct.CreatedAt}");
                Console.WriteLine("-------------------------------------------------------------------------------");
            }
        }

        private static async Task PrintGetAllWindow()
        {
            var products = (await productService.GetAllAsync()).Result;

            foreach (Product product in products)
            {
                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine($"id: {product.Id}, name: {product.Name}, amount: {product.Amount}, price: {product.Price}, created at: {product.CreatedAt}");
                Console.WriteLine("-------------------------------------------------------------------------------");
            }
        }

        #region PrintSellWindow
        private static async Task PrintSellWindow()
        {
            decimal totalMoney = 0;
            while (true)
            {
                Console.Clear();
                Console.WriteLine($"total amount you have to pay is: {totalMoney}");

                Console.Write("Enter QR code(ID): ");
                long id = long.Parse(Console.ReadLine());
                // Id|Name|Price|Amount|CreatedAt|UpdatedAt
                Console.Write("How many do you sell: ");
                int amount = int.Parse(Console.ReadLine());

                var response = await soldProductService.CreateAsync(new SoldProduct()
                {
                    Amount = amount,
                    Productid = id,
                });
                if (response.StatusCode == 404)
                {
                    Console.WriteLine(response.Message);
                }
                else
                {
                    totalMoney += response.Result.TotalPrice;
                    Console.WriteLine("successfully sold");
                }

                Console.Write("1. Sotib olishni davom ettirish, ~ Asosiy menyuga qaytish.");
                string choice = Console.ReadLine();

                if (choice != "1")
                {
                    Console.WriteLine($"To'lashingiz kerak bo'lgan miqdor: {totalMoney}");
                    break;
                }

            }
        }
        #endregion

        private static async Task PrintDeleteWindow()
        {
            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());

            Response<bool> response = await productService.DeleteAsync(id);

            Console.WriteLine(response.Message);
        }

        private static async Task PrintUpdateWindow()
        {
            Console.Write("ID: ");
            int id = int.Parse(Console.ReadLine());
            Console.Write("Mahsulot nomi: ");
            string name = Console.ReadLine();
            // Id|Name|Price|Amount|CreatedAt|UpdatedAt
            Console.Write("Narxi: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Miqdori: ");
            int amount = int.Parse(Console.ReadLine());

            var response = await productService.UpdateAsync(id, new Product
            {
                Name = name,
                Price = price,
                Amount = amount
            });

            Console.WriteLine(response.Message);
        }

        private static async Task PrintSearchWindow()
        {
            Console.WriteLine("1. Ism bilan");
            Console.WriteLine("2. Id bilan");
            string choice = Console.ReadLine();

            if (choice == "1")
            {
                Console.Write("Ismi: ");
                string name = Console.ReadLine();

                List<Product> entities = (await productService.GetAllByNameAsync(name)).Result;
                if (entities is null)
                {
                    Console.WriteLine("Nothing found");
                    return;
                }
                foreach (Product product in entities)
                {
                    Console.WriteLine("-------------------------------------------------------------------------------");
                    Console.WriteLine($"id: {product.Id}, name: {product.Name}, amount: {product.Amount}, price: {product.Price}, created at: {product.CreatedAt}");
                    Console.WriteLine("-------------------------------------------------------------------------------");
                }
            }
            else if (choice == "2")
            {
                Console.Write("Id: ");
                int id = int.Parse(Console.ReadLine());

                Product product = (await productService.GetByIdAsync(id)).Result;
                if (product is null)
                {
                    Console.WriteLine("nothing found!");
                    return;
                }

                Console.WriteLine("-------------------------------------------------------------------------------");
                Console.WriteLine($"id: {product.Id}, name: {product.Name}, amount: {product.Amount}, price: {product.Price}, created at: {product.CreatedAt}");
                Console.WriteLine("-------------------------------------------------------------------------------");
            }
            else
            {
                return;
            }
        }

        private static async Task PrintCreateWindow()
        {
            Console.Write("Mahsulot nomi: ");
            string name = Console.ReadLine();
            Console.Write("Narxi: ");
            decimal price = decimal.Parse(Console.ReadLine());
            Console.Write("Miqdori: ");
            int amount = int.Parse(Console.ReadLine());
            Response<Product> response = await productService.CreateAsync(new Product
            {
                Name = name,
                Price = price,
                Amount = amount
            });
            Console.WriteLine(response.Message);
        }
    }
}