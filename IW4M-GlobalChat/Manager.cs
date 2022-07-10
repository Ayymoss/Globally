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
            RemoveGlobalChatUser(client);
            return false;
        }

        client.SetAdditionalProperty("GCToggle", true);
        _globalChatUsers.Add(client);
        return true;
    }

    // TODO: Maybe add a chat queuing system to prevent IW4MAdmin flooding?
    public void SendGlobalChatMessage(EFClient client, string message)
    {
        foreach (var globalChatUser in _globalChatUsers
                     .Where(globalChatUser => globalChatUser.IsIngame)
                     .Where(globalChatUser => globalChatUser.CurrentServer != client.CurrentServer))
        {
            globalChatUser.Tell($"[{client.CurrentServer.Hostname}] {client.Name}: {message}");
        }
    }

    public void RemoveGlobalChatUser(EFClient client)
    {
        if (_globalChatUsers.Contains(client))
        {
            _globalChatUsers.Remove(client);
        }
    }
}
