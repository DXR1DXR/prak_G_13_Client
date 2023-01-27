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

namespace prak_G_13_Client
{
    /// <summary>
    /// Логика взаимодействия для StuffPage.xaml
    /// </summary>
    public partial class StuffPage : Page
    {
        public StuffPage()
        {
            InitializeComponent();
        }

        private void LetterTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            Convert.ToInt32(LetterTB.Text);
        }

        private void ZeroTB_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
