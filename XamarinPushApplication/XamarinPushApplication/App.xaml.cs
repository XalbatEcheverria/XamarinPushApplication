using System.Linq;
using Xamarin.Forms;
using XamarinPushApplication.Enums;
using XamarinPushApplication.Interfaces;
using XamarinPushApplication.Views;

namespace XamarinPushApplication
{
    public partial class App : Application
    {
        static bool WasMFA = false;
        ITokenAccessor _tokenAccessor;

        public App(ITokenAccessor tokenAccessor, int id = 0)
        {
            InitializeComponent();
            Device.SetFlags(new[] {
                "SwipeView_Experimental"
            });
            _tokenAccessor = tokenAccessor;
            MainPage = new MainPage(tokenAccessor, id);
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
            var messageManager = InjectionContainer.IoCContainer.GetInstance<IMessageManager>();
            WasMFA = messageManager.MessagePending;
        }

        protected override void OnResume()
        {
            var messageManager = InjectionContainer.IoCContainer.GetInstance<IMessageManager>();
            if (messageManager.MessagePending)
            {
                MainPage = new MainPage(_tokenAccessor, (int)RequestedPage.MFA);
            }
            else if (WasMFA)
            {
                MainPage = new MainPage(_tokenAccessor);
            }
            base.OnResume();
        }
    }
}
