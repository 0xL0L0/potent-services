using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Potency.Services.Runtime.Configs;
using Potency.Services.Runtime.Utils;
using UnityEngine;
#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Potency.Services.Runtime.AssetResolver
{
	public interface IAssetConfig<T> : IConfig where T : struct
	{
		List<T> Configs { get; set; }
	}

	public interface IAssetPairConfig<TKey, TValue, TAsset> : IAssetConfig<Pair<TKey, TValue>>

	{
		Type AssetType { get; }
	}

	// Refactor to TId, TAsset, TRef, Pair<TId, TAsset>
	public abstract class AssetConfigScriptableObject<TId, TRef, TAsset> : ScriptableObject,
		IAssetPairConfig<TId, TRef, TAsset>, ISerializationCallbackReceiver
	{
#if ODIN_INSPECTOR
		[TableList(AlwaysExpanded = true), Searchable] [SerializeField]
#endif
		private List<Pair<TId, TRef>> _configs = new();

		public Type AssetType => typeof(TAsset);

		public List<Pair<TId, TRef>> Configs
		{
			get => _configs;
			set => _configs = value;
		}

		public IReadOnlyDictionary<TId, TRef> ConfigsDictionary { get; private set; }

		public void OnBeforeSerialize()
		{
		}

		public virtual void OnAfterDeserialize()
		{
			var dictionary = new Dictionary<TId, TRef>();

			foreach (var config in Configs)
			{
				dictionary.Add(config.Key, config.Value);
			}

			ConfigsDictionary = new ReadOnlyDictionary<TId, TRef>(dictionary);
		}
	}
}