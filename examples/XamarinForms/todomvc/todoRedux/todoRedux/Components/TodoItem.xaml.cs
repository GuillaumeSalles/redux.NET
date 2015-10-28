using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace todoRedux
{
    public partial class TodoItem : ContentView
    {
//        Todo Item Changed

//        public static readonly DependencyProperty TodoProperty =
//            DependencyProperty.Register("Todo", typeof(Todo), typeof(TodoItem), new PropertyMetadata(-1, OnTodoChanged));
//
//        public Todo Todo
//        {
//            get { return (Todo)GetValue(TodoProperty); }
//            set { SetValue(TodoProperty, value); }
//        }

//        private static void OnTodoChanged(object sender, EventArgs args)
//        {
//            var todoItem = (TodoItem)sender;
//            var todo = (Todo)args.NewValue;
//
//            todoItem.TodoItemTextBlock.Text = todo.Text;
//            todoItem.CompleteCheckBox.IsChecked = todo.IsCompleted;
//        }

        public TodoItem()
        {
            this.InitializeComponent();
        }

        private void CompleteCheckBox_Click(object sender, EventArgs e)
        {
            App.Store.Dispatch(new CompleteTodoAction
                {
                    TodoId = Todo.Id
                });
        }

        private void DeleteTodoItemButton_Click(object sender, EventArgs e)
        {
            App.Store.Dispatch(new DeleteTodoAction
                {
                    TodoId = Todo.Id
                });
        }
    }
}

