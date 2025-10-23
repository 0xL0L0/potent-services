using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Potency.Services.Runtime.Utils.Logging;
using UnityEngine;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Potency.Services.Runtime.AssetResolver
{
	public class AssetResolver : IAssetResolver
	{
		private Dictionary<Type, Dictionary<object, AssetReference>> _assetMap = new();
		private Dictionary<Type, Dictionary<object, AssetBundle>> _bundleMap = new();
		private Dictionary<string, AssetBundle> _loadedBundles = new();

		public async Task LoadAssets<TId, TRef, TAsset>(AssetConfigScriptableObject<TId,TRef,TAsset> assets,
			bool loadAsynchronously = true, Action onLoadCallback = null) 
			where TAsset : Object
			where TRef : AssetReference
		{
			var tasks = new List<Task<TAsset>>();
			var dictionary = new Dictionary<TId, TRef>();
			
			foreach (var pair in assets.ConfigsDictionary)
			{
				var id = pair.Key;
				var operation = pair.Value.LoadAssetAsync<TAsset>();
				
				dictionary.Add(id, pair.Value);
				
				tasks.Add(operation.Task);
				
				if (!loadAsynchronously)
				{
					operation.WaitForCompletion();
				}
			}
			
			await Task.WhenAll(tasks);
            
			AddToAssetMap(assets);
			PLog.Info($"LOADED assets - {assets.name}");
			
			onLoadCallback?.Invoke();
		}

		public void UnloadAssets<TId, TRef, TAsset>(AssetConfigScriptableObject<TId, TRef, TAsset> assets) 
			where TAsset : Object
			where TRef : AssetReference
		{
			foreach (var pair in assets.Configs)
			{
				if (pair.Value.IsValid())
				{
					pair.Value.ReleaseAsset();
				}
			}
			
			RemoveFromAssetMap(assets);
			
			PLog.Info($"UNLOADED assets - {assets.name}");
		}

		public TAsset RequestAsset<TId, TAsset>(TId id) where TAsset :Object
		{
			if (_bundleMap.TryGetValue(typeof(TAsset), out var bundleMap))
			{
				string idString = id.ToString();
				
				if (bundleMap.ContainsKey(idString))
				{
					var asset = (TAsset) bundleMap[idString].LoadAsset(idString);
					if (asset) { return asset; }
				}
			}
			
			if (_assetMap.TryGetValue(typeof(TAsset), out var refMap))
			{
				return RequestAddressable<TId, TAsset>(id, refMap);
			}
			
			throw new IndexOutOfRangeException($"Nor AddressableMap neither BundleMap does not have {typeof(TAsset)} ref map.");
		}

		private TAsset RequestAddressable<TId, TAsset>(TId id, Dictionary<object, AssetReference> addressableMap) where TAsset : Object
		{
			if (!addressableMap.TryGetValue(id, out var addressableRef))
			{
				throw new IndexOutOfRangeException($"Addressable RefMap does not have an asset ref for id {id}.");
			}
            
			return (TAsset) addressableRef.Asset;
		}

		private void AddToAssetMap<TId, TRef, TAsset>(AssetConfigScriptableObject<TId, TRef, TAsset> assets) where TRef : AssetReference
		{
			Dictionary<object, AssetReference> refMap = null;
			
			if (!_assetMap.TryGetValue(typeof(TAsset), out refMap))
			{
				refMap = new Dictionary<object, AssetReference>();
				_assetMap.Add(typeof(TAsset), refMap);
			}

			foreach (var pairs in assets.Configs)
			{
				refMap.Add(pairs.Key, pairs.Value);
			}
		}
		
		private void RemoveFromAssetMap<TId, TRef, TAsset>(AssetConfigScriptableObject<TId, TRef, TAsset> assets) where TRef : AssetReference
		{
			Dictionary<object, AssetReference> refMap = null;

			if (!_assetMap.TryGetValue(typeof(TAsset), out refMap))
			{
				throw new IndexOutOfRangeException("Asset map missing nested ref map");
			}

			foreach (var pairs in assets.Configs)
			{
				refMap.Remove(pairs.Key);
			}

			if (refMap.Count == 0)
			{
				_assetMap.Remove(typeof(TAsset));
			}
		}
	}
}