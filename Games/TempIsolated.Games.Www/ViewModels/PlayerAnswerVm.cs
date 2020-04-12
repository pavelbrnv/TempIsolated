using System;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class PlayerAnswerVm : ObservingVm<PlayerAnswer>
    {
        #region Properties

        public string PlayerName => Model.Player.Name;

        public bool HasAnswer => Model.HasAnswer;

        public string Answer => Model.Answer?.Text ?? string.Empty;

        public bool IsAnswerCorrect
        {
            get => Model.IsAnswerCorrect;
            set => Model.IsAnswerCorrect = value;
        }

        #endregion

        #region Ctor

        public PlayerAnswerVm(PlayerAnswer answer)
            : base(answer)
        {
            Initialize();
        }

        #endregion

        #region Subscribes and handlers

        protected override void SubscribeModel(bool subscribe)
        {
            if (subscribe)
            {
                Model.AnswerSet += ModelAnswerSet;
                Model.IsAnswerCorrectChanged += ModelIsAnswerCorrectChanged;
            }
            else
            {
                Model.AnswerSet -= ModelAnswerSet;
                Model.IsAnswerCorrectChanged -= ModelIsAnswerCorrectChanged;
            }
        }

        private void ModelAnswerSet(object sender, EventArgs e)
        {
            RaisePropertyChanged(nameof(HasAnswer));
            RaisePropertyChanged(nameof(Answer));
        }

        private void ModelIsAnswerCorrectChanged(object sender, BooleanValueChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(IsAnswerCorrect));
        }

        #endregion
    }
}
