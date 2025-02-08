using System;
using System.Collections.Generic;
using System.IO;

namespace Grocery_Store_Discount_Calculator
{
    public class Inventory
    {
        private const string filePath = "C:\\Users\\owner\\source\\repos\\GroceryStoreDiscountCalculator\\GroceryStoreDiscountCalculator\\inventory.txt";
        private List<Item> items = new List<Item>();

        public Inventory()
        {
            LoadInventory();
        }

        public void AddItem(string name, int quantity, decimal price)
        {
            items.Add(new Item { Name = name, Quantity = quantity, Price = price });
            SaveInventory();
        }

        public void UpdateItem(string name, int quantity, decimal price)
        {
            var item = items.Find(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                item.Quantity = quantity;
                item.Price = price;
                SaveInventory();
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        public void DeleteItem(string name)
        {
            var item = items.Find(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            if (item != null)
            {
                items.Remove(item);
                SaveInventory();
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        public void DisplayAllItems()
        {
            Console.Clear();
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine(" Item Name\t\tQuantity\t\tPrice");
            Console.WriteLine("-----------------------------------------------------------");
            foreach (var item in items)
            {
                Console.WriteLine($"{item.Name}\t\t\t{item.Quantity}\t\t\t${item.Price:F2}");
            }
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("Press any key to return to the Admin Interface...");
            Console.ReadKey();
        }

        public Item? FindItem(string name)
        {
            return items.Find(i => i.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public void SaveInventory()
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (var item in items)
                {
                    writer.WriteLine($"{item.Name},{item.Quantity},{item.Price}");
                }
            }
        }

        private void LoadInventory()
        {
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                foreach (string line in lines)
                {
                    string[] parts = line.Split(',');
                    if (parts.Length == 3)
                    {
                        items.Add(new Item
                        {
                            Name = parts[0],
                            Quantity = int.Parse(parts[1]),
                            Price = decimal.Parse(parts[2])
                        });
                    }
                }
            }
        }
    }

    public class Item
    {
        public required string Name { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }



    namespace Grocery_Store_Discount_Calculator
    {
        internal class Program
        {
            private static Inventory inventory = new Inventory();

            static void Main(string[] args)
            {
                while (true)
                {
                    Console.Clear();
                    Console.WriteLine("-----------------------------------------------------------");
                    Console.WriteLine("\n\t\tWelcome to the System");
                    Console.WriteLine("\n-----------------------------------------------------------");
                    Console.WriteLine("\n1. Admin");
                    Console.WriteLine("2. Cashier");
                    Console.WriteLine("3. Exit Program");
                    Console.WriteLine("\n-----------------------------------------------------------");
                    Console.Write("Please select an option: ");
                    string option = Console.ReadLine() ?? string.Empty;

                    if (option == "1")
                    {
                        AdminInterface();
                    }
                    else if (option == "2")
                    {
                        CashierInterface();
                    }
                    else if (option == "3")
                    {
                        Console.WriteLine("Exiting the system...");
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please try again.");
                    }
                }
            }

            static void AdminInterface()
            {
                string choice;
                do
                {
                    Console.Clear();
                    Console.WriteLine("-----------------------------------------------------------");
                    Console.WriteLine("\nAdmin Interface");
                    Console.WriteLine("1. Add Item");
                    Console.WriteLine("2. Display All Items");
                    Console.WriteLine("3. Update Item");
                    Console.WriteLine("4. Delete Item");
                    Console.WriteLine("5. To Main Menu");
                    Console.WriteLine("\n-----------------------------------------------------------");
                    Console.Write("Please select an option: ");
                    choice = Console.ReadLine() ?? string.Empty;

                    if (choice == "1")
                    {
                        AddItem();
                    }
                    else if (choice == "2")
                    {
                        inventory.DisplayAllItems();
                    }
                    else if (choice == "3")
                    {
                        UpdateItem();
                    }
                    else if (choice == "4")
                    {
                        DeleteItem();
                    }
                    else if (choice == "5")
                    {
                        Console.WriteLine("Exiting Admin Interface...");
                    }
                    else
                    {
                        Console.WriteLine("Invalid option. Please try again.");
                    }
                } while (choice != "5");
            }

            static void AddItem()
            {
                Console.Clear();
                Console.WriteLine("-----------------------------------------------------------");
                Console.Write("\nEnter an item name: ");
                string itemName = Console.ReadLine() ?? string.Empty;

                // Check if the item already exists in the inventory
                var existingItem = inventory.FindItem(itemName);
                if (existingItem != null)
                {
                    Console.WriteLine("Exception: Item already exists in the inventory.");
                    return;
                }

                Console.Write("Enter quantity: ");
                int quantity;
                while (!int.TryParse(Console.ReadLine(), out quantity))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for quantity.");
                    Console.Write("Enter quantity: ");
                }
                Console.Write("Enter price: ");
                decimal price;
                while (!decimal.TryParse(Console.ReadLine(), out price))
                {
                    Console.WriteLine("Invalid input. Please enter a valid decimal for price.");
                    Console.Write("Enter price: ");
                    Console.WriteLine("\n-----------------------------------------------------------");
                }

                inventory.AddItem(itemName, quantity, price);
            }



            static void UpdateItem()
            {
                Console.Clear();
                Console.Write("Enter the item name to update: ");
                string item = Console.ReadLine() ?? string.Empty;

                Console.Write("Enter new quantity: ");
                int quantity;
                while (!int.TryParse(Console.ReadLine(), out quantity))
                {
                    Console.WriteLine("Invalid input. Please enter a valid integer for quantity.");
                    Console.Write("Enter new quantity: ");
                }

                Console.Write("Enter new price: ");
                decimal price;
                while (!decimal.TryParse(Console.ReadLine(), out price))
                {
                    Console.WriteLine("Invalid input. Please enter a valid decimal for price.");
                    Console.Write("Enter new price: ");
                }

                inventory.UpdateItem(item, quantity, price);
            }

            static void DeleteItem()
            {
                Console.Clear();
                Console.Write("Enter the item name to delete: ");
                string item = Console.ReadLine() ?? string.Empty;

                inventory.DeleteItem(item);
            }

            static void CashierInterface()
            {
                Console.Clear();
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine("\nWelcome to the Cashier Interface");
                string choice;
                decimal totalCost = 0.00m, discount = 0.00m, finalCost = 0.000m;

                // Temporary storage for the current session
                var sessionItems = new System.Collections.Generic.List<Item>();

                do
                {
                    Console.Write("Enter an item name: ");
                    string item = Console.ReadLine() ?? string.Empty;

                    var existingItem = inventory.FindItem(item);
                    if (existingItem != null)
                    {
                        Console.Write("Enter quantity: ");
                        int quantity;
                        while (!int.TryParse(Console.ReadLine(), out quantity) || quantity > existingItem.Quantity)
                        {
                            if (quantity > existingItem.Quantity)
                            {
                                Console.WriteLine("Entered quantity exceeds available stock. Please enter a quantity that is equal or less than to the available stocks.");
                            }
                            else
                            {
                                Console.WriteLine("Invalid input. Please enter a valid integer for quantity.");
                            }
                            Console.Write("Enter quantity: ");
                            Console.WriteLine("\n----------------------------------------------------------");
                        }
                        totalCost += existingItem.Price * quantity;

                        // Store the item details in the session list
                        sessionItems.Add(new Item { Name = existingItem.Name, Quantity = quantity, Price = existingItem.Price });

                        // Update the inventory
                        existingItem.Quantity -= quantity;

                        // Save the updated inventory to the file
                        inventory.SaveInventory();

                        Console.Write("Do you want to enter another item? (YES/NO): ");
                        choice = (Console.ReadLine() ?? string.Empty).ToUpper();
                    }
                    else
                    {
                        Console.WriteLine("Item does not exist. Please enter a valid item name.");
                        choice = "YES";
                    }
                } while (choice != "NO");

                if (totalCost > 500) discount = totalCost * .20m;
                else if (totalCost > 200) discount = totalCost * .15m;
                else if (totalCost > 100) discount = totalCost * .10m;
                else discount = 0.00m;

                finalCost = totalCost - discount;
                Console.Clear();
                Console.WriteLine("\n-----------------------------------------------------------");
                Console.WriteLine("\n\t\t\tRECEIPT");
                Console.WriteLine("\n-----------------------------------------------------------");
                Console.WriteLine("Item/s\t\t\tQuantity\t\tPrice");
                Console.WriteLine("-----------------------------------------------------------");
                foreach (var sessionItem in sessionItems)
                {
                    Console.WriteLine($"{sessionItem.Name}\t\t\t{sessionItem.Quantity}\t\t\t${sessionItem.Price:F2}");
                }
                Console.WriteLine("-----------------------------------------------------------");
                Console.WriteLine($"Total Cost:\t\t\t\t\t${totalCost:F2}");
                Console.WriteLine($"Discount:\t\t\t\t\t-${discount:F2}");
                Console.WriteLine($"Final Cost:\t\t\t\t\t ${finalCost:F2}");
                Console.WriteLine("-----------------------------------------------------------");

                Console.WriteLine("Press any key to return to the main menu...");
                Console.ReadKey();
            }
        }
    }
}
