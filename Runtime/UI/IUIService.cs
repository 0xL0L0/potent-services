using System.Threading.Tasks;

namespace Potency.Services.Runtime.UI
{
    public interface IUIService
    {
        Task<TView> ShowView<TView>(UiLoadMode loadMode = UiLoadMode.StackClosePrevious, bool ignoreContainers = false) where TView : UIView, new();
        Task<TView> ShowView<TView, TModel>(IUIModel model, UiLoadMode loadMode = UiLoadMode.StackClosePrevious, bool ignoreContainers = false) where TView : UIView<TModel>, new() where TModel : struct;
        Task<TView> ShowPopup<TView>(UiLoadMode loadMode = UiLoadMode.StackClosePrevious, bool ignoreContainers = false) where TView : UIView, new();
        Task<TView> ShowPopup<TView, TModel>(IUIModel model, UiLoadMode loadMode = UiLoadMode.StackClosePrevious, bool ignoreContainers = false) where TView : UIView<TModel>, new() where TModel : struct;
        T GetUi<T>() where T : UIView;
        bool HasActiveUi<T>() where T : UIView;
        bool HasUi<T>() where T : UIView;
        void CloseUi<T>() where T : UIView;
        void CloseUi<T>(T menu) where T : UIView;
        void CloseCurrentStackView();
        void CloseCurrentStackPopup();
        void CloseAllViews();
        void CloseAllPopups();
        void NavigateViewBack(bool canCloseLastView = false);
        void NavigatePopupBack();
    }
}