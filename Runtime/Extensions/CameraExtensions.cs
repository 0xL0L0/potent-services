using UnityEngine;

namespace Potency.Services.Runtime.Extensions
{
    public static class CameraExtensions
    {
        public static void GetViewFrustumSize(this Camera camera, Transform targetTransform, out float frustumHeight, out float frustumWidth)
        {
            float distanceToFrustumCenter = Vector3.Distance(camera.transform.position, targetTransform.position);
            float fov = camera.fieldOfView;
            frustumHeight = 2.0f * distanceToFrustumCenter * Mathf.Tan(fov * 0.5f * Mathf.Deg2Rad);
            frustumWidth = frustumHeight * camera.aspect;
        }
        
        public static void ForceAspectRatio(this Camera camera, float aspectRatio)
        {
            float currentAspectRatio = camera.aspect;
            float ratio = currentAspectRatio / aspectRatio;
            Rect rect = camera.rect;
 
            if (ratio < 1)
            {
                rect.width = 1;
                rect.height = ratio;
                rect.x = 0;
                rect.y = (1 - ratio) / 2;
            }
            else
            {
                ratio = 1 / ratio;
                rect.width = ratio;
                rect.height = 1;
                rect.x = (1 - ratio) / 2;
                rect.y = 0;
            }
 
            camera.rect = rect;
            camera.aspect = aspectRatio;
        }
    }
}