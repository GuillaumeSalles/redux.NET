using Redux.Async.Models;

namespace Redux.Async
{
    public static class Reducers
    {
        public static ApplicationState ReduceApplicationState(ApplicationState state, IAction action)
        {
            return new ApplicationState
            {
                Repositories = ReduceRepositories(state.Repositories, action)
            };
        }

        public static Repository[] ReduceRepositories(Repository[] state, IAction action)
        {
            if (action is SearchRepositoriesAction)
            {
                return ((SearchRepositoriesAction)action).Repositories;
            }

            return state;
        }
    }
}
