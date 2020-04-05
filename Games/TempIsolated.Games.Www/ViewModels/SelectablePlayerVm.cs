using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;
using TempIsolated.Core;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class SelectablePlayerVm : NotifyPropertyChanged
    {
        private bool isSelected;

        public User Player { get; }

        public string Name => Player.Name;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    RaisePropertyChanged(nameof(IsSelected));
                }
            }
        }

        public SelectablePlayerVm(User player)
        {
            Contracts.Requires(player != null);

            Player = player;
        }
    }
}
