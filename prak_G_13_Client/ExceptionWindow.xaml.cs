using prak_G_13_Client.Properties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Diagnostics;
using System.IO;

namespace prak_G_13_Client
{
    /// <summary>
    /// Interaction logic for ExceptionWindow.xaml
    /// </summary>
    public partial class ExceptionWindow : Window
    {
        Exception _ex;
        public ExceptionWindow(Exception ex)
        {
            _ex = ex;
            InitializeComponent();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            sp1.Visibility = Visibility.Visible;
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            sp1.Visibility = Visibility.Collapsed;
        }
        private Random random = new Random();
        public string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private bool Is_String_Random(string s)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(AppInfo.GetInstance().baseAdress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage responseMessage = client.GetAsync($"Orders/\"{s}\"").Result;
            string val = responseMessage.Content.ReadAsStringAsync().Result;
            if (val == "true")
            {
                return true;
            }
            return false;
        }

        private void ReadyBT_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)cb1.IsChecked)
            {
                string ident = RandomString(30);
                while (!Is_String_Random(ident))
                {
                    ident = RandomString(30);
                }
                Send_Report_To_DB(ident);
                Send_Report_To_Mail(ident);
            }
            this.Close();
        }
        private void Send_Report_To_DB(string ident)
        {
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(AppInfo.GetInstance().baseAdress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            
            List<string> strings = new List<string>();
            strings.Add(ident);
            strings.Add(_ex.Message);
            strings.Add(StepsTB.Text);
            strings.Add(AppInfo.GetInstance().userId.ToString());
            HttpResponseMessage responseMessage = client.PostAsync($"Orders/", new StringContent(JsonSerializer.Serialize(strings), Encoding.UTF8, "application/json")).Result;
        }


        private void Send_Report_To_Mail(string ident)
        {
            var processes = Process.GetProcesses();
            var file = File.CreateText($"{Environment.CurrentDirectory}Process.txt");
            foreach (Process p in processes)
            {
                //Process.Start($"Process.txt");
                file.WriteLine(p.ProcessName);

            }
            file.Close();
            try
            {
                //отправитель
                string from = @"sender.prak.golovin@yandex.ru";
                string pass = "iwebccfamdwfyxql";
                MailMessage mess = new MailMessage();
                mess.To.Add("receiver.prak.golovin@yandex.ru"); // кому отправляем
                mess.From = new MailAddress(from);
                // тема письма
                mess.Subject = ident;
                // текст письма
                mess.Body = SystemInfo.FullInfo;
                mess.Attachments.Add(new Attachment($"Process.txt"));
                // адрес smtp-сервера и порт, с которого будем отправлять письмо
                SmtpClient smtp = new SmtpClient("smtp.yandex.ru", 587);
                // логин и пароль
                smtp.EnableSsl = true;
                smtp.Credentials = new NetworkCredential(from.Split('@')[0], pass);
                smtp.Send(mess);

            }
            catch (Exception e)
            {
                throw new Exception("Mail.Send: " + e.Message);
            }
        }
    }
}
