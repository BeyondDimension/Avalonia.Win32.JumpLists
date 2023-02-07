using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Example.Views;

public sealed partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
#if DEBUG
        this.AttachDevTools();
#endif
    }

    void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}
