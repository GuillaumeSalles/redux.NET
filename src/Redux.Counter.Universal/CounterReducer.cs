namespace Redux.Counter.Universal
{
    public static class CounterReducer
    {
        public static int Execute(int previousState, IAction action)
        {
            if(action is IncrementAction)
            {
                return previousState + 1;
            }

            if(action is DecrementAction)
            {
                return previousState - 1;
            }

            return previousState;
        }
    }
}
