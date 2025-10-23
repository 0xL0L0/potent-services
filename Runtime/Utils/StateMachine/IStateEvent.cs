using System;

namespace Potency.Services.Runtime.Utils.StateMachine
{
    public interface IStateEvent : IEquatable<IStateEvent>
    {
        /// <summary>
        /// Gets the unique ID of the event
        /// Automatically incremented
        /// </summary>
        int Id { get; }
		
        /// <summary>
        /// Gets name of the event
        /// </summary>
        string Name { get; }
    }
}
