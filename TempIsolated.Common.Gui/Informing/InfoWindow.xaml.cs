using System.Windows;
using TempIsolated.Common.Extensions.ViewModels;

namespace TempIsolated.Common.Gui.Informing
{
    public partial class InfoWindow
    {
        private InfoWindow(InfoWindowContext context)
        {
            InitializeComponent();

            DataContext = context;
        }

        private void ButtonOkClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public static void Show(string caption, string info, string details = null)
        {
            var context = new InfoWindowContext(caption, info, details);
            var window = new InfoWindow(context);
            window.Show();
        }

        public static void ShowDialog(string caption, string info, string details = null)
        {
            var context = new InfoWindowContext(caption, info, details);
            var window = new InfoWindow(context);
            window.ShowDialog();
        }
    }

    public class InfoWindowContext : NotifyPropertyChanged
    {
        private readonly string caption;
        private readonly string info;
        private readonly string details;

        private readonly bool isExpandable;
        private bool isExpanded;

        public string Caption
        {
            get { return caption; }
        }

        public string Info
        {
            get { return info; }
        }

        public string Details
        {
            get { return details; }
        }

        public bool IsExpandable
        {
            get { return isExpandable; }
        }

        public bool IsExpanded
        {
            get { return isExpanded; }
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;
                    RaisePropertyChanged(nameof(IsExpanded));
                }
            }
        }

        public InfoWindowContext(string caption, string info, string details)
        {
            this.caption = caption;
            this.info = info;
            this.details = details;

            isExpandable = !string.IsNullOrWhiteSpace(details);
        }
    }
}
