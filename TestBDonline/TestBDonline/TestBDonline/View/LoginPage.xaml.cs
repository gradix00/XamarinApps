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
        }

        private void Login(object sender, EventArgs e)
        {
            indicator.IsRunning = true;
            layout.IsEnabled = false;
            string log = login.Text;
            string pass = pwd.Text;
            Task.Delay(750); 

            var authentication = new Authentication();
            if (authentication.InitiateLogin(log, pass))
            {
                DisplayAlert("Zalogowano!", $"Witaj {authentication.UserData.Nickname}! Jak ci mija dzień?", "Ok");
                Navigation.PushAsync(new Main(authentication));

                authentication.CreateNewLog(new Scripts.Structs.EventData
                {
                    Autor = authentication.UserData.Nickname,
                    Details = $"Zalogowano się na konto. Email: {login.Text}",
                    Date = DateTime.Now
                });
                this.Navigation.RemovePage(this);
            }
            else
                DisplayAlert("Nie można się zalogować!", "Przyczyny:\n-Login lub hasło jest poprawne\n-Brak połączenia z internetem\n-Nasz serwer bazy nie działa :(", "Ok");
           
            indicator.IsRunning = false;
            layout.IsEnabled = true;
        }

        private void OpenRegisterPage(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RegisterPage(login.Text));
        }
    }
}
