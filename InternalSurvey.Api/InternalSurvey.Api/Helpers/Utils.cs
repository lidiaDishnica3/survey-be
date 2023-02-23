using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternalSurvey.Api.Helpers
{
    public static class Utils
    {
        public static bool LogicDelete(DateTime startDate)
        {
            return startDate <= DateTime.Now;
        }
        public static void ForEach<T>(this IEnumerable<T> enumeration, Action<T> action)
        {
            foreach (T item in enumeration)
            {
                action(item);
            }
        }
        public static bool HasExpired(DateTime date)
        {
            if (DateTime.Now > date)
            {
                return true;
            }
            return false;

        } 
        public static string[]  ConvertStringToList(string items)
        {
            if (string.IsNullOrEmpty(items))
            {
                return new string[] { };
            }
            return items.Split(";");            
        }
    }
}
