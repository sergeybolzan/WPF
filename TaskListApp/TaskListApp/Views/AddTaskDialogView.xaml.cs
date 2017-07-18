namespace TaskListApp2.Views
{
    using Catel.Windows;

    using ViewModels;

    /// <summary>
    /// Interaction logic for AddTaskDialogView.xaml.
    /// </summary>
    public partial class AddTaskDialogView : DataWindow
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AddTaskDialogView"/> class.
        /// </summary>
        public AddTaskDialogView()
            : this(null) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AddTaskDialogView"/> class.
        /// </summary>
        /// <param name="viewModel">The view model to inject.</param>
        /// <remarks>
        /// This constructor can be used to use view-model injection.
        /// </remarks>
        public AddTaskDialogView(AddTaskDialogViewModel viewModel)
            : base(viewModel, DataWindowMode.Custom)
        {
            InitializeComponent();
        }
    }
}
