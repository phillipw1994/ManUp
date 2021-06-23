using System;
using System.Windows.Input;
using Xamarin.Forms;

namespace Man_Up.ViewModels
{
    public class AboutViewModel : LocationViewModel
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("http://powerplayit.nz")));
        }

        /// <summary>
        /// Command to open browser to xamarin.com
        /// </summary>
        public ICommand OpenWebCommand { get; }
    }
}
