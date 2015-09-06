namespace Redux.Async.Universal
{
    public static class ApplicationReducer
    {
        public static ApplicationState Execute(ApplicationState previousState, IAction action)
        {
            return previousState;
        }
    }
}
