using System;

namespace todoRedux
{
    public class Todo
    {
        public string Text { get; set; }

        public bool IsCompleted { get; set; }

        public Guid Id { get; set; }

        public override string ToString ()
        {
            return string.Format ("[Todo: Text={0}, IsCompleted={1}, Id={2}]", Text, IsCompleted, Id);
        }
    }
}

