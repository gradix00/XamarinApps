using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using TestBDonline.Scripts;

namespace TestBDonline.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RegisterPage : ContentPage
	{
		public RegisterPage (string email)
		{
			InitializeComponent ();

			login.Text = email;
			float width = (float)DeviceDisplay.MainDisplayInfo.Width;
			nickname.WidthRequest = width;
			login.WidthRequest = width;
			pwd1.WidthRequest = width;
			pwd2.WidthRequest = width;
		}

		private void Register(object sender, EventArgs e)
        {
			if (pwd1.Text == pwd2.Text)
			{
				var authentication = new Authentication();
				if (authentication.InitiateRegister(nickname.Text, login.Text, pwd1.Text))
				{
					DisplayAlert("Zarejstrowano!", "Brawo! Udało Ci się zarejestrować w naszej aplikacji. Teraz zaloguj sie i zarabiaj :>", "Ok");
					this.Navigation.RemovePage(this);
				}
				else
					DisplayAlert("Nie można się zarejestrować!", "Przyczyny:\n-Hasłą się różnią od siebie\n-Brak połączenia z internetem\n-Nasz serwer bazy nie działa :(", "Ok");
			}
			else
				DisplayAlert("Różne hasła!", "Popraw hasła, gdyż są różne", "Ok");
        }
	}
}