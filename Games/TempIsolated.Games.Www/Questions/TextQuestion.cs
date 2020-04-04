using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TempIsolated.Common.Extensions;

namespace TempIsolated.Games.Www.Questions
{
    public sealed class TextQuestion : Question
    {
        public string Text { get; }

        public TextQuestion(string text, TimeSpan thinkingTime, TimeSpan fillTime)
            : base(thinkingTime, fillTime)
        {
            Contracts.Requires(text != null);

            Text = text;
        }
    }
}
