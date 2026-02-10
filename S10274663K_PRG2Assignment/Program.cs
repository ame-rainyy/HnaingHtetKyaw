// Hnaing did basic feature 1, 4, 6, 8, and advanced feature (a)
// Marcus did basic feature 2, 3, 5, 7, and advanced feature (b)

//========================================================== 
// Student Number : S10274663K
// Student Name : HNAING HTET KYAW
// Partner Name : Marcus Mah En Hao 
//==========================================================

using System;
using System.Collections.Generic;
using System.IO;


// name
namespace S10274663K_PRG2Assignment
{
    class Program
    {
        // ========================================================= EXECUTING BASIC FEATURES   =========================================================
        static List<Restaurant> restaurantsList = new List<Restaurant>();
        static List<Customer> customersList = new List<Customer>();
        static List<Order> orders = new List<Order>();
        static Stack<Order> refundStack = new Stack<Order>();
        static void Main(string[] args)
        {
            // ===== Load data at startup (BF1 & BF2) =====
            LoadRestaurants("restaurants.csv");
            LoadFoodItems("fooditems - Copy.csv");
            LoadCustomers("customers.csv");
            LoadOrders("orders - Copy.csv");



            Console.WriteLine("Welcome to the Gruberoo Food Delivery System");
            Console.WriteLine($"{restaurantsList.Count} restaurants loaded!");
            Console.WriteLine($"{customersList.Count} customers loaded!");
            Console.WriteLine($"{orders.Count} orders loaded!");
            Console.WriteLine();

            while (true)
            {
                Console.WriteLine("\n===== Gruberoo Food Delivery System =====");
                Console.WriteLine("1. List all restaurants and menu items");
                Console.WriteLine("2. List all orders");
                Console.WriteLine("3. Create a new order");
                Console.WriteLine("4. Process an order");
                Console.WriteLine("5. Modify an existing order");
                Console.WriteLine("6. Delete an existing order");
                Console.WriteLine("7. Bulk process unprocessed orders (Advanced)");
                Console.WriteLine("8. Total order amount report (Advanced)");
                Console.WriteLine("0. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ListRestaurants(restaurantsList);   // 1
                        break;

                    case "2":
                        ListAllOrders();                     // 2
                        break;

                    case "3":
                        CreateNewOrder();                    // 5
                        break;

                    case "4":
                        ProcessOrders();                     // 6
                        break;

                    case "5":
                        ModifyExistingOrder();               // 7
                        break;

                    case "6":
                        DeleteExistingOrder();               // 8
                        break;

                    case "7":
                        BulkProcessUnprocessedOrders();   // Advanced Feature A
                        break;

                    case "8":
                        DisplayTotalOrderAmount();        // Advanced Feature B
                        break;

                    case "0":
                        SaveQueueAndStack();
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        // ========================================================= NEW FILES FOR OPTION OF EXITING ====================================
        static void SaveQueueAndStack()
        {
            try
            {

                // Save Queue 

                using (StreamWriter swQueue = new StreamWriter("queue.csv", false))
                {
                    swQueue.WriteLine("OrderId,CustomerEmail,RestaurantId,Status");

                    foreach (Restaurant r in restaurantsList)
                    {
                        foreach (Order o in r.OrderQueue)
                        {
                            swQueue.WriteLine(
                                $"{o.OrderId},{o.Customer.EmailAddress},{r.RestaurantId},{o.Status}"
                            );
                        }
                    }
                }


                // Save Stack (refunds)

                using (StreamWriter swStack = new StreamWriter("stack.csv", false))
                {
                    swStack.WriteLine("OrderId,CustomerEmail,RefundAmount");

                    foreach (Order o in refundStack)
                    {
                        double refundAmount = o.CalculateTotal() + 5.00; // includes delivery fee
                        swStack.WriteLine(
                            $"{o.OrderId},{o.Customer.EmailAddress},{refundAmount:F2}"
                        );
                    }
                }

                Console.WriteLine("Queue and refund stack saved successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving queue/stack: {ex.Message}");
            }
        }
        // ========================================================= Basic feature 1  =========================================================
        static void LoadRestaurants(string fileName)
        {
            string[] lines = File.ReadAllLines(fileName);

            for (int i = 1; i < lines.Length; i++)
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

            for (int i = 1; i < lines.Length; i++)
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
        // ========================================================= Basic feature 2 =========================================================


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

            for (int i = 1; i < lines.Length; i++)
            {
                string[] data = lines[i].Split(',', 10);

                int orderId = Convert.ToInt32(data[0]);
                string customerEmail = data[1].Trim();
                string restaurantId = data[2].Trim();

                string[] dateParts = data[3].Split('/');
                int day = Convert.ToInt32(dateParts[0]);
                int month = Convert.ToInt32(dateParts[1]);
                int year = Convert.ToInt32(dateParts[2]);

                DateTime deliveryDate = new DateTime(year, month, day);

                TimeSpan deliveryTime = TimeSpan.Parse(data[4]);
                DateTime deliveryDateTime = deliveryDate.Add(deliveryTime);

                string deliveryAddress = data[5];
                double totalAmount = Convert.ToDouble(data[7]);
                string status = data[8];

                Customer customer = customersList.Find(c => c.EmailAddress.Trim() == customerEmail);
                Restaurant restaurant = restaurantsList.Find(r => r.RestaurantId.Trim() == restaurantId);

                if (customer != null && restaurant != null)
                {
                    Order order = new Order(
                        orderId,
                        customer,
                        restaurant,
                        deliveryDateTime,
                        totalAmount,
                        status
                    );

                    orders.Add(order);
                    customer.AddOrder(order);
                    restaurant.OrderQueue.Enqueue(order);
                }
            }
        }
        // ========================================================= Basic feature 3 =========================================================
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

        // ================================================================= Basic feature 4 =========================================================
        static void ListAllOrders()
        {
            Console.WriteLine("All Orders");
            Console.WriteLine("==========");

            if (orders.Count == 0)
            {
                Console.WriteLine("No orders available.");
                return;
            }

            Console.WriteLine(
                $"{"Order ID",-10} {"Customer",-15} {"Restaurant",-20} {"Delivery Date/Time",-22} {"Amount",-10} {"Status",-10}"
            );
            Console.WriteLine(new string('-', 90));

            foreach (Order order in orders)
            {
                string customerName = order.Customer != null ? order.Customer.Name : "Unknown";
                string restaurantName = order.Restaurant != null ? order.Restaurant.RestaurantName : "Unknown";

                Console.WriteLine(
                    $"{order.OrderId,-10} " +
                    $"{customerName,-15} " +
                    $"{restaurantName,-20} " +
                    $"{order.DeliveryDateTime,-22:dd/MM/yyyy HH:mm} " +
                    $"{order.TotalAmount,-9:F2} " +
                    $"{order.Status,-10}"
                );
            }
        }

        // ================================================================= Basic feature 5 =========================================================
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
            newOrder.SetRestaurant(restaurant);

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

            string itemsText = "";
            for (int i = 0; i < selectedFoodItems.Count; i++)
            {
                itemsText += $"{selectedFoodItems[i].ItemName}, {quantities[i]}";
                if (i < selectedFoodItems.Count - 1)
                {
                    itemsText += "|";
                }
            }

            try
            {
                using (StreamWriter sw = new StreamWriter("orders.csv", true))
                {
                    double orderTotal = newOrder.CalculateTotal() + deliveryFee;

                    string line =
                        $"{newOrder.OrderId}," +
                        $"{customer.EmailAddress}," +
                        $"{restaurant.RestaurantId}," +
                        $"{deliveryDate:dd/MM/yyyy}," +
                        $"{deliveryTime:hh\\:mm}," +
                        $"{deliveryAddress}," +
                        $"{DateTime.Now:dd/MM/yyyy HH:mm}," +
                        $"{orderTotal:F1}," +
                        $"{newOrder.Status}," +
                        $"\"{itemsText}\"";

                    sw.WriteLine(line);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Warning: Could not save to file - {ex.Message}");
            }

            Console.WriteLine($"\nOrder {newOrderID} created successfully! Status: Pending");
        }

        // ========================================================= Basic feature 6 =========================================================

        static void ProcessOrders()
        {
            Console.WriteLine("\nProcess Order");
            Console.WriteLine("=============");

            Console.Write("Enter Restaurant ID: ");
            string restaurantId = Console.ReadLine().ToUpper();

            Restaurant restaurant = restaurantsList.Find(r => r.RestaurantId == restaurantId);
            if (restaurant == null)
            {
                Console.WriteLine("Error: Restaurant not found!");
                return;
            }

            if (restaurant.OrderQueue.Count == 0)
            {
                Console.WriteLine("No orders to process.");
                return;
            }

            int ordersToProcess = restaurant.OrderQueue.Count;

            for (int i = 0; i < ordersToProcess; i++)
            {
                Order order = restaurant.OrderQueue.Peek();

                Console.WriteLine($"\nOrder {order.OrderId}:");
                Console.WriteLine($"Customer: {order.Customer.CustomerName}");
                Console.WriteLine("Ordered Items:");

                int itemNo = 1;
                foreach (OrderedFoodItem item in order.OrderedItems)
                {
                    Console.WriteLine($"{itemNo}. {item}");
                    itemNo++;
                }

                Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");
                Console.WriteLine($"Total Amount: ${order.CalculateTotal():F2}");
                Console.WriteLine($"Order Status: {order.Status}");

                Console.Write("\n[C]onfirm / [R]eject / [S]kip / [D]eliver: ");
                string choice = Console.ReadLine().ToUpper();

                switch (choice)
                {
                    case "C":
                        if (order.Status == "Pending")
                        {
                            order.SetStatus("Preparing");
                            Console.WriteLine($"Order {order.OrderId} confirmed. Status: Preparing");
                        }
                        else
                        {
                            Console.WriteLine("Beware that order is not Pending.");
                        }
                        break;

                    case "R":
                        if (order.Status == "Pending")
                        {
                            order.SetStatus("Rejected");
                            refundStack.Push(order);
                            restaurant.OrderQueue.Dequeue();
                            Console.WriteLine($"Order {order.OrderId} rejected. Refund processed.");
                        }
                        else
                        {
                            Console.WriteLine("Action not allowed. Order is not Pending.");
                        }
                        break;

                    case "S":
                        if (order.Status == "Cancelled")
                        {
                            restaurant.OrderQueue.Dequeue();
                            Console.WriteLine("Order skipped.");
                        }
                        else
                        {
                            Console.WriteLine("Action not allowed. Order is not Cancelled.");
                        }
                        break;

                    case "D":
                        if (order.Status == "Preparing")
                        {
                            order.SetStatus("Delivered");
                            restaurant.OrderQueue.Dequeue();
                            Console.WriteLine($"Order {order.OrderId} delivered successfully.");
                        }
                        else
                        {
                            Console.WriteLine("Action not allowed. Order is not Preparing.");
                        }
                        break;

                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        // ========================================================= Basic feature 7 =========================================================
        static void ModifyExistingOrder()
        {
            Console.WriteLine("\nModify Order");
            Console.WriteLine("============");

            // 1. Customer email
            Console.Write("Enter Customer Email: ");
            string email = Console.ReadLine();

            Customer customer = customersList
                .Find(c => c.EmailAddress.Equals(email, StringComparison.OrdinalIgnoreCase));

            if (customer == null)
            {
                Console.WriteLine("Customer not found.");
                return;
            }

            // 2. Show pending orders
            List<Order> pendingOrders = customer.OrderList
                .Where(o => o.Status == "Pending")
                .ToList();

            if (pendingOrders.Count == 0)
            {
                Console.WriteLine("No pending orders found.");
                return;
            }

            Console.WriteLine("Pending Orders:");
            foreach (Order o in pendingOrders)
                Console.WriteLine(o.OrderId);

            // 3. Select order
            Console.Write("Enter Order ID: ");
            int orderId = int.Parse(Console.ReadLine());

            Order selectedOrder = pendingOrders.Find(o => o.OrderId == orderId);
            if (selectedOrder == null)
            {
                Console.WriteLine("Invalid Order ID.");
                return;
            }

            // 4. Display order details
            Console.WriteLine("\nOrder Items:");
            int i = 1;
            foreach (var item in selectedOrder.OrderedItems)
                Console.WriteLine($"{i++}. {item}");

            Console.WriteLine("\nAddress:");
            Console.WriteLine(selectedOrder.DeliveryAddress);

            Console.WriteLine("\nDelivery Date/Time:");
            Console.WriteLine($"{selectedOrder.DeliveryDateTime:dd/MM/yyyy HH:mm}");

            // 5. Modification menu
            Console.Write("\nModify: [1] Items [2] Address [3] Delivery Time: ");
            int choice = int.Parse(Console.ReadLine());

            double deliveryFee = 5.0;
            double oldTotal = selectedOrder.CalculateTotal() + deliveryFee;

            // ===== MODIFY ITEMS =====
            if (choice == 1)
            {
                Restaurant restaurant = restaurantsList
                    .Find(r => r.OrderQueue.Any(o => o.OrderId == orderId));

                if (restaurant == null)
                {
                    Console.WriteLine("Restaurant not found.");
                    return;
                }

                selectedOrder.OrderedItems.Clear();

                List<FoodItem> foodItems = restaurant.Menus
                    .SelectMany(m => m.FoodItems)
                    .ToList();

                Console.WriteLine("\nAvailable Food Items:");
                for (int j = 0; j < foodItems.Count; j++)
                    Console.WriteLine($"{j + 1}. {foodItems[j].ItemName} - ${foodItems[j].ItemPrice:F2}");

                while (true)
                {
                    Console.Write("Enter item number (0 to finish): ");
                    int itemChoice = int.Parse(Console.ReadLine());
                    if (itemChoice == 0) break;

                    Console.Write("Enter quantity: ");
                    int qty = int.Parse(Console.ReadLine());

                    selectedOrder.AddItem(new OrderedFoodItem(
                        foodItems[itemChoice - 1], qty));
                }

                double newTotal = selectedOrder.CalculateTotal() + deliveryFee;

                if (newTotal > oldTotal)
                {
                    Console.Write($"Additional payment required (${newTotal - oldTotal:F2}). Proceed? [Y/N]: ");
                    if (Console.ReadLine().ToUpper() != "Y")
                    {
                        Console.WriteLine("Modification cancelled.");
                        return;
                    }
                }

                Console.WriteLine($"Order {orderId} updated. New Total: ${newTotal:F2}");
            }

            // ===== MODIFY ADDRESS =====
            else if (choice == 2)
            {
                Console.Write("Enter new Delivery Address: ");
                selectedOrder.DeliveryAddress = Console.ReadLine();
                Console.WriteLine($"Order {orderId} updated. New Address saved.");
            }

            // ===== MODIFY DELIVERY TIME =====
            else if (choice == 3)
            {
                Console.Write("Enter new Delivery Time (hh:mm): ");
                TimeSpan newTime = TimeSpan.Parse(Console.ReadLine());

                selectedOrder.DeliveryDateTime =
                    selectedOrder.DeliveryDateTime.Date.Add(newTime);

                Console.WriteLine($"Order {orderId} updated. New Delivery Time: {newTime:hh\\:mm}");
            }
        }

        // ========================================================= Basic feature 8  =========================================================
        static void DeleteExistingOrder()
        {
            Console.WriteLine("\nDelete Order");
            Console.WriteLine("============");

            Console.Write("Enter Customer Email: ");
            string email = Console.ReadLine().ToLower();

            Customer customer = customersList
                .Find(c => c.EmailAddress.ToLower() == email);

            if (customer == null)
            {
                Console.WriteLine("Error: Customer not found!");
                return;
            }

            List<Order> pendingOrders =
                customer.OrderList.FindAll(o => o.Status == "Pending");

            if (pendingOrders.Count == 0)
            {
                Console.WriteLine("No pending orders found.");
                return;
            }

            Console.WriteLine("Pending Orders:");
            foreach (Order o in pendingOrders)
                Console.WriteLine(o.OrderId);

            Console.Write("Enter Order ID: ");
            if (!int.TryParse(Console.ReadLine(), out int orderId))
            {
                Console.WriteLine("Invalid Order ID.");
                return;
            }

            Order order = pendingOrders.Find(o => o.OrderId == orderId);
            if (order == null)
            {
                Console.WriteLine("Order not found.");
                return;
            }

            Console.WriteLine($"\nCustomer: {customer.CustomerName}");
            Console.WriteLine("Ordered Items:");
            int i = 1;
            foreach (var item in order.OrderedItems)
                Console.WriteLine($"{i++}. {item}");

            Console.WriteLine($"Delivery date/time: {order.DeliveryDateTime:dd/MM/yyyy HH:mm}");

            double refundAmount = order.CalculateTotal() + 5.00;
            Console.WriteLine($"Total Amount: ${refundAmount:F2}");
            Console.WriteLine($"Order Status: {order.Status}");

            Console.Write("Confirm deletion? [Y/N]: ");
            if (Console.ReadLine().ToUpper() != "Y")
            {
                Console.WriteLine("Deletion cancelled.");
                return;
            }

            order.SetStatus("Cancelled");
            refundStack.Push(order);

            Console.WriteLine(
                $"Order {order.OrderId} cancelled. Refund of ${refundAmount:F2} processed."
            );
        }
        // ========================================================= Advanced feature (a) =========================================================
        static void BulkProcessUnprocessedOrders()
        {
            Console.WriteLine("\nBulk Processing of Unprocessed Orders");
            Console.WriteLine("=====================================");

            DateTime today = DateTime.Today;

            int totalPending = 0;
            int processedCount = 0;
            int preparingCount = 0;
            int rejectedCount = 0;
            int totalOrders = 0;

            foreach (Restaurant restaurant in restaurantsList)
            {
                int queueSize = restaurant.OrderQueue.Count;

                for (int i = 0; i < queueSize; i++)
                {
                    Order order = restaurant.OrderQueue.Peek();
                    totalOrders++;

                    // pending orders for today
                    if (order.Status == "Pending" &&
                        order.DeliveryDateTime.Date == today)
                    {
                        totalPending++;

                        TimeSpan timeToDelivery =
                            order.DeliveryDateTime - DateTime.Now;

                        if (timeToDelivery.TotalHours < 1)
                        {
                            order.SetStatus("Rejected");
                            refundStack.Push(order);
                            restaurant.OrderQueue.Dequeue();
                            rejectedCount++;
                        }
                        else
                        {
                            order.SetStatus("Preparing");
                            restaurant.OrderQueue.Enqueue(order);
                            restaurant.OrderQueue.Dequeue();
                            preparingCount++;
                        }

                        processedCount++;
                    }
                    else
                    {
                        // rotation
                        restaurant.OrderQueue.Enqueue(order);
                        restaurant.OrderQueue.Dequeue();
                    }
                }
            }

            Console.WriteLine($"\nTotal Pending Orders Today: {totalPending}");
            Console.WriteLine($"Orders Processed: {processedCount}");
            Console.WriteLine($"Preparing Orders: {preparingCount}");
            Console.WriteLine($"Rejected Orders: {rejectedCount}");

            double percentage =
                totalOrders == 0 ? 0 : (processedCount * 100.0 / totalOrders);

            Console.WriteLine(
                $"Percentage Processed: {percentage:F2}%"
            );
        }
        // ========================================================= Advanced feature (b) =========================================================
        static void DisplayTotalOrderAmount()
        {
            Console.WriteLine("\nTotal Order Amount Report");
            Console.WriteLine("=========================\n");

            double deliveryFee = 5.00;
            double commissionRate = 0.30;

            double grandTotalOrders = 0;
            double grandTotalRefunds = 0;

            foreach (Restaurant restaurant in restaurantsList)
            {
                Console.WriteLine($"Restaurant: {restaurant.RestaurantName} ({restaurant.RestaurantId})");
                Console.WriteLine(new string('-', 60));

                double restaurantOrdersTotal = 0;
                double restaurantRefundsTotal = 0;
                int deliveredCount = 0;
                int refundedCount = 0;

                // Delivered orders
                foreach (Order order in orders)
                {
                    if (order.Restaurant != null &&
                        order.Restaurant.RestaurantId == restaurant.RestaurantId &&
                        order.Status == "Delivered")
                    {
                        restaurantOrdersTotal += order.CalculateTotal();
                        deliveredCount++;
                    }
                }

                // Refunds
                foreach (Order order in refundStack)
                {
                    if (order.Restaurant != null &&
                        order.Restaurant.RestaurantId == restaurant.RestaurantId)
                    {
                        restaurantRefundsTotal += order.CalculateTotal() + deliveryFee;
                        refundedCount++;
                    }
                }

                Console.WriteLine($"Successful Orders (Delivered): {deliveredCount}");
                Console.WriteLine($"Total Order Amount: ${restaurantOrdersTotal:F2}");
                Console.WriteLine($"Refunded Orders (Cancelled): {refundedCount}");
                Console.WriteLine($"Total Refunds: ${restaurantRefundsTotal:F2}\n");

                grandTotalOrders += restaurantOrdersTotal;
                grandTotalRefunds += restaurantRefundsTotal;
            }

            Console.WriteLine(new string('=', 60));
            Console.WriteLine("OVERALL SUMMARY");
            Console.WriteLine(new string('=', 60));

            Console.WriteLine($"Total Order Amount (All Restaurants): ${grandTotalOrders:F2}");
            Console.WriteLine($"Total Refunds (All Restaurants): ${grandTotalRefunds:F2}");

            double commission = grandTotalOrders * commissionRate;
            Console.WriteLine($"Gruberoo Commission (30%): ${commission:F2}");

            double finalEarnings = commission - grandTotalRefunds;
            Console.WriteLine($"Final Amount Gruberoo Earns: ${finalEarnings:F2}");
            Console.WriteLine(new string('=', 60));
        }
    }
}