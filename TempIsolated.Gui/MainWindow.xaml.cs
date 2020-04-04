using System;
using TempIsolated.Common.Gui.Informing;
using TempIsolated.Core;
using TempIsolated.Core.Gui;
using TempIsolated.Core.ViewModels;
using TempIsolated.Games.Www;
using TempIsolated.Games.Www.Interaction;

namespace TempIsolated.Gui
{
    public partial class MainWindow
    {
        private readonly Root root;
        private readonly RootVm rootVm;
        private readonly RootControl rootControl;

        public MainWindow()
        {
            InitializeComponent();

            Closed += WindowClosed;

            var logger = new InfoWindowLogger();

            var modesFactories = new IModeFactory[]
            {
                new WwwLeaderFactory(new DummyGameServer(), logger),
                new WwwPlayerFactory(logger)
            };

            root = new Root(new Storage());
            rootVm = new RootVm(root, modesFactories);
            rootControl = new RootControl() { DataContext = rootVm };

            Content = rootControl;
        }

        private void WindowClosed(object sender, EventArgs e)
        {
            Content = null;

            rootVm?.Dispose();
            root?.Dispose();
        }
    }
}
