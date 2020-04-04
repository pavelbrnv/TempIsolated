using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public interface IGameServer
    {
        IReadOnlyList<User> Players { get; }

        event EventHandler<CollectionChangedEventArgs<User>> PlayersChanged;

        Task<Answer> AskPlayer(User player, Question question, CancellationToken cancellationToken);
    }
}
