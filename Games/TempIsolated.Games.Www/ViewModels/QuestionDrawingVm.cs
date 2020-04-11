using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class QuestionDrawingVm : ObservingVm<QuestionDrawing>
    {
        #region Properties

        public string Title => Model.Question.Title;

        public string QuestionText => Model.Question.Text;

        public string State => Properties.Resources.ResourceManager.TryLocalize(Model.State);

        public ActionCommand CommandStartDrawing { get; }

        public ActionCommand CommandStopDrawing { get; }

        public bool IsStartDrawingAvailable => Model.State == DrawingState.Waiting;

        public bool IsStopDrawingAvailable => Model.State == DrawingState.Drawing;

        public QuestionAnswersVm AnswersVm { get; }

        #endregion

        #region Ctor

        public QuestionDrawingVm(QuestionDrawing drawing)
            : base(drawing)
        {
            Contracts.Requires(drawing != null);

            CommandStartDrawing = new ActionCommand(StartDrawing, Properties.Resources.StartDrawing);
            CommandStopDrawing = new ActionCommand(StopDrawing, Properties.Resources.StopDrawing);

            AnswersVm = new QuestionAnswersVm(drawing.Answers);

            Initialize();
        }

        #endregion

        #region Commands actions

        private void StartDrawing()
        {
            if (IsStartDrawingAvailable)
            {
                Model.StartDrawing();
            }
        }

        private void StopDrawing()
        {
            if (IsStopDrawingAvailable)
            {
                Model.StopDrawing();
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
            RaisePropertyChanged(nameof(State));
            RaisePropertyChanged(nameof(IsStartDrawingAvailable));
            RaisePropertyChanged(nameof(IsStopDrawingAvailable));
        }

        #endregion

        #region Disposing

        protected override void DisposeResources()
        {
            AnswersVm.Dispose();

            base.DisposeResources();
        }

        #endregion
    }
}
