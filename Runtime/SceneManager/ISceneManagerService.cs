using System.Threading.Tasks;

namespace Potency.Services.Runtime.SceneManager
{
    public interface ISceneManagerService
    {
        Task LoadSceneAsync(string sceneName, LoadSceneMode loadSceneMode);
        
        Task UnloadSceneAsync(string sceneName);
        
        public enum LoadSceneMode
        {
            Single,
            Additive,
        }
    }
}