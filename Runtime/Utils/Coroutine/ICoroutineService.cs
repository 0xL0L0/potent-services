using System.Collections;

namespace Potency.Services.Runtime.Utils.Coroutine
{
    /// <summary>
    /// This service allows starting/stopping coroutines from any object, even ones that don't inherit from MonoBehaviour.
    /// </summary>
    public interface ICoroutineService
    {
        /// <summary>
        /// Starts a coroutine on an external monobehaviour
        /// </summary>
        UnityEngine.Coroutine StartCoroutine(IEnumerator routine);

        /// <summary>
        /// Stops the given coroutine on an external monobehaviour
        /// </summary>
        /// <param name="coroutine"></param>
        void StopCoroutine(UnityEngine.Coroutine coroutine);
    }
}