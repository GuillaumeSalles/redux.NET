namespace Redux.Tests
{
    public static class Reducers
    {
        public static TState PassThrough<TState>(TState previousState, object action)
        {
            return previousState;
        }

        public static TState Replace<TState>(TState previousState, object action)
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
