using UnityEngine;

namespace Potency.Services.Runtime.UI
{
    public class UIMonoComponent : MonoBehaviour
    {
        public Transform ViewsTransform;
        public Transform PopupsTransform;

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}