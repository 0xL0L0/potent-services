using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;

namespace Potency.Services.Runtime.Utils.StateMachine
{
    public class ConditionState : State, IConditionalState
    {
        public List<Tuple<IState, Func<bool>>> StateConditionQueue;
        
        public ConditionState(string name) : base(name)
        {
            StateConditionQueue = new List<Tuple<IState, Func<bool>>>();
        }

        public void TargetIf(IState targetState, Func<bool> conditionToPass)
        {
            StateConditionQueue.Add(new Tuple<IState, Func<bool>>(targetState, conditionToPass));
        }
        
        public void Target(IState targetState)
        {
            StateConditionQueue.Add(new Tuple<IState, Func<bool>>(targetState, null));
        }

        public async UniTask<IState> GetTriggeredState()
        {
            await UniTask.Yield();
            
            foreach (var stateCondition in StateConditionQueue.ToList())
            {
                // If no condition specified, return the state
                if (stateCondition.Item2 == null)
                {
                    return stateCondition.Item1;
                }

                // Check if condition has passed
                if (stateCondition.Item2() == true)
                {
                    return stateCondition.Item1;
                }
            }

            // Always return last state - one element must pass the condition
            return StateConditionQueue.Last().Item1;
        }
    }
}