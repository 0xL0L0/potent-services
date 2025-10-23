using System;
using System.Collections;
using System.Collections.Generic;
using Potency.Services.Runtime.Installer;
using Potency.Services.Runtime.Utils.Coroutine;
using UnityEngine;

namespace Potency.Services.Runtime.Tutorial
{
    public abstract class TutorialSequenceBase : IDisposable
    {
        public virtual string Id => "None";
        public TutorialConditionData RequirementConditions { get; private set; }
        public TutorialConditionData CurrentStepConditions { get; private set; }

        protected Coroutine _sequenceCoroutine;

        protected TutorialSequenceBase()
        {
            RequirementConditions = new TutorialConditionData();
            CurrentStepConditions = new TutorialConditionData();
        }

        public void StartSequence()
        {
            _sequenceCoroutine =  GameInstaller.Resolve<ICoroutineService>().StartCoroutine(TutorialSequence());
        }

        public void StopSequence()
        {
            if(_sequenceCoroutine != null)
            {
                GameInstaller.Resolve<ICoroutineService>().StopCoroutine(_sequenceCoroutine);
            }
        }

        protected virtual IEnumerator TutorialSequence()
        {
            yield return null;
        }

        protected virtual void OnSequenceEnd()
        {
        }

        protected bool ConditionsHaveBeenMet()
        {
            return RequirementConditions.CheckAllConditionsPassed(CurrentStepConditions);
        }

        protected void ClearAllConditions()
        {
            CurrentStepConditions.ClearConditions();
            RequirementConditions.ClearConditions();
        }

        protected void ClearAllConditionsAddRequirements(List<string> requirements)
        {
            CurrentStepConditions.ClearConditions();
            RequirementConditions.ClearConditions();

            foreach(var requirement in requirements)
            {
                RequirementConditions.AddCondition(requirement);
            }
        }

        public virtual void Dispose()
        {
            StopSequence();
        }
    }
}
