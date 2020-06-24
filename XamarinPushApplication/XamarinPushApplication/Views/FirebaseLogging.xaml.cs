
using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;
using XamarinPushApplication.Interfaces;

namespace XamarinPushApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class FirebaseLogging : ContentPage
    {
        private readonly IFirebaseMessaging _messaging;
        public FirebaseLogging(IFirebaseMessaging messaging)
        {
            _messaging = messaging;
            InitializeComponent();
        }

        async void OnLogButtonClicked(object sender, EventArgs e)
        {
            string token = await _messaging.GetToken();
            if (string.IsNullOrEmpty(token))
            {
                Log.Warning("Messaging", "No token found");
                BindingContext = string.Empty;
            }
            else
            {
                Log.Warning("Messaging", $"Firebase token : {token}");
                BindingContext = token;
            }
        }
    }
}