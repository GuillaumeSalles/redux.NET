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

		private static void OnTodoChanged (BindableObject obj, Todo oldValue, Todo newValue)
		{
			var todoItem = (TodoItem)obj;
			todoItem.TodoItemTextBlock.Text = newValue.Text;
			todoItem.CompleteCheckBox.IsToggled = newValue.IsCompleted;
		}

		public TodoItem ()
		{
			this.InitializeComponent ();
		}

		private void CompleteCheckBox_Click (object sender, EventArgs e)
		{
			App.Store.Dispatch (new CompleteTodoAction {
				TodoId = Todo.Id
			});
		}

		private void DeleteTodoItemButton_Click (object sender, EventArgs e)
		{
			App.Store.Dispatch (new DeleteTodoAction {
				TodoId = Todo.Id
			});
		}
	}
}

