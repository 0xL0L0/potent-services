using UnityEngine;

namespace Potency.Services.Runtime.Tutorial
{
    public class TutorialService : ITutorialService
    {
        private TutorialSequenceBase _currentTutorialSequence;

        public string CurrentRunningTutorialSequence => _currentTutorialSequence?.Id;

        public void StartTutorialSequence(TutorialSequenceBase sequence)
        {
            _currentTutorialSequence = sequence;
            _currentTutorialSequence?.StartSequence();
        }

        public void StopCurrentTutorialSequence()
        {
            _currentTutorialSequence?.StopSequence();
            _currentTutorialSequence?.Dispose();
            _currentTutorialSequence = null;
        }

        public void AddTutorialStepCondition(string conditionId)
        {
            _currentTutorialSequence?.CurrentStepConditions.AddCondition(conditionId);
        }

        public TutorialTargetMonoComponent GetTutorialTarget(string targetId)
        {
            var targets = Object.FindObjectsByType<TutorialTargetMonoComponent>(FindObjectsSortMode.None);

            foreach(var target in targets)
            {
                if(target.Id == targetId)
                {
                    return target;
                }
            }

            return null;
        }
    }
}