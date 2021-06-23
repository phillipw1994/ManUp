using Man_Up.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Man_Up.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : ContentPage
    {
        bool ShownMessage = false;

        public HomePage()
        {
            InitializeComponent();
        }

        /*async void AddItem_Clicked(object sender, EventArgs e)
        {
            //await Navigation.PushAsync(new NewItemPage());
            await DisplayAlert("Button Pressed", "Add New Button Pressed", "Cancel");
        }

        async void OnAlertYesNoClicked(object sender, EventArgs e)
        {
            var answer = await DisplayAlert("Question?", "Would you like to play a game", "Yes", "No");
            Debug.WriteLine("Answer: " + answer);
        }*/

        async void DeveloperInfo(object sender, EventArgs e)
        {
            await Navigation.PushAsync(new AboutPage() { Title = "About App" });
        }

        public void OnLoading()
        {
            if (Constants.BETA && !ShownMessage)
            {
                DisplayAlert("Man Up Canterbury:\nBeta-Release Version", "Current Beta Version is:" +  Constants.VERSION +"\n\nYou are currently running a Beta Version of the Man Up Canterbury App", "OK");
                ShownMessage = true;
            }
            else if (Constants.ALPHA && !ShownMessage)
            {
                DisplayAlert("Man Up Canterbury:\nAlpha-Release Version", "Current Alpha Version is:" + Constants.VERSION + "\n\nYou are currently running a Alpha Version of the Man Up Canterbury App", "OK");
                ShownMessage = true;
            }

        }

        void OpenWebPage(object sender, EventArgs args)
        {
            Device.OpenUri(new Uri("http://facebook.com/manupcanterbury"));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            OnLoading();
        }
    }
}