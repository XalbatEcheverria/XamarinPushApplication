using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinPushApplication.Enums;
using XamarinPushApplication.Interfaces;

namespace XamarinPushApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MFA : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        private readonly IMessageManager _messaging;

        public MFA()
        {
            _messaging = InjectionContainer.IoCContainer.GetInstance<IMessageManager>();
            InitializeComponent();
        }

        private void Accept_Invoked(object sender, EventArgs e)
        {
            BackgroundColor = Color.MediumSpringGreen;
            ConfirmTreatment();
        }

        private void Deny_Invoked(object sender, EventArgs e)
        {
            BackgroundColor = Color.OrangeRed;
            ConfirmTreatment();
        }

        private void ConfirmTreatment()
        {
            _messaging.MessagePending = false;
            _messaging.MessageData = null;
            _messaging.DeleteNotification(_messaging.NotificationId);
            _messaging.NotificationId = null;

            RootPage.NavigateFromMenu((int)RequestedPage.Home);
        }
    }
}