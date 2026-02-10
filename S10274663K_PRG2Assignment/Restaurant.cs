//==========================================================
// Student Number : S10273819
// Student Name : Marcus Mah En Hao
// Partner Name : HNAING HTET KYAW
//==========================================================

using S10274663K_PRG2Assignment;

namespace S10274663K_PRG2Assignment
{
    public class Restaurant
    {
        private string restaurantId;
        private string restaurantName;
        private string restaurantEmail;
        private List<Menu> menus;
        private List<SpecialOffer> specialOffers;
        private Queue<Order> orderQueue;
        public string RestaurantId
        {
            get { return restaurantId; }
            set { restaurantId = value; }
        }
        public string RestaurantName
        {
            get { return restaurantName; }
            set { restaurantName = value; }
        }
        public string RestaurantEmail
        {
            get { return restaurantEmail; }
            set { restaurantEmail = value; }
        }

        public List<Menu> Menus { get; set; }
        public List<SpecialOffer> SpecialOffers { get; set; }
        public Queue<Order> OrderQueue { get; set; }

        public Restaurant(string restaurantId, string restaurantName, string restaurantEmail)
        {
            RestaurantId = restaurantId;
            RestaurantName = restaurantName;
            RestaurantEmail = restaurantEmail;

            Menus = new List<Menu>();
            SpecialOffers = new List<SpecialOffer>();
            OrderQueue = new Queue<Order>();
        }

        public void AddMenu(Menu menu)
        {
            Menus.Add(menu);
        }
        public bool RemoveMenu(string menuID)
        {
            Menu m = Menus.Find(menu => menu.MenuId == menuID);
            if (m != null)
            {
                Menus.Remove(m);
                return true;
            }
            return false;
        }

        public void DisplayMenu()
        {
            Console.WriteLine($"Restaurant: {restaurantName} ({restaurantId})");

            foreach (Menu m in Menus)
            {
                Console.WriteLine(m);
                m.DisplayFoodItems();
                Console.WriteLine();
            }
        }

        public void DisplaySpecialOffers()
        {
            Console.WriteLine("Special Offers:");

            if (specialOffers.Count == 0)
            {
                Console.WriteLine("No special offers available.");
                return;
            }

            foreach (SpecialOffer so in specialOffers)
            {
                Console.WriteLine(so);
            }
        }

        public void DisplayOrders()
        {
            Console.WriteLine($"Orders for {restaurantName}");

            if (orderQueue.Count == 0)
            {
                Console.WriteLine("No orders available.");
                return;
            }
            foreach (Order o in orderQueue)
            {
                Console.WriteLine(
                    $"Order {o.OrderId} | Customer: {o.Customer.Name} | " +
                    $"Status: {o.Status} | Amount: ${o.CalculateTotal():F2}"
                );
            }
        }
        public override string ToString()
        {
            return $"{RestaurantName} ({RestaurantId})";
        }
    }
}