using S10274663K_PRG2Assignment;
using S10274663K_PRG2Assignment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

//========================================================== 
// Student Number : S10274663K
// Student Name : HNAING HTET KYAW
// Partner Name : Marcus Mah En Hao 
//==========================================================

namespace S10274663K_PRG2Assignment
{
    public class Customer
    {
        private string customerName;
        private string emailAddress;
        private List<Order> orderList;

        public string CustomerName
        {
            get { return customerName; }
        }

        public string EmailAddress
        {
            get { return emailAddress; }
        }

        public List<Order> OrderList
        {
            get { return orderList; }
        }

        public Customer(string name, string email)
        {
            customerName = name;
            emailAddress = email;
            orderList = new List<Order>();
        }

        public string Name { get { return customerName; } }
        public void AddOrder(Order order)
        {
            orderList.Add(order);
        }

        public void RemoveOrder(Order order)
        {
            orderList.Remove(order);
        }

        public void DisplayAllOrders()
        {
            if (orderList.Count == 0)
            {
                Console.WriteLine("No orders found.");
                return;
            }

            foreach (Order order in orderList)
            {
                Console.WriteLine(order);
            }
        }
    }
}