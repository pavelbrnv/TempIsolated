using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;

namespace TempIsolated.Games.Www.Interaction
{
    public sealed class InternalGameClient : IGameClient
    {
        #region Fields

        private readonly InternalGameServer server;

        private readonly object sync = new object();

        #endregion

        #region Properties

        public User Player { get; }

        #endregion

        #region Ctor

        public InternalGameClient(InternalGameServer server, User player)
        {
            Contracts.Requires(server != null);
            Contracts.Requires(player != null);

            this.server = server;
            
            Player = player;
        }

        public void Init()
        {
            server.AddClient(this);
        }

        #endregion

        #region Disposig

        public void Dispose()
        {
            server.RemoveClient(this);
        }

        #endregion
    }
}
