namespace Potency.Services.Runtime.Configs
{
    public interface IConfigsResolver
    {
        /// <summary>
        /// Returns bound config of the given type
        /// </summary>
        T GetConfig<T>() where T : IConfig;
    }
}