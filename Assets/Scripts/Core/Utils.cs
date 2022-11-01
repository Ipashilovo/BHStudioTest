using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Core
{
    public static class Utils
    {
        public static T GetRandom<T>(this IEnumerable<T> objects)
        {
            return GetRandom(objects.ToArray());
        }
        
        public static T GetRandom<T>(this T[] objects)
        {
            return objects[Random.Range(0, objects.Length)];
        }
    }
}