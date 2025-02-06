namespace Grocery_Store_Discount_Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const int maxItems = 100;
            string[] items = new string[maxItems];
            int[] quantities = new int[maxItems];
            decimal[] prices = new decimal[maxItems];

            string choice, item;
            int quantity;
            decimal totalCost = 0.00m, discount = 0.00m, finalCost = 0.000m;

            int itemCount = 0;

            do
            {
                Console.Write("Enter an item name: ");
                item = Console.ReadLine() ?? string.Empty;


                // Check if the item already exists
                int existingItemIndex = Array.FindIndex(items, i => string.Equals(i, item, StringComparison.OrdinalIgnoreCase));
                if (existingItemIndex != -1)
                {
                    // Item exists, update the quantity
                    Console.Write("Enter additional quantity: ");
                    quantity = Convert.ToInt32(Console.ReadLine());
                    quantities[existingItemIndex] += quantity; // Update the existing quantity
                }
                else
                {
                    // Item does not exist, add it
                    Console.Write("Enter quantity: ");
                    quantity = Convert.ToInt32(Console.ReadLine());
                    Console.Write("Enter price: ");
                    decimal price = Convert.ToDecimal(Console.ReadLine());

                    items[itemCount] = item;
                    quantities[itemCount] = quantity;
                    prices[itemCount] = price;
                    itemCount++;
                }


                Console.Write("Do you want to enter another item? (YES/NO): ");
                choice = (Console.ReadLine() ?? string.Empty).ToUpper();
                Console.Clear();
            } while (choice != "NO");

            // Total Cost calculator.
            for (int i = 0; i < items.Length; i++)
            {
                totalCost += (prices[i] * quantities[i]);
            }

            // Discount calculator.
            if (totalCost > 100 && totalCost < 200) discount = totalCost * .10m;
            else if (totalCost > 200 && totalCost < 500) discount = totalCost * .15m;
            else if (totalCost > 500) discount = totalCost * .20m;
            else discount = 0.00m;

            // Final Cost calculator.
            finalCost = totalCost - discount;


            // Prints the receipt.
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("\n\t\t\tRECEIPT\t\t\t");
            Console.WriteLine("\n-----------------------------------------------------------");
            Console.WriteLine("Grocery List\t\tQuanties\t\tPrice");
            for (int i2 = 0; i2 < itemCount; ++i2)
            {
                Console.WriteLine($"{items[i2]}\t\t\t{quantities[i2]}\t\t\t${prices[i2]:F2}");
            }
            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine($"Total Cost:\t\t\t\t\t ${totalCost:F2}");
            Console.WriteLine($"Discount:\t\t\t\t\t-${discount:F2}");
            Console.WriteLine($"Final Cost:\t\t\t\t\t ${finalCost:F2}");
            Console.WriteLine("-----------------------------------------------------------");
        }
    }
}
