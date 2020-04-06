using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public interface IGameServer : IDisposable
    {
        IReadOnlyList<User> Players { get; }

        event EventHandler<CollectionChangedEventArgs<User>> PlayersChanged;

        Task<Answer> AskPlayer(User player, Question question, CancellationToken cancellationToken);
    }
}
