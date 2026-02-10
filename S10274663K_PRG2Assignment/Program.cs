using System;
using System.Collections.Generic;
using System.IO;


// name
namespace S10274663K_PRG2Assignment
{
    class Program
    {
        static List<Restaurant> restaurants = new List<Restaurant>();

        static void Main(string[] args)
        {
            LoadRestaurants("restaurants.csv");
            LoadFoodItems("fooditems - Copy.csv");

            Console.WriteLine($"{restaurants.Count} restaurants loaded!");

            int foodCount = 0;
            foreach (Restaurant r in restaurants)
            {
                foreach (Menu m in r.Menus)
                {
                    foodCount += m.FoodItems.Count;
                }
            }
            Console.WriteLine($"{foodCount} food items loaded!\n");

            // Display restaurants and their menus
            foreach (Restaurant r in restaurants)
            {
                r.DisplayMenu();
            }
        }

        static void LoadRestaurants(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            for (int i = 1; i < lines.Length; i++) // skip header
            {
                string[] data = lines[i].Split(',');

                string restaurantId = data[0];
                string restaurantName = data[1];
                string restaurantEmail = data[2];

                Restaurant restaurant =
                    new Restaurant(restaurantId, restaurantName, restaurantEmail);

                // Each restaurant has at least one menu
                Menu menu = new Menu("M001", "Main Menu");
                restaurant.AddMenu(menu);

                restaurants.Add(restaurant);
            }
        }

        static void LoadFoodItems(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            for (int i = 1; i < lines.Length; i++) // skip header
            {
                string[] data = lines[i].Split(',');

                string restaurantId = data[0];
                string itemName = data[1];
                string itemDesc = data[2];
                double itemPrice = Convert.ToDouble(data[3]);

                FoodItem foodItem =
                    new FoodItem(itemName, itemDesc, itemPrice);

                Restaurant restaurant =
                    restaurants.Find(r => r.RestaurantId == restaurantId);

                if (restaurant != null)
                {
                    restaurant.Menus[0].AddFoodItem(foodItem);
                }
            }
        }
        static List<Customer> customers = new List<Customer>();
        static List<Order> orders = new List<Order>();
        static void Main(string[] args)
        {
            LoadRestaurants("restaurants.csv");
            LoadFoodItems("fooditems.csv");
            LoadCustomers("customers.csv");
            LoadOrders("orders.csv");

            Console.WriteLine($"{restaurants.Count} restaurants loaded!");

            int foodCount = 0;
            foreach (Restaurant r in restaurants)
            {
                foreach (Menu m in r.Menus)
                {
                    foodCount += m.FoodItems.Count;
                }
            }
            Console.WriteLine($"{foodCount} food items loaded!");
            Console.WriteLine($"{customers.Count} customers loaded!");
            Console.WriteLine($"{orders.Count} orders loaded!\n");

            foreach (Restaurant r in restaurants)
            {
                r.DisplayMenu();
            }
        }
        static void LoadCustomers(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            for (int i = 1; i < lines.Length; i++) // skip header
            {
                string[] data = lines[i].Split(',');

                string name = data[0];
                string email = data[1];

                Customer customer = new Customer(name, email);
                customers.Add(customer);
            }
        }
        static void LoadOrders(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            for (int i = 1; i < lines.Length; i++) // skip header
            {
                string[] data = lines[i].Split(',');

                int orderId = Convert.ToInt32(data[0]);
                string customerEmail = data[1];
                string restaurantId = data[2];
                DateTime deliveryDateTime = Convert.ToDateTime(data[3]);
                double totalAmount = Convert.ToDouble(data[4]);
                string status = data[5];

                Customer customer =
                    customers.Find(c => c.EmailAddress == customerEmail);

                Restaurant restaurant =
                    restaurants.Find(r => r.RestaurantId == restaurantId);

                if (customer != null && restaurant != null)
                {
                    Order order = new Order(orderId, customer, restaurant,
                                            deliveryDateTime, totalAmount, status);

                    // add to system list
                    orders.Add(order);

                    // add to customer's order list
                    customer.AddOrder(order);

                    // add to restaurant's order queue
                    restaurant.AddOrderToQueue(order);
                }
            }
        }
        static void ListRestaurants(List<Restaurant> restaurants)
        {
            Console.WriteLine("All Restaurants and Menu Items");
            Console.WriteLine("==============================");

            foreach (Restaurant r in restaurants)
            {
                Console.WriteLine($"Restaurant: {r.RestaurantName} ({r.RestaurantId})");
                foreach (Menu m in r.Menus)
                {
                    foreach (FoodItem f in m.FoodItems)
                    {
                        Console.WriteLine(
                            $"  - {f.ItemName}: {f.ItemDesc} - ${f.ItemPrice:F2}"
                        );
                    }
                }

                Console.WriteLine();
            }
        }
    }
}

