using System.Collections;
using MonoBehaviour = UnityEngine.MonoBehaviour;

namespace Potency.Services.Runtime.Utils.Coroutine
{
    /// <summary>
    /// Coroutines run on this object when called from external scripts through <see cref="CoroutineService"/>
    /// </summary>
    public class CoroutineServiceMonoComponent : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public UnityEngine.Coroutine ExternalStartCoroutine(IEnumerator routine)
        {
            return StartCoroutine(routine);
        }
        
        public void ExternalStopCoroutine(UnityEngine.Coroutine coroutine)
        {
            StopCoroutine(coroutine);
        }
    }
}