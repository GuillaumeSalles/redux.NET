using System;

namespace Redux.TodoMvc.Universal.States
{
    public class Todo
    {
        public string Text { get; set; }

        public bool IsCompleted { get; set; }

        public Guid Id { get; set; }
    }
}
