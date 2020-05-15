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
                Size = new Size(1,1)
            };

            Apple green_apple = new Apple()
            {
                Color = Color.Green,
                Weight = 1,
                Size = new Size(10,10)
            };

            // Register the state of red apple's 
            // marked members
            // (usually called from within the object)
            red_apple.Tracker.Register(red_apple);

            // copy the tracked values over to green apple
            red_apple.Tracker.AddTo(red_apple, green_apple);
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