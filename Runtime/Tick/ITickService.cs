using System;

namespace Potency.Services.Runtime.Tick
{
	/// <summary>
	/// Tick service allows for subscribing to OnUpdate, OnFixedUpdate and OnLateUpdate to receive callbacks on
	/// both mono and non-mono components.
	///
	/// This service creates a monobehaviour object upon creation on which all the update functions run
	/// </summary>
	public interface ITickService
	{
		/// <summary>
		/// Subscribes the action to regular update, and runs the tick every delta time
		/// If overflow is active, the tick will be tied to the provided delta time
		/// </summary>
		void SubscribeOnUpdate(Action<float> action, float deltaTime = 0f, bool invokeOnSubscribe = false, bool overflowTick = false);
		
		/// <summary>
		/// Subscribes the action to late update, and runs the tick every delta time
		/// If overflow is active, the tick will be tied to the provided delta time
		/// </summary>
		void SubscribeOnLateUpdate(Action<float> action, float deltaTime = 0f, bool invokeOnSubscribe = false, bool overflowTick = false);
		
		/// <summary>
		/// Subscribes the action to fixed
		/// </summary>
		void SubscribeOnFixedUpdate(Action<float> action, bool invokeOnSubscribe = false);
		
		/// <summary>
		/// Unsubscribes the action from any of the updates
		/// </summary>
		void Unsubscribe(Action<float> action);
		
		/// <summary>
		/// Unsubscribes the action from any of OnUpdate
		/// </summary>
		void UnsubscribeOnUpdate(Action<float> action);
		
		/// <summary>
		/// Unsubscribes the action from any of OnFixedUpdate
		/// </summary>
		void UnsubscribeOnFixedUpdate(Action<float> action);
		
		/// <summary>
		/// Unsubscribes the action from any of OnLateUpdate
		/// </summary>
		void UnsubscribeOnLateUpdate(Action<float> action);

		/// <summary>
		/// Unsubscribes from all updates
		/// </summary>
		void UnsubscribeAll();
		
		/// <summary>
		/// Unsubscribes from all updates from the given subscriber
		/// </summary>
		void UnsubscribeAll(object subscriber);
		
		/// <summary>
		/// Unsubscribes from all on updates from the given subscriber
		/// </summary>
		void UnsubscribeAllOnUpdate(object subscriber);
		
		/// <summary>
		/// Unsubscribes from all on fixed updates from the given subscriber
		/// </summary>
		void UnsubscribeAllOnFixedUpdate(object subscriber);
		
		/// <summary>
		/// Unsubscribes from all on fixed updates from the given subscriber
		/// </summary>
		void UnsubscribeAllOnLateUpdate(object subscriber);
		
		/// <summary>
		/// Unsubscribes from all on fixed updates
		/// </summary>
		void UnsubscribeAllOnLateUpdate();
		
		/// <summary>
		/// Unsubscribes from all on updates
		/// </summary>
		void UnsubscribeAllOnUpdate();
		
		
		
		/// <summary>
		/// Unsubscribes from all on fixed updates
		/// </summary>
		void UnsubscribeAllOnFixedUpdate();
	}
}