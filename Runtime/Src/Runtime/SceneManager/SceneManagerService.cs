using System.Threading.Tasks;

namespace Potency.Services.Runtime.SceneManager
{
    public class SceneManagerService : ISceneManagerService
    {
        // Unity finishes scene loading at 90% progress
        const float SceneLoadProgressThreshold = 0.9f;

        public async Task LoadSceneAsync(string sceneName,
            ISceneManagerService.LoadSceneMode loadSceneMode)
        {
            UnityEngine.AsyncOperation asyncOperation =
                UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName,
                    (UnityEngine.SceneManagement.LoadSceneMode)loadSceneMode);

            asyncOperation.allowSceneActivation = false;

            while(asyncOperation.progress < SceneLoadProgressThreshold)
            {
                await Task.Yield();
            }

            asyncOperation.allowSceneActivation = true;
        }

        public async Task UnloadSceneAsync(string sceneName)
        {
            UnityEngine.AsyncOperation asyncOperation =
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);

            while(asyncOperation.progress < SceneLoadProgressThreshold)
            {
                await Task.Yield();
            }
        }
    }
}