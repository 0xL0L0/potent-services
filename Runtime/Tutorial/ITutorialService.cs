namespace Potency.Services.Runtime.Tutorial
{
    public interface ITutorialService
    {
        string CurrentRunningTutorialSequence { get; }
        void StartTutorialSequence(TutorialSequenceBase sequence);
        void StopCurrentTutorialSequence();
        void AddTutorialStepCondition(string conditionId);

        TutorialTargetMonoComponent GetTutorialTarget(string targetId);
    }
}
