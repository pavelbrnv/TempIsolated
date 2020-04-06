using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;

namespace TempIsolated.Games.Www.Interaction
{
    public sealed class InternalGameServer : IGameServer
    {
        #region Fields

        private bool disposed;

        private readonly List<InternalGameClient> clients = new List<InternalGameClient>();

        private readonly object sync = new object();

        #endregion

        #region Properties

        public IReadOnlyList<User> Players
        {
            get
            {
                lock (sync)
                {
                    return clients.Select(client => client.Player).ToArray();
                }
            }
        }

        #endregion

        #region Events

        public event EventHandler<CollectionChangedEventArgs<User>> PlayersChanged = delegate { };

        #endregion

        #region Events raisers

        private void OnPlayersChanged(IReadOnlyList<User> added, IReadOnlyList<User> removed)
        {
            var args = new CollectionChangedEventArgs<User>(added, removed);
            PlayersChanged(this, args);
        }

        #endregion

        #region Public methods

        public bool AddClient(InternalGameClient client)
        {
            Contracts.Requires(client != null);

            lock (sync)
            {
                if (disposed || clients.Contains(client))
                {
                    return false;
                }

                clients.Add(client);
                OnPlayersChanged(new[] { client.Player }, new User[0]);
                return true;
            }
        }

        public bool RemoveClient(InternalGameClient client)
        {
            Contracts.Requires(client != null);

            lock (sync)
            {
                if (disposed || !clients.Contains(client))
                {
                    return false;
                }

                clients.Remove(client);
                OnPlayersChanged(new User[0], new[] { client.Player });
                return true;
            }
        }

        public Task<Answer> AskPlayer(User player, Question question, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Answer("dummy answer"));
        }

        #endregion

        #region Disposig

        public void Dispose()
        {
            lock (sync)
            {
                if (disposed)
                {
                    return;
                }
                disposed = true;

                var existingClients = clients.ToArray();
                foreach (var client in clients)
                {
                    RemoveClient(client);
                }
            }
        }

        #endregion
    }
}
