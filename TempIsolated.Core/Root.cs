using System;
using System.Collections.Generic;
using System.Linq;
using TempIsolated.Common.Extensions;

namespace TempIsolated.Core
{
    public sealed class Root : IDisposable
    {
        #region Fields

        private readonly IStorage storage;

        private readonly HashSet<Mode> activeModes = new HashSet<Mode>();

        private readonly object sync = new object();

        #endregion

        #region Properties

        public User User { get; }

        public IReadOnlyCollection<Mode> ActiveModes
        {
            get
            {
                lock (sync)
                {
                    return activeModes.ToArray();
                }                
            }
        }

        #endregion

        #region Ctor

        public Root(IStorage storage)
        {
            Contracts.Requires(storage != null);

            this.storage = storage;

            User = storage.LoadUser();

            SubscribeToUser(true);
        }

        #endregion

        #region Events

        public event EventHandler<CollectionChangedEventArgs<Mode>> ActiveModesChanged = delegate { };

        #endregion

        #region Events raisers

        private void OnActiveModesChanged(IReadOnlyList<Mode> added, IReadOnlyList<Mode> removed)
        {
            var args = new CollectionChangedEventArgs<Mode>(added, removed);
            ActiveModesChanged(this, args);
        }

        #endregion

        #region Public methods

        public void AddActiveMode(Mode mode)
        {
            Contracts.Requires(mode != null);

            lock (sync)
            {
                if (activeModes.Add(mode))
                {
                    OnActiveModesChanged(new[] { mode }, new Mode[0]);
                }
            }
        }

        public void RemoveActiveMode(Mode mode)
        {
            Contracts.Requires(mode != null);

            lock (sync)
            {
                if (activeModes.Remove(mode))
                {
                    OnActiveModesChanged(new Mode[0], new[] { mode });
                    mode.Dispose();
                }
            }
        }

        #endregion

        #region Subscribes and handlers

        private void SubscribeToUser(bool subscribe)
        {
            if (subscribe)
            {
                User.NameChanged += UserNameChanged;
            }
            else
            {
                User.NameChanged -= UserNameChanged;
            }
        }

        private void UserNameChanged(object sender, StringValueChangedEventArgs e)
        {
            storage.SaveUser(User);
        }

        #endregion

        #region Disposing

        public void Dispose()
        {
            foreach (var mode in ActiveModes)
            {
                RemoveActiveMode(mode);
            }

            SubscribeToUser(false);
        }

        #endregion
    }
}
