using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Controls;

namespace TodoList
{
    /// <summary>
    ///     Interaction logic for AddProductWindow.xaml
    /// </summary>
    public partial class AddTaskWindow : Window
    {
        public AddTaskWindow()
        {
            InitializeComponent();
        }

        private void OnInit(object sender, RoutedEventArgs e)
        {
            Task t;
            t.title = "";
            t.description = "";
            t.deadline = DateTime.Now;
            t.tags = [];
            DataContext = new TaskItem(t);
        }

        private void ShowError(string message)
        {
            ErrorTextBlock.Visibility = Visibility.Visible;
            ErrorTextBlock.Text = message;
            if (AutomationPeer.ListenerExists(AutomationEvents.LiveRegionChanged))
            {
                var automationPeer = UIElementAutomationPeer.CreatePeerForElement(ErrorTextBlock);
                automationPeer?.RaiseAutomationEvent(AutomationEvents.LiveRegionChanged);
            }
        }

        private void SubmitTask(object sender, RoutedEventArgs e)
        {
            var automationPeer = UIElementAutomationPeer.CreatePeerForElement(ErrorTextBlock);

            if (TitleEntryForm.Text.Length == 0)
            {
                ShowError("Please, fill task title");
            }
            else if (DescriptionEntryForm.Text.Length == 0)
            {
                ShowError("Please, fill description");
            }
            else if (Validation.GetHasError(DeadlineEntryForm))
            {
                ShowError("Please, enter a valid date");
            }
            else
            {
                var item = (TaskItem)(DataContext);
                ((App)Application.Current).TaskItems.Add(item);
                Close();
            }

        }

        private void OnValidationError(object sender, ValidationErrorEventArgs e)
        {
            // Get the current UIA ItemStatus from the element element. 
            var oldStatus = AutomationProperties.GetItemStatus((DependencyObject)sender);

            // Set some sample new ItemStatus here... 
            var newStatus = e.Action == ValidationErrorEventAction.Added ? e.Error.ErrorContent.ToString() : String.Empty;
            AutomationProperties.SetItemStatus((DependencyObject)sender, newStatus);

            // Having just set the new ItemStatus, raise a UIA property changed event. Note that the peer may 
            // be null here unless a UIA client app such as Narrator or the AccEvent SDK tool are running. 
            var automationPeer = UIElementAutomationPeer.FromElement((UIElement)sender);
            automationPeer?.RaisePropertyChangedEvent(AutomationElementIdentifiers.ItemStatusProperty, oldStatus, newStatus);
        }
    }
}
