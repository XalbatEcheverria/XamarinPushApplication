using Xamarin.Forms;
using XamarinPushApplication.Firebase;
using XamarinPushApplication.Views;

namespace XamarinPushApplication
{
    public partial class App : Application
    {
        private IFirebaseMessaging _messaging;

        public App(IFirebaseMessaging messaging)
        {
            InitializeComponent();
            _messaging = messaging;
            MainPage = new MainPage(messaging);
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
