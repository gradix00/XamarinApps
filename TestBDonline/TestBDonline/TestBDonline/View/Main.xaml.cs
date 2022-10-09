using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using TestBDonline.Scripts;

namespace TestBDonline.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Main : CarouselPage
    {
        public Main(Authentication data)
        {
            InitializeComponent();
            if (data.UserData.Status == Scripts.Structs.Status.banned)
            {
                main.Children.Add(new BannedInfo());               
            }
            else
            {
                main.Children.Add(new News(data));
                if (data.UserData.Status == Scripts.Structs.Status.admin)
                    main.Children.Add(new AdminPanel(data));
            }
        }
    }
}