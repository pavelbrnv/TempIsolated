using System.Collections.Generic;
using System.Linq;
using TempIsolated.Common.Extensions;

namespace TempIsolated.Games.Www
{
    public sealed class GameScore
    {
        public IReadOnlyList<string> PlayersNames { get; }

        public IReadOnlyList<QuestionScore> QuestionsScores { get; }

        private GameScore(IReadOnlyList<string> playersNames, IReadOnlyList<QuestionScore> questionsScores)
        {
            PlayersNames = playersNames;
            QuestionsScores = questionsScores;
        }

        public sealed class Builder
        {
            private readonly HashSet<string> playersNames = new HashSet<string>();
            private readonly List<QuestionScore> questionsScores = new List<QuestionScore>();

            public void AddAnswers(QuestionAnswers questionAnswers)
            {
                Contracts.Requires(questionAnswers != null);

                var questionScore = new QuestionScore(questionAnswers.Question.Title);
                foreach (var playerAnswer in questionAnswers.Answers)
                {
                    playersNames.Add(playerAnswer.Player.Name);
                    questionScore.SetPlayerScore(playerAnswer.Player.Name, playerAnswer.IsAnswerCorrect ? 1 : 0);
                }

                questionsScores.Add(questionScore);
            }

            public GameScore Build()
            {
                return new GameScore(playersNames.ToArray(), questionsScores);
            }
        }
    }    

    public sealed class QuestionScore
    {
        private readonly Dictionary<string, int> resultsByPlayers = new Dictionary<string, int>();

        public string QuestionTitle { get; }

        public int this[string playerName]
        {
            get
            {
                return resultsByPlayers.TryGetValue(playerName, out var score) ? score : 0;
            }
        }

        public QuestionScore(string questionTitle)
        {
            QuestionTitle = questionTitle;
        }

        public void SetPlayerScore(string playerName, int playerScore)
        {
            resultsByPlayers[playerName] = playerScore;
        }
    }
}
