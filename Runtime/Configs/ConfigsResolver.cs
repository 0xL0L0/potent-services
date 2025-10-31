using System;
using System.Collections.Generic;
using Potency.Services.Runtime.Configs.Configs;
using Potency.Services.Runtime.Utils.Logging;

namespace Potency.Services.Runtime.Configs
{
    public class ConfigsResolver : IConfigsResolver
    {
        private readonly Dictionary<Type, IConfig> _configsDic = new();
        
        public ConfigsResolver(ConfigsCollection configsCollection)
        {
            foreach(var configObject in configsCollection.Configs)
            {
                _configsDic.Add(configObject.GetType(), (IConfig)configObject);
                PLog.Info("CONFIGS", "Bound new config: " + configObject.name);
            }
        }

        public T GetConfig<T>() where T : IConfig
        {
            var type = typeof(T);
            
            if(_configsDic.TryGetValue(type, out var config))
            {
                return (T) config;
            } 
            
            throw new ArgumentException();
        }
    }
}