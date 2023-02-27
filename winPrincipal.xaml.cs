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
    /// Lógica de interacción para winPrincipal.xaml
    /// </summary>
    public partial class winPrincipal : Window
    {
        public winPrincipal()
        {
            InitializeComponent();
        }



        private void btSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void btAcercaDe_Click(object sender, RoutedEventArgs e)
        {
            winAcercade acercaDe = new winAcercade(this);
            IsEnabled = false;
            acercaDe.Show();
            
        }

        private void btLibros_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow(this);
            IsEnabled= false;
            mw.Show();
        }

        private void btLibrosAutor_Click(object sender, RoutedEventArgs e)
        {
            winLibrosAutor wla = new winLibrosAutor(this);
            IsEnabled= false;
            wla.Show();
        }

        private void btLibrosEditorial_Click(object sender, RoutedEventArgs e)
        {
            winLibrosEditorial wla = new winLibrosEditorial(this);
            IsEnabled = false;
            wla.Show();
        }

        private void btLibrosGenero_Click(object sender, RoutedEventArgs e)
        {
            winLibrosGenero wla = new winLibrosGenero(this);
            IsEnabled = false;
            wla.Show();
        }

        private void btLibrosLibreria_Click(object sender, RoutedEventArgs e)
        {
            winLibrosLibreria wla = new winLibrosLibreria(this);
            IsEnabled = false;
            wla.Show();
        }

        private void btAutores_Click(object sender, RoutedEventArgs e)
        {
            winAutores wla = new winAutores(this);
            IsEnabled = false;
            wla.Show();
        }

        private void btEditoriales_Click(object sender, RoutedEventArgs e)
        {
            winEditoriales wla = new winEditoriales(this);
            IsEnabled = false;
            wla.Show();
        }

        private void btGeneros_Click(object sender, RoutedEventArgs e)
        {
            winGenero wla = new winGenero(this);
            IsEnabled = false;
            wla.Show(); 
        }

        private void btLibrerias_Click(object sender, RoutedEventArgs e)
        {
            winLibreria wla = new winLibreria(this);
            IsEnabled = false;
            wla.Show();
        }
    }
}
