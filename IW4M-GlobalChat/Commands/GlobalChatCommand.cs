using Data.Models.Client;
using SharedLibraryCore;
using SharedLibraryCore.Configuration;
using SharedLibraryCore.Interfaces;

namespace IW4M_GlobalChat.Commands;

public class GlobalChatCommand : Command
{
    public GlobalChatCommand(CommandConfiguration config, ITranslationLookup layout) : base(config, layout)
    {
        Name = "globalchat";
        Description = "Toggle Global Chat";
        Alias = "gc";
        Permission = EFClient.Permission.Moderator;
        RequiresTarget = false;
    }

    public override Task ExecuteAsync(GameEvent gameEvent)
    {

        if (Plugin.Manager.GlobalChatCommand(gameEvent.Origin))
        {
            gameEvent.Origin.Tell("Global Chat Enabled");
            return Task.CompletedTask;
        }

        gameEvent.Origin.Tell("Global Chat Disabled");
        return Task.CompletedTask;
    }
}
