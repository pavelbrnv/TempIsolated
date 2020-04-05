using System;
using TempIsolated.Common.Extensions;

namespace TempIsolated.Games.Www
{
    public abstract class Question
    {
        public string Title { get; }

        public TimeSpan ThinkingTime { get; }
        
        public TimeSpan FillTime { get; }

        protected Question(string title, TimeSpan thinkingTime, TimeSpan fillTime)
        {
            Contracts.Requires(title != null);

            Title = title;
            ThinkingTime = thinkingTime;
            FillTime = fillTime;
        }
    }
}
