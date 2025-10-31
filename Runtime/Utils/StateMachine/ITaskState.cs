using System;
using Cysharp.Threading.Tasks;

namespace Potency.Services.Runtime.Utils.StateMachine
{
	public interface ITaskState : IState
	{
		/// <summary>
		/// Runs a specified task on the state, after which the state machine will continue
		/// </summary>
		void TargetWait(IState targetState, Func<UniTask> task);

		/// <summary>
		/// Runs the task of the state, and calls back with transition target
		/// </summary>
		UniTask<IState> RunTask();
	}
