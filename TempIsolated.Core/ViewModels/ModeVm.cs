using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Core.ViewModels
{
    public abstract class ModeVm : ObservingVm<Mode>
    {
        protected ModeVm(Mode mode)
            : base(mode)
        {
        }
    }
}
