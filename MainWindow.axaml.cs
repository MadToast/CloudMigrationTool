using Avalonia.Controls;

namespace CloudMigrationTool;

public partial class MainWindow : Window
{
    public bool SourceCloudChosen { get; set; };

    public MainWindow()
    {
        InitializeComponent();
        SourceCloudChosen = false;
    }
}