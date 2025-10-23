using System;
using UnityEngine;

namespace Potency.Services.Runtime.Tick
{
    /// <summary>
    /// This is a service component of <see cref="ITickService"/> to process/call updates
    /// </summary>
    public class TickServiceMonoComponent : MonoBehaviour
    {
        public Action OnUpdate;
        public Action OnFixedUpdate;
        public Action OnLateUpdate;

        private void Update()
        {
            OnUpdate?.Invoke();
        }
        private void FixedUpdate()
        {
            OnFixedUpdate?.Invoke();
        }
        private void LateUpdate()
        {
            OnLateUpdate?.Invoke();
        }
    }
}
