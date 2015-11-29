using System;
using System.Collections.Generic;
using Xamarin.Forms;
using System.Linq;
using Redux.TodoMvc.States;
using Redux.TodoMvc.Actions;

namespace Redux.TodoMvc.Forms
{
    public partial class Header : ContentView
    {
        public Header()
        {
            this.InitializeComponent();

            App.Store.Subscribe((ApplicationState state) =>
                {
                    CompleteAllCheckBox.IsVisible = state.Todos.Any() ? true : false;
                    CompleteAllCheckBox.IsToggled = state.Todos.All(x => x.IsCompleted);
                });
        }

        private void TodoInputTextBox_Completed(object sender, EventArgs e)
        {
            App.Store.Dispatch(new AddTodoAction
                {
                    Text = TodoInputTextBox.Text
                });

            TodoInputTextBox.Text = string.Empty;
        }

        private void CompleteAllCheckBox_Click(object sender, EventArgs e)
        {
            App.Store.Dispatch(new CompleteAllTodosAction
                {
                    IsCompleted = CompleteAllCheckBox.IsToggled
                });
        }
    }
}

