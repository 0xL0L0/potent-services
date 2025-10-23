using System;
using UnityEngine;

namespace Potency.Services.Runtime.Utils
{
    public class UnityCallbacksMonoComponent : MonoBehaviour
    {
        public Action<bool> OnApplicationChangedFocus;
        public Action<bool> OnApplicationPaused;
        public Action OnApplicationHasQuit;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            OnApplicationChangedFocus?.Invoke(hasFocus);
        }
        
        private void OnApplicationPause(bool pauseStatus)
        {
            OnApplicationPaused?.Invoke(pauseStatus);
        }

        private void OnApplicationQuit()
        {
            OnApplicationHasQuit?.Invoke();
        }
    }
}
