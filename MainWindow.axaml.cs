using Avalonia.Controls;
using Avalonia.Interactivity;

namespace CloudMigrationTool;

public partial class MainWindow : Window
{
    public bool SourceCloudChosen { get; set; }
    public bool DestinationCloudChosen { get; set; }
    public bool IsMigrating { get; set; }
    public int MigrationProgress { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        IsMigrating = false;
        SourceCloudChosen = false;
        DestinationCloudChosen = false;
        MigrationProgress = 0;
        DataContext = this;
    }
    private void StartMigration(object sender, RoutedEventArgs e)
    {
        IsMigrating = true;
    }

    private void CancelMigration(object sender, RoutedEventArgs e)
    {
        IsMigrating = false;
    }
}