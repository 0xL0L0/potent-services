namespace Potency.Services.Runtime.Utils.StateMachine
{
	public interface IEventState : IState
	{
		/// <summary>
		/// Sets a state to target when a certain trigger is hit
		/// </summary>
		void Target(IState targetState, IStateEvent trigger);
		
		/// <summary>
		/// Returns a target state if the supplied trigger matches a target state
		/// </summary>
		IState CheckTriggerTransition(IStateEvent trigger);
	}
