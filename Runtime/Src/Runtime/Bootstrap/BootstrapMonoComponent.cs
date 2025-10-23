using System.Threading.Tasks;
using Potency.Services.Runtime.AssetResolver;
using Potency.Services.Runtime.Audio;
using Potency.Services.Runtime.Configs;
using Potency.Services.Runtime.Configs.Configs;
using Potency.Services.Runtime.Installer;
using Potency.Services.Runtime.LocalStorage;
using Potency.Services.Runtime.MessageBus;
using Potency.Services.Runtime.SceneManager;
using Potency.Services.Runtime.Tick;
using Potency.Services.Runtime.Tutorial;
using Potency.Services.Runtime.UI;
using Potency.Services.Runtime.Utils.Coroutine;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Potency.Services.Runtime.Bootstrap
{
    public class BootstrapComponent : MonoBehaviour, IBootstrap
    {
        private void Awake()
        {
#pragma warning disable CS4014
            Bootstrap();
#pragma warning restore CS4014
        }

        public virtual async Task Bootstrap()
        {
            DontDestroyOnLoad(gameObject);
            
            Application.targetFrameRate = 60;

            var localDataService = new LocalDataService();
            GameInstaller.Bind<ILocalDataService>(localDataService);

            var configsCollection = await Addressables.LoadAssetAsync<ConfigsCollection>("ConfigsCollection").Task;
            var configsResolver = new ConfigsResolver(configsCollection);
            GameInstaller.Bind<IConfigsResolver>(configsResolver);

            var messageBusService = new MessageBusService();
            GameInstaller.Bind<IMessageBusService>(messageBusService);
            
            var coroutineService = new CoroutineService();
            GameInstaller.Bind<ICoroutineService>(coroutineService);

            var assetResolver = new AssetResolver.AssetResolver();
            GameInstaller.Bind<IAssetResolver>(assetResolver);

            var sceneManagerService = new SceneManagerService();
            GameInstaller.Bind<ISceneManagerService>(sceneManagerService);

            var tickService = new TickService();
            GameInstaller.Bind<ITickService>(tickService);

            var uiConfig = configsResolver.GetConfig<UiConfig>();
            var uiService = new UIService(uiConfig, assetResolver);
            GameInstaller.Bind<IUIService>(uiService);

            var audioConfig = configsResolver.GetConfig<AudioConfig>();
            var audioService = new AudioService(audioConfig);
            GameInstaller.Bind<IAudioService>(audioService);
            
            var tutorialService = new TutorialService();
            GameInstaller.Bind<ITutorialService>(tutorialService);
        }
    }
}