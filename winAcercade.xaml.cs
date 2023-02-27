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

namespace ProyectoFinal
{
    /// <summary>
    /// Lógica de interacción para winAcercade.xaml
    /// </summary>
    public partial class winAcercade : Window
    {
        private winPrincipal mw;


        public winAcercade(winPrincipal mw)
        {
            InitializeComponent();
            this.mw = mw;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            mw.IsEnabled = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
