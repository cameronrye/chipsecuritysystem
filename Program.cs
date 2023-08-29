using System;
using System.Collections.Generic;
using System.Linq;

namespace ChipSecuritySystem
{
    internal class Program
    {
        // List of all possible colors
        private static readonly List<ColorChip> Chips = new List<ColorChip>();

        // Random number generator
        private static readonly Random Rand = new Random();

        private static void Main()
        {
            // Clear console and display options
            Console.Clear();
            Console.WriteLine("Chips Required to Unlock Master Panel\n");
            Console.WriteLine("1. Try your luck with randomly generated chips.");
            Console.WriteLine("2. Manually enter chips.");
            Console.WriteLine("3. Use example chips: [Blue, Yellow] [Red, Green] [Yellow, Red] [Orange, Purple].");
            
            // Validate input
            int option;
            while (!int.TryParse(Console.ReadLine(), out option) || (option != 1 && option != 2 && option != 3))
            {
                Console.WriteLine("Invalid input. Please enter 1, 2 or 3.");
            }
            
            // Run routine based on user input
            switch (option)
            {
                case 1:
                    // Generate random chips
                    GenerateChips();
                    break;
                case 2:
                    // Get user input for chips
                    InputChips();
                    break;
                case 3:
                    // Generate Sample Chips
                    Chips.Add(new ColorChip(Color.Blue, Color.Yellow));
                    Chips.Add(new ColorChip(Color.Red, Color.Green));
                    Chips.Add(new ColorChip(Color.Yellow, Color.Red));
                    Chips.Add(new ColorChip(Color.Orange, Color.Purple));
                    break;
            }

            Console.WriteLine("\nYour Chips\n----------\n");

            // Display chips
            foreach (var chip in Chips)
            {
                Console.WriteLine("[" + chip + "]");
            }
            Console.WriteLine(); // New line

            var chipSet = new HashSet<ColorChip>();

            // Find a chain of chips that unlocks the master panel
            foreach (var chip in Chips.Where(chip => chip.StartColor == Color.Blue))
            {
                // Add chip to set and check if a chain is found
                chipSet.Add(chip);
                if (FindChain(chip, chipSet))
                {
                    Console.WriteLine("Correct Sequence\n----------------");

                    // Display chain
                    foreach (var c in chipSet)
                    {
                        Console.WriteLine("[" + c + "]");
                    }

                    // Display success message
                    Console.WriteLine("\nMaster panel unlocked!\n");
                    return;
                }

                // Remove chip from set if no chain is found
                chipSet.Remove(chip);
            }

            // Display error message if no chain is found
            Console.WriteLine(Constants.ErrorMessage + "!\n");
        }

        // Generate random chips
        private static void GenerateChips()
        {
            // Generate four chips
            for (var i = 0; i < 4; i++)
            {
                var startColor = (Color)Rand.Next(0, 6);
                var endColor = (Color)Rand.Next(0, 6);
                Chips.Add(new ColorChip(startColor, endColor));
            }
        }

        // Get user input for chips
        private static void InputChips()
        {
            Console.WriteLine("\nEnter colors for four chips. Choices:");

            // Display color choices
            var values = Enum.GetValues(typeof(Color));
            for (var j = 0; j < values.Length; j++)
            {
                Console.WriteLine($"{j + 1}. {values.GetValue(j)}");
            }

            // Get input for each chip
            for (var i = 0; i < 4; i++)
            {
                Console.WriteLine($"\nChip {i + 1}:");

                // Get start and end color
                Color startColor = GetColorInput(nameof(startColor));
                Color endColor = GetColorInput(nameof(endColor));

                // Add chip to list
                Chips.Add(new ColorChip(startColor, endColor));
            }
        }

        // Get color input from user
        private static Color GetColorInput(string colorType)
        {
            Console.Write($"{colorType}: ");

            // Validate input
            int input;
            while (!int.TryParse(Console.ReadLine(), out input)
                || input < 1 || input > Enum.GetValues(typeof(Color)).Length)
            {
                Console.WriteLine("Invalid input. Please try again.");
            }
            return (Color)(input - 1);
        }

        // Recursive method to find a chain of chips that unlocks the master panel
        // Returns true if a chain is found, false otherwise
        private static bool FindChain(ColorChip lastChip, ISet<ColorChip> chipSet)
        {
            // Check if chain is found
            if (lastChip.EndColor == Color.Green)
            {
                return true;
            }

            // Check if chain is possible
            foreach (var nextChip in Chips.Where(c => c.StartColor == lastChip.EndColor))
            {

                // Check if chip is already in set
                if (!chipSet.Add(nextChip)) continue;

                // Check if chain is found
                if (FindChain(nextChip, chipSet))
                {
                    return true;
                }

                // Remove chip from set if no chain is found
                chipSet.Remove(nextChip);
            }

            // Return false if no chain is found
            return false;
        }
    }
}
