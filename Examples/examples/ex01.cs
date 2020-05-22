using System;
using System.Drawing;
using ClassTracker;


namespace ClassTracker.Examples
{
    public static class Example01
    {
        public static void DoExample()
        {
            Apple red_apple, green_apple;
            var tracker = new ClassTracker<Apple>();

            // Create our apples
            green_apple = new Apple()
            {
                Color = Color.Green,
                Weight = 10,
                Size = new Size(50, 50)
            };
            red_apple = new Apple()
            {
                Color = Color.Green,
                Weight = 10,
                Size = new Size(50, 50)
            };

            // Register the state of red apple's marked members
            tracker.Register(red_apple);

            // Mutate our red_apple into a small red apple
            red_apple.Color = Color.Red;
            red_apple.Weight = 1;
            red_apple.Size = new Size(5, 5);

            // print state of the red apple
            Console.WriteLine($"Red Apple:\n\tColor - {red_apple.Color}\n\tWeight - {red_apple.Weight}\n\tSize - {red_apple.Size}");
            // print state of the green apple
            Console.WriteLine($"Green Apple:\n\tColor - {green_apple.Color}\n\tWeight - {green_apple.Weight}\n\tSize - {green_apple.Size}");

            Console.WriteLine("-------------------");

            Console.WriteLine("Whats changed in Red Apple...");
            foreach(var (name, _) in tracker.CheckChanged(red_apple))
            {
                Console.WriteLine(name);
            }

            Console.WriteLine("\nApplying the tracked state of the Red Apple to Green Apple...");
            // copy the tracked values over to green apple
            tracker.AssignTo(red_apple, green_apple);

            // print state of the red apple
            Console.WriteLine($"Red Apple:\n\tColor - {red_apple.Color}\n\tWeight - {red_apple.Weight}\n\tSize - {red_apple.Size}");
            // print state of the green apple
            Console.WriteLine($"Green Apple:\n\tColor - {green_apple.Color}\n\tWeight - {green_apple.Weight}\n\tSize - {green_apple.Size}");
        }
    }

    class Apple
    {
        // Tracked property
        [TrackedItem]
        public Color Color { get; set; }
        
        // Tracked field
        [TrackedItem]
        public int Weight;
        
        // Non-Tracked field
        public Size Size;
    }  

}