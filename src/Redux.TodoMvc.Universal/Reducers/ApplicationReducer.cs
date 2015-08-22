using System.Collections.Immutable;

namespace Redux.TodoMvc.Universal.Reducers
{
    public class ApplicationReducer : IReducer<ApplicationState>
    {
        public IReducer<ImmutableArray<Todo>>[] TodosReducers()
        {
            return new IReducer<ImmutableArray<Todo>>[]
            {
                new AddTodoReducer(),
                new MarkTodoReducer(),
                new DeleteTodoReducer(),
                new ClearMarkedTodosReducer(),
                new MarkAllTodosReducer()
            };
        }

        public ApplicationState Execute(ApplicationState previousState, ISignal signal)
        {
            return new ApplicationState
            {
                Filter = new FilterTodosReducer().Execute(previousState.Filter, signal),
                Todos = TodosReducers().Combine()(previousState.Todos,signal)
            };
        }
    }
}
