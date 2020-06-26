﻿using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
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
            Loading.IsVisible = true;
            Loading.IsRunning = true;
            await Task.Delay(2000);
            Loading.IsVisible = false;
            Loading.IsRunning = false;
            BackgroundColor = actionColor;
            await Task.Delay(1000);
            ConfirmTreatment();
        }

        private void ConfirmTreatment()
        {
            _messaging.MessagePending = false;
            _messaging.MessageData = null;
            _messaging.DeleteNotification(_messaging.NotificationId);
            _messaging.NotificationId = null;
        }
    }
}