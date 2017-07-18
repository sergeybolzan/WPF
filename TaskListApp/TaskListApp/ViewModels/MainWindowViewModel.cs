namespace TaskListApp2.ViewModels
{
    using Catel.Data;
    using Catel.MVVM;
    using Catel.Services;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Threading.Tasks;

    public class MainWindowViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService visualizerService;
        private TasksListViewModel tasksListViewModel;
        private PersonsListViewModel personsListViewModel;
        public MainWindowViewModel(IUIVisualizerService visualizerService)
        {
            this.visualizerService = visualizerService;
            tasksListViewModel = new TasksListViewModel(this.visualizerService);
            personsListViewModel = new PersonsListViewModel();
        }

        protected override async Task InitializeAsync()
        {
            await base.InitializeAsync();

            // TODO: subscribe to events here
        }

        protected override async Task CloseAsync()
        {
            // TODO: unsubscribe from events here

            await base.CloseAsync();
        }

        public IViewModel CurrentForm
        {
            get { return GetValue<IViewModel>(CurrentFormProperty); }
            set { SetValue(CurrentFormProperty, value); }
        }
        public static readonly PropertyData CurrentFormProperty = RegisterProperty("CurrentForm", typeof(IViewModel), null);

        #region Commadns
        /// <summary>
        /// Команда для отображения списка задач
        /// </summary>
        public Command ShowTasksList
        {
            get
            {
                return new Command(() => { CurrentForm = tasksListViewModel; });
            }
        }

        /// <summary>
        /// Команда для отображения списка персон
        /// </summary>
        public Command ShowPersonsList
        {
            get
            {
                return new Command(() => { CurrentForm = personsListViewModel; });
            }
        }
        #endregion

    }
}
