//==========================================================
// Student Number : S10273819
// Student Name : Marcus Mah En Hao
// Partner Name : HNAING HTET KYAW
//==========================================================

namespace S10274663K_PRG2Assignment
{
    public class Menu
    {
        private string menuId;
        private string menuName;
        private List<FoodItem> foodItems;
        public string MenuId
        {
            get { return menuId; }
            set { menuId = value; }
        }
        public string MenuName
        {
            get { return menuName; }
            set { menuName = value; }
        }
        public List<FoodItem> FoodItems
        {
            get { return foodItems; }
            set { foodItems = value; }
        }

        public Menu(string menuId, string menuName)
        {
            MenuId = menuId;
            MenuName = menuName;
            FoodItems = new List<FoodItem>();
        }

        public void AddFoodItem(FoodItem item)
        {
            FoodItems.Add(item);
        }

        public bool RemoveFoodItem(string itemName)
        {
            FoodItem item = foodItems.Find(f => f.ItemName == itemName);

            if (item != null)
            {
                foodItems.Remove(item);
                return true;
            }

            return false;
        }

        public void DisplayFoodItems()
        {
            Console.WriteLine($"Menu: {menuName}");

            int index = 1;
            foreach (FoodItem f in foodItems)
            {
                Console.WriteLine($"{index}. {f.ItemName} - {f.ItemDesc} - ${f.ItemPrice:F2}");
                index++;
            }
        }

        public override string ToString()
        {
            return $"{menuName} ({menuId}) - {foodItems.Count} items";
        }
    }
}