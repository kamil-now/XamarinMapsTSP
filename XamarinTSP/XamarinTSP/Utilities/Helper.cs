using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinTSP.TSP;

namespace XamarinTSP.Utilities
{
    public static class Helper
    {
        private static Random random = new Random();
        public static double RandomPercent() => random.NextDouble();
        public static int Random(int val) => random.Next(val);
        public static int Random(int val, int max) => random.Next(val, max);
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
        public static IEnumerable<T> DistinctBy<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
        {
            return items.GroupBy(property).Select(x => x.First());
        }
        public static IEnumerable<int> GetRandomData(int size)
        {
            var data = Enumerable.Range(0, size).ToList();
            data.Shuffle();
            return data;
        }

        public static string GetDescription(Enum value)
        {
            return
                value
                    .GetType()
                    .GetMember(value.ToString())
                    .FirstOrDefault()
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
        }
        public static string GetDescription(PropertyInfo property)
        {
            return property
                    ?.GetCustomAttribute<DescriptionAttribute>()
                    ?.Description;
        }
        public static IEnumerable<Enum> GetFlags(Enum input)
        {
            foreach (Enum value in Enum.GetValues(input.GetType()))
                if (input.HasFlag(value))
                    yield return value;
        }
        public static string GetTimestamp(DateTime time) => ((int)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds).ToString();
        public static void Display(Population population)
        {
            //TODO
        }
        public static Task InvokeOnMainThreadAsync(Action action, int delay = 0)
        {
            var task = new TaskCompletionSource<object>();
            Device.BeginInvokeOnMainThread(async () =>
            {
                try
                {
                    if (delay > 0)
                    {
                        await Task.Delay(delay);
                    }
                    action();
                    task.SetResult(null);
                }
                catch (Exception ex)
                {
                    task.SetException(ex);
                }
            }); return task.Task;
        }
    }
}
