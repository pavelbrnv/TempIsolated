using TempIsolated.Common.Extensions;
using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Games.Www.ViewModels
{
    public sealed class SelectableQuestionVm : NotifyPropertyChanged
    {
        private bool isSelected;

        public Question Question { get; }

        public string Title => Question.Title;

        public bool IsSelected
        {
            get => isSelected;
            set
            {
                if (isSelected != value)
                {
                    isSelected = value;
                    RaisePropertyChanged(nameof(IsSelected));
                }
            }
        }

        public SelectableQuestionVm(Question question)
        {
            Contracts.Requires(question != null);

            Question = question;
        }
    }
}
