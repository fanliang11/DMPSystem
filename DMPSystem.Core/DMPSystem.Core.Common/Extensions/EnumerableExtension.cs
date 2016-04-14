using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;

namespace DMPSystem.Core.Common.Extensions
{
    public static class EnumerableExtension
    {


        public static TDestination BuildObjectFromRow<TDestination>(this IEnumerable enumerable, string nameColumn,
                                                                    string valueColumn)
            where TDestination : class, new()
        {
            //IEnumerable<T> 类型需要创建元素的映射
            var des = new TDestination();
            var sourceProperties = enumerable.GetType().GetProperties();
            var destinationProperties = typeof (TDestination).GetProperties();
            foreach (var source in enumerable)
            {

                var name = source.GetType().GetProperty(nameColumn).GetValue(source);
                var val = source.GetType().GetProperty(valueColumn).GetValue(source);
                if (name != null && val != null)
                {
                    var desProperty = des.GetType().GetProperty(name.ToString());
                    desProperty.SetValue(des, val);
                }
            }
            return des;
        }


        public static void ForEach<T>(this IEnumerable<T> values, Action<T> action)
        {
            foreach (var value in values)
                action(value);
        }

        public static IEnumerable<T> IgnoreNulls<T>(this IEnumerable<T> target)
        {
            if (ReferenceEquals(target, null))
                yield break;

            foreach (var item in target.Where(item => !ReferenceEquals(item, null)))
                yield return item;
        }

        public static TItem MaxItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector,
                                                   out TValue maxValue)
            where TItem : class
            where TValue : IComparable
        {
            TItem maxItem = null;
            maxValue = default(TValue);

            foreach (var item in items)
            {
                if (item == null)
                    continue;

                var itemValue = selector(item);

                if ((maxItem != null) && (itemValue.CompareTo(maxValue) <= 0))
                    continue;

                maxValue = itemValue;
                maxItem = item;
            }

            return maxItem;
        }

        public static TItem MaxItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector)
            where TItem : class
            where TValue : IComparable
        {
            TValue maxValue;

            return items.MaxItem(selector, out maxValue);
        }

        public static TItem MinItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector,
                                                   out TValue minValue)
            where TItem : class
            where TValue : IComparable
        {
            TItem minItem = null;
            minValue = default(TValue);

            foreach (var item in items)
            {
                if (item == null)
                    continue;
                var itemValue = selector(item);

                if ((minItem != null) && (itemValue.CompareTo(minValue) >= 0))
                    continue;
                minValue = itemValue;
                minItem = item;
            }

            return minItem;
        }

        public static TItem MinItem<TItem, TValue>(this IEnumerable<TItem> items, Func<TItem, TValue> selector)
            where TItem : class
            where TValue : IComparable
        {
            TValue minValue;

            return items.MinItem(selector, out minValue);
        }

        public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> source, Func<T, TKey> expression)
        {
            return source == null ? Enumerable.Empty<T>() : source.GroupBy(expression).Select(i => i.First());
        }

        public static IEnumerable<T> RemoveAll<T>(this IEnumerable<T> source, Predicate<T> predicate)
        {
            if (source == null)
                return Enumerable.Empty<T>();

            var list = source.ToList();
            list.RemoveAll(predicate);
            return list;
        }

        public static string ToCsv<T>(this IEnumerable<T> source, char separator)
        {
            if (source == null)
                return string.Empty;

            var csv = new StringBuilder();
            source.ForEach(value => csv.AppendFormat("{0}{1}", value, separator));
            return csv.ToString(0, csv.Length - 1);
        }

        public static string ToCsv<T>(this IEnumerable<T> source)
        {
            return source == null ? string.Empty : source.ToCsv(',');
        }

    }
}
