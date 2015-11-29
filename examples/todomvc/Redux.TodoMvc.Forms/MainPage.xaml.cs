using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Linq;
using Redux.TodoMvc.States;

namespace Redux.TodoMvc.Forms
{
    public partial class MainPage : ContentPage
    {
        public MainPage ()
        {
            InitializeComponent ();

            App.Store.Subscribe((ApplicationState state) =>
                {
                    Footer.IsVisible = state.Todos.Any() ? true : false;
                });
        }
    }
}

