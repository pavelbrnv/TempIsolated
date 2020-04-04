using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Informing;
using TempIsolated.Core;
using TempIsolated.Core.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class WwwLeaderVm : ModeVm
    {
        #region Fields

        private readonly ILogger logger;

        #endregion

        #region Ctor

        public WwwLeaderVm(WwwLeader leader, ILogger logger)
            : base(leader)
        {
            Contracts.Requires(leader != null);
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
