using System;

namespace Redux.TodoMvc.Universal.Signals
{
    public class AddTodoSignal : ISignal
    {
        public string Text { get; set; }
    }

    public class DeleteTodoSignal : ISignal
    {
        public Guid TodoId { get; set; }
    }

    public class CompleteTodoSignal : ISignal
    {
        public Guid TodoId { get; set; }
    }

    public class CompleteAllTodosSignal : ISignal
    {
        public bool IsCompleted { get; set; }
    }

    public class ClearCompletedTodosSignal : ISignal
    {

    }

    public class FilterTodosSignal : ISignal
    {
        public TodosFilter Filter { get; set; }
    }
}
