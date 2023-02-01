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
using System.Windows.Shapes;
using VkNet;
using VkNet.Model;

namespace prak_G_13_Client
{
    /// <summary>
    /// Логика взаимодействия для TwoFactor.xaml
    /// </summary>
    public partial class TwoFactor : Page
    {
        public TwoFactor()
        {
            InitializeComponent();
        }

        private void EnterBT_Click(object sender, RoutedEventArgs e)
        {
            VkApi api = new VkApi();
            var login = LoginTB.Text;
            var password = PasswordPB.Password;
            ulong appId = 51539239;
            try
            {
                api.Authorize(new ApiAuthParams { ApplicationId = appId, Login = login, Password = password, Settings = VkNet.Enums.Filters.Settings.All });
                
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Неверный"))
                {
                    MessageBox.Show("Неправильный логин или пароль");
                }
                else if (ex.Message.Contains("Двух"))
                {
                    if (CodeTB.Text == "")
                    {
                        MessageBox.Show("Необходим код 2FA");
                        return;
                    }
                    api = new VkApi();
                    string code = CodeTB.Text;
                    api.Authorize(new ApiAuthParams { ApplicationId = appId, Login = login, Password = password, Settings = VkNet.Enums.Filters.Settings.All, TwoFactorAuthorization = () => { return code; } });
                    if (api.UserId == null)
                    {
                        MessageBox.Show("Код 2FA неверен");
                        return;
                    }
                    MessageBox.Show("Успешная авторизация!");
                }

            }
        }
    }
}
