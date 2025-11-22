using System;

namespace Potency.Services.Utils.StateMachine
{
    public interface IState : IDisposable
    {
        /// <summary>
        /// Gets the unique ID of the state
        /// Automatically incremented
        /// </summary>
        int Id { get; }
		
        /// <summary>
        /// Gets name of the state
        /// </summary>
        string Name { get; }
        
        /// <summary>
        /// Adds an entry action when this state is transitioned to
        /// </summary>
        void OnEnter(Action onEnterAction);

        /// <summary>
        /// Adds an exit action when this state is transitioned from
        /// </summary>
        void OnExit(Action onExitAction);

        /// <summary>
        /// Triggers all entry actions
        /// </summary>
        void TriggerOnEnterActions();

        /// <summary>
        /// Triggers all exit actions
        /// </summary>
        void TriggerOnExitActions();
    }
}