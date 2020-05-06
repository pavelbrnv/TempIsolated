using System;
using System.Threading;

namespace TempIsolated.Common.Extensions.ViewModels
{
    public sealed class TimerVm : NotifyPropertyChanged, IDisposable
    {
        #region Fields

        private readonly TimeSpan time;
        private readonly TimeSpan updatePeriod;
        private readonly Action callback;

        private readonly Timer timer;

        private DateTime startTime;

        private string text;      

        #endregion

        #region Properties

        public string Text
        {
            get => text;
            private set
            {
                text = value;
                RaisePropertyChanged(nameof(Text));
            }
        }

        #endregion

        #region Ctor

        public TimerVm(TimeSpan time, TimeSpan updatePeriod, Action callback = null)
        {
            this.time = time;
            this.updatePeriod = updatePeriod;
            this.callback = callback;

            timer = new Timer(args => Update());
        }

        #endregion

        #region Public methods

        public void Start()
        {
            startTime = DateTime.Now;
            timer.Change(TimeSpan.Zero, updatePeriod);
        }

        public void Stop()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        #endregion

        #region Private methods

        private void Update()
        {
            var operationTime = DateTime.Now - startTime;

            if (operationTime >= time)
            {
                operationTime = time;

                Stop();

                callback?.Invoke();
            }

            var timeLeft = time - operationTime;
            Text = timeLeft.ToString("mm':'ss");
        }

        #endregion

        #region Disposing

        public void Dispose()
        {
            timer.Dispose();
        }

        #endregion
    }
}
