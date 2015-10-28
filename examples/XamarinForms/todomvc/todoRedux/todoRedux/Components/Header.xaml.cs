using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace todoRedux
{
    public partial class Header : ContentView
    {
        public Header()
        {
            this.InitializeComponent();

            App.Store.Subscribe(state =>
                {
                    CompleteAllCheckBox.IsVisible = state.Todos.Any() ? true : false;
                    CompleteAllCheckBox.Toggled = state.Todos.All(x => x.IsCompleted);
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
                    IsCompleted = CompleteAllCheckBox.Toggled
                });
        }
    }
}

