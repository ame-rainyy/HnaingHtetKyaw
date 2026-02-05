//==========================================================
// Student Number : S10273819
// Student Name : Marcus Mah En Hao
// Partner Name : HNAING HTET KYAW
//==========================================================

namespace S10274663K_PRG2Assignment
{
    public class FoodItem
    {
        private string itemName;
        private string itemDesc;
        private double itemPrice;
        private string customise;
        public string ItemName
        {
            get { return itemName; }
            set { itemName = value; }
        }
        public string ItemDesc
        {
            get { return itemDesc; }
            set { itemDesc = value; }
        }
        public double ItemPrice
        {
            get { return itemPrice; }
            set { itemPrice = value; }
        }
        public string Customise
        {
            get { return customise; }
            set { customise = value; }
        }

        public FoodItem(string itemName, string itemDesc, double itemPrice)
        {
            ItemName = itemName;
            ItemDesc = itemDesc;
            ItemPrice = itemPrice;
            Customise = "";
        }

        public override string ToString()
        {
            return $"{ItemName}: {ItemDesc} - ${ItemPrice:F2}";
        }
    }
}