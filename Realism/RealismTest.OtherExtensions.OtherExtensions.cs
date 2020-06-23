// RealismTest.OtherExtensions.OtherExtensions
using Rage;
using System.Collections.Generic;
using System.Reflection;

namespace RealismTest.OtherExtensions
{
    public static class OtherExtensions
    {
        public static void Dump(this object obj)
        {
            if (obj == null)
            {
                Game.Console.Print("Object is null");
                return;
            }
            Game.Console.Print("=============================");
            Game.Console.Print("Hash: " + obj.GetHashCode());
            Game.Console.Print("Type: " + obj.GetType().ToString());
            Dictionary<string, string> properties = GetProperties(obj);
            if (properties.Count > 0)
            {
                Game.Console.Print("-------------------------");
            }
            foreach (KeyValuePair<string, string> item in properties)
            {
                Game.Console.Print(item.Key + ": " + item.Value);
            }
        }

        private static Dictionary<string, string> GetProperties(object obj)
        {
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            if (obj == null)
            {
                return dictionary;
            }
            PropertyInfo[] properties = obj.GetType().GetProperties();
            foreach (PropertyInfo propertyInfo in properties)
            {
                object value = propertyInfo.GetValue(obj, new object[0]);
                string value2 = (value == null) ? "" : value.ToString();
                dictionary.Add(propertyInfo.Name, value2);
            }
            return dictionary;
        }
    }
}