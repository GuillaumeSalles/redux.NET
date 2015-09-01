namespace Redux.DevTools.Universal
{
    public class PauseTimeMachineAction : IAction
    {

    }

    public class ResumeTimeMachineAction : IAction
    {

    }

    public class SetTimeMachinePositionAction : IAction
    {
        public int Position { get; set; }
    }
}
