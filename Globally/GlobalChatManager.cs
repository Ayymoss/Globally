using SharedLibraryCore.Database.Models;

namespace Globally;

public class GlobalChatManager
{
    private readonly List<EFClient> _globalChatUsers = [];

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

    public void SendGlobalChatMessage(EFClient client, string message)
    {
        lock (_globalChatUsers)
        {
            var globalChatUsers = _globalChatUsers
                .Where(globalChatUser => globalChatUser.IsIngame)
                .Where(globalChatUser => globalChatUser.CurrentServer != client.CurrentServer);

            foreach (var globalChatUser in globalChatUsers)
            {
                globalChatUser.Tell($"[{client.CurrentServer.Hostname}(Color::White)] {client.Name}(Color::White): {message}");
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
                    _globalChatUsers.Remove(client);
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
