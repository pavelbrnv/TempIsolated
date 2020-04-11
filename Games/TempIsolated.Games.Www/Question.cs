using System;
using TempIsolated.Common.Extensions;

namespace TempIsolated.Games.Www
{
    public sealed class Question
    {
        public string Title { get; }

        public string Text { get; }

        public TimeSpan ThinkingTime { get; }
        
        public TimeSpan FillTime { get; }

        public Question(string title, string text, TimeSpan thinkingTime, TimeSpan fillTime)
        {
            Contracts.Requires(title != null);
            Contracts.Requires(text != null);

            Title = title;
            Text = text;
            ThinkingTime = thinkingTime;
            FillTime = fillTime;
        }
    }
}
