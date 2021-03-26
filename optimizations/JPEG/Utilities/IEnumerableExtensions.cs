using System;
using System.Collections.Generic;
using System.Linq;

namespace JPEG.Utilities
{
	static class IEnumerableExtensions
	{
		public static T MinOrDefault<T>(this IEnumerable<T> enumerable, Func<T, int> selector)
		{
			return enumerable.OrderBy(selector).FirstOrDefault();
		}

		public static IEnumerable<T> Without<T>(this IEnumerable<T> enumerable, params T[] elements)
		{
			return enumerable.Where(x => !elements.Contains(x));
		}
		
		public static IEnumerable<T> ToEnumerable<T>(this T element)
		{
			yield return element;
		}
	}
}