using System;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;
using TempIsolated.Common.Informing;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class QuestionAnsweringVm : ObservingVm<QuestionAnswering>
    {
        #region Fields

        private string timerText;
        private bool showThinkingTimer;
        private bool showFillingTimer;

        private bool canAnswer = true;
        private string answerText;

        private readonly ILogger logger;

        private readonly object sync = new object();

        #endregion

        #region Properties

        public string Title => Model.Question.Title;

        public string QuestionText => Model.Question.Text;

        public string TimerText
        {
            get => timerText;
            private set
            {
                timerText = value;
                RaisePropertyChanged(nameof(TimerText));
            }
        }

        public TimerVm ThinkingTimerVm { get; }

        public bool ShowThinkingTimer
        {
            get => showThinkingTimer;
            private set
            {
                showThinkingTimer = value;
                RaisePropertyChanged(nameof(ShowThinkingTimer));
            }
        }

        public TimerVm FillingTimerVm { get; }

        public bool ShowFillingTimer
        {
            get => showFillingTimer;
            private set
            {
                showFillingTimer = value;
                RaisePropertyChanged(nameof(ShowFillingTimer));
            }
        }

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

            TimerText = Properties.Resources.Think;

            ThinkingTimerVm = new TimerVm(answering.Question.ThinkingTime, TimeSpan.FromSeconds(1), ChangeThinkingTimerToFilling);
            ShowThinkingTimer = true;

            FillingTimerVm = new TimerVm(answering.Question.FillTime, TimeSpan.FromSeconds(1), SetNoAnswer);
            ShowFillingTimer = false;

            CommandSetAnswer = new ActionCommand(SetAnswer, Properties.Resources.SetAnswer);

            Initialize();

            ThinkingTimerVm.Start();
        }

        #endregion

        #region Private methods

        private void ChangeThinkingTimerToFilling()
        {
            lock (sync)
            {
                if (!CanAnswer)
                {
                    return;
                }

                ShowThinkingTimer = false;
                ShowFillingTimer = true;

                TimerText = Properties.Resources.FillAnswer;                              

                FillingTimerVm.Start();
            }
        }

        private void SetNoAnswer()
        {
            lock (sync)
            {
                if (!CanAnswer)
                {
                    return;
                }

                ShowThinkingTimer = false;
                ShowFillingTimer = false;

                TimerText = Properties.Resources.NoAnswer;

                CanAnswer = false;
            }
        }

        private void SetAnswer()
        {
            lock (sync)
            {
                try
                {
                    if (!CanAnswer)
                    {
                        return;
                    }

                    ThinkingTimerVm.Stop();
                    FillingTimerVm.Stop();

                    ShowThinkingTimer = false;
                    ShowFillingTimer = false;

                    TimerText = string.Empty;

                    Model.SetAnswer(new Answer(AnswerText));

                    CanAnswer = false;
                }
                catch (Exception e)
                {
                    logger.LogError("Error while setting answer", e);
                }
            }
        }

        #endregion

        #region Subscribes

        protected override void SubscribeModel(bool subscribe)
        {
        }

        #endregion

        #region Disposing

        protected override void DisposeResources()
        {
            FillingTimerVm.Dispose();
            ThinkingTimerVm.Dispose();

            base.DisposeResources();
        }

        #endregion
    }
}
