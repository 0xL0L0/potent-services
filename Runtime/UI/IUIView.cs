using System;

namespace Potency.Services.Runtime.UI
{
    public interface IUIView : IDisposable
    {
        Type ViewType { get; }

        void SetVisible(bool isVisible);
    }
}