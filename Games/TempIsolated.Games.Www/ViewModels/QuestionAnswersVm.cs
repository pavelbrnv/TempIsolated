using System.Collections.Generic;
using System.Linq;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class QuestionAnswersVm : ObservingVm<QuestionAnswers>
    {
        #region Properties

        public IReadOnlyCollection<PlayerAnswerVm> AnswersVms { get; }

        #endregion

        #region Ctor

        public QuestionAnswersVm(QuestionAnswers answers)
            : base(answers)
        {
            Contracts.Requires(answers != null);

            AnswersVms = answers.Answers.Select(answer => new PlayerAnswerVm(answer)).ToArray();

            Initialize();
        }

        #endregion

        #region Subscribes and handlers

        protected override void SubscribeModel(bool subscribe)
        {
        }

        #endregion

        #region Disposing

        protected override void DisposeResources()
        {
            foreach (var answerVm in AnswersVms)
            {
                answerVm.Dispose();
            }

            base.DisposeResources();
        }

        #endregion
    }
}
