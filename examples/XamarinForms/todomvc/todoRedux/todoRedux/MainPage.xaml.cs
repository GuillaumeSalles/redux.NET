using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace todoRedux
{
    public partial class MainPage : ContentPage
    {
        public MainPage ()
        {
            InitializeComponent ();

            App.Store.Subscribe(state =>
                {
                    Footer.IsVisible = state.Todos.Any() ? true : false;
                });
        }
    }
}

