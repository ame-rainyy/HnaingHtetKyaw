using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//========================================================== 
// Student Number : S10274663K
// Student Name : HNAING HTET KYAW
// Partner Name : Marcus Mah En Hao 
//==========================================================

namespace S10274663K_PRG2Assignment
{
    public class OrderedFoodItem
    {
        // Attributes
        private FoodItem foodItem;
        private int quantity;
        private string specialRequest;

        // Constructor
        public OrderedFoodItem(FoodItem foodItem, int quantity, string specialRequest = "")
        {
            this.foodItem = foodItem;
            this.quantity = quantity;
            this.specialRequest = specialRequest;
        }

        // Methods
        public double GetSubTotal()
        {
            return foodItem.ItemPrice * quantity;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(specialRequest))
            {
                return $"{foodItem.ItemName} x {quantity}";
            }
            else
            {
                return $"{foodItem.ItemName} x {quantity} (Request: {specialRequest})";
            }
        }
    }
}