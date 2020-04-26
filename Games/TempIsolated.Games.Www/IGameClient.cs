using System;
using TempIsolated.Common.Extensions;

namespace TempIsolated.Games.Www
{
    public interface IGameClient : IDisposable
    {
        event EventHandler<ItemEventArgs<QuestionAnswering>> QuestionAsked;
    }
}
