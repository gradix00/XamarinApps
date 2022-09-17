using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using TestBDonline.Scripts;
using System.Collections.ObjectModel;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserManagement : ContentPage
    {
        public UserManagement(Authentication data)
        {
            InitializeComponent();
            LoadListUsers(data);
        }

        private void LoadListUsers(Authentication data)
        {
            List<UserCell> userCells = new List<UserCell>();
            var usersData = data.GetListAllUsers();

            foreach(var user in usersData)
            {
                userCells.Add(new UserCell
                {
                    UserID = user.ID,
                    UserNickname = user.Nickname,
                    Points = user.Points
                });
            }
            list.ItemsSource = userCells;
        }
    }
}