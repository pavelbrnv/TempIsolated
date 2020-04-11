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

        #endregion

        #region Ctor

        public QuestionDrawingVm(QuestionDrawing drawing)
            : base(drawing)
        {
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
        }

        #endregion
    }
}
