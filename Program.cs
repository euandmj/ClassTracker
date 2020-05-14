using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace ClassTracker
{
    class Program
    {
        static void Main(string[] args)
        {
            var apple1 = new Apple();
            var apple2 = new Apple()
            {
                skinColor = Color.Green
            };

            apple2.size = new Size(69, 69);
            apple2.age = 99;
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

        public int age;

        public Apple()
        {
            skinColor = Color.Red;
            size = new Size(1, 1);
            weight = 35;
            age = 5;

            PropertyList = new ClassTracker<Apple>();


            AddProperties();
        }

        public void AddProperties()
        {
            PropertyList.AddProperty(nameof(skinColor), skinColor);
            PropertyList.AddProperty(nameof(size), size);
            PropertyList.AddProperty(nameof(weight), weight);
            PropertyList.AddField(nameof(age), age);
        }

        public Apple AddTo(Apple b)
        {
            return PropertyList.AddTo(this, b);
        }

        public IEnumerable<(string name, object value)> CheckChanged()
        {
            return PropertyList.CheckChanged(this);
        }
    }
}