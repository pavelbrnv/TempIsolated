using System;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;
using TempIsolated.Common.Informing;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class QuestionDrawingVm : ObservingVm<QuestionDrawing>
    {
        #region Fields

        private bool showDrawingTimer;

        private readonly ILogger logger;

        #endregion

        #region Properties

        public string Title => Model.Question.Title;

        public string QuestionText => Model.Question.Text;

        public string State => Properties.Resources.ResourceManager.TryLocalize(Model.State);

        public TimerVm DrawingTimerVm { get; }

        public bool ShowDrawingTimer
        {
            get => showDrawingTimer;
            private set
            {
                showDrawingTimer = value;
                RaisePropertyChanged(nameof(ShowDrawingTimer));
            }
        }

        public ActionCommand CommandStartDrawing { get; }

        public ActionCommand CommandStopDrawing { get; }

        public bool IsStartDrawingAvailable => Model.State == DrawingState.Waiting;

        public bool IsStopDrawingAvailable => Model.State == DrawingState.Drawing;

        public QuestionAnswersVm AnswersVm { get; }

        #endregion

        #region Ctor

        public QuestionDrawingVm(QuestionDrawing drawing, ILogger logger)
            : base(drawing)
        {
            Contracts.Requires(drawing != null);
            Contracts.Requires(logger != null);

            this.logger = logger;

            DrawingTimerVm = new TimerVm(drawing.Question.ThinkingTime + drawing.Question.FillTime, TimeSpan.FromSeconds(1));

            CommandStartDrawing = new ActionCommand(StartDrawing, Properties.Resources.StartDrawing);
            CommandStopDrawing = new ActionCommand(StopDrawing, Properties.Resources.StopDrawing);

            AnswersVm = new QuestionAnswersVm(drawing.Answers);

            Initialize();
        }

        #endregion

        #region Commands actions

        private void StartDrawing()
        {
            try
            {
                if (IsStartDrawingAvailable)
                {
                    Model.StartDrawing();
                }
            }
            catch (Exception e)
            {
                logger.LogError("Error while drawing starting", e);
            }
        }

        private void StopDrawing()
        {
            try
            {
                if (IsStopDrawingAvailable)
                {
                    Model.StopDrawing();
                }
            }
            catch (Exception e)
            {
                logger.LogError("Error while drawing stopping", e);
            }
        }

        #endregion

        #region Subscribes

        protected override void SubscribeModel(bool subscribe)
        {
            if (subscribe)
            {
                Model.StateChanged += ModelStateChanged;
            }
            else
            {
                Model.StateChanged -= ModelStateChanged;
            }
        }

        private void ModelStateChanged(object sender, ValueChangedEventArgs<DrawingState> e)
        {
            if (e.NewValue == DrawingState.Drawing)
            {
                ShowDrawingTimer = true;
                DrawingTimerVm.Start();
            }
            else
            {
                DrawingTimerVm.Stop();
                ShowDrawingTimer = false;                
            }

            RaisePropertyChanged(nameof(State));
            RaisePropertyChanged(nameof(IsStartDrawingAvailable));
            RaisePropertyChanged(nameof(IsStopDrawingAvailable));
        }

        #endregion

        #region Disposing

        protected override void DisposeResources()
        {
            DrawingTimerVm.Dispose();

            AnswersVm.Dispose();

            base.DisposeResources();
        }

        #endregion
    }
}
