using System;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core;
using TempIsolated.Core.ViewModels;
using TempIsolated.Games.Www.ViewModels;

namespace TempIsolated.Games.Www
{
    public sealed class WwwPlayerFactory : IModeFactory
    {
        private readonly Func<IGameClient> createClient;
        private readonly ILogger logger;

        public string Name => Properties.Resources.Player;

        public Type ModeType => typeof(WwwPlayer);

        public WwwPlayerFactory(Func<IGameClient> createClient, ILogger logger)
        {
            Contracts.Requires(createClient != null);
            Contracts.Requires(logger != null);

            this.createClient = createClient;
            this.logger = logger;
        }

        public Mode Create()
        {
            return new WwwPlayer(createClient(), logger);
        }

        public ModeVm CreateVm(Mode mode)
        {
            return new WwwPlayerVm((WwwPlayer)mode, logger);
        }
    }
}
