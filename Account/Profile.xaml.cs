using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace yp_02_trushkin.Account
{
    /// <summary>
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : Page
    {
        private Entities db = new Entities();
        int CurrentUserId;
        public Profile()
        {
            InitializeComponent();
            CurrentUserId = (int)Application.Current.Resources["CurrentUserID"];
            LoadAds("Активно");
        }



        private void BtnActiveAds_Click(object sender, RoutedEventArgs e)
        {
            BtnActiveAds.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E7D32"));
            BtnCompletedAds.Background = new SolidColorBrush(Colors.Gray);

            LoadAds("Активно");
        }

        private void BtnCompletedAds_Click(object sender, RoutedEventArgs e)
        {
            BtnCompletedAds.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2E7D32"));
            BtnActiveAds.Background = new SolidColorBrush(Colors.Gray);

            LoadAds("Завершено");
        }

        private void LoadAds(string status)
        {
            try
            {
                var ads = db.ads
                    .Where(a => a.user_id == CurrentUserId && a.ad_statuses.ad_status_name == status)
                    .Select(a => new
                    {
                        a.ad_id,
                        a.title,
                        a.description,
                        a.ad_price,
                        a.publish_date,
                        CityName = a.cities.city_name,
                        CategoryName = a.ad_categories.ad_category_name,
                        TypeName = a.ad_types.ad_type_name,
                        StatusName = a.ad_statuses.ad_status_name,
                        a.ad_image
                    }).ToList();

                AdsListView.ItemsSource = ads;

                if (status == "Завершено")
                {
                    decimal totalProfit = ads.Sum(a => a.ad_price);
                    TotalProfitTextBlock.Text = $"Продажи принесли {totalProfit:C}";
                }
                else
                {
                    TotalProfitTextBlock.Text = "";
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки объявлений ({status}): {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }



        }

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            ProfileFrame.Navigate(new AddAdvert());
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedAd = null;

            if (AdsListView.SelectedItem != null)
            {
                selectedAd = AdsListView.SelectedItem;
            }


            if (selectedAd != null)
            {
                int adId = selectedAd.ad_id;
                ProfileFrame.Navigate(new AddAdvert(adId));
            }
            else
            {
                MessageBox.Show("Выберите объявление для редактирования.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedAd = null;

            if (AdsListView.SelectedItem != null)
            {
                selectedAd = AdsListView.SelectedItem;
            }



            if (selectedAd != null)
            {
                int advID = selectedAd.ad_id;

                if (MessageBox.Show("Хотите удалить выбранное объявление?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        var adToDelete = db.ads.FirstOrDefault(a => a.ad_id == advID);
                        if (adToDelete != null)
                        {
                            db.ads.Remove(adToDelete);
                            db.SaveChanges();
                            LoadAds("Активно");
                        }
                        else
                        {
                            MessageBox.Show("Не найдено объявление.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка удаления объявления: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Выберите объявление для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void BtnDone_Click(object sender, RoutedEventArgs e)
        {
            dynamic selectedAd = null;

            if (AdsListView.SelectedItem != null)
            {
                selectedAd = AdsListView.SelectedItem;
            }

            if (selectedAd != null)
            {
                int adId = selectedAd.ad_id;

                var adToFinish = db.ads.FirstOrDefault(a => a.ad_id == adId);
                if (adToFinish != null)
                {
                    var completedStatus = db.ad_statuses.FirstOrDefault(s => s.ad_status_name == "Завершено");
                    if (adToFinish.ad_status_id == completedStatus?.ad_status_id)
                    {
                        MessageBox.Show("Это объявление уже помечено как завершённое.", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                    }

                    if (MessageBox.Show("Вы уверены, что хотите завершить это объявление?", "Подтверждение действия", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    {
                        try
                        {
                            if (completedStatus != null)
                            {
                                adToFinish.ad_status_id = completedStatus.ad_status_id;
                                db.SaveChanges();

                                LoadAds("Активно");

                                MessageBox.Show("Объявление успешно закрыто.", "Готово", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Произошла ошибка при закрытии объявления: {ex.Message}", "Ошибка выполнения", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Не удалось найти выбранное объявление в базе данных.", "Ошибка поиска", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Сначала выберите объявление для завершения.", "Напоминание", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }


        private void AListAds_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            var selectedAd = AdsListView.SelectedItem as dynamic;

            if (selectedAd != null)
            {
                int adId = selectedAd.ad_id;
                ProfileFrame.Navigate(new AddAdvert(adId));
            }
        }
        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            Window.GetWindow(this)?.Close();
        }

        private void AListAds_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dynamic selectedAd = ((ListView)sender).SelectedItem;

            if (selectedAd != null)
            {
                int adId = selectedAd.ad_id;
                ProfileFrame.Navigate(new AddAdvert(adId));
            }
        }

       


    }
}
