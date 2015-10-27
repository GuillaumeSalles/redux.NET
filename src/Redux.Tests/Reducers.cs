namespace Redux.Tests
{
    public static class Reducers
    {
        public static TState PassThrough<TState>(TState previousState, IAction action)
        {
            return previousState;
        }

        public static TState Replace<TState>(TState previousState, IAction action)
        {
            var fakeAction = action as FakeAction<TState>;

            if(fakeAction != null)
            {
                return fakeAction.Value;
            }

            return previousState;
        }
    }
}
