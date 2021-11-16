using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WebClient
{
    static class Program
    {
        static async Task Main()
        {
            await Menu();
            Console.ReadLine();
        }

        static async Task Menu()
        {
            ConsoleKeyInfo key;

            do
            {
                Console.WriteLine("0 - Выход");
                Console.WriteLine("1 - Найти покупателя по ID");
                Console.WriteLine("2 - Создать покупателя");

                Console.WriteLine();
                Console.Write(">>> ");
                key = Console.ReadKey();

                switch (key.Key)
                {
                    case ConsoleKey.D0:
                        break;

                    case ConsoleKey.D1:
                        await GetUserById();
                        break;

                    case ConsoleKey.D2:
                        await CreateNewCustomer();
                        break;

                    default:
                        break;
                }


            } while (key.Key != ConsoleKey.D0);
        }

        static async Task GetUserById()
        {
            Console.Clear();

            Console.Write("Введите ID: ");
            var input = Console.ReadLine();

            if (!long.TryParse(input, out long id))
            {
                Console.WriteLine("Не удалось считать ID");
                return;
            }

            using var webClient = new HttpClient();

            var url = $"https://localhost:5001/customers/{id}";
            using var request = new HttpRequestMessage(HttpMethod.Get, url);

            using var response = await webClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var customer = JsonSerializer.Deserialize<Customer>(content);

            Console.WriteLine($"FirstName: {customer.Firstname}");
            Console.WriteLine($"LastName: {customer.Lastname}");
            Console.WriteLine();
        }

        static async Task CreateNewCustomer()
        {
            Console.Clear();

            var customer = RandomCustomer();

            Console.WriteLine("Новый покупатель:");
            Console.WriteLine($"FirstName: {customer.Firstname}");
            Console.WriteLine($"LastName: {customer.Lastname}");

            var json = JsonSerializer.Serialize(customer);
            using var content = new StringContent(json, Encoding.UTF8, "application/json");

            using var webClient = new HttpClient();
            var url = "https://localhost:5001/customers";

            using var response = await webClient.PostAsync(url, content);
            string result = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"Покупатель создан, ID = {result}");
        }

        private static CustomerCreateRequest RandomCustomer()
        {
            var rnd = new Random(DateTime.Now.Millisecond);
            int next = rnd.Next(1, 100);

            return new CustomerCreateRequest
            {
                Firstname = $"CustomerFirst{next}",
                Lastname = $"CustomerLast{next}"
            };
        }
    }
}