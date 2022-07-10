using SharedLibraryCore.Database.Models;

namespace IW4M_GlobalChat;

public class Manager
{
    private readonly List<EFClient> _globalChatUsers = new();

    public bool GlobalChatCommand(EFClient client)
    {
        if (client.GetAdditionalProperty<bool>("GCToggle"))
        {
            ManageGlobalChatUsers(client, Action.Remove);
            return false;
        }

        ManageGlobalChatUsers(client, Action.Add);
        return true;
    }

    // TODO: Maybe add a chat queuing system to prevent IW4MAdmin flooding?
    public void SendGlobalChatMessage(EFClient client, string message)
    {
        lock (_globalChatUsers)
        {
            foreach (var globalChatUser in _globalChatUsers
                         .Where(globalChatUser => globalChatUser.IsIngame)
                         .Where(globalChatUser => globalChatUser.CurrentServer != client.CurrentServer))
            {
                globalChatUser.Tell(
                    $"[{client.CurrentServer.Hostname}(Color::White)] {client.Name}(Color::White): {message}");
            }
        }
    }

    public void ManageGlobalChatUsers(EFClient client, Action action)
    {
        lock (_globalChatUsers)
        {
            switch (action)
            {
                case Action.Add:
                    client.SetAdditionalProperty("GCToggle", true);
                    _globalChatUsers.Add(client);
                    break;
                case Action.Remove:
                    client.SetAdditionalProperty("GCToggle", false);
                    if (_globalChatUsers.Contains(client))
                    {
                        _globalChatUsers.Remove(client);
                    }
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(action), action, null);
            }
        }
    }
}

public enum Action
{
    Add,
    Remove
}
