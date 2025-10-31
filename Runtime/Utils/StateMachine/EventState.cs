using System.Collections.Generic;

namespace Potency.Services.Runtime.Utils.StateMachine
{
    public class EventState : State, IEventState
    {
        private Dictionary<IStateEvent, IState> _targetStates;
        public EventState(string name) : base(name)
        {
            _targetStates = new Dictionary<IStateEvent, IState>();
        }
        
        public IState CheckTriggerTransition(IStateEvent trigger)
        {
            return _targetStates.TryGetValue(trigger, out var state) ? state : null;
        }
        
        public void Target(IState targetState, IStateEvent trigger)
        {
            _targetStates.Add(trigger, targetState);
        }

        public new void Dispose()
        {
            _targetStates.Clear();
        }
    }
}