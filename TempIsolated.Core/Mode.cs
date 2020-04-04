using System;

namespace TempIsolated.Core
{
    public abstract class Mode : IDisposable
    {
        public abstract void Dispose();
    }
}
