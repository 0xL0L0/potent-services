namespace Potency.Services.Runtime.Installer
{
    /// <summary>
    /// Interface used for marking an object as a container of bindings.
    /// These installers are used to bind interfaces to be resolved later when a class
    /// requires dependencies.
    /// </summary>
    public interface IInstaller
    {
        /// <summary>
        /// Binds an interface of a class into the installer
        /// </summary>
        T Bind<T>(T instance);
        
        /// <summary>
        /// Resolves and returns a binding.
        /// Throws and exception if binding is not found.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        T Resolve<T>();
        
        /// <summary>
        /// Tries to resolve a bindind. Returns success status, and instance if resolution succedeed.
        /// </summary>
        bool TryResolve<T>(out T instance);

        /// <summary>
        /// Removes a binding of the given type from the installer
        /// </summary>
        bool Clean<T>() where T : class;
        
        /// <summary>
        /// Removes all bindings from the installer
        /// </summary>
        void Clean();
    }
}
