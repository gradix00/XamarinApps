using System;
using Xamarin.Forms;
using System.Text;
using TestBDonline.Scripts;
using Xamarin.Essentials;

namespace TestBDonline.View
{
    public partial class LoginPage : ContentPage
    {
        public LoginPage()
        {
            InitializeComponent();
            float width = (float)DeviceDisplay.MainDisplayInfo.Width/2;
            login.WidthRequest = width;
            pwd.WidthRequest = width;
        }

        private void Login(object sender, EventArgs e)
        {
            string log = login.Text;
            string pass = pwd.Text;

            var authentication = new Authentication();
            if (authentication.InitiateLogin(log, pass))
            {
                DisplayAlert("Zalogowano!", $"Witaj {authentication.UserData.Nickname}! Jak ci mija dzień?", "Ok");
                Navigation.PushAsync(new Main(authentication));
                this.Navigation.RemovePage(this);
            }
            else
                DisplayAlert("Nie można się zalogować!", "Przyczyny:\n-Login lub hasło jest poprawne\n-Brak połączenia z internetem\n-Nasz serwer bazy nie działa :(", "Ok");
        }

        private void OpenRegisterPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage(login.Text));
        }
    }
}
