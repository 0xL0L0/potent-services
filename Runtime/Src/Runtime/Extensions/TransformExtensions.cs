using UnityEngine;

namespace Potency.Services.Runtime.Extensions
{
    public static class TransformExtensions
    {
        public static void SetLocalScaleX(this Transform transform, float x)
        {
            Vector3 localScale = transform.localScale;
            localScale.x = x;
            transform.localScale = localScale;
        }

        public static void SetEulerAngleZ(this Transform transform, float z)
        {
            Vector3 eulerAngles = transform.eulerAngles;
            eulerAngles.z = z;
            transform.eulerAngles = eulerAngles;
        }

        public static void SetPositionX(this Transform transform, float x)
        {
            Vector3 position = transform.position;
            position.x = x;
            transform.position = position;
        }
    
        public static void SetPositionY(this Transform transform, float y)
        {
            Vector3 position = transform.position;
            position.y = y;
            transform.position = position;
        }
    
        public static void SetLocalPositionX(this Transform transform, float x)
        {
            Vector3 position = transform.localPosition;
            position.x = x;
            transform.localPosition = position;
        }
    
        public static void SetLocalPositionY(this Transform transform, float y)
        {
            Vector3 position = transform.localPosition;
            position.y = y;
            transform.localPosition = position;
        }

        public static void SetSizeDeltaX(this RectTransform rectTransform, float x)
        {
            Vector2 sizeDelta = rectTransform.sizeDelta;
            sizeDelta.x = x;
            rectTransform.sizeDelta = sizeDelta;
        }
        
        public static void SetAnchorMinY(this RectTransform rectTransform, float y)
        {
            Vector2 anchorMin = rectTransform.anchorMin;
            anchorMin.y = y;
            rectTransform.anchorMin = anchorMin;
        }
    }
}