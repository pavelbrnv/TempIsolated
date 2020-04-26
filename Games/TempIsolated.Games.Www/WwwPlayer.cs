using System;
using System.Collections.Generic;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public sealed class WwwPlayer : Mode
    {
        #region Fields

        private readonly List<QuestionAnswering> answerings = new List<QuestionAnswering>();

        private readonly object sync = new object();

        private bool disposed;

        #endregion

        #region Properties

        public IGameClient Client { get; }

        public IReadOnlyList<QuestionAnswering> Answerings
        {
            get
            {
                lock (sync)
                {
                    return answerings.ToArray();
                }
            }
        }

        #endregion

        #region Ctor

        public WwwPlayer(IGameClient client)
        {
            Contracts.Requires(client != null);

            Client = client;
            SubscribeToClient(true);
        }

        #endregion

        #region Events

        public event EventHandler<ItemEventArgs<QuestionAnswering>> AnsweringsAdded = delegate { };

        #endregion

        #region Events raisers

        private void OnAnsweringsAdded(QuestionAnswering answering)
        {
            var args = new ItemEventArgs<QuestionAnswering>(answering);
            AnsweringsAdded(this, args);
        }

        #endregion

        #region Subscribes and handlers

        private void SubscribeToClient(bool subscribe)
        {
            if (subscribe)
            {
                Client.QuestionAsked += ClientQuestionAsked;
            }
            else
            {
                Client.QuestionAsked -= ClientQuestionAsked;
            }
        }

        private void ClientQuestionAsked(object sender, ItemEventArgs<QuestionAnswering> e)
        {
            lock (sync)
            {
                if (disposed)
                {
                    return;
                }

                answerings.Add(e.Item);
                OnAnsweringsAdded(e.Item);
            }
        }

        #endregion

        #region Disposing

        public override void Dispose()
        {
            lock (sync)
            {
                disposed = true;

                SubscribeToClient(false);
                Client.Dispose();
            }
        }

        #endregion
    }
}
