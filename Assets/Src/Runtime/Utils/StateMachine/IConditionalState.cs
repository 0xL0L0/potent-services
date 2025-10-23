using System;
using Cysharp.Threading.Tasks;

namespace Potency.Services.Runtime.Utils.StateMachine
{
    public interface IConditionalState : IState
    {
        void Target(IState targetState);
        
        void TargetIf(IState targetState, Func<bool> conditionToPass);

        UniTask<IState> GetTriggeredState();
    }
}