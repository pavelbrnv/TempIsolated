using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core;

namespace TempIsolated.Games.Www
{
    public sealed class WwwPlayer : Mode
    {
        #region Fields

        private readonly ILogger logger;

        #endregion

        #region Ctor

        public WwwPlayer(ILogger logger)
        {
            Contracts.Requires(logger != null);

            this.logger = logger;
        }

        #endregion

        #region Disposing

        public override void Dispose()
        {
        }

        #endregion
    }
}
