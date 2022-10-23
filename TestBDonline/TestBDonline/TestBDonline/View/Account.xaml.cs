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
            nick.Text = $"{data.UserData.Nickname}";
            pts.Text = $"Twoje punkty\n{data.UserData.Points}";
        }
    }
}