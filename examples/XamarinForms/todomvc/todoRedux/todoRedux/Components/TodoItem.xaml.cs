using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.ComponentModel;

namespace todoRedux
{
	public partial class TodoItem : ViewCell, INotifyPropertyChanged
	{
		//        Todo Item Changed

		public static readonly BindableProperty TodoProperty = BindableProperty.Create <TodoItem, Todo> (p => p.Todo, null, BindingMode.Default, null, OnTodoChanged);

		public Todo Todo { 
			get {
				return (Todo)GetValue (TodoProperty);
			}
			set {
				SetValue (TodoProperty, value);
				OnPropertyChanged ();
			}
		}

        public bool IsChanging = false;

		private static void OnTodoChanged (BindableObject obj, Todo oldValue, Todo newValue)
		{
			var todoItem = (TodoItem)obj;

            todoItem.IsChanging = true;

            if (todoItem.TodoItemTextBlock.Text != newValue.Text)
			    todoItem.TodoItemTextBlock.Text = newValue.Text;

            if (todoItem.CompleteCheckBox.IsToggled != newValue.IsCompleted)
                todoItem.CompleteCheckBox.IsToggled = newValue.IsCompleted;

            todoItem.IsChanging = false;
		}

		public TodoItem ()
		{
			this.InitializeComponent ();
		}

		private void CompleteCheckBox_Click (object sender, EventArgs e)
		{
            if (!IsChanging) 
            {
                App.Store.Dispatch (new CompleteTodoAction {
                    TodoId = Todo.Id
                });
            }
		}

		private void DeleteTodoItemButton_Click (object sender, EventArgs e)
		{
			App.Store.Dispatch (new DeleteTodoAction {
				TodoId = Todo.Id
			});
		}
	}
}

