using System;
using System.Drawing;
using ClassTracker;


namespace ClassTracker.Examples
{
    public static class Example01
    {
        public static void DoExample()
        {
            Apple red_apple = new Apple()
            {
                Color = Color.Red,
                Weight = 10,
                Size = new Size(23, 23)
            };

            Apple green_apple = new Apple()
            {
                Color = Color.Green,
                Weight = 1,
                Size = new Size(57, 57)
            };

            // Register the state of red apple's marked members
            // (usually called from within the object)
            red_apple.Tracker.Register(red_apple);

            // print state of the red apple
            Console.WriteLine($"Red Apple:\n\tColor - {red_apple.Color}\n\tWeight - {red_apple.Weight}\n\tSize - {red_apple.Size}");
            // print state of the green apple
            Console.WriteLine($"Green Apple:\n\tColor - {green_apple.Color}\n\tWeight - {green_apple.Weight}\n\tSize - {green_apple.Size}");

            Console.WriteLine("-------------------");

            Console.WriteLine("Applying the tracked state of the Red Apple to Green Apple...");
            // copy the tracked values over to green apple
            red_apple.Tracker.AddTo(red_apple, green_apple);

            // print state of the red apple
            Console.WriteLine($"Red Apple:\n\tColor - {red_apple.Color}\n\tWeight - {red_apple.Weight}\n\tSize - {red_apple.Size}");
            // print state of the green apple
            Console.WriteLine($"Green Apple:\n\tColor - {green_apple.Color}\n\tWeight - {green_apple.Weight}\n\tSize - {green_apple.Size}");
        }
    }

    class Apple
    {
        public ClassTracker<Apple> Tracker;

        // Tracked property
        [TrackedItem]
        public Color Color { get; set; }
        
        // Tracked field
        [TrackedItem]
        public int Weight;
        
        // Non-Tracked field
        public Size Size;

        public Apple()
        {
            Tracker = new ClassTracker<Apple>();
        }
    }  

}