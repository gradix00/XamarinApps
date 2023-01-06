using System;
using Xamarin.Forms;
using Xamarin.Essentials;
using Xamarin.Forms.Xaml;
using TestBDonline.Scripts;
using System.Threading.Tasks;

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

		private async void Register(object sender, EventArgs e)
        {
			if (pwd1.Text == pwd2.Text)
			{
				var authentication = new Authentication();

				indicator.IsVisible = true;
				controls.IsEnabled = false;
				bool res = await Task.Run(()=> authentication.InitiateRegister(nickname.Text, login.Text, pwd1.Text));
				if (res)
				{
					await DisplayAlert("Zarejstrowano!", "Brawo! Udało Ci się zarejestrować w naszej aplikacji. Teraz zaloguj sie i zarabiaj :>", "Ok");

					authentication.CreateNewLog(new Scripts.Structs.EventData 
					{ 
						Autor = nickname.Text,
						Details = $"Utworzono nowe konto! Email: {login.Text}",
						Date = DateTime.Now
					});
					this.Navigation.RemovePage(this);
				}
				else
					await DisplayAlert("Nie można się zarejestrować!", "Przyczyny:\n-Hasłą się różnią od siebie\n-Brak połączenia z internetem\n-Nasz serwer bazy nie działa :(", "Ok");
			}
			else
				await DisplayAlert("Różne hasła!", "Popraw hasła, gdyż są różne", "Ok");
            indicator.IsVisible = false;
            controls.IsEnabled = true;
        }
	}
}