using System;

namespace TempIsolated.Common.Extensions
{
    public static class Contracts
    {
        public static void Requires(bool value)
        {
            if (!value)
            {
                throw new Exception("Invalid contract condition");
            }
        }
    }
}
