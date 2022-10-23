using System;
using TestBDonline.Scripts;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestBDonline.View
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ResetPasswordPage : ContentPage
	{
		Authentication Data { get; set; }
		public ResetPasswordPage(Authentication data)
        {
            InitializeComponent();
            Data = data;
        }

        private void ChangePassword(object sender, EventArgs e)
        {
            if (Data.ChangePasswordUser(Data.UserData, pwd.Text))
            {
                DisplayAlert("Informacja", "Twoje hasło zostało zmienione pomyślnie. Zaloguj się na konto ponownie.", "Ok");
                Navigation.PushAsync(new LoginPage());
                Navigation.RemovePage(this);
            }
            else
                DisplayAlert("Błąd", "Nie można zmienić hasła! Sprawdź połączenie sieciowe lub urządzenie", "Ok");
        }
    }
}