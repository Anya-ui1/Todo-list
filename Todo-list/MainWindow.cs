using System.Windows;
using System.Windows.Data;

namespace TodoList
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly CollectionViewSource _listingDataView;

        public MainWindow()
        {
            InitializeComponent();
            _listingDataView = (CollectionViewSource)(Resources["ListingDataView"]);
        }

        private void OpenAddTaskWindow(object sender, RoutedEventArgs e)
        {
            var addProductWindow = new AddTaskWindow();
            addProductWindow.ShowDialog();
        }
    }

}
