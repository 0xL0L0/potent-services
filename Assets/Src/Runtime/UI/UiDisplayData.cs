using System;

namespace Potency.Services.Runtime.UI
{
    public class UiDisplayData : IDisposable
    {
        public UIView View { get; set; }

        public void Dispose()
        {
            View.Dispose();
        }
    }
}