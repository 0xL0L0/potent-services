using UnityEngine;

namespace Potency.Services.Runtime.Extensions
{
    public static class RectTransformExtensions
    {
        public static void SetAnchoredPositionX(this RectTransform rectTransform, float x)
        {
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.x = x;
            rectTransform.anchoredPosition = anchoredPosition;
        }
        
        public static void SetAnchoredPositionY(this RectTransform rectTransform, float y)
        {
            Vector2 anchoredPosition = rectTransform.anchoredPosition;
            anchoredPosition.y = y;
            rectTransform.anchoredPosition = anchoredPosition;
        }
        
        public static Vector2 AnchoredDistanceTo(this RectTransform originRect, RectTransform targetRect)
        {
            var toDestinationInWorldSpace = targetRect.position - originRect.position;
            var toDestinationInLocalSpace = originRect.InverseTransformVector(toDestinationInWorldSpace);    
            return originRect.anchoredPosition + (Vector2)toDestinationInLocalSpace;
        }
    }
}