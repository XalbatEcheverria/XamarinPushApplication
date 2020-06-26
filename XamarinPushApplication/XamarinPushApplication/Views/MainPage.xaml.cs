using System.Collections.Generic;
using System.ComponentModel;
using Xamarin.Forms;
using XamarinPushApplication.Enums;
using XamarinPushApplication.Interfaces;

namespace XamarinPushApplication.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        readonly Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        private readonly ITokenAccessor _tokenAccessor;

        public MainPage(ITokenAccessor tokenAccessor, int id = 0)
        {
            _tokenAccessor = tokenAccessor;
            InitializeComponent();
            NavigateFromMenu(id);
        }

        public void NavigateFromMenu(int id)
        {
            var messageManager = InjectionContainer.IoCContainer.GetInstance<IMessageManager>();
            if (id == (int)RequestedPage.MFA && MenuPages.ContainsKey(id))
                MenuPages.Remove(id);
            else if (id == (int)RequestedPage.Home && messageManager.MessagePending)
                id = (int)RequestedPage.MFA;

            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)RequestedPage.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));
                        break;
                    case (int)RequestedPage.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)RequestedPage.EditStuff:
                        MenuPages.Add(id, new NavigationPage(new EditStuffPage()));
                        break;
                    case (int)RequestedPage.FirebaseLogging:
                        MenuPages.Add(id, new NavigationPage(new FirebaseLogging(_tokenAccessor)));
                        break;
                    case (int)RequestedPage.MFA:
                        MenuPages.Add(id, new NavigationPage(new MFA()));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;
                IsPresented = false;
            }
        }
    }
}
