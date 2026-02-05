using System;
using System.Collections.Generic;
using System.IO;

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
    }
}