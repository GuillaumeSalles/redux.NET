using Redux.TodoMvc.Universal.Signals;

namespace Redux.TodoMvc.Universal.Reducers
{
    public class FilterTodosReducer : Reducer<TodosFilter, FilterTodosSignal>
    {
        protected override TodosFilter Execute(TodosFilter previousState, FilterTodosSignal signal)
        {
            return signal.Filter;
        }
    }
}
