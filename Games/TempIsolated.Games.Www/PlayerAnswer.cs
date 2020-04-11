using System;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public sealed class PlayerAnswer
    {
        #region Fields

        private Answer answer;
        private AnswerStatus status = AnswerStatus.Unchecked;

        #endregion

        #region Properties

        public User Player { get; }

        public Answer Answer => answer;

        public AnswerStatus Status
        {
            get => status;
            set
            {
                if (status != value)
                {
                    var oldStatus = status;
                    status = value;
                    OnStatusChanged(oldStatus, status);
                }
            }
        }

        #endregion

        #region Ctor

        public PlayerAnswer(User player)
        {
            Contracts.Requires(player != null);

            Player = player;
        }

        #endregion

        #region Events

        public event EventHandler<AnswerStatusChangedEventArgs> StatusChanged = delegate { };

        #endregion

        #region Events raisers

        private void OnStatusChanged(AnswerStatus oldStatus, AnswerStatus newStatus)
        {
            var args = new AnswerStatusChangedEventArgs(oldStatus, newStatus);
            StatusChanged(this, args);
        }

        #endregion

        #region Public methods

        public void SetAnswer(Answer playerAnswer)
        {
            // todo
        }

        #endregion
    }

    public sealed class AnswerStatusChangedEventArgs : EventArgs
    {
        public AnswerStatus OldStatus { get; }

        public AnswerStatus NewStatus { get; }

        public AnswerStatusChangedEventArgs(AnswerStatus oldStatus, AnswerStatus newStatus)
        {
            OldStatus = oldStatus;
            NewStatus = newStatus;
        }
    }
}
