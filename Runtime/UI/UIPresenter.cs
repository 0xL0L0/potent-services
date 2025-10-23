using System;
using UnityEngine;

namespace Potency.Services.Runtime.UI
{
    public abstract class UIView : MonoBehaviour, IUIView
    {
        public Type ViewType => GetType();
        
        public void Dispose()
        {
            Destroy(gameObject);
        }

        public void SetVisible(bool isVisible)
        {
            gameObject.SetActive(isVisible);
        }
    }
    
    public abstract class UIView<T> : UIView where T : struct
    {
        protected T Model;

        public void Init(T model)
        {
            Model = model;
        }
    }
}