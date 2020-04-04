using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class WwwPlayerVm : ModeVm
    {
        #region Fields

        private readonly ILogger logger;

        #endregion

        #region Ctor

        public WwwPlayerVm(WwwPlayer player, ILogger logger)
            : base(player)
        {
            Contracts.Requires(player != null);
            Contracts.Requires(logger != null);

            this.logger = logger;
        }

        #endregion

        #region Sunscribes and handlers

        protected override void SubscribeModel(bool subscribe)
        {
            if (subscribe)
            {

            }
            else
            {

            }
        }

        #endregion
    }
}
