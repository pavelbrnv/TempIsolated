using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public sealed class QuestionDrawing : IDisposable
    {
        #region Fields

        private readonly IGameServer server;
        private readonly Question question;
        private readonly IReadOnlyCollection<User> players;
        private readonly ILogger logger;

        private Task drawingTask;
        private CancellationTokenSource drawingCancellationSource;

        private bool disposed;

        private DrawingState state = DrawingState.Waiting;

        private readonly object sync = new object();

        #endregion

        #region Properties

        public QuestionAnswers Answers { get; }

        public DrawingState State
        {
            get => state;
            private set
            {
                var oldState = state;
                state = value;
                OnStateChanged(oldState, state);
            }
        }

        #endregion

        #region Ctor

        public QuestionDrawing(IGameServer server, Question question, IReadOnlyCollection<User> players, ILogger logger)
        {
            Contracts.Requires(server != null);
            Contracts.Requires(question != null);
            Contracts.Requires(players != null);
            Contracts.Requires(logger != null);

            this.server = server;
            this.question = question;
            this.players = players;
            this.logger = logger;

            Answers = new QuestionAnswers(question);
        }

        #endregion

        #region Events

        public event EventHandler<ValueChangedEventArgs<DrawingState>> StateChanged = delegate { };

        #endregion

        #region Events raisers

        private void OnStateChanged(DrawingState oldState, DrawingState newState)
        {
            var args = new ValueChangedEventArgs<DrawingState>(oldState, newState);
            StateChanged(this, args);
        }

        #endregion

        #region Public methods

        public void StartDrawing()
        {
            lock (sync)
            {
                if (disposed)
                {
                    throw new InvalidOperationException("Question drawing is disposed");
                }

                if (drawingTask != null)
                {
                    throw new InvalidOperationException("Question can be drawn only once");
                }

                drawingTask = Drawing();
            }
        }

        public void StopDrawing()
        {
            lock (sync)
            {
                drawingCancellationSource?.Cancel();
            }
        }

        #endregion

        #region Private methods

        private async Task Drawing()
        {
            lock (sync)
            {
                State = DrawingState.Drawing;

                drawingCancellationSource = new CancellationTokenSource();
            }

            var answersWaiters = new ConcurrentBag<Task>();

            Parallel.ForEach(players, player => answersWaiters.Add(AskPlayer(player, drawingCancellationSource.Token)));

            await Task.WhenAll(answersWaiters);

            lock (sync)
            {
                drawingCancellationSource.Dispose();
                drawingCancellationSource = null;

                State = DrawingState.Drawn;
            }
        }

        private async Task AskPlayer(User player, CancellationToken cancellationToken)
        {
            try
            {
                var answer = await server.AskPlayer(player, question, cancellationToken);
                Answers.SetPlayerAnswer(player, answer);
            }
            catch (OperationCanceledException)
            {
            }
            catch (Exception e)
            {
                logger.LogError($"Error while asking {player.Name}", e);
            }
        }

        #endregion

        #region Disposing

        public void Dispose()
        {
            Task awaitingTask;

            lock (sync)
            {
                if (disposed)
                {
                    return;
                }
                disposed = true;

                awaitingTask = drawingTask;
            }

            StopDrawing();

            awaitingTask?.Wait();
        }

        #endregion
    }
}
