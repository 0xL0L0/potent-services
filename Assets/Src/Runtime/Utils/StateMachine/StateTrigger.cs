using System;

namespace Potency.Services.Runtime.Utils.StateMachine
{
    public class StateTrigger : IStateTrigger
    {
        public Action<IStateEvent> OnEventTriggered { get; set; }

        public void TriggerEvent(IStateEvent stateEvent)
        {
            OnEventTriggered.Invoke(stateEvent);
        }
    }
}
