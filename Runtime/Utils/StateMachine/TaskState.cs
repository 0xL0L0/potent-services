using System;
using Cysharp.Threading.Tasks;

namespace Potency.Services.Utils.StateMachine
{
	public class TaskState : State, ITaskState
	{
		private IState _targetState;
		private Func<UniTask> _task;
		
		public TaskState(string name) : base(name)
		{
		}

		public void TargetWait(IState targetState, Func<UniTask> task)
		{
			_targetState = targetState;
			_task = task;
		}

		public async UniTask<IState> RunTask()
		{
			await _task();
			return _targetState;
		}
	}
}