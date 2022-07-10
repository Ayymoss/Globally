using SharedLibraryCore.Database.Models;

namespace IW4M_GlobalChat;

public class Manager
{
    private readonly List<EFClient> _globalChatUsers = new();

    public bool GlobalChatCommand(EFClient client)
    {
        if (client.GetAdditionalProperty<bool>("GCToggle"))
        {
            client.SetAdditionalProperty("GCToggle", false);
            _globalChatUsers.Remove(client);
            return false;
        }

        client.SetAdditionalProperty("GCToggle", true);
        _globalChatUsers.Add(client);
        return true;
    }

    public void SendGlobalChatMessage(EFClient client, string message)
    {
        foreach (var globalChatUser in _globalChatUsers
                     .Where(globalChatUser => globalChatUser.IsIngame)
                     .Where(globalChatUser => globalChatUser.CurrentServer != client.CurrentServer))
        {
            globalChatUser.Tell($"[{client.CurrentServer.Hostname}] {client.CleanedName}: {message}");
        }
    }
}
