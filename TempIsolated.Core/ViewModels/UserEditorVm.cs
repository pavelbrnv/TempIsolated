using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Core.ViewModels
{
    public sealed class UserEditorVm : NotifyPropertyChanged
    {
        private readonly User user;

        public string Name
        {
            get => user.Name;
            set
            {
                user.Name = value;
                RaisePropertyChanged(nameof(Name));
            }
        }

        public UserEditorVm(User user)
        {
            Contracts.Requires(user != null);

            this.user = user;
        }
    }
}
