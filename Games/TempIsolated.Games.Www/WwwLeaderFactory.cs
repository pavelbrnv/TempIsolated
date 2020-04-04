using System;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core;
using TempIsolated.Core.ViewModels;
using TempIsolated.Games.Www.ViewModels;

namespace TempIsolated.Games.Www
{
    public sealed class WwwLeaderFactory : IModeFactory
    {
        private readonly IGameServer server;
        private readonly ILogger logger;

        public string Name => Properties.Resources.Leader;

        public Type ModeType => typeof(WwwLeader);

        public WwwLeaderFactory(IGameServer server, ILogger logger)
        {
            Contracts.Requires(server != null);
            Contracts.Requires(logger != null);

            this.server = server;
            this.logger = logger;
        }

        public Mode Create()
        {
            return new WwwLeader(server, logger);
        }

        public ModeVm CreateVm(Mode mode)
        {
            return new WwwLeaderVm((WwwLeader)mode, logger);
        }
    }
}
