using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System;
using TestBDonline.Scripts;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AdminPanel : ContentPage
    {
        private Authentication data;
        public AdminPanel(Authentication data)
        {
            InitializeComponent();
            this.data = data;
        }

        private void OpenPageUsersManagement(object sender, EventArgs e)
        {
            Navigation.PushAsync(new UserManagement(data));
        }
    }
}