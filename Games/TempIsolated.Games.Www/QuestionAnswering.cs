using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;

namespace TempIsolated.Games.Www
{
    public sealed class QuestionAnswering
    {
        #region Fields

        private Answer answer;

        #endregion

        #region Properties

        public Question Question { get; }

        public bool HasAnswer => Answer != null;

        public Answer Answer
        {
            get => answer;
            private set
            {
                answer = value;
                OnAnswerSet(answer);
            }
        }

        #endregion

        #region Ctor

        public QuestionAnswering(Question question)
        {
            Contracts.Requires(question != null);

            Question = question;
        }

        #endregion

        #region Events

        public event EventHandler<ItemEventArgs<Answer>> AnswerSet = delegate { };

        #endregion

        #region Events raisers
        
        private void OnAnswerSet(Answer value)
        {
            var args = new ItemEventArgs<Answer>(value);
            AnswerSet(this, args);
        }

        #endregion

        #region Public methods

        public void SetAnswer(Answer value)
        {
            if (HasAnswer)
            {
                throw new InvalidOperationException("Answer has already set");
            }
            Answer = value;
        }

        #endregion
    }
}
