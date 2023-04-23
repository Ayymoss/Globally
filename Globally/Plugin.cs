using Microsoft.Extensions.DependencyInjection;
using SharedLibraryCore.Events.Game;
using SharedLibraryCore.Events.Management;
using SharedLibraryCore.Interfaces;
using SharedLibraryCore.Interfaces.Events;

namespace Globally;

public class Plugin : IPluginV2
{
    private readonly GlobalChatManager _globalChatManager;
    public string Name => "Globally";
    public string Version => "2023-04-23";
    public string Author => "Amos";

    public Plugin(GlobalChatManager globalChatManager)
    {
        _globalChatManager = globalChatManager;

        IManagementEventSubscriptions.Load += OnLoad;
        IManagementEventSubscriptions.ClientStateDisposed += OnClientStateDisposed;
        IGameEventSubscriptions.ClientMessaged += OnClientMessaged;
    }

    public static void RegisterDependencies(IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<GlobalChatManager>();
    }

    private Task OnLoad(IManager manager, CancellationToken token)
    {
        Console.WriteLine($"[{Name}] loaded. Version: {Version}");
        return Task.CompletedTask;
    }

    private Task OnClientStateDisposed(ClientStateDisposeEvent clientEvent, CancellationToken token)
    {
        _globalChatManager.ManageGlobalChatUsers(clientEvent.Client, Action.Remove);
        return Task.CompletedTask;
    }

    private Task OnClientMessaged(ClientMessageEvent clientEvent, CancellationToken token)
    {
        _globalChatManager.SendGlobalChatMessage(clientEvent.Client, clientEvent.Message);
        return Task.CompletedTask;
    }
}
