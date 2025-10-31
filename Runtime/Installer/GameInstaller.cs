using System;

namespace Potency.Services.Runtime.Installer
{
    /// <summary>
    /// Utility Installer class based on <see cref="IInstaller"/>
    /// Instantiates the base installer class and provides static access to the interface methods.
    /// </summary>
    public static class GameInstaller
    {
        private static readonly IInstaller _installer = new Installer();
		
        /// <inheritdoc cref="IInstaller.Bind{T}"/>
        public static T Bind<T>(T instance)
        {
            return _installer.Bind(instance);
        }

        /// <inheritdoc cref="IInstaller.TryResolve{T}"/>
        public static bool TryResolve<T>(out T instance)
        {
            return _installer.TryResolve(out instance);
        }

        /// <inheritdoc cref="IInstaller.Resolve{T}"/>
        public static T Resolve<T>()
        {
            return _installer.Resolve<T>();
        }

        /// <inheritdoc cref="IInstaller.Clean"/>
        public static bool Clean<T>() where T : class
        {
            return _installer.Clean<T>();
        }

        /// <inheritdoc cref="IInstaller.Clean"/>
        public static bool CleanDispose<T>() where T : class, IDisposable
        {
            _installer.Resolve<T>().Dispose();
			
            return _installer.Clean<T>();
        }

        /// <inheritdoc cref="IInstaller.Clean"/>
        public static void Clean()
        {
            _installer.Clean();
        }
    }
}