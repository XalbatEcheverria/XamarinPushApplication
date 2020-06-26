using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinPushApplication.Enums;
using XamarinPushApplication.Interfaces;
using XamarinPushApplication.ViewModels;

namespace XamarinPushApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MFA : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }
        private readonly IMessageManager _messaging;
        private MFAViewModel binding;

        public MFA()
        {
            _messaging = InjectionContainer.IoCContainer.GetInstance<IMessageManager>();
            binding = new MFAViewModel
            {
                Location = _messaging.MessageData["Location"],
                Target = _messaging.MessageData["Target"]
            };

            BindingContext = binding;

            InitializeComponent();
        }

        private async void Accept_Invoked(object sender, EventArgs e)
        {
            await ActionInvoked(Color.MediumSpringGreen);
            RootPage.NavigateFromMenu((int)RequestedPage.Home);
        }

        private async void Deny_Invoked(object sender, EventArgs e)
        {
            await ActionInvoked(Color.OrangeRed);
            RootPage.NavigateFromMenu((int)RequestedPage.Home);
        }

        private async Task ActionInvoked(Color actionColor)
        {
            Loading.Color = actionColor;
            Target.IsVisible = false;
            Location.IsVisible = false;
            SetLoadingStatus(true);
            await Task.Delay(2000);
            SetLoadingStatus(false);
            BackgroundColor = actionColor;
            await Task.Delay(1000);
            _messaging.DeleteMessage(_messaging.NotificationId);
        }

        private void SetLoadingStatus(bool loading)
        {
            Loading.IsVisible = loading;
            Loading.IsRunning = loading;
        }
    }
}