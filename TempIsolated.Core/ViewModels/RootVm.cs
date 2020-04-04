using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Core.ViewModels
{
    public sealed class RootVm : ObservingVm<Root>
    {
        #region Fields

        private readonly Dictionary<Type, Func<Mode, ModeVm>> modesVmsFactoriesByTypes = new Dictionary<Type, Func<Mode, ModeVm>>();

        private readonly Dictionary<Mode, ModeVm> activeModesVmsByModes = new Dictionary<Mode, ModeVm>();
        private readonly ObservableCollection<ModeVm> activeModesVms = new ObservableCollection<ModeVm>();

        private readonly object sync = new object();

        #endregion

        #region Properties

        public UserEditorVm UserEditorVm { get; }

        public bool HasActiveModes => ActiveModesVms.Count > 0;

        public ReadOnlyObservableCollection<ModeVm> ActiveModesVms { get; }

        public IReadOnlyList<ActionCommand> ModesCreationCommands { get; }

        #endregion

        #region Ctor

        public RootVm(Root root, IReadOnlyList<IModeFactory> modesFactories)
            : base(root)
        {
            Contracts.Requires(root != null);
            Contracts.Requires(modesFactories != null);

            foreach (var factory in modesFactories)
            {
                modesVmsFactoriesByTypes.Add(factory.ModeType, factory.CreateVm);
            }

            UserEditorVm = new UserEditorVm(root.User);

            ActiveModesVms = new ReadOnlyObservableCollection<ModeVm>(activeModesVms);

            ModesCreationCommands = modesFactories.Select(factory => CreateModeCreationCommand(factory)).ToArray();

            Initialize();
        }

        #endregion

        #region Private methods

        private ActionCommand CreateModeCreationCommand(IModeFactory modeFactory)
        {
            return new ActionCommand(
                canExecute: () => true,
                execute: () =>
                {
                    var mode = modeFactory.Create();
                    Model.AddActiveMode(mode);
                },
                publicName: modeFactory.Name);
        }

        private void AddModeVm(Mode mode)
        {
            lock (sync)
            {
                if (!modesVmsFactoriesByTypes.TryGetValue(mode.GetType(), out var createVm))
                {
                    throw new InvalidOperationException($"Unknown mode type '{mode.GetType().Name}'");
                }

                var modeVm = createVm(mode);
                activeModesVmsByModes.Add(mode, modeVm);
                activeModesVms.Add(modeVm);

                RaisePropertyChanged(nameof(HasActiveModes));
            }
        }

        private void RemoveModeVm(Mode mode)
        {
            lock (sync)
            {
                if (activeModesVmsByModes.TryGetValue(mode, out var modeVm))
                {
                    activeModesVms.Remove(modeVm);
                    activeModesVmsByModes.Remove(mode);
                    modeVm.Dispose();

                    RaisePropertyChanged(nameof(HasActiveModes));
                }
            }
        }

        #endregion

        #region Subscribes

        protected override void SubscribeModel(bool subscribe)
        {
            if (subscribe)
            {
                Model.ActiveModesChanged += ModelActiveModesChanged;
            }
            else
            {
                Model.ActiveModesChanged -= ModelActiveModesChanged;
            }
        }

        private void ModelActiveModesChanged(object sender, CollectionChangedEventArgs<Mode> e)
        {
            if (e.HasAdded)
            {
                foreach (var added in e.Added)
                {
                    AddModeVm(added);
                }
            }
            if (e.HasRemoved)
            {
                foreach (var removed in e.Removed)
                {
                    RemoveModeVm(removed);
                }
            }
        }

        #endregion

        #region Disposing

        protected override void DisposeResources()
        {
            lock (sync)
            {
                foreach (var mode in activeModesVmsByModes.Keys.ToArray())
                {
                    RemoveModeVm(mode);
                }
            }

            base.DisposeResources();
        }

        #endregion
    }
}
