using System;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;
using Object = UnityEngine.Object;

namespace Potency.Services.Runtime.AssetResolver
{
	// TODO - V2 implementation
	// - Split adding configs, and loading the assets
	// - When configs are added, assets can be loaded all at once, or lazy loaded when requested
	// - Add SceneLoading/Unloading addressable functionality
	public interface IAssetResolver
	{
		Task LoadAssets<TId, TRef, TAsset>(AssetConfigScriptableObject<TId,TRef,TAsset> assets,
			bool loadAsynchronously = true, Action onLoadCallback = null) 
			where TAsset : Object
			where TRef : AssetReference;

		void UnloadAssets<TId, TRef, TAsset>(AssetConfigScriptableObject<TId,TRef,TAsset> assets) 
			where TAsset : Object
			where TRef : AssetReference;
		
		TAsset RequestAsset<TId, TAsset>(TId id) where TAsset :Object;
	}
}