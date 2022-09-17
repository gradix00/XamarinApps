using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TestBDonline.Scripts;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Account : ContentPage
    {
        public Account(Authentication data)
        {
            InitializeComponent();
            Title = "Konto";
            txt.Text = $"Twój nick: {data.UserData.Nickname}\nTwoje punkty: {data.UserData.Points}";
        }
    }
}