using System;
using Cysharp.Threading.Tasks;
using Potency.Services.Runtime.Utils.Logging;

namespace Potency.Services.Runtime.Utils.StateMachine
{
    public class StateMachine : IStateMachine, IDisposable
    {
        private IState _currentState;
        protected readonly IStateTrigger _stateTrigger;

        protected StateMachine(IStateTrigger stateTrigger)
        {
            _stateTrigger = stateTrigger;
            _stateTrigger.OnEventTriggered += OnStateEventTriggered;
        }
        
        public virtual void Setup()
        {
        }

        public async void SetCurrentState(IState state)
        {
            _currentState = state;
            _currentState.TriggerOnEnterActions();
            PLog.Info("STATE SET", $"{state}");
            
            if (_currentState is ITaskState currentTaskState)
            {
                await RunTaskState(currentTaskState);
            }
        }
        
        public async void TransitionToState(IState nextState)
        {
            PLog.Info("STATE TRANSITION", $"{_currentState} -> {nextState}");
            
            _currentState.TriggerOnExitActions();
            _currentState = nextState;
            _currentState.TriggerOnEnterActions();
            
            if (_currentState is ITaskState taskState)
            {
                await RunTaskState(taskState);
            }

            if (_currentState is IConditionalState conditionalState)
            {
                await RunConditionalState(conditionalState);
            }
        }
        
        public void OnStateEventTriggered(IStateEvent triggerEvent)
        {
            PLog.Info("STATE EVENT", $"{triggerEvent}");
            
            if (_currentState is not IEventState currentEventState)
            {
                return;
            }

            var nextState = currentEventState.CheckTriggerTransition(triggerEvent);

            if (nextState == null) 
            {
                return;
            }
            
            TransitionToState(nextState);
        }

        private async UniTask RunTaskState(ITaskState taskState)
        {
            var nextState = await taskState.RunTask();
            TransitionToState(nextState);
        }

        private async UniTask RunConditionalState(IConditionalState state)
        {
            var nextState = await state.GetTriggeredState();
            TransitionToState(nextState);
        }

        public void Dispose()
        {
            _stateTrigger.OnEventTriggered -= OnStateEventTriggered;
        }
    }
}