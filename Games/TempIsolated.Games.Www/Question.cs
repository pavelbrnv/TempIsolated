using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TempIsolated.Games.Www
{
    public abstract class Question
    {
        public TimeSpan ThinkingTime { get; }
        
        public TimeSpan FillTime { get; }

        protected Question(TimeSpan thinkingTime, TimeSpan fillTime)
        {
            ThinkingTime = thinkingTime;
            FillTime = fillTime;
        }
    }
}
