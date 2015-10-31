using Redux.TodoMvc.States;

namespace Redux.TodoMvc.ViewModels
{
    public class FooterViewModel
    {
        public bool ClearTodosIsVisible { get; set; }

        public string ActiveTodosCounterMessage { get; set; }

        public TodosFilter SelectedFilter { get; set; }

        public bool AreFiltersVisible { get; set; }
    }
}
