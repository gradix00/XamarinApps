using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TestBDonline.Scripts;
using TestBDonline.Scripts.Structs;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersActivity : ContentPage
    {
        UserData CurrentUser { get; }

        public UsersActivity(UserData currentUser)
        {
            InitializeComponent();
            CurrentUser = currentUser;
            Load(null, null);
        }

        private async void Load(object sender, EventArgs e) => LoadUsersActivity();

        private async void LoadUsersActivity()
        {
            indicator.IsVisible = true;
            list.IsVisible = false;

            list.Children.Clear();
            var auth = new Authentication();
            var users = await Task.Run(()=> auth.GetListAllUsers());

            List<UserData> usersActivity = new List<UserData>();
            foreach (UserData user in users)
                if (user.IsActive && user.Nickname != CurrentUser.Nickname)
                    usersActivity.Add(user);

            if (usersActivity.Count > 0)
                foreach (UserData user in usersActivity)
                    list.Children.Add(CreateElement(user));
            else
            {
                list.Children.Add(CreateElement(new UserData
                {
                    Nickname = "brak aktywnych użytkowników",
                    Gender = Gender.Unidentified
                }));
            }    

            indicator.IsVisible = false;
            list.IsVisible = true;
            refreshView.IsRefreshing = false;
        }

        private Frame CreateElement(UserData user)
        {
            StackLayout layout = new StackLayout
            {
                MinimumHeightRequest = 70,
                Orientation = StackOrientation.Horizontal,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            Label nickname = new Label
            {
                Text = user.Nickname,
                TextColor = Color.CornflowerBlue,
                FontAttributes = FontAttributes.Bold
            };

            Label gender = new Label
            {
                Text = user.Gender.ToString(),
                TextColor = Color.Green,
                FontAttributes = FontAttributes.Bold
            };

            if (user.Gender == Gender.Unidentified)
                gender.Text = null;

            layout.Children.Add(nickname);
            layout.Children.Add(gender);

            Frame fr = new Frame
            {
                BorderColor = Color.CornflowerBlue,
                CornerRadius = 10
            };

            fr.Content = layout;
            return fr;
        }
    }
}