using System;
using TempIsolated.Common.Extensions;

namespace TempIsolated.Games.Www.Questions
{
    public sealed class TextQuestion : Question
    {
        public string Text { get; }

        public TextQuestion(string title, string text, TimeSpan thinkingTime, TimeSpan fillTime)
            : base(title, thinkingTime, fillTime)
        {
            Contracts.Requires(text != null);

            Text = text;
        }
    }
}
