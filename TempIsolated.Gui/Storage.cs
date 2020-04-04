using System;
using TempIsolated.Common.Extensions;
using TempIsolated.Core;
using TempIsolated.Gui.Properties;

namespace TempIsolated.Gui
{
    public sealed class Storage : IStorage
    {
        public User LoadUser()
        {
            var userId = Settings.Default.UserId;
            var userName = Settings.Default.UserName;

            if (userId == Guid.Empty)
            {
                userId = Guid.NewGuid();

                Settings.Default.UserId = userId;
                Settings.Default.Save();
            }

            return new User(userId, userName);
        }

        public void SaveUser(User user)
        {
            Contracts.Requires(user != null);

            Settings.Default.UserId = user.Id;
            Settings.Default.UserName = user.Name;

            Settings.Default.Save();
        }
    }
}
