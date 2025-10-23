using UnityEngine;

namespace Potency.Services.Runtime.Tutorial
{
    public class TutorialTargetMonoComponent : MonoBehaviour
    {
        [field : SerializeField] public string Id { get; private set; }
        [field : SerializeField] public bool Is3dSpaceTarget { get; private set; }

        private RectTransform _rectTransform;

        public RectTransform RectTransform
        {
            get
            {
                if(_rectTransform == null)
                {
                    _rectTransform = GetComponent<RectTransform>();
                }
                
                return _rectTransform;
            }
        }
    }
}
