using System.Collections.ObjectModel;
using System.Linq;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core.ViewModels;
using TempIsolated.Games.Www.Interaction.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class WwwPlayerVm : ModeVm
    {
        #region Fields

        private readonly ObservableCollection<QuestionAnsweringVm> answeringsVms = new ObservableCollection<QuestionAnsweringVm>();
        private QuestionAnsweringVm selectedAnsweringVm;

        private readonly ILogger logger;

        private readonly object sync = new object();

        private bool disposed;

        #endregion

        #region Properties

        public WwwPlayer Player => (WwwPlayer)Model;

        public GameClientVm ClientVm { get; }

        public ReadOnlyObservableCollection<QuestionAnsweringVm> AnsweringsVms { get; }

        public QuestionAnsweringVm SelectedAnsweringVm
        {
            get => selectedAnsweringVm;
            set
            {
                if (selectedAnsweringVm != value)
                {
                    selectedAnsweringVm = value;
                    RaisePropertyChanged(nameof(SelectedAnsweringVm));
                }
            }
        }

        #endregion

        #region Ctor

        public WwwPlayerVm(WwwPlayer player, ILogger logger)
            : base(player)
        {
            Contracts.Requires(player != null);
            Contracts.Requires(logger != null);

            this.logger = logger;

            ClientVm = InteractionVmsCreator.CreateVm(player.Client);

            AnsweringsVms = new ReadOnlyObservableCollection<QuestionAnsweringVm>(answeringsVms);

            Initialize();
        }

        #endregion

        #region Private methods

        private void AddAnsweringVm(QuestionAnswering answering)
        {
            lock (sync)
            {
                if (disposed)
                {
                    return;
                }

                var answeringVm = new QuestionAnsweringVm(answering, logger);
                answeringsVms.Add(answeringVm);

                SelectedAnsweringVm = answeringVm;
            }
        }

        private void ClearAnsweringsVms()
        {
            lock (sync)
            {
                SelectedAnsweringVm = null;

                var currentAnsweringsVms = answeringsVms.ToArray();
                answeringsVms.Clear();
                foreach (var answeringVm in currentAnsweringsVms)
                {
                    answeringVm.Dispose();
                }
            }
        }

        #endregion

        #region Sunscribes and handlers

        protected override void SubscribeModel(bool subscribe)
        {
            if (subscribe)
            {
                Player.AnsweringsAdded += PlayerAnsweringsAdded;
            }
            else
            {
                Player.AnsweringsAdded -= PlayerAnsweringsAdded;
            }
        }

        private void PlayerAnsweringsAdded(object sender, ItemEventArgs<QuestionAnswering> e)
        {
            AddAnsweringVm(e.Item);
        }

        #endregion

        #region Disposing

        protected override void DisposeResources()
        {
            lock (sync)
            {
                if (disposed)
                {
                    return;
                }
                disposed = true;
            }

            ClearAnsweringsVms();

            base.DisposeResources();
        }

        #endregion
    }
}
