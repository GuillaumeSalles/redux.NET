namespace Redux.Async
{
    public static class Reducers
    {
        public static ApplicationState ReduceApplicationState(ApplicationState state, IAction action)
        {
            return new ApplicationState
            {
                Repositories = ReduceRepositories(state.Repositories, action),
                IsSearching = ReduceIsSearching(state.IsSearching, action)
            };
        }

        public static Repository[] ReduceRepositories(Repository[] state, IAction action)
        {
            if (action is ReceiveRepositoriesAction)
            {
                return ((ReceiveRepositoriesAction)action).Repositories;
            }

            return state;
        }

        public static bool ReduceIsSearching(bool isSearching, IAction action)
        {
            if(action is ReceiveRepositoriesAction)
            {
                return false;
            }

            if(action is SearchRepositoriesAction)
            {
                return true;
            }

            return isSearching;
        }
    }
}
