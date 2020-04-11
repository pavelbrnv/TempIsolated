using System.Collections.Generic;
using System.Linq;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public sealed class QuestionAnswers
    {
        #region Fields

        private readonly Dictionary<User, PlayerAnswer> answersByPlayers;

        #endregion

        #region Properties

        public Question Question { get; }

        public IReadOnlyCollection<PlayerAnswer> Answers => answersByPlayers.Values;

        #endregion

        #region Ctor

        public QuestionAnswers(Question question, IReadOnlyCollection<User> players)
        {
            Contracts.Requires(question != null);
            Contracts.Requires(players != null);

            Question = question;

            answersByPlayers = players.ToDictionary(player => player, player => new PlayerAnswer(player));
        }

        #endregion

        #region Public methods

        public void SetPlayerAnswer(User player, Answer answer)
        {
            if (answersByPlayers.TryGetValue(player, out var playerAnswer))
            {
                playerAnswer.SetAnswer(answer);
            }
        }

        #endregion
    }
}
