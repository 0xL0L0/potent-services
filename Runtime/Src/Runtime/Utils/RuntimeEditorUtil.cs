using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.U2D;

namespace Potency.Services.Runtime.Utils
{
	public static class RuntimeEditorUtil
	{
#if UNITY_EDITOR		
		public static List<Pair<string, AssetReferenceAtlasedSprite>> GetStringAssetRefsFromAtlas(SpriteAtlas atlas)
		{
			var sprites = new Sprite[1000];
			var spriteCount = atlas.GetSprites(sprites);
			Array.Resize(ref sprites, spriteCount);
			Debug.LogError($"Sprites retrieved from atlas {spriteCount}");

			var configs = new List<Pair<string, AssetReferenceAtlasedSprite>>();

			foreach (var sprite in sprites)
			{
				var newPair = new Pair<string, AssetReferenceAtlasedSprite>();
				newPair.Key = sprite.name.Replace("(Clone)", "");

				// Create new AssetReferenceAtlasedSprite, and null out  m_DerivedClassType, before setting the _atlas and sprite.
				// This is because otherwise, you cannot properly set the atlas and sprite
				// In editor UI, when a field is added, there is a default "instance" in the field with null values, and constructor doesn't
				// run on it. Constructor for this asset ref modifies data that makes changing the asset refs programatically
				// not work, as the execution path changes. Lovely oversight and idiotic code from our favourite engine.
				newPair.Value = new AssetReferenceAtlasedSprite(null);
				var type = typeof(AssetReferenceAtlasedSprite);
				var field = type.GetField("m_DerivedClassType", BindingFlags.NonPublic | BindingFlags.Instance);
				field.SetValue(newPair.Value, null);

				newPair.Value.SetEditorAsset(atlas);
				newPair.Value.SetEditorSubObject(sprite);
				configs.Add(newPair);
			}

			return configs;
		}
#endif		
	}
}