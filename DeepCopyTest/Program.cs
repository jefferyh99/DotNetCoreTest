using System;
using System.Collections.Generic;
using System.Reflection;

namespace DeepCopyTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var room1 = new Room()
            {
                Name = "Room1",
                TableName = new List<string>() { "Table1", "Table2" },
                Windows = new List<Window>() 
                {
                    new Window()
                    {
                        Name = "window1",
                        Color = "red",
                        GlassCount = 2
                    },
                    new Window()
                    {
                        Name = "window2",
                        Color = "blue",
                        GlassCount = 1
                    }
                }
            };

            var clone = DeepCopy<Room>(room1);

            var a = clone.Windows[0].Name;
            var b = clone.Windows[0].Color;

            var c = room1 == clone;

            Console.WriteLine("Hello World!");
        }

        /// <summary>
        /// 有问题
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static T DeepCopy<T>(T obj)
        {
            //如果是字符串或值类型则直接返回
            if (obj is string || obj.GetType().IsValueType) return obj;

            object retval = Activator.CreateInstance(obj.GetType());
            FieldInfo[] fields = obj.GetType().GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            foreach (FieldInfo field in fields)
            {
                try { field.SetValue(retval, DeepCopy(field.GetValue(obj))); }
                catch { }
            }
            return (T)retval;
        }
    }

    public class Room
    {
        public List<Window> Windows { get; set; }

        public string Name { get; set; }

        public List<string> TableName { get; set; }
    }

    public class Window
    {
        public string Name { get; set; }

        public string Color { get; set; }

        public int GlassCount { get; set; }
    }
}
