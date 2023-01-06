using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TestBDonline.Scripts;
using TestBDonline.Scripts.Structs;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : TabbedPage
    {
        public UserData CurrentUser { get; }

        public Main(Authentication data)
        {
            CurrentUser = data.UserData;
            InitializeComponent();
            if (data.UserData.Status == Scripts.Structs.Status.banned)
            {
                main.Children.Add(new BannedInfo());               
            }
            else
            {
                main.Children.Add(new UsersActivity(data.UserData));
                main.Children.Add(new News(data));
                main.Children.Add(new Account(data));
                if (data.UserData.Status == Scripts.Structs.Status.admin)
                    main.Children.Add(new AdminPanel(data));

                CurrentPage = main.Children[1];
            }
        }
    }
}