namespace Potency.Services.Utils.StateMachine
{
    public interface IStateMachine
    {
        /// <summary>
        /// Setups up the whole state chart that the state machine will use
        /// </summary>
        void Setup();

        /// <summary>
        /// Called when receiving events from state trigger, to call transitions
        /// </summary>
        void OnStateEventTriggered(IStateEvent triggerEvent);
        
        /// <summary>
        /// Transitions from current state, to next state, triggering enter/exit actions
        /// </summary>
        void TransitionToState(IState nextState);
        
        /// <summary>
        /// Overwrites the current state directly, and triggers on enter actions
        /// </summary>
        void SetCurrentState(IState state);
    }
}