using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

namespace Scrappy.Views.Rutor
{
    public partial class RutorReleaseView
    {
        public RutorReleaseView()
        {
            this.InitializeComponent();
        }

        private void OnImdbClick(object sender, MouseButtonEventArgs e)
        {
            dynamic context = DataContext;

            Process.Start(context.Release.ImdbLink);
        }

        private void OnSourceClick(object sender, MouseButtonEventArgs e)
        {
            dynamic context = DataContext;

            Process.Start(context.Release.Link);
        }

        private void OnHashClick(object sender, MouseButtonEventArgs e)
        {
            dynamic context = DataContext;

            Clipboard.SetText(context.Release.Hash);
        }
    }
}