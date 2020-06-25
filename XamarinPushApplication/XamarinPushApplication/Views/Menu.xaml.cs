using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using XamarinPushApplication.Enums;
using XamarinPushApplication.Models;

namespace XamarinPushApplication.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Menu : ContentPage
    {
        MainPage RootPage { get => Application.Current.MainPage as MainPage; }

        List<HomeMenuItem> MenuItems => new List<HomeMenuItem>
            {
                new HomeMenuItem { Id = RequestedPage.Home, Title = "Home" },
                new HomeMenuItem { Id = RequestedPage.About, Title = "About" },
                new HomeMenuItem { Id = RequestedPage.EditStuff, Title = "Edit Stuff" },
                new HomeMenuItem { Id = RequestedPage.FirebaseLogging, Title = "Firebase Logging" }
            };

        public Menu()
        {
            InitializeComponent();

            ListViewMenu.ItemsSource = MenuItems;
            ListViewMenu.SelectedItem = MenuItems[0];

            ListViewMenu.ItemSelected += (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;
                int id = (int)((HomeMenuItem)e.SelectedItem).Id;

                RootPage.NavigateFromMenu(id);
            };
        }
    }
}