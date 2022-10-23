using System;
using Xamarin.Forms;
using System.Threading.Tasks;
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

            if (GeneralSettings.DataSave)
            {
                login.Text = GeneralSettings.LastEmail;
                pwd.Text = GeneralSettings.LastPassword;
                rememberData.IsChecked = true;
            }
        }

        private void Login(object sender, EventArgs e)
        {
            string log = login.Text;
            string pass = pwd.Text;

            if (rememberData.IsChecked)
            {
                //save login and pwd
                GeneralSettings.LastEmail = log;
                GeneralSettings.LastPassword = pass;
                GeneralSettings.DataSave = true;
            }

            var authentication = new Authentication();
            if (authentication.InitiateLogin(log, pass))
            {
                if(authentication.UserData.RequirePasswordReset)
                    Navigation.PushAsync(new ResetPasswordPage(authentication));
                else
                    Navigation.PushAsync(new Main(authentication));

                authentication.CreateNewLog(new Scripts.Structs.EventData
                {
                    Autor = authentication.UserData.Nickname,
                    Details = $"Zalogowano się na konto. Email: {login.Text}",
                    Date = DateTime.UtcNow
                });
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
