using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Todo.Desktop.Application;
using Todo.Desktop.Modules.Todo;
using Todo.Desktop.Modules.Todo.TodoList;
using Todo.Desktop.Pages.Main;
using Todo.Desktop.Shared.Navigation;

namespace Todo.Desktop;

public class App : Avalonia.Application
{
    private IHost _host;
    public override void Initialize()
    {
        //make it with tabs rather than with navigation service
        AvaloniaXamlLoader.Load(this);
        
        _host = Bootstrapper.Build();
        GlobalExceptionHandler.Setup(_host);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            desktop.MainWindow = new MainWindow();
            desktop.MainWindow.DataContext = _host.Services.GetRequiredService<MainWindowViewModel>();
            
            var navigationStore = _host.Services.GetRequiredService<NavigationService>();
            var todoStore = _host.Services.GetRequiredService<TodoStore>();
            
            navigationStore.NavigateTo<TodoListViewModel>();
            
            Dispatcher.UIThread.Post(async () => await todoStore.Init());
        }

        base.OnFrameworkInitializationCompleted();
    }
}