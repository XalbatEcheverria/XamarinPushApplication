using Xamarin.Forms;
using XamarinPushApplication.Interfaces;
using XamarinPushApplication.Views;

namespace XamarinPushApplication
{
    public partial class App : Application
    {
        private static bool WasMFA = false;
        private readonly ITokenAccessor _tokenAccessor;

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
            if (!messageManager.MessagePending && WasMFA)
            {
                MainPage = new MainPage(_tokenAccessor);
            }
            base.OnResume();
        }
    }
}
