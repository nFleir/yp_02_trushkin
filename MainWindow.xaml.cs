using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using System.Windows;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using System;

namespace yp_02_trushkin
{
    public partial class MainWindow : Window
    {
        int CurrentUserId;
        private readonly Entities db = new Entities();
        public MainWindow()
        {
            InitializeComponent();
            LoadData();
            FilterAds();

            if (Application.Current.Resources.Contains("CurrentUserID"))
            {
                CurrentUserId = (int)Application.Current.Resources["CurrentUserID"];
            }
            else
            {
                CurrentUserId = -1;
            }

            if (CurrentUserId > 0)
            {
                EnterBtn.Visibility = Visibility.Collapsed;
                ProfileBtn.Visibility = Visibility.Visible;
            }

            CityComboBox.SelectionChanged += (s, e) => FilterAds();
            CategoryComboBox.SelectionChanged += (s, e) => FilterAds();
            TypeComboBox.SelectionChanged += (s, e) => FilterAds();
            SearchTextBox.TextChanged += (s, e) => FilterAds();
        }

        private void LoadData()
        {
            CityComboBox.ItemsSource = db.cities.ToList();
            CityComboBox.DisplayMemberPath = "city_name";
            CityComboBox.SelectedValuePath = "city_id";

            CategoryComboBox.ItemsSource = db.ad_categories.ToList();
            CategoryComboBox.DisplayMemberPath = "ad_category_name";
            CategoryComboBox.SelectedValuePath = "ad_category_id";

            TypeComboBox.ItemsSource = db.ad_types.ToList();
            TypeComboBox.DisplayMemberPath = "ad_type_name";
            TypeComboBox.SelectedValuePath = "ad_type_id";

        }

        private void FilterAds()
        {
            var ads = db.ads.AsQueryable();

            int completedStatusId = GetCompletedStatusId();
            if (completedStatusId != -1)
            {
                ads = ads.Where(a => a.ad_status_id != completedStatusId);
            }

            if (CityComboBox.SelectedValue != null)
            {
                int cityId = (int)CityComboBox.SelectedValue;
                ads = ads.Where(a => a.city_id == cityId);
            }

            if (CategoryComboBox.SelectedValue != null)
            {
                int categoryId = (int)CategoryComboBox.SelectedValue;
                ads = ads.Where(a => a.ad_category_id == categoryId);
            }

            if (TypeComboBox.SelectedValue != null)
            {
                int typeId = (int)TypeComboBox.SelectedValue;
                ads = ads.Where(a => a.ad_type_id == typeId);
            }


            string searchText = SearchTextBox.Text.Trim().ToLower();
            if (!string.IsNullOrEmpty(searchText))
            {
                ads = ads.Where(a => a.title.ToLower().Contains(searchText) || a.description.ToLower().Contains(searchText));
            }

            ListAds.ItemsSource = ads.ToList();
        }

        private void SearchTextBox_PreviewMouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (SearchTextBox.Text == "Поиск по объявлениям...") SearchTextBox.Clear();
        }

        private void ClearFilters_Click(object sender, RoutedEventArgs e)
        {
            CityComboBox.SelectedValue = null;
            CategoryComboBox.SelectedValue = null;
            TypeComboBox.SelectedValue = null;
            SearchTextBox.Text = "Поиск по объявлениям...";

            FilterAds();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Account.Autorization());
        }

        private void BtnProfile_Click(object sender, RoutedEventArgs e)
        {
            MainFrame.Navigate(new Account.Profile());
        }

        private int GetCompletedStatusId()
        {
            var completedStatus = db.ad_statuses.FirstOrDefault(s => s.ad_status_name == "Завершено");
            return completedStatus?.ad_status_id ?? -1;
        }


    }

}
