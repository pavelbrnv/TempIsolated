using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public sealed class WwwPlayer : Mode
    {
        #region Fields

        private readonly ILogger logger;

        private readonly object sync = new object();

        #endregion

        #region Properties

        public IGameClient Client { get; }

        #endregion

        #region Ctor

        public WwwPlayer(IGameClient client, ILogger logger)
        {
            Contracts.Requires(client != null);
            Contracts.Requires(logger != null);

            Client = client;

            this.logger = logger;
        }

        #endregion

        #region Disposing

        public override void Dispose()
        {
            lock (sync)
            {
                Client.Dispose();
            }
        }

        #endregion
    }
}
