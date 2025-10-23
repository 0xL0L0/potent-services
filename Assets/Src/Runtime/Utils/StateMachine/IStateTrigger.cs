using System;

namespace Potency.Services.Runtime.Utils.StateMachine
{
    public interface IStateTrigger
    {
        /// <summary>
        /// Gets the callback action, triggered from <see cref="TriggerEvent"/>
        /// </summary>
        Action<IStateEvent> OnEventTriggered { get; set; }

        /// <summary>
        /// Triggers a state event to move state machine that use this trigger
        /// </summary>
        void TriggerEvent(IStateEvent stateEvent);
    }
}
