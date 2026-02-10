//========================================================== 
// Student Number : S10274663K
// Student Name : HNAING HTET KYAW
// Partner Name : Marcus Mah En Hao 
//==========================================================

using S10274663K_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private bool orderPaid;

        private double orderTotal;


        private List<OrderedFoodItem> orderedItems;


        public Customer Customer { get; private set; }
        public Restaurant Restaurant { get; private set; }


        // Constructor 

        public Order(int orderId,
                     Customer customer,
                     Restaurant restaurant,
                     DateTime deliveryDateTime,
                     double orderTotal,
                     string status)
        {
            this.orderId = orderId;
            Customer = customer;
            Restaurant = restaurant;
            this.deliveryDateTime = deliveryDateTime;
            this.orderTotal = orderTotal;
            orderStatus = status;

            orderDateTime = DateTime.Now;
            deliveryAddress = "";
            paymentMethod = "";
            orderPaid = false;

            orderedItems = new List<OrderedFoodItem>();
        }

        public Order(int orderId, Customer customer, DateTime deliveryDateTime, string deliveryAddress)
        {
            this.orderId = orderId;
            Customer = customer;
            Restaurant = null;
            this.deliveryDateTime = deliveryDateTime;
            this.deliveryAddress = deliveryAddress;

            orderDateTime = DateTime.Now;
            orderStatus = "Pending";
            paymentMethod = "";
            orderPaid = false;
            orderTotal = 0;

            orderedItems = new List<OrderedFoodItem>();
        }



        public void AddOrderedFoodItem(OrderedFoodItem item)
        {
            orderedItems.Add(item);
        }

        public void AddItem(OrderedFoodItem item)
        {
            orderedItems.Add(item);
        }

        public bool RemoveOrderedFoodItem(OrderedFoodItem item)
        {
            return orderedItems.Remove(item);
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

        public void SetRestaurant(Restaurant restaurant)
        {
            this.Restaurant = restaurant;
        }


        public void SetPaymentMethod(string method)
        {
            paymentMethod = method;
        }

        public void SetStatus(string status)
        {
            orderStatus = status;
        }


        // Properties

        public int OrderId
        {
            get { return orderId; }
        }

        public DateTime DeliveryDateTime
        {
            get { return deliveryDateTime; }
            set { deliveryDateTime = value; }
        }

        public string DeliveryAddress
        {
            get { return deliveryAddress; }
            set { deliveryAddress = value; }
        }

        public string Status
        {
            get { return orderStatus; }
        }

        public string PaymentMethod
        {
            get { return paymentMethod; }
        }

        public bool OrderPaid
        {
            get { return orderPaid; }
            set { orderPaid = value; }
        }


        public double TotalAmount
        {
            get { return orderTotal; }
        }

        public List<OrderedFoodItem> OrderedItems
        {
            get { return orderedItems; }
        }


        public override string ToString()
        {
            return $"Order ID: {orderId}\n" +
                   $"Customer: {Customer.CustomerName}\n" +
                   $"Restaurant: {Restaurant.RestaurantName}\n" +
                   $"Delivery Date/Time: {deliveryDateTime}\n" +
                   $"Status: {orderStatus}\n" +
                   $"Total Amount: ${orderTotal:F2}";
        }
    }
}