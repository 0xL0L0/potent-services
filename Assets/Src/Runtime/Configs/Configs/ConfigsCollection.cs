using System.Collections.Generic;
using UnityEngine;

namespace Potency.Services.Runtime.Configs.Configs
{
    [CreateAssetMenu(fileName = "ConfigsCollection", menuName = "Potency/Configs/ConfigsCollection", order = 0)]
    public class ConfigsCollection : ScriptableObject
    {
        public List<ScriptableObject> Configs;
    }
}
