using System;
using Cysharp.Threading.Tasks;

namespace Potency.Services.Utils.StateMachine
{
    public interface IConditionalState : IState
    {
        void Target(IState targetState);
        
        void TargetIf(IState targetState, Func<bool> conditionToPass);

        UniTask<IState> GetTriggeredState();
    }
}