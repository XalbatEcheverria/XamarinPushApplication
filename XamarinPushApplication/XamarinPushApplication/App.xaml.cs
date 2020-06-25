using Xamarin.Forms;
using XamarinPushApplication.Interfaces;
using XamarinPushApplication.Views;

namespace XamarinPushApplication
{
    public partial class App : Application
    {
        public App(ITokenAccessor tokenAccessor, int id = 0)
        {
            InitializeComponent();
            Device.SetFlags(new[] {
                "SwipeView_Experimental"
            });
            MainPage = new MainPage(tokenAccessor, id);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
