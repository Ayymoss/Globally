using Data.Models.Client;
using SharedLibraryCore;
using SharedLibraryCore.Configuration;
using SharedLibraryCore.Interfaces;

namespace Globally.Commands;

public class GlobalChatCommand : Command
{
    private readonly GlobalChatManager _globalChatManager;

    public GlobalChatCommand(CommandConfiguration config, ITranslationLookup layout, GlobalChatManager globalChatManager)
        : base(config, layout)
    {
        _globalChatManager = globalChatManager;
        Name = "globalchat";
        Description = "Toggle Global Chat";
        Alias = "gc";
        Permission = EFClient.Permission.Moderator;
        RequiresTarget = false;
    }

    public override Task ExecuteAsync(GameEvent gameEvent)
    {
        if (_globalChatManager.GlobalChatCommand(gameEvent.Origin))
        {
            gameEvent.Origin.Tell("(Color::Accent)Global Chat: (Color::Green)Enabled");
            return Task.CompletedTask;
        }

        gameEvent.Origin.Tell("(Color::Accent)Global Chat: (Color::Red)Disabled");
        return Task.CompletedTask;
    }
}
