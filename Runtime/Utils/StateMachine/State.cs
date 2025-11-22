using System;
using System.Collections.Generic;

namespace Potency.Services.Utils.StateMachine
{
	public abstract class State : IState
	{
		protected static int _id;

		protected List<Action> _onEnterActions;
		protected List<Action> _onExitActions;
		

		public int Id { get; }
		public string Name { get; }

		public State(string name)
		{
			_onEnterActions = new List<Action>();
			_onExitActions = new List<Action>();

			Name = name;
			Id = ++_id;
		}

		public void OnEnter(Action onEnterAction)
		{
			_onEnterActions.Add(onEnterAction);
		}

		public void OnExit(Action onExitAction)
		{
			_onExitActions.Add(onExitAction);
		}

		public void TriggerOnEnterActions()
		{
			foreach (var action in _onEnterActions)
			{
				action.Invoke();
			}
		}

		public void TriggerOnExitActions()
		{
			foreach (var action in _onExitActions)
			{
				action.Invoke();
			}
		}

		public override int GetHashCode()
		{
			return Id;
		}

		public override string ToString()
		{
			return Name;
		}

		public void Dispose()
		{
			_onEnterActions.Clear();
			_onExitActions.Clear();
		}
	}
}