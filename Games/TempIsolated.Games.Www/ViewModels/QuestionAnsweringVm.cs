using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class QuestionAnsweringVm : ObservingVm<QuestionAnswering>
    {
        #region Properties

        public string Title => Model.Question.Title;

        #endregion

        #region Ctor

        public QuestionAnsweringVm(QuestionAnswering answering)
            : base(answering)
        {
            Contracts.Requires(answering != null);

            Initialize();
        }

        #endregion

        #region Subscribes

        protected override void SubscribeModel(bool subscribe)
        {
        }

        #endregion
    }
}
