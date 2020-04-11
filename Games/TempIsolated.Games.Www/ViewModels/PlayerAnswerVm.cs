using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class PlayerAnswerVm : ObservingVm<PlayerAnswer>
    {
        #region Properties

        public string PlayerName => Model.Player.Name;

        #endregion

        #region Ctor

        public PlayerAnswerVm(PlayerAnswer answer)
            : base(answer)
        {
            Initialize();
        }

        #endregion

        #region Subscribes and handlers

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
