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
                new HomeMenuItem { Id = MenuItemType.Home, Title = "Home" },
                new HomeMenuItem { Id = MenuItemType.About, Title = "About" },
                new HomeMenuItem { Id = MenuItemType.EditStuff, Title = "Edit Stuff" }
            };

        public Menu()
        {
            InitializeComponent();

            ListViewMenu.ItemsSource = MenuItems;
            ListViewMenu.SelectedItem = MenuItems[0];

            ListViewMenu.ItemSelected += async (sender, e) =>
            {
                if (e.SelectedItem == null)
                    return;
                int id = (int)((HomeMenuItem)e.SelectedItem).Id;

                await RootPage.NavigateFromMenu(id);
            };
        }
    }
}