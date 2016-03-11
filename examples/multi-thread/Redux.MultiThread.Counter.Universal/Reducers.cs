namespace Redux.MultiThread.Counter.Universal
{
    public static class Reducers
    {
        public static int ReduceCounter(int state, IAction action)
        {
            if(action is IncrementAction)
            {
                return state + 1;
            }

            return state;
        }
    }
}
