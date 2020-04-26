using System;
using System.Threading;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;

namespace TempIsolated.Games.Www.Interaction
{
    public sealed class InternalGameClient : IGameClient
    {
        #region Fields

        private readonly InternalGameServer server;

        private readonly object sync = new object();

        #endregion

        #region Properties

        public User Player { get; }

        #endregion

        #region Ctor

        public InternalGameClient(InternalGameServer server, User player)
        {
            Contracts.Requires(server != null);
            Contracts.Requires(player != null);

            this.server = server;
            
            Player = player;
        }

        public void Init()
        {
            server.AddClient(this);
        }

        #endregion

        #region Events

        public event EventHandler<ItemEventArgs<QuestionAnswering>> QuestionAsked = delegate { };

        #endregion

        #region Events raisers

        private void OnQuestionAsked(QuestionAnswering questionAnswering)
        {
            var args = new ItemEventArgs<QuestionAnswering>(questionAnswering);
            QuestionAsked(this, args);
        }

        #endregion

        #region Internal methods

        internal async Task<Answer> AskQuestion(Question question, CancellationToken cancellationToken)
        {
            var questionAnswering = new QuestionAnswering(question);

            var tcs = new TaskCompletionSource<Answer>(TaskCreationOptions.RunContinuationsAsynchronously);

            void Handler(object sender, ItemEventArgs<Answer> e)
            {
                tcs.SetResult(e.Item);
            }

            questionAnswering.AnswerSet += Handler;
            try
            {
                using (cancellationToken.Register(() => tcs.TrySetCanceled()))
                {
                    using (new Timer(args => tcs.TrySetCanceled(), null, question.ThinkingTime + question.FillTime, TimeSpan.FromMilliseconds(-1)))
                    {
                        OnQuestionAsked(questionAnswering);
                        return await tcs.Task;
                    }
                }                
            }
            finally
            {
                questionAnswering.AnswerSet -= Handler;
            }
        }

        #endregion

        #region Disposig

        public void Dispose()
        {
            server.RemoveClient(this);
        }

        #endregion
    }
}
