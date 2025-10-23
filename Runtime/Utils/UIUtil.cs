using UnityEngine;

namespace Potency.Services.Runtime.Utils
{
	public static class UIUtil
	{
		public static void StretchFillParent(this RectTransform childRect)
		{
			childRect.anchorMin = new Vector2(0, 0);
			childRect.anchorMax = new Vector2(1, 1);
			childRect.offsetMin = new Vector2(0, 0);
			childRect.offsetMax = new Vector2(0, 0);
		}
        
		public static Vector3 GetCombinedScale(this Transform uiElement)
		{
			Vector3 combinedScale = Vector3.one;

			// Iterate through the parent hierarchy
			while (uiElement != null)
			{
				combinedScale = Vector3.Scale(combinedScale, uiElement.localScale);
				uiElement = uiElement.parent;
			}

			return combinedScale;
		}
	}
}