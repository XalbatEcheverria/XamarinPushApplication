using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinPushApplication.Enums;
using XamarinPushApplication.Firebase;

namespace XamarinPushApplication.Views
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : MasterDetailPage
    {
        readonly Dictionary<int, NavigationPage> MenuPages = new Dictionary<int, NavigationPage>();
        private IFirebaseMessaging _messaging;

        public MainPage(IFirebaseMessaging messaging)
        {
            _messaging = messaging;
            InitializeComponent();
        }

        public async Task NavigateFromMenu(int id)
        {
            if (!MenuPages.ContainsKey(id))
            {
                switch (id)
                {
                    case (int)MenuItemType.Home:
                        MenuPages.Add(id, new NavigationPage(new HomePage()));
                        break;
                    case (int)MenuItemType.About:
                        MenuPages.Add(id, new NavigationPage(new AboutPage()));
                        break;
                    case (int)MenuItemType.EditStuff:
                        MenuPages.Add(id, new NavigationPage(new EditStuffPage()));
                        break;
                    case (int)MenuItemType.FirebaseLogging:
                        MenuPages.Add(id, new NavigationPage(new FirebaseLogging(_messaging)));
                        break;
                }
            }

            var newPage = MenuPages[id];

            if (newPage != null && Detail != newPage)
            {
                Detail = newPage;

                if (Device.RuntimePlatform == Device.Android)
                    await Task.Delay(100);

                IsPresented = false;
            }
        }
    }
}
