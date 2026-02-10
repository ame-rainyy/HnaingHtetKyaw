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
        static void CreateNewOrder()
        {
            Console.WriteLine("\nCreate New Order");
            Console.WriteLine("================");

            Console.Write("Enter Customer Email: ");
            string customerEmail = Console.ReadLine();

            Customer customer = null;
            foreach (Customer c in customerList)
            {
                if (c.EmailAddress.ToLower() == customerEmail.ToLower())
                {
                    customer = c;
                    break;
                }
            }

            if (customer == null)
            {
                Console.WriteLine("Error: Customer not found!");
                return;
            }

            Console.Write("Enter Restaurant ID: ");
            string restaurantID = Console.ReadLine().ToUpper();

            Restaurant restaurant = null;
            foreach (Restaurant r in restaurantList)
            {
                if (r.RestaurantId == restaurantID)
                {
                    restaurant = r;
                    break;
                }
            }

            if (restaurant == null)
            {
                Console.WriteLine("Error: Restaurant not found!");
                return;
            }

            List<FoodItem> allFoodItems = new List<FoodItem>();
            foreach (Menu menu in restaurant.Menus)
            {
                allFoodItems.AddRange(menu.FoodItems);
            }

            if (allFoodItems.Count == 0)
            {
                Console.WriteLine("Error: This restaurant has no menu items!");
                return;
            }

            DateTime deliveryDate;
            while (true)
            {
                Console.Write("Enter Delivery Date (dd/mm/yyyy): ");
                string dateInput = Console.ReadLine();

                if (DateTime.TryParseExact(dateInput, "dd/MM/yyyy", null,
                    System.Globalization.DateTimeStyles.None, out deliveryDate))
                {
                    if (deliveryDate >= DateTime.Today)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: Delivery date cannot be in the past!");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Invalid date format! Please use dd/mm/yyyy");
                }
            }

            TimeSpan deliveryTime;
            while (true)
            {
                Console.Write("Enter Delivery Time (hh:mm): ");
                string timeInput = Console.ReadLine();

                if (TimeSpan.TryParseExact(timeInput, "hh\\:mm", null, out deliveryTime))
                {
                    DateTime deliveryDateTime = deliveryDate.Add(deliveryTime);
                    if (deliveryDateTime > DateTime.Now)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: Delivery time must be in the future!");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Invalid time format! Please use hh:mm");
                }
            }

            string deliveryAddress;
            while (true)
            {
                Console.Write("Enter Delivery Address: ");
                deliveryAddress = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(deliveryAddress))
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: Delivery address cannot be empty!");
                }
            }

            Console.WriteLine("\nAvailable Food Items:");
            for (int i = 0; i < allFoodItems.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {allFoodItems[i].ItemName} - ${allFoodItems[i].ItemPrice:F2}");
            }

            List<FoodItem> selectedFoodItems = new List<FoodItem>();
            List<int> quantities = new List<int>();

            while (true)
            {
                int choice;
                while (true)
                {
                    Console.Write("Enter item number (0 to finish): ");
                    if (int.TryParse(Console.ReadLine(), out choice))
                    {
                        if (choice == 0)
                        {
                            break;
                        }
                        else if (choice >= 1 && choice <= allFoodItems.Count)
                        {
                            break;
                        }
                        else
                        {
                            Console.WriteLine($"Error: Please enter a number between 0 and {allFoodItems.Count}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Error: Invalid input! Please enter a number.");
                    }
                }

                if (choice == 0)
                {
                    if (selectedFoodItems.Count == 0)
                    {
                        Console.WriteLine("Error: You must order at least one item!");
                        continue;
                    }
                    break;
                }

                FoodItem selectedItem = allFoodItems[choice - 1];

                int quantity;
                while (true)
                {
                    Console.Write("Enter quantity: ");
                    if (int.TryParse(Console.ReadLine(), out quantity) && quantity > 0)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: Please enter a valid positive quantity!");
                    }
                }

                selectedFoodItems.Add(selectedItem);
                quantities.Add(quantity);
            }

            Console.Write("Add special request? [Y/N]: ");
            string specialRequest = "";
            string response = Console.ReadLine().ToUpper();

            if (response == "Y")
            {
                Console.Write("Enter special request: ");
                specialRequest = Console.ReadLine();
            }

            double subtotal = 0;
            for (int i = 0; i < selectedFoodItems.Count; i++)
            {
                subtotal += selectedFoodItems[i].ItemPrice * quantities[i];
            }

            double deliveryFee = 5.00;
            double total = subtotal + deliveryFee;

            Console.WriteLine($"\nOrder Total: ${subtotal:F2} + ${deliveryFee:F2} (delivery) = ${total:F2}");

            Console.Write("Proceed to payment? [Y/N]: ");
            response = Console.ReadLine().ToUpper();

            if (response != "Y")
            {
                Console.WriteLine("Order cancelled.");
                return;
            }

            string paymentMethod = "";
            while (true)
            {
                Console.Write("Payment method:\n[CC] Credit Card / [PP] PayPal / [CD] Cash on Delivery: ");
                string paymentCode = Console.ReadLine().ToUpper();

                if (paymentCode == "CC")
                {
                    paymentMethod = "Credit Card";
                    break;
                }
                else if (paymentCode == "PP")
                {
                    paymentMethod = "PayPal";
                    break;
                }
                else if (paymentCode == "CD")
                {
                    paymentMethod = "Cash on Delivery";
                    break;
                }
                else
                {
                    Console.WriteLine("Error: Invalid payment method! Please enter CC, PP, or CD.");
                }
            }

            int newOrderID = 1000;
            foreach (Customer c in customerList)
            {
                foreach (Order o in c.OrderList)
                {
                    if (o.OrderId >= newOrderID)
                    {
                        newOrderID = o.OrderId + 1;
                    }
                }
            }

            DateTime deliveryDateTime = deliveryDate.Add(deliveryTime);
            Order newOrder = new Order(newOrderID, customer, deliveryDateTime, deliveryAddress);

            for (int i = 0; i < selectedFoodItems.Count; i++)
            {
                string itemSpecialRequest = "";
                if (i == 0 && !string.IsNullOrEmpty(specialRequest))
                {
                    itemSpecialRequest = specialRequest;
                }

                OrderedFoodItem orderedItem = new OrderedFoodItem(
                    selectedFoodItems[i],
                    quantities[i],
                    itemSpecialRequest
                );
                newOrder.AddItem(orderedItem);
            }

            newOrder.SetPaymentMethod(paymentMethod);

            customer.AddOrder(newOrder);

            restaurant.OrderQueue.Enqueue(newOrder);

            try
            {
                using (StreamWriter sw = new StreamWriter("orders.csv", true))
                {
                    // Calculate total including delivery fee
                    double orderTotal = newOrder.CalculateTotal() + deliveryFee;

                    string line = $"{newOrder.OrderId},{customer.EmailAddress},{restaurant.RestaurantId}," +
                                 $"{deliveryDateTime:dd/MM/yyyy HH:mm},{deliveryAddress}," +
                                 $"{orderTotal:F1},{newOrder.Status}";
                    sw.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not save to file - {ex.Message}");
            }

            Console.WriteLine($"\nOrder {newOrderID} created successfully! Status: Pending");
        }
    }
}

