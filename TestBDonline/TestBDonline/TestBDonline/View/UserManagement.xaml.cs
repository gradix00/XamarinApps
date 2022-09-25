using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TestBDonline.Scripts;
using System.Collections.ObjectModel;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UserManagement : ContentPage
    {
        Authentication Data { get; set; }

        public UserManagement(Authentication data)
        {
            InitializeComponent();
            Data = data;
            LoadListUsers(Data);
        }

        private async void RefresUsersList(object sender, EventArgs e)
        {
            await Task.Delay(1000);
            LoadListUsers(Data);
            btn.IsEnabled = false;
        }

        private void LoadListUsers(Authentication data)
        {
            ObservableCollection<UserCell> userCells = new ObservableCollection<UserCell>();
            var usersData = data.GetListAllUsers();

            foreach(var user in usersData)
            {
                userCells.Add(new UserCell
                {
                    ID = user.ID,
                    Nickname = user.Nickname,
                    Points = user.Points
                });
            }        
            list.ItemsSource = userCells;
            list.IsRefreshing = false;
        }

        private void ManageUser(object sender, EventArgs e)
        {
            var user = Data.GetUserDataByID((list.SelectedItem as UserCell).ID);
            Navigation.PushAsync(new UserEditingData(user));
            list.SelectedItem = null;
        }

        private void SelectedItem(object sender, SelectedItemChangedEventArgs e)
        {
            if (list.SelectedItem != null)
                btn.IsEnabled = true;
            else
                btn.IsEnabled = false;
        }
    }
}