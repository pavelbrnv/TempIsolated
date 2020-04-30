namespace TempIsolated.Games.Www
{
    public sealed class Answer
    {
        public string Text { get; }

        public Answer(string text)
        {
            Text = text ?? string.Empty;
        }
    }
}
