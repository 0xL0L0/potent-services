using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Potency.Services.Runtime.UI
{
    public class UIRuntimeDisplayWrapper
    {
        private Transform _originalRoot;
        
        public Stack<UiDisplayData> UiStack { get; }
        public List<UiDisplayData> UiStandaloneList { get; }
        public Transform Root { get; private set; }
        public Transform OriginalRoot => _originalRoot;
        
        public UIRuntimeDisplayWrapper(Transform root)
        {
            UiStack = new Stack<UiDisplayData>();
            UiStandaloneList = new List<UiDisplayData>();
            _originalRoot = root;
            Root = root;
        }

        public bool HasUi<T>() where T : UIView
        {
            return UiStack.Any(displayData => displayData.View is T) || UiStandaloneList.Any(displayData => displayData.View is T);
        }

        public bool HasActiveUi<T>() where T : UIView
        {
            if(UiStack.Count > 0)
            {
                return UiStack.Peek().View is T;
            }
            
            return UiStandaloneList.Any(displayData => displayData.View is T);
        }
        
        public T GetUi<T>() where T : UIView
        {
            foreach(var displayData in UiStack.Where(displayData => displayData.View is T))
            {
                return displayData.View as T;
            }

            foreach(var displayData in UiStandaloneList.Where(displayData => displayData.View is T))
            {
                return displayData.View as T;
            }

            return default;
        }
        
        public void CloseUi<T>() where T : UIView
        {
            if(UiStack.Count > 0 && UiStack.Peek().View is T)
            {
                CloseCurrentStack();
                return;
            }

            if(UiStandaloneList.Count <= 0)
            {
                return;
            }
            
            foreach(var displayData in UiStandaloneList.Where(displayData => displayData.View is T).ToList())
            {
                displayData.Dispose();
                UiStandaloneList.Remove(displayData);
            }
        }
        
        public void CloseUi<T>(T menu) where T : UIView
        {
            if(UiStack.Count > 0 && UiStack.Peek().View == menu)
            {
                CloseCurrentStack();
                return;
            }

            if(UiStandaloneList.Count <= 0)
            {
                return;
            }
            
            foreach(var displayData in UiStandaloneList.Where(displayData => displayData.View == menu).ToList())
            {
                displayData.Dispose();
                UiStandaloneList.Remove(displayData);
            }
        }

        public void AddUi(UiDisplayData displayData, UiLoadMode loadMode)
        {
            switch (loadMode)
            {
                case UiLoadMode.StackClosePrevious:
                    CloseCurrentStack();
                    UiStack.Push(displayData);
                    break;

                case UiLoadMode.StackCloseAll:
                    CloseAllStack();
                    UiStack.Push(displayData);
                    break;

                case UiLoadMode.StackHidePrevious:
                    HideCurrentStack();
                    UiStack.Push(displayData);
                    break;
                
                case UiLoadMode.Standalone:
                    UiStandaloneList.Add(displayData);
                    break;
                
                default:
                    throw new ArgumentOutOfRangeException(nameof(loadMode), loadMode, null);
            }
        }

        public void CloseCurrentStack()
        {
            if(UiStack.Count <= 0)
            {
                return;
            }

            UiStack.Peek().Dispose();
            UiStack.Pop();
            
            if(UiStack.Count <= 0)
            {
                return;
            }
            
            UiStack.Peek().View.SetVisible(true);
        }
        
        public void HideCurrentStack()
        {
            if(UiStack.Count <= 0)
            {
                return;
            }
            
            UiStack.Peek().View.SetVisible(false);
        }
            
        public void CloseAllStack()
        {
            if(UiStack.Count > 0)
            {
                foreach(var displayData in UiStack)
                {
                    displayData.Dispose();
                }
                
                UiStack.Clear();
            }
        }

        public void CloseAllUi()
        {
            if(UiStack.Count > 0)
            {
                foreach(var displayData in UiStack)
                {
                    displayData.Dispose();
                }
                
                UiStack.Clear();
            }

            if(UiStandaloneList.Count > 0)
            {
                foreach(var displayData in UiStandaloneList)
                {
                    displayData.Dispose();
                }
                
                UiStandaloneList.Clear();
            }
        }
    }
}
