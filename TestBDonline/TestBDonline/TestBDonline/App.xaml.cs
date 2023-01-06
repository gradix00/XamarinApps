using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TestBDonline.View;
using TestBDonline.Scripts;
using TestBDonline.Scripts.Structs;
using System.Threading.Tasks;

namespace TestBDonline
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            MainPage = new NavigationPage(new LoginPage());
        }

        protected override void OnStart()
        {
        }

        protected override async void OnSleep()
        {
            Authentication auth = new Authentication();
            var users = await Task.Run(()=> auth.GetListAllUsers());
            string email = GeneralSettings.LastEmail;

            foreach(UserData user in users)
                if(user.Email == email)
                {
                    var s = user;
                    s.IsActive = false;
                    await Task.Run(()=> auth.UpdateUserData(s));
                }
        }

        protected override async void OnResume()
        {
            Authentication auth = new Authentication();
            var users = await Task.Run(() => auth.GetListAllUsers());
            string email = GeneralSettings.LastEmail;

            foreach (UserData user in users)
                if (user.Email == email)
                {
                    var s = user;
                    s.IsActive = true;
                    await Task.Run(() => auth.UpdateUserData(s));
                }
        }
    }
}
