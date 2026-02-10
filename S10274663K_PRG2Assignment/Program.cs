// Hnaing did basic feature 1, 4, 6, 8, and advanced feature (a)
// Marcus did basic feature 2, 3, 5, 7, and advanced feature (b)
using System;
using System.Collections.Generic;
using System.IO;


// name
namespace S10274663K_PRG2Assignment
{
    class Program
    {
        // Basic feature 1
        static List<Restaurant> restaurantsList = new List<Restaurant>();

        static void Main(string[] args)
        {
            LoadRestaurants("restaurants.csv");
            LoadFoodItems("fooditems - Copy.csv");

            Console.WriteLine($"{restaurantsList.Count} restaurants loaded!");

            int foodCount = 0;
            foreach (Restaurant r in restaurantsList)
            {
                foreach (Menu m in r.Menus)
                {
                    foodCount += m.FoodItems.Count;
                }
            }
            Console.WriteLine($"{foodCount} food items loaded!\n");

            // Display restaurants and their menus
            foreach (Restaurant r in restaurantsList)
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

                restaurantsList.Add(restaurant);
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
                    restaurantsList.Find(r => r.RestaurantId == restaurantId);

                if (restaurant != null)
                {
                    restaurant.Menus[0].AddFoodItem(foodItem);
                }
            }
        }
        // Basic feature 2
        static List<Customer> customersList = new List<Customer>();
        static List<Order> orders = new List<Order>();
        static void Main(string[] args)
        {
            LoadRestaurants("restaurants.csv");
            LoadFoodItems("fooditems - Copy.csv");
            LoadCustomers("customers.csv");
            LoadOrders("orders.csv");

            Console.WriteLine($"{restaurantsList.Count} restaurants loaded!");

            int foodCount = 0;
            foreach (Restaurant r in restaurantsList)
            {
                foreach (Menu m in r.Menus)
                {
                    foodCount += m.FoodItems.Count;
                }
            }
            Console.WriteLine($"{foodCount} food items loaded!");
            Console.WriteLine($"{customersList.Count} customers loaded!");
            Console.WriteLine($"{orders.Count} orders loaded!\n");

            foreach (Restaurant r in restaurantsList)
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
                customersList.Add(customer);
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
                    customersList.Find(c => c.EmailAddress == customerEmail);

                Restaurant restaurant =
                    restaurantsList.Find(r => r.RestaurantId == restaurantId);

                if (customer != null && restaurant != null)
                {
                    Order order = new Order(orderId, customer, restaurant,
                                            deliveryDateTime, totalAmount, status);

                    orders.Add(order);

                    customer.AddOrder(order);

                    restaurant.OrderQueue.Enqueue(order);
                }
            }
        }
        // Basic feature 3
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
        // Basic feature 5
        static void CreateNewOrder()
        {
            Console.WriteLine("\nCreate New Order");
            Console.WriteLine("================");

            Console.Write("Enter Customer Email: ");
            string customerEmail = Console.ReadLine();

            Customer customer = null;
            foreach (Customer c in customersList)
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
            foreach (Restaurant r in restaurantsList)
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
            DateTime deliveryDateTime;
            while (true)
            {
                Console.Write("Enter Delivery Time (hh:mm): ");
                string timeInput = Console.ReadLine();

                if (TimeSpan.TryParseExact(timeInput, "hh\\:mm", null, out deliveryTime))
                {
                    deliveryDateTime = deliveryDate.Add(deliveryTime);
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
            foreach (Customer c in customersList)
            {
                foreach (Order o in c.OrderList)
                {
                    if (o.OrderId >= newOrderID)
                    {
                        newOrderID = o.OrderId + 1;
                    }
                }
            }

            deliveryDateTime = deliveryDate.Add(deliveryTime);
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
        // Basic feature 7
        static void ModifyExistingOrder()
        {
            Console.WriteLine("\nModify Order");
            Console.WriteLine("============");

            Console.Write("Enter Customer Email: ");
            string customerEmail = Console.ReadLine();

            Customer customer = null;
            foreach (Customer c in customersList)
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

            List<Order> pendingOrders = new List<Order>();
            foreach (Order order in customer.OrderList)
            {
                if (order.Status == "Pending")
                {
                    pendingOrders.Add(order);
                }
            }

            if (pendingOrders.Count == 0)
            {
                Console.WriteLine("No pending orders found for this customer.");
                return;
            }

            Console.WriteLine("Pending Orders:");
            foreach (Order order in pendingOrders)
            {
                Console.WriteLine(order.OrderId);
            }

            int orderID;
            Order selectedOrder = null;
            while (true)
            {
                Console.Write("Enter Order ID: ");
                if (int.TryParse(Console.ReadLine(), out orderID))
                {
                    foreach (Order order in pendingOrders)
                    {
                        if (order.OrderId == orderID)
                        {
                            selectedOrder = order;
                            break;
                        }
                    }

                    if (selectedOrder != null)
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: Order ID not found in pending orders!");
                    }
                }
                else
                {
                    Console.WriteLine("Error: Please enter a valid Order ID!");
                }
            }

            Restaurant orderRestaurant = null;
            foreach (Restaurant r in restaurantsList)
            {
                foreach (Order o in r.OrderQueue)
                {
                    if (o.OrderId == orderID)
                    {
                        orderRestaurant = r;
                        break;
                    }
                }
                if (orderRestaurant != null)
                    break;
            }

            Console.WriteLine("\nOrder Items:");

            int itemNum = 1;
            foreach (OrderedFoodItem item in selectedOrder.OrderedItems)
            {
                Console.WriteLine($"{itemNum}. {item}");
                itemNum++;
            }

            Console.WriteLine("\nAddress:");
            Console.WriteLine(selectedOrder.DeliveryAddress);

            Console.WriteLine("\nDelivery Date/Time:");
            Console.WriteLine($"{selectedOrder.DeliveryDateTime:dd/M/yyyy, HH:mm}");

            int modifyChoice;
            while (true)
            {
                Console.Write("\nModify: [1] Items [2] Address [3] Delivery Time: ");
                if (int.TryParse(Console.ReadLine(), out modifyChoice) &&
                    modifyChoice >= 1 && modifyChoice <= 3)
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Error: Please enter 1, 2, or 3!");
                }
            }

            double deliveryFee = 5.00;
            double oldTotal = selectedOrder.CalculateTotal() + deliveryFee;

            if (modifyChoice == 1)
            {
                if (orderRestaurant == null)
                {
                    Console.WriteLine("Error: Cannot find restaurant for this order!");
                    return;
                }

                Console.WriteLine("\nCurrent Items:");
                itemNum = 1;
                foreach (OrderedFoodItem item in selectedOrder.OrderedItems)
                {
                    Console.WriteLine($"{itemNum}. {item}");
                    itemNum++;
                }

                List<FoodItem> allFoodItems = new List<FoodItem>();
                foreach (Menu menu in orderRestaurant.Menus)
                {
                    allFoodItems.AddRange(menu.FoodItems);
                }

                Console.WriteLine("\nAvailable Food Items:");
                for (int i = 0; i < allFoodItems.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {allFoodItems[i].ItemName} - ${allFoodItems[i].ItemPrice:F2}");
                }

                selectedOrder.OrderedItems.Clear();

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
                        if (selectedOrder.OrderedItems.Count == 0)
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

                    OrderedFoodItem orderedItem = new OrderedFoodItem(selectedItem, quantity);
                    selectedOrder.AddItem(orderedItem);
                }

                double newTotal = selectedOrder.CalculateTotal() + deliveryFee;

                Console.WriteLine($"\nOrder {orderID} updated. Items modified.");
                Console.WriteLine($"Previous Total: ${oldTotal:F2}");
                Console.WriteLine($"New Total: ${newTotal:F2}");

                if (newTotal > oldTotal)
                {
                    double additionalAmount = newTotal - oldTotal;
                    Console.WriteLine($"\nAdditional payment required: ${additionalAmount:F2}");
                    Console.Write("Proceed to payment? [Y/N]: ");
                    string response = Console.ReadLine().ToUpper();

                    if (response == "Y")
                    {
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

                        selectedOrder.SetPaymentMethod(paymentMethod);
                        Console.WriteLine("Additional payment processed successfully!");
                    }
                    else
                    {
                        Console.WriteLine("Order modification cancelled - payment not completed.");
                        return;
                    }
                }
            }
            else if (modifyChoice == 2)
            {
                string newAddress;
                while (true)
                {
                    Console.Write("Enter new Delivery Address: ");
                    newAddress = Console.ReadLine();

                    if (!string.IsNullOrWhiteSpace(newAddress))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Error: Delivery address cannot be empty!");
                    }
                }

                selectedOrder.DeliveryAddress = newAddress;
                Console.WriteLine($"\nOrder {orderID} updated. New Address: {newAddress}");
            }
            else if (modifyChoice == 3)
            {
                TimeSpan newDeliveryTime;
                while (true)
                {
                    Console.Write("Enter new Delivery Time (hh:mm): ");
                    string timeInput = Console.ReadLine();

                    if (TimeSpan.TryParseExact(timeInput, "hh\\:mm", null, out newDeliveryTime))
                    {
                        DateTime newDeliveryDateTime = selectedOrder.DeliveryDateTime.Date.Add(newDeliveryTime);

                        if (newDeliveryDateTime > DateTime.Now)
                        {
                            selectedOrder.DeliveryDateTime = newDeliveryDateTime;
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

                Console.WriteLine($"\nOrder {orderID} updated. New Delivery Time: {newDeliveryTime:hh\\:mm}");
            }

            UpdateOrdersFile();
        }

        static void UpdateOrdersFile()
        {
            try
            {
                using (StreamWriter sw = new StreamWriter("orders.csv", false))
                {
                    double deliveryFee = 5.00;

                    foreach (Customer customer in customersList)
                    {
                        foreach (Order order in customer.OrderList)
                        {
                            string restaurantID = "";
                            foreach (Restaurant r in restaurantsList)
                            {
                                foreach (Order o in r.OrderQueue)
                                {
                                    if (o.OrderId == order.OrderId)
                                    {
                                        restaurantID = r.RestaurantId;
                                        break;
                                    }
                                }
                                if (!string.IsNullOrEmpty(restaurantID))
                                    break;
                            }

                            double orderTotal = order.CalculateTotal() + deliveryFee;

                            string line = $"{order.OrderId},{customer.EmailAddress},{restaurantID}," +
                                         $"{order.DeliveryDateTime:dd/MM/yyyy HH:mm},{order.DeliveryAddress}," +
                                         $"{orderTotal:F1},{order.Status}";
                            sw.WriteLine(line);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not update orders file - {ex.Message}");
            }
        }
        // Advanced feature (b)
        static void DisplayTotalOrderAmount()
        {
            Console.WriteLine("\nTotal Order Amount Report");
            Console.WriteLine("=========================\n");

            double grandTotalOrders = 0;
            double grandTotalRefunds = 0;
            double deliveryFee = 5.00;
            double gruberooCommission = 0.30;

            foreach (Restaurant restaurant in restaurantsList)
            {
                Console.WriteLine($"Restaurant: {restaurant.RestaurantName} ({restaurant.RestaurantId})");
                Console.WriteLine(new string('-', 60));

                double restaurantTotalOrders = 0;
                double restaurantTotalRefunds = 0;
                int deliveredCount = 0;
                int refundedCount = 0;

                foreach (Order order in restaurant.OrderQueue)
                {
                    if (order.Status == "Delivered")
                    {
                        double orderSubtotal = order.CalculateTotal();
                        restaurantTotalOrders += orderSubtotal;
                        deliveredCount++;
                    }
                    else if (order.Status == "Rejected" || order.Status == "Cancelled")
                    {
                        double refundAmount = order.CalculateTotal() + deliveryFee;
                        restaurantTotalRefunds += refundAmount;
                        refundedCount++;
                    }
                }

                Console.WriteLine($"Successful Orders (Delivered): {deliveredCount}");
                Console.WriteLine($"Total Order Amount: ${restaurantTotalOrders:F2}");
                Console.WriteLine($"Refunded Orders (Rejected/Cancelled): {refundedCount}");
                Console.WriteLine($"Total Refunds: ${restaurantTotalRefunds:F2}");
                Console.WriteLine();

                grandTotalOrders += restaurantTotalOrders;
                grandTotalRefunds += restaurantTotalRefunds;
            }

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("OVERALL SUMMARY");
            Console.WriteLine(new string('=', 60));
            Console.WriteLine($"Total Order Amount (All Restaurants): ${grandTotalOrders:F2}");
            Console.WriteLine($"Total Refunds (All Restaurants): ${grandTotalRefunds:F2}");

            double gruberooEarnings = grandTotalOrders * gruberooCommission;
            Console.WriteLine($"Gruberoo Commission (30%): ${gruberooEarnings:F2}");

            double finalAmount = gruberooEarnings - grandTotalRefunds;
            Console.WriteLine($"Final Amount Gruberoo Earns: ${finalAmount:F2}");
            Console.WriteLine(new string('=', 60));
        }
    }
}