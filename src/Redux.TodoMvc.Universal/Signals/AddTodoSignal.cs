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

    public class MarkTodoSignal : ISignal
    {
        public Guid TodoId { get; set; }
    }

    public class MarkAllSignal : ISignal
    {
        public bool IsMarked { get; set; }
    }

    public class ClearMarkedSignal : ISignal
    {

    }

    public class FilterTodosSignal : ISignal
    {
        public TodosFilter Filter { get; set; }
    }
}
