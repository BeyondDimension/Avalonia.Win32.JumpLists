using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Example.ViewModels;
using Example.Views;

namespace Example;

public sealed class App : Application
{
    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow
            {
                DataContext = new MainWindowViewModel(),
            };

            desktop.Startup += Desktop_Startup;
        }

        base.OnFrameworkInitializationCompleted();
    }

    void Desktop_Startup(object? sender, ControlledApplicationLifetimeStartupEventArgs e)
    {
        JumpLists.Init();
    }
}
