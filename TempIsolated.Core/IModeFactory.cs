using System;
using TempIsolated.Core.ViewModels;

namespace TempIsolated.Core
{
    public interface IModeFactory
    {
        string Name { get; }

        Type ModeType { get; }

        Mode Create();

        ModeVm CreateVm(Mode mode);
    }
}
