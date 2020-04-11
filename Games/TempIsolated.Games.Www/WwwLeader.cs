using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public sealed class WwwLeader : Mode
    {
        #region Fields

        private readonly List<QuestionDrawing> drawings = new List<QuestionDrawing>();

        private readonly ILogger logger;

        private readonly object sync = new object();

        #endregion

        #region Properties

        public IGameServer Server { get; }

        public IReadOnlyList<Question> Questions { get; } = new Question[]
        {
            new Question("Вопрос 1", "Чему равно 1 + 1?", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(15)),
            new Question("Вопрос 2", "Чему равно 2 + 2?", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(15)),
            new Question("Вопрос 3", "Чему равно 3 + 3?", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(15)),
            new Question("Вопрос 4", "Чему равно 4 + 4?", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(15)),
            new Question("Вопрос 5", "Чему равно 5 + 5?", TimeSpan.FromMinutes(1), TimeSpan.FromSeconds(15))
        };

        public IReadOnlyList<QuestionDrawing> Drawings
        {
            get
            {
                lock (sync)
                {
                    return drawings.ToArray();
                }
            }
        }

        #endregion

        #region Ctor

        public WwwLeader(IGameServer server, ILogger logger)
        {
            Contracts.Requires(server != null);
            Contracts.Requires(logger != null);

            Server = server;

            this.logger = logger;
        }

        #endregion

        #region Events

        public event EventHandler<ItemEventArgs<QuestionDrawing>> DrawingAdded = delegate { };

        #endregion

        #region Events raisers

        private void OnDrawingAdded(QuestionDrawing drawing)
        {
            var args = new ItemEventArgs<QuestionDrawing>(drawing);
            DrawingAdded(this, args);
        }

        #endregion

        #region Public methods

        public void PlayQuestion(Question question, IReadOnlyCollection<User> players)
        {
            var drawing = new QuestionDrawing(Server, question, players, logger);

            lock (sync)
            {
                drawings.Add(drawing);
                OnDrawingAdded(drawing);
            }
        }

        #endregion

        #region Disposing

        public override void Dispose()
        {
            lock (sync)
            {
                Parallel.ForEach(drawings, drawing => drawing.Dispose());

                Server.Dispose();
            }
        }

        #endregion
    }
}
