using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;

namespace TempIsolated.Games.Www.Interaction
{
    public sealed class DummyGameServer : IGameServer
    {
        public IReadOnlyList<User> Players { get; } = new User[0];

        public event EventHandler<CollectionChangedEventArgs<User>> PlayersChanged;

        public Task<Answer> AskPlayer(User player, Question question, CancellationToken cancellationToken)
        {
            return Task.FromResult(new Answer("dummy answer"));
        }
    }
}
