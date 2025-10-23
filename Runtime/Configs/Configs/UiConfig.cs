using UnityEngine;

namespace Potency.Services.Runtime.Configs.Configs
{
    [CreateAssetMenu(fileName = "UiConfig", menuName = "Potency/Configs/UiConfig", order = 0)]
    public class UiConfig : ScriptableObject, IConfig
    {
        public GameObject UIPrefab;
    }
}