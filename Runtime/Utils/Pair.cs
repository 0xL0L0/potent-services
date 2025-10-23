using System;

namespace Potency.Services.Runtime.Utils
{
	[Serializable]
	public struct Pair<TKey, TValue>
	{
		public TKey Key;
		public TValue Value;

		public Pair(TKey key, TValue value)
		{
			Key = key;
			Value = value;
		}

		public void SetKey(TKey key)
		{
			Key = key;
		}

		public void SetValue(TValue value)
		{
			Value = value;
		}

		public override string ToString()
		{
			return $"[{Key.ToString()},{Value.ToString()}]";
		}
	}
}