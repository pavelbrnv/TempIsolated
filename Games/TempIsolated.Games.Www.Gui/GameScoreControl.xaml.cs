using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace TempIsolated.Games.Www.Gui
{
    public partial class GameScoreControl
    {
        public GameScoreControl()
        {
            InitializeComponent();

            scoreGrid.ItemsSource = null;
            scoreGrid.Columns.Clear();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            scoreGrid.ItemsSource = null;
            scoreGrid.Columns.Clear();

            var gameScore = e.NewValue as GameScore;
            if (gameScore == null)
            {
                return;
            }

            scoreGrid.ItemsSource = gameScore.QuestionsScores;
            foreach (var playerName in gameScore.PlayersNames)
            {
                var column = new DataGridTextColumn
                {
                    Header = playerName,
                    Binding = new Binding($"[{playerName}]")
                };
                scoreGrid.Columns.Add(column);
            }
        }
    }
}