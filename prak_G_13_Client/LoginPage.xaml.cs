using Microsoft.Win32;
using prak_G_13_Client.Properties;
using System;
using System.Diagnostics;
using System.IO;
using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using VkNet;
using VkNet.Enums.Filters;
using VkNet.Model;

namespace prak_G_13_Client
{
    /// <summary>
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage(int vkID)
        {
            InitializeComponent();
        }
        public LoginPage()
        {
            InitializeComponent();
        }

        private void EnterBT_Click(object sender, RoutedEventArgs e)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(AppInfo.GetInstance().baseAdress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = client.GetAsync($"Users/{LoginTB.Text}/{PasswordPB.Password}/{1}").Result;
            string userId = responseMessage.Content.ReadAsStringAsync().Result;
            if (userId != "null")
            {
                AppInfo.GetInstance().userId = Convert.ToInt32(userId);
                NavigationService.Navigate(new StuffPage());
            }
        }

        private void EnterVKBT_Click(object sender, RoutedEventArgs e)
        {
            VkApi api = new VkApi();
            var login = LoginTB.Text;
            var password = PasswordPB.Password;
            ulong appId = 51539239;
            try
            {
                api.Authorize(new ApiAuthParams { Login = login, Password = password, Settings = VkNet.Enums.Filters.Settings.All });
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Неправильный"))
                {
                    //...
                } else if(ex.Message.Contains("Two")){
                    api = new VkApi();

                    api.Authorize(new ApiAuthParams { Login = login, Password = password, Settings = VkNet.Enums.Filters.Settings.All });
                }

            }

        }
    }
}
