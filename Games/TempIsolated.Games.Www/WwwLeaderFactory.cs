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
        private readonly Func<IGameServer> createServer;
        private readonly ILogger logger;

        public string Name => Properties.Resources.Leader;

        public Type ModeType => typeof(WwwLeader);

        public WwwLeaderFactory(Func<IGameServer> createServer, ILogger logger)
        {
            Contracts.Requires(createServer != null);
            Contracts.Requires(logger != null);

            this.createServer = createServer;
            this.logger = logger;
        }

        public Mode Create()
        {
            return new WwwLeader(createServer(), logger);
        }

        public ModeVm CreateVm(Mode mode)
        {
            return new WwwLeaderVm((WwwLeader)mode, logger);
        }
    }
}
