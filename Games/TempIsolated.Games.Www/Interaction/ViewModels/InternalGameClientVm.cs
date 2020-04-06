using System.Windows.Input;
using TempIsolated.Common.Extensions;

namespace TempIsolated.Games.Www.Interaction.ViewModels
{
    public sealed class InternalGameClientVm : GameClientVm
    {
        public ActionCommand CommandConnect { get; }

        public InternalGameClientVm(InternalGameClient client)
        {
            Contracts.Requires(client != null);

            CommandConnect = new ActionCommand(() => client.Init(), Properties.Resources.Connect);
        }
    }
}
