using System;
using System.Collections.Generic;
using System.Linq;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public sealed class QuestionAnswers
    {
        #region Fields

        private readonly Dictionary<User, PlayerAnswer> answersByPlayers = new Dictionary<User, PlayerAnswer>();

        private readonly object sync = new object();

        #endregion

        #region Properties

        public Question Question { get; }

        public IReadOnlyCollection<PlayerAnswer> Answers
        {
            get
            {
                lock (sync)
                {
                    return answersByPlayers.Values.ToArray();
                }
            }
        }

        #endregion

        #region Ctor

        public QuestionAnswers(Question question)
        {
            Contracts.Requires(question != null);

            Question = question;
        }

        #endregion

        #region Events

        public event EventHandler<PlayerAnswerReceivedEventArgs> AnswerReceived = delegate { };

        #endregion

        #region Events raisers

        private void OnAnswerReceived(PlayerAnswer playerAnswer)
        {
            var args = new PlayerAnswerReceivedEventArgs(playerAnswer);
            AnswerReceived(this, args);
        }

        #endregion

        #region Public methods

        public void SetPlayerAnswer(User player, Answer answer)
        {
            var playerAnswer = new PlayerAnswer(player, answer);

            lock (sync)
            {
                answersByPlayers[player] = playerAnswer;
                OnAnswerReceived(playerAnswer);
            }
        }

        #endregion
    }

    public sealed class PlayerAnswerReceivedEventArgs : EventArgs
    {
        public PlayerAnswer PlayerAnswer { get; }

        public PlayerAnswerReceivedEventArgs(PlayerAnswer playerAnswer)
        {
            Contracts.Requires(playerAnswer != null);

            PlayerAnswer = playerAnswer;
        }
    }
}
