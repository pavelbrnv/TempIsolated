using System;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;
using TempIsolated.Common.Informing;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class QuestionAnsweringVm : ObservingVm<QuestionAnswering>
    {
        #region Fields

        private bool canAnswer = true;
        private string answerText;

        private readonly ILogger logger;

        #endregion

        #region Properties

        public string Title => Model.Question.Title;

        public string QuestionText => Model.Question.Text;

        public bool CanAnswer
        {
            get => canAnswer;
            private set
            {
                canAnswer = value;
                RaisePropertyChanged(nameof(CanAnswer));
            }
        }

        public string AnswerText
        {
            get => answerText;
            set
            {
                answerText = value;
                RaisePropertyChanged(nameof(AnswerText));
            }
        }

        public ActionCommand CommandSetAnswer { get; }

        #endregion

        #region Ctor

        public QuestionAnsweringVm(QuestionAnswering answering, ILogger logger)
            : base(answering)
        {
            Contracts.Requires(answering != null);
            Contracts.Requires(logger != null);

            this.logger = logger;

            CommandSetAnswer = new ActionCommand(SetAnswer, Properties.Resources.SetAnswer);

            Initialize();
        }

        #endregion

        #region Commands actions

        private void SetAnswer()
        {
            try
            {
                if (!CanAnswer)
                {
                    return;
                }

                Model.SetAnswer(new Answer(AnswerText));
                CanAnswer = false;
            }
            catch (Exception e)
            {
                logger.LogError("Error while setting answer", e);
            }
        }

        #endregion

        #region Subscribes

        protected override void SubscribeModel(bool subscribe)
        {
        }

        #endregion
    }
}
