using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using SCG = System.Collections.Generic;
using Godot.Collections;
using System.Linq;
using Array = Godot.Collections.Array;

namespace scripts.Utils
{
	[SuppressMessage("ReSharper", "SuggestBaseTypeForParameter")]
	public static class Extensions
	{
		private static IEnumerable<KeyValuePair<TKey,TVal>> CastEach<TKey, TVal>(this Dictionary source)
		{
			return
				from DictionaryEntry entry in source
				select new KeyValuePair<TKey, TVal>((TKey)entry.Key, (TVal)entry.Value);
		}

		public static SCG.Dictionary<TKey, TVal> CastDict<TKey, TVal>(this Dictionary source)
		{
			return source.CastEach<TKey, TVal>().ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
		}

		public static IEnumerable<T> CastArr<T>(this Array source)
		{
			return
				from object element in source
				select (T)element;
		}
	}
}
