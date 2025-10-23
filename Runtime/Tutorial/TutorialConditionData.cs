using System.Collections.Generic;
using System.Linq;

namespace Potency.Services.Runtime.Tutorial
{
    public class TutorialConditionData
    {
        private List<string> _currentConditions = new();

        public void AddCondition(string id)
        {
            _currentConditions.Add(id);
        }

        public void ClearConditions()
        {
            _currentConditions.Clear();
        }

        public bool HasCondition(string id)
        {
            // LINQ shortening of "check every element, and see if any element contains id characters"
            return _currentConditions.Any(id.Contains);
        }

        public bool CheckAllConditionsPassed(TutorialConditionData otherConditions)
        {
            foreach(var condition in _currentConditions)
            {
                if(!otherConditions.HasCondition(condition))
                {
                    return false;
                }
            }

            return true;
        }
    }
}
