using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;

namespace ClassTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var apple1 = new Apple();
            var apple2 = new Apple()
            {
                skinColor = Color.Green,
                
            };

            apple2.size = new Size(69, 69);
            //var l = apple2.CheckChanged();

            apple2.AddTo(apple1);
        }
    }

    public class Apple
    {
        public ClassTracker<Apple> PropertyList { get; }     

        public Color skinColor { get; set; }
        public Size size { get; set; }
        
        public int weight { get; set; }

        private int age;

        public Apple()
        {
            skinColor = Color.Red;
            size = new Size(1, 1);
            weight = 35;

            PropertyList = new ClassTracker<Apple>();

            PropertyList.Register(this);
        }

        public void AddTo(Apple b)
        {
            PropertyList.AddTo(this, b);
        }

        public IEnumerable<(string name, object value)> CheckChanged()
        {
            return PropertyList.CheckChanged(this);
        }
    }
}