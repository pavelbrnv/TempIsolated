using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class QuestionDrawingVm : ObservingVm<QuestionDrawing>
    {
        #region Properties

        public string Title => Model.Question.Title;

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
        }

        #endregion
    }
}
