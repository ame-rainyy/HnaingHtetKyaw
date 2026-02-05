//========================================================== 
// Student Number : S10274663K
// Student Name : HNAING HTET KYAW
// Partner Name : Marcus Mah En Hao 
//==========================================================

using S10274663K_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Linq;
using System.Linq;
using System.Text;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks;

namespace S10274663K_PRG2Assignment
{
    public class Order
    {
        // Attributes
        private int orderId;
        private DateTime orderDateTime;
        private DateTime deliveryDateTime;
        private string deliveryAddress;
        private string orderStatus;
        private string paymentMethod;
        private List<OrderedFoodItem> orderedItems;
        private Customer customer;

        // Constructor
        public Order(int orderId, Customer customer, DateTime deliveryDateTime, string deliveryAddress)
        {
            this.orderId = orderId;
            this.customer = customer;
            this.deliveryDateTime = deliveryDateTime;
            this.deliveryAddress = deliveryAddress;

            orderDateTime = DateTime.Now;
            orderStatus = "Pending";
            paymentMethod = "";
            orderedItems = new List<OrderedFoodItem>();
        }


        // Methods
        public void AddItem(OrderedFoodItem item)
        {
            orderedItems.Add(item);
        }

        public void RemoveItem(OrderedFoodItem item)
        {
            orderedItems.Remove(item);
        }
        public double CalculateTotal()
        {
            double total = 0;

            foreach (OrderedFoodItem item in orderedItems)
            {
                total += item.GetSubTotal();
            }

            return total;
        }

        public int OrderId
        {
            get { return orderId; }
        }

        public string Status
        {
            get { return orderStatus; }
        }
        public Customer Customer { get; set; }
        public void SetPaymentMethod(string method)
        {
            paymentMethod = method;
        }

        public void SetStatus(string status)
        {
            orderStatus = status;
        }



        public override string ToString()
        {
            string output = $"Order ID: {orderId}\n";
            output += $"Order Date/Time: {orderDateTime}\n";
            output += $"Delivery Date/Time: {deliveryDateTime}\n";
            output += $"Delivery Address: {deliveryAddress}\n";
            output += $"Status: {orderStatus}\n";
            output += "Items:\n";

            foreach (OrderedFoodItem item in orderedItems)
            {
                output += "- " + item + "\n";
            }

            output += $"Total Amount: ${CalculateTotal():0.00}";

            return output;
        }
    }
}