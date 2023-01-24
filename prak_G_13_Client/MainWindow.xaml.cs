using System;
using System.Collections.Generic;
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
            Get_Latest_Version();
            InitializeComponent(); //
            
        }
        private async Version Get_Latest_Version()
        {
            GitHubClient gitHubClient = new GitHubClient(new ProductHeaderValue("prak_G_13_Client"));
            var releases = await gitHubClient.Repository.Release.GetAll("DXR1DXR", "prak_G_13_Client");
            var currentversion = Assembly.GetExecutingAssembly().GetName().Version;
            return new Version(currentversion);
        }
        
    }
}
