using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
using Octokit;
using Application = System.Windows.Application;

namespace prak_G_13_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Update();
            InitializeComponent();
            this.Dispatcher.UnhandledException += Dispatcher_UnhandledException;
        }

        private void Dispatcher_UnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            ExceptionWindow exceptionWindow = new ExceptionWindow(e.Exception);
            exceptionWindow.Show();
            e.Handled = true;
            Close();
        }

        private void Update()
        {
            try
            {
                Version latestVersion = Get_Latest_Version().Result;
                var currentversion = Assembly.GetExecutingAssembly().GetName().Version;
                if (currentversion < latestVersion)//TODO <
                {
                    var tempPath = System.IO.Path.GetTempPath();
                    var tempProjPath = tempPath + "prak_G_13";
                    var updaterPath = tempProjPath + "\\Updater.exe";
                    Directory.CreateDirectory(tempProjPath);
                    var ress = GetType().Assembly.GetManifestResourceNames();
                    using (Stream resource = new MemoryStream(Properties.Resources.Updater))
                    {
                        if (resource == null)
                        {
                            throw new ArgumentException("No such resource", "resourceName");
                        }
                        using (Stream output = File.OpenWrite(updaterPath))
                        {
                            resource.CopyTo(output);
                        }
                    }
                    string user = AppInfo.GetInstance().user;
                    string project = AppInfo.GetInstance().project;
                    string file = AppInfo.GetInstance().file;
                    Process.Start(updaterPath, new List<string>() { Directory.GetCurrentDirectory() + "\\prak_G_13_Client.exe", user, project, file });
                    this.Close();
                }
            }
            catch
            {

            }
        }

        private async Task<Version> Get_Latest_Version()
        {
            GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("prak_G_13_Client"));
            var releases = gitHubClient.Repository.Release.GetLatest("DXR1DXR", "prak_G_13_Client");
            var latestVersion = new Version(releases.Result.Name.Remove(0, 1));
            return latestVersion;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            MainFrame.NavigationService.Navigate(new LoginPage());
        }
        private void TitleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
                Application.Current.MainWindow.DragMove();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            AdjustWindowSize();
        }
        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
        private void AdjustWindowSize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                MaxButton.Content = "Развернуть";
            }
            else
            {
                this.WindowState = WindowState.Maximized;
                MaxButton.Content = "Свернуть";
            }

        }

        private void Window_StateChanged(object sender, EventArgs e)
        {
            if (TopWindow.WindowState == WindowState.Normal)
            {
                SecondBorder.Margin = new Thickness(0);
                //RootWindow.Margin = new Thickness(5, 5, 5, 0);
                //MainFrame.Margin = new Thickness(0);
            } else if(TopWindow.WindowState == WindowState.Maximized)
            {
                SecondBorder.Margin = new Thickness(5);
            }
        }
    }
}
