using System.Collections;
using UnityEngine;

namespace Potency.Services.Runtime.Utils.Coroutine
{
    /// <inheritdoc cref="ICoroutineService"/>
    public class CoroutineService : ICoroutineService
    {
        private CoroutineServiceMonoComponent _coroutineComponent;

        public CoroutineService()
        {
            var gameObject = new GameObject(nameof(CoroutineServiceMonoComponent));

            _coroutineComponent = gameObject.AddComponent<CoroutineServiceMonoComponent>();
			
            Object.DontDestroyOnLoad(gameObject);
        }

        public UnityEngine.Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return _coroutineComponent.ExternalStartCoroutine(coroutine);
        }

        public void StopCoroutine(UnityEngine.Coroutine coroutine)
        {
            _coroutineComponent.ExternalStopCoroutine(coroutine);
        }
    }
}