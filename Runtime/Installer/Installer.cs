using System;
using System.Collections.Generic;

namespace Potency.Services.Runtime.Installer
{
    /// <summary>
    /// Base implementation of <see cref="IInstaller"/>
    /// </summary>
    public class Installer : IInstaller
    {
        private readonly Dictionary<Type, object> _bindings = new ();
        
        public T Bind<T>(T instance)
        {
            var type = typeof(T);

            if (!type.IsInterface)
            {
                throw new ArgumentException($"Cannot bind {instance} because {type} is not an interface");
            }

            _bindings.Add(type, instance);
            return instance;
        }
        
        public bool TryResolve<T>(out T instance)
        {
            var success = _bindings.TryGetValue(typeof(T), out var inst);

            instance = (T)inst;

            return success;
        }

        public T Resolve<T>()
        {
            if (!_bindings.TryGetValue(typeof(T), out var instance))
            {
                throw new ArgumentException($"Type {typeof(T)} is not bound");
            }

            return (T) instance;
        }
        
        public bool Clean<T>() where T : class
        {
            return _bindings.Remove(typeof(T));
        }
        
        public void Clean()
        {
            _bindings.Clear();
        }
    }
}
