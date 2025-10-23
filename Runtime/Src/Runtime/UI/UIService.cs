using System;
using System.Threading.Tasks;
using Potency.Services.Runtime.AssetResolver;
using Potency.Services.Runtime.Configs.Configs;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Potency.Services.Runtime.UI
{
    public class UIService : IUIService
    {
        private readonly IAssetResolver _assetResolver;
        private readonly UIRuntimeDisplayWrapper _viewRuntimeDisplayWrapper;
        private readonly UIRuntimeDisplayWrapper _popupRuntimeDisplayWrapper;

        public UIService(UiConfig uiConfig, IAssetResolver assetResolver)
        {
            _assetResolver = assetResolver;

            var uiObj = Object.Instantiate(uiConfig.UIPrefab);
            var uiComponent = uiObj.GetComponent<UIMonoComponent>();

            _viewRuntimeDisplayWrapper = new UIRuntimeDisplayWrapper(uiComponent.ViewsTransform);
            _popupRuntimeDisplayWrapper = new UIRuntimeDisplayWrapper(uiComponent.PopupsTransform);
        }

        public async Task<TView> ShowView<TView>(UiLoadMode loadMode = UiLoadMode.StackClosePrevious,
            bool ignoreContainers = false) where TView : UIView, new()
        {
            if (HasActiveUi<TView>())
            {
                return _viewRuntimeDisplayWrapper.GetUi<TView>();
            }

            return await Show<TView>(_viewRuntimeDisplayWrapper, loadMode, ignoreContainers);
        }

        public async Task<TView> ShowView<TView, TModel>(IUIModel model,
            UiLoadMode loadMode = UiLoadMode.StackClosePrevious, bool ignoreContainers = false)
            where TView : UIView<TModel>, new() where TModel : struct
        {
            if (HasActiveUi<TView>())
            {
                return _viewRuntimeDisplayWrapper.GetUi<TView>();
            }

            return await Show<TView, TModel>(model, _viewRuntimeDisplayWrapper, loadMode, ignoreContainers);
        }

        public async Task<TView> ShowPopup<TView>(UiLoadMode loadMode = UiLoadMode.StackClosePrevious,
            bool ignoreContainers = false) where TView : UIView, new()
        {
            return await Show<TView>(_popupRuntimeDisplayWrapper, loadMode, ignoreContainers);
        }

        public async Task<TView> ShowPopup<TView, TModel>(IUIModel model,
            UiLoadMode loadMode = UiLoadMode.StackClosePrevious, bool ignoreContainers = false)
            where TView : UIView<TModel>, new() where TModel : struct
        {
            return await Show<TView, TModel>(model, _popupRuntimeDisplayWrapper, loadMode, ignoreContainers);
        }

        public T GetUi<T>() where T : UIView
        {
            if (_viewRuntimeDisplayWrapper.HasUi<T>())
            {
                return _viewRuntimeDisplayWrapper.GetUi<T>();
            }

            if (_popupRuntimeDisplayWrapper.HasUi<T>())
            {
                return _popupRuntimeDisplayWrapper.GetUi<T>();
            }

            return null;
        }

        public bool HasActiveUi<T>() where T : UIView
        {
            return _viewRuntimeDisplayWrapper.HasActiveUi<T>() || _popupRuntimeDisplayWrapper.HasActiveUi<T>();
        }

        public bool HasUi<T>() where T : UIView
        {
            return _viewRuntimeDisplayWrapper.HasUi<T>() || _popupRuntimeDisplayWrapper.HasUi<T>();
        }

        public void CloseUi<T>() where T : UIView
        {
            if (_viewRuntimeDisplayWrapper.HasUi<T>())
            {
                _viewRuntimeDisplayWrapper.CloseUi<T>();
            }

            if (_popupRuntimeDisplayWrapper.HasUi<T>())
            {
                _popupRuntimeDisplayWrapper.CloseUi<T>();
            }
        }

        public void CloseUi<T>(T menu) where T : UIView
        {
            if (_viewRuntimeDisplayWrapper.HasUi<T>())
            {
                _viewRuntimeDisplayWrapper.CloseUi<T>(menu);
            }

            if (_popupRuntimeDisplayWrapper.HasUi<T>())
            {
                _popupRuntimeDisplayWrapper.CloseUi<T>(menu);
            }
        }

        public void CloseCurrentStackView()
        {
            _viewRuntimeDisplayWrapper.CloseCurrentStack();
        }

        public void CloseCurrentStackPopup()
        {
            _popupRuntimeDisplayWrapper.CloseCurrentStack();
        }

        public void CloseAllViews()
        {
            _viewRuntimeDisplayWrapper.CloseAllUi();
        }

        public void CloseAllPopups()
        {
            _popupRuntimeDisplayWrapper.CloseAllUi();
        }

        public void NavigateViewBack(bool canCloseLasTView = false)
        {
            var viewCloseLimit = canCloseLasTView ? 0 : 1;

            if (_viewRuntimeDisplayWrapper.UiStack.Count > viewCloseLimit)
            {
                _viewRuntimeDisplayWrapper.CloseCurrentStack();
            }
        }

        public void NavigatePopupBack()
        {
            if (_popupRuntimeDisplayWrapper.UiStack.Count > 0)
            {
                _popupRuntimeDisplayWrapper.CloseCurrentStack();
            }
        }

        private async Task<TView> Show<TView>(UIRuntimeDisplayWrapper runtimeDisplayWrapper,
            UiLoadMode loadMode = UiLoadMode.StackClosePrevious, bool ignoreContainers = false)
            where TView : UIView, new()
        {
            var viewAndObject = await LoadView<TView>(runtimeDisplayWrapper, loadMode, ignoreContainers);
            return viewAndObject.Item1;
        }

        private async Task<TView> Show<TView, TModel>(IUIModel model,
            UIRuntimeDisplayWrapper runtimeDisplayWrapper,
            UiLoadMode loadMode = UiLoadMode.StackClosePrevious, bool ignoreContainers = false)
            where TView : UIView<TModel>, new() where TModel : struct
        {
            var viewAndObject = await LoadView<TView>(runtimeDisplayWrapper, loadMode, ignoreContainers);
            viewAndObject.Item1.Init((TModel)model);
            return viewAndObject.Item1;
        }
        
#pragma warning disable 1998
        private async Task<Tuple<TView, GameObject>> LoadView<TView>(
            UIRuntimeDisplayWrapper runtimeDisplayWrapper,
            UiLoadMode loadMode = UiLoadMode.StackClosePrevious, bool ignoreContainers = false)
#pragma warning restore 1998
        {
            var viewPrefab = _assetResolver.RequestAsset<string, GameObject>(typeof(TView).Name);

            GameObject viewInstance = null;

            if (viewPrefab == null)
            {
                return null;
            }

            viewInstance = Object.Instantiate(viewPrefab,
                ignoreContainers ? runtimeDisplayWrapper.OriginalRoot : runtimeDisplayWrapper.Root);

            var presenter = viewInstance.GetComponent<UIView>();
            var viewComponent = viewInstance.GetComponent<TView>();

            var displayData = new UiDisplayData()
            {
                View = presenter
            };

            runtimeDisplayWrapper.AddUi(displayData, loadMode);

            return new Tuple<TView, GameObject>(viewComponent, viewInstance);
        }
    }

    public enum UiLoadMode
    {
        StackClosePrevious,
        StackHidePrevious,
        StackCloseAll,
        Standalone,
    }
}