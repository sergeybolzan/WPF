namespace TaskListApp2.ViewModels
{
    using Catel.MVVM;
    using Catel.Services;
    using System.Collections.ObjectModel;
    using TaskListApp2.Models;

    public class TasksListViewModel : ViewModelBase
    {
        private readonly IUIVisualizerService visualizerService;
        public TasksListViewModel(IUIVisualizerService visualizerService)
        {
            this.visualizerService = visualizerService;
        }

        private ObservableCollection<Task> tasks;
        public ObservableCollection<Task> Tasks
        {
            get
            {
                if (tasks == null) tasks = TaskRepository.Tasks;
                return tasks;
            }
        }

        /// <summary>
        /// Команда для добавления новой задачи
        /// </summary>
        public Command AddNewTask
        {
            get
            {
                return new Command(() => { visualizerService.ShowDialog(new AddTaskDialogViewModel()); });
            }
        }
    }
}
