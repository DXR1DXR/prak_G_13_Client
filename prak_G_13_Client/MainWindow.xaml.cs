using System;
using System.Collections.Generic;
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

namespace prak_G_13_Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
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
                Process.Start(updaterPath, new List<string>() { Directory.GetCurrentDirectory() + "\\prak_G_13_Client.exe" });
                this.Close();

            }
            InitializeComponent();
        }
        private async Task<Version> Get_Latest_Version()
        {
            GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("prak_G_13_Client"));
            var releases = gitHubClient.Repository.Release.GetLatest("DXR1DXR", "prak_G_13_Client");
            var latestVersion = new Version(releases.Result.Name.Remove(0, 1));
            return latestVersion;
        }
        
    }
}
