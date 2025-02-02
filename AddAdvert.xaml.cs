using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace yp_02_trushkin
{
    /// <summary>
    /// Логика взаимодействия для AddAdvert.xaml
    /// </summary>
    public partial class AddAdvert : Page
    {

        private readonly string pricePattern = @"^\d+$";
        private readonly string titlePattern = @"^[a-zA-Zа-яА-ЯёЁ0-9\s,.'-]+$";
        private readonly string descriptionPattern = @"^[a-zA-Zа-яА-ЯёЁ0-9\s,.'-]+$";

        int CurrentUserId;
        //CurrentUserId = (int) Application.Current.Resources["CurrentUserID"];
        private readonly Entities db = new Entities();
        private int? adIdToEdit;

        public AddAdvert(int? adId = null)
        {
            CurrentUserId = (int)Application.Current.Resources["CurrentUserID"];
            InitializeComponent();
            LoadCities();
            LoadCategories();
            LoadTypes();
            LoadStatuses();

            if (adId.HasValue)
            {
                adIdToEdit = adId.Value;
                LoadAdData(adIdToEdit.Value);
                AddAdButton.Content = "Сохранить изменения";
            }
            else
            {
                AddAdButton.Content = "Добавить объявление";
            }
        }

        private bool HasFullName()
        {
            var user = db.users.FirstOrDefault(u => u.user_id == CurrentUserId);
            return user != null && !string.IsNullOrWhiteSpace(user.first_name) && !string.IsNullOrWhiteSpace(user.last_name);
        }

        private void LoadAdData(int adId)
        {
            try
            {

                if (!HasFullName())
                {
                    MessageBox.Show("Для добавления или редактирования объявления необходимо указать ФИО.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var ad = db.ads.FirstOrDefault(a => a.ad_id == adId);
                if (ad != null)
                {

                    TitleTextBox.Text = ad.title;
                    DescriptionTextBox.Text = ad.description;
                    PriceTextBox.Text = ad.ad_price.ToString();
                    ImagePathTextBox.Text = ad.ad_image;

                    CityComboBox.SelectedValue = ad.city_id;
                    CategoryComboBox.SelectedValue = ad.ad_category_id;
                    AdTypeComboBox.SelectedValue = ad.ad_type_id;
                    StatusComboBox.SelectedValue = ad.ad_status_id;
                }
                else
                {
                    MessageBox.Show("Не удалось найти объявление для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке данных объявления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadCities()
        {
            var cities = db.cities.ToList();
            CityComboBox.ItemsSource = cities;
            CityComboBox.DisplayMemberPath = "city_name";
            CityComboBox.SelectedValuePath = "city_id";
        }

        private void LoadCategories()
        {
            var categories = db.ad_categories.ToList();
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.DisplayMemberPath = "ad_category_name";
            CategoryComboBox.SelectedValuePath = "ad_category_id";
        }

        private void LoadTypes()
        {
            var types = db.ad_types.ToList();
            AdTypeComboBox.ItemsSource = types;
            AdTypeComboBox.DisplayMemberPath = "ad_type_name";
            AdTypeComboBox.SelectedValuePath = "ad_type_id";
        }

        private void LoadStatuses()
        {
            var statuses = db.ad_statuses.ToList();
            StatusComboBox.ItemsSource = statuses;
            StatusComboBox.DisplayMemberPath = "ad_status_name";
            StatusComboBox.SelectedValuePath = "ad_status_id";
        }

        private void AddAdButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(TitleTextBox.Text) ||
                    string.IsNullOrWhiteSpace(DescriptionTextBox.Text) ||
                    string.IsNullOrWhiteSpace(PriceTextBox.Text) ||
                    CityComboBox.SelectedItem == null ||
                    CategoryComboBox.SelectedItem == null ||
                    AdTypeComboBox.SelectedItem == null ||
                    StatusComboBox.SelectedItem == null ||
                    string.IsNullOrWhiteSpace(ImagePathTextBox.Text))
                {
                    MessageBox.Show("Пожалуйста, заполните все поля, включая путь к картинке.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!Regex.IsMatch(TitleTextBox.Text, titlePattern))
                {
                    MessageBox.Show("Название объявления должно содержать только русские и английские буквы, цифры и пробелы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!Regex.IsMatch(DescriptionTextBox.Text, descriptionPattern))
                {
                    MessageBox.Show("Описание объявления должно содержать только русские и английские буквы, цифры и пробелы.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!Regex.IsMatch(PriceTextBox.Text, pricePattern))
                {
                    MessageBox.Show("Цена должна быть целым числом без запятой.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                decimal price = decimal.Parse(PriceTextBox.Text);

                if (adIdToEdit.HasValue)
                {
                    var existingAd = db.ads.FirstOrDefault(a => a.ad_id == adIdToEdit.Value);
                    if (existingAd != null)
                    {
                        existingAd.title = TitleTextBox.Text;
                        existingAd.description = DescriptionTextBox.Text;
                        existingAd.ad_price = price;
                        existingAd.ad_image = ImagePathTextBox.Text;
                        existingAd.city_id = (int)CityComboBox.SelectedValue;
                        existingAd.ad_category_id = (int)CategoryComboBox.SelectedValue;
                        existingAd.ad_type_id = (int)AdTypeComboBox.SelectedValue;
                        existingAd.ad_status_id = (int)StatusComboBox.SelectedValue;

                        db.SaveChanges();

                        MessageBox.Show("Объявление обновлено успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                        NavigationService?.Navigate(new Account.Profile());
                    }
                    else
                    {
                        MessageBox.Show("Не удалось найти объявление для обновления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    var newAd = new ads
                    {
                        user_id = CurrentUserId,
                        ad_category_id = (int)CategoryComboBox.SelectedValue,
                        ad_type_id = (int)AdTypeComboBox.SelectedValue,
                        ad_status_id = (int)StatusComboBox.SelectedValue,
                        city_id = (int)CityComboBox.SelectedValue,
                        title = TitleTextBox.Text,
                        description = DescriptionTextBox.Text,
                        ad_price = price,
                        ad_image = ImagePathTextBox.Text,
                        publish_date = DateTime.Now
                    };

                    db.ads.Add(newAd);
                    db.SaveChanges();

                    MessageBox.Show("Объявление добавлено успешно!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    NavigationService?.Navigate(new Account.Profile());
                }

                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при добавлении/редактировании объявления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ClearForm()
        {
            TitleTextBox.Clear();
            DescriptionTextBox.Clear();
            PriceTextBox.Clear();
            ImagePathTextBox.Clear();
            CityComboBox.SelectedIndex = -1;
            CategoryComboBox.SelectedIndex = -1;
            AdTypeComboBox.SelectedIndex = -1;
            StatusComboBox.SelectedIndex = -1;
        }

        private void BackAdButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService?.Navigate(new Account.Profile());
        }
    }
}
