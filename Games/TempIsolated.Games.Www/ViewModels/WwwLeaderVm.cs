using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core;
using TempIsolated.Core.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class WwwLeaderVm : ModeVm
    {
        #region Fields

        private readonly Dictionary<User, SelectablePlayerVm> playersVmsByPlayers = new Dictionary<User, SelectablePlayerVm>();
        private readonly ObservableCollection<SelectablePlayerVm> playersVms = new ObservableCollection<SelectablePlayerVm>();

        private readonly ILogger logger;

        private bool disposed;

        private readonly object playersSync = new object();

        #endregion

        #region Properties

        public WwwLeader Leader => (WwwLeader)Model;

        public IReadOnlyList<SelectableQuestionVm> QuestionsVms { get; }

        public ReadOnlyObservableCollection<SelectablePlayerVm> PlayersVms { get; }

        public ICommand CommandPlaySelectedQuestions { get; }

        #endregion

        #region Ctor

        public WwwLeaderVm(WwwLeader leader, ILogger logger)
            : base(leader)
        {
            Contracts.Requires(leader != null);
            Contracts.Requires(logger != null);

            this.logger = logger;

            QuestionsVms = leader.Questions.Select(question => new SelectableQuestionVm(question)).ToArray();

            PlayersVms = new ReadOnlyObservableCollection<SelectablePlayerVm>(playersVms);

            CommandPlaySelectedQuestions = new ActionCommand(PlaySelectedQuestions);

            Initialize();
        }

        #endregion

        #region Private methods

        private void AddPlayerVm(User player)
        {
            var playerVm = new SelectablePlayerVm(player);
            playersVmsByPlayers.Add(player, playerVm);
            playersVms.Add(playerVm);
        }

        private void RemovePlayerVm(User player)
        {
            if (playersVmsByPlayers.TryGetValue(player, out var playerVm))
            {
                playersVms.Remove(playerVm);
                playersVmsByPlayers.Remove(player);
            }
        }

        private void PlaySelectedQuestions()
        {
            User[] selectedPlayers;
            lock (playersSync)
            {
                if (disposed)
                {
                    return;
                }

                selectedPlayers = playersVms
                    .Where(playerVm => playerVm.IsSelected)
                    .Select(playerVm => playerVm.Player)
                    .ToArray();
            }

            var selectedQuestions = QuestionsVms
                .Where(questionVm => questionVm.IsSelected)
                .Select(questionVm => questionVm.Question)
                .ToArray();

            foreach (var question in selectedQuestions)
            {
                Leader.PlayQuestion(question, selectedPlayers);
            }
        }

        #endregion

        #region Sunscribes and handlers

        protected override void SubscribeModel(bool subscribe)
        {
            if (subscribe)
            {
                Leader.Server.PlayersChanged += ServerPlayersChanged;
            }
            else
            {
                Leader.Server.PlayersChanged -= ServerPlayersChanged;
            }
        }

        private void ServerPlayersChanged(object sender, CollectionChangedEventArgs<User> e)
        {
            lock (playersSync)
            {
                if (disposed)
                {
                    return;
                }

                if (e.HasAdded)
                {
                    foreach (var added in e.Added)
                    {
                        AddPlayerVm(added);
                    }
                }

                if (e.HasRemoved)
                {
                    foreach (var removed in e.Removed)
                    {
                        RemovePlayerVm(removed);
                    }
                }
            }
        }

        #endregion

        #region Disposing

        protected override void DisposeResources()
        {
            lock (playersSync)
            {
                if (disposed)
                {
                    return;
                }
                disposed = true;

                foreach (var player in playersVmsByPlayers.Keys.ToArray())
                {
                    RemovePlayerVm(player);
                }
            }    

            base.DisposeResources();
        }

        #endregion
    }
}
