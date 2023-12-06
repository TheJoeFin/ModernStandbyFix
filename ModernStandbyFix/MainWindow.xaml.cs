using System.Reflection;
using System.Windows;

namespace ModernStandbyFix;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        Initialize();
    }

    private void Initialize()
    {
        versionTextBlock.Text = "Version: " + Assembly.GetExecutingAssembly()?.GetName()?.Version?.ToString();
        runOnStartupCheckBox.IsChecked = TaskSchedulerUtils.IsRunningOnStartup(App.GetApplicationPath());
        runOnStartupCheckBox.Checked += runOnStartupCheckBox_Checked;
        runOnStartupCheckBox.Unchecked += runOnStartupCheckBox_Checked;
    }

    private void runOnStartupCheckBox_Checked(object sender, RoutedEventArgs e)
    {
        if (TaskSchedulerUtils.ToggleRunOnStartup(App.GetApplicationPath()))
        {
            runOnStartupCheckBox.IsChecked = TaskSchedulerUtils.IsRunningOnStartup(App.GetApplicationPath());
        }
    }

    private void hideButton_Click(object sender, RoutedEventArgs e)
    {
        this.Hide();
    }
}
