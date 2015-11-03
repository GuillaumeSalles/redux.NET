using Redux.Async.Models;

namespace Redux.Async
{
    public class SearchRepositoriesAction : IAction
    {
        public Repository[] Repositories { get; set; }
    }
}
