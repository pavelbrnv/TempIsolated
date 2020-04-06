using System;

namespace TempIsolated.Games.Www.Interaction.ViewModels
{
    public static class InteractionVmsCreator
    {
        public static GameServerVm CreateVm(IGameServer server)
        {
            if (server is InternalGameServer)
            {
                return new InternalGameServerVm();
            }

            throw new ArgumentException("Unable to create vm. Unknown server type.");
        }

        public static GameClientVm CreateVm(IGameClient client)
        {
            if (client is InternalGameClient internalGameClient)
            {
                return new InternalGameClientVm(internalGameClient);
            }

            throw new ArgumentException("Unable to create vm. Unknown client type.");
        }
    }
}
