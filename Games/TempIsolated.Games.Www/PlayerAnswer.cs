using System;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public sealed class PlayerAnswer
    {
        #region Fields

        private bool isAnswerCorrect;

        #endregion

        #region Properties

        public User Player { get; }

        public bool HasAnswer => Answer != null;

        public Answer Answer { get; private set; }

        public bool IsAnswerCorrect
        {
            get => isAnswerCorrect;
            set
            {
                if (!HasAnswer)
                {
                    return;
                }
                if (isAnswerCorrect != value)
                {
                    isAnswerCorrect = value;
                    OnIsAnswerCorrectChanged(value);
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

        public event EventHandler AnswerSet = delegate { };

        public event EventHandler<BooleanValueChangedEventArgs> IsAnswerCorrectChanged = delegate { };

        #endregion

        #region Events raisers

        private void OnAnswerSet()
        {
            AnswerSet(this, EventArgs.Empty);
        }

        private void OnIsAnswerCorrectChanged(bool newValue)
        {
            var args = new BooleanValueChangedEventArgs(!newValue, newValue);
            IsAnswerCorrectChanged(this, args);
        }

        #endregion

        #region Public methods

        public void SetAnswer(Answer playerAnswer)
        {
            if (HasAnswer)
            {
                throw new InvalidOperationException($"Answer for player '{Player.Name}' is already set");
            }

            Answer = playerAnswer;
            OnAnswerSet();
        }

        #endregion
    }
}
