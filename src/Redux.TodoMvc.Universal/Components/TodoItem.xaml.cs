using Redux.TodoMvc.Universal.Signals;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Redux.TodoMvc.Universal.Components
{
    public sealed partial class TodoItem : UserControl
    {
        public static readonly DependencyProperty TodoProperty =
             DependencyProperty.Register("Todo", typeof(Todo), typeof(TodoItem), new PropertyMetadata(-1, OnTodoChanged));
        
        public Todo Todo
        {
            get { return (Todo)GetValue(TodoProperty); }
            set { SetValue(TodoProperty, value); }
        }

        private static void OnTodoChanged(object sender, DependencyPropertyChangedEventArgs args)
        {
            var todoItem = (TodoItem)sender;
            var todo = (Todo)args.NewValue;

            todoItem.TodoItemTextBlock.Text = todo.Text;
            todoItem.MarkedCheckBox.IsChecked = todo.IsMarked;
        }

        public TodoItem()
        {
            this.InitializeComponent();
        }

        private void MarkedCheckBox_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            App.Store.Dispatch(new MarkTodoSignal
            {
                TodoId = Todo.Id
            });
        }

        private void DeleteTodoItemButton_Click(object sender, RoutedEventArgs e)
        {
            App.Store.Dispatch(new DeleteTodoSignal
            {
                TodoId = Todo.Id
            });
        }
    }
}
