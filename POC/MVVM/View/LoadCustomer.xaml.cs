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
using System.Windows.Shapes;

namespace POC.MVVM.View
{
    /// <summary>
    /// Interaction logic for LoadCustomer.xaml
    /// </summary>
    public partial class LoadCustomer : Window
    {
        public LoadCustomer()
        {
            InitializeComponent();
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!Regex.IsMatch(e.Text, @"[\d]+", RegexOptions.IgnoreCase))
            {
                e.Handled = true;
            }
        }
    }
}
