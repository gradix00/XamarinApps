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

        private async void LoadListUsers(Authentication data)
        {
            ObservableCollection<UserCell> userCells = new ObservableCollection<UserCell>();
            var usersData = await Task.Run(()=> data.GetListAllUsers());

            foreach(var user in usersData)
            {
                userCells.Add(new UserCell
                {
                    ID = user.ID,
                    Nickname = user.Nickname,
                    Status = user.Status.ToString(),
                    Points = user.Points
                });
            }        
            list.ItemsSource = userCells;
            list.IsRefreshing = false;
        }

        private async void ManageUser(object sender, EventArgs e)
        {
            var auth = new Authentication();
            var userEdited =await Task.Run(()=> auth.GetUserDataByID((list.SelectedItem as UserCell).ID));
            await Navigation.PushAsync(new UserEditingData(Data, userEdited));
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