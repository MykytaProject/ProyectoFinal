using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
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
    /// Lógica de interacción para winLibrosLibreria.xaml
    /// </summary>
    public partial class winLibrosLibreria : Window
    {
        public winLibrosLibreria(winPrincipal wp)
        {
            InitializeComponent();
            this.wp = wp;
        }

        private winPrincipal wp;

        public static OleDbConnection miconexion;
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string conecta = (@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\datos\libros.accdb"); //Acceso con ruta relativa 
            miconexion = new OleDbConnection(conecta);
            miconexion.Open();
            OleDbDataAdapter adaptador = new OleDbDataAdapter("SELECT * FROM librerias", miconexion);
            DataSet ds = new DataSet();
            adaptador.Fill(ds);
            dgProveedor.ItemsSource = ds.Tables[0].DefaultView;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (dgProveedor.SelectedIndex > -1) //Comprobamos que hay algún proveedor
            { // seleccionado en el dataGrid

                DataView datos = (DataView)dgProveedor.ItemsSource; // Creamos DataView datos
                                                                    // con copia de itemsource del datagrid
                string ced =//Recogemos la celda de código proveedor de la fila seleccionada
                datos.Table.Rows[dgProveedor.SelectedIndex]["codigolibreria"].ToString();

                OleDbDataAdapter adaptador = new
                OleDbDataAdapter("SELECT * FROM libros where codigolibreria=@micod",
                miconexion); /* Creamos dataAdapter con sentencia sql, que permite extraer
                    articulos de la tabla artículos con código del proveedor que indicamos mediante
                    parámetro sql */
                adaptador.SelectCommand.Parameters.AddWithValue("@micod", ced); //Asignamos
                                                                                // valor a parámetro de la sentencia SQL anterior

                DataSet ds = new DataSet(); //Creamos una nueva instancia de la clase dataset
                                            //que representa una memoria caché de datos en memoria.
                adaptador.Fill(ds); /* Devuelve número de filas agregadas o actualizadas
                    correctamente en la clase dataset. No se incluyen las filas afectadas por
                    instrucciones que no devuelven filas.*/
                dgArticulos.ItemsSource = ds.Tables[0].DefaultView; // Obtenemos vista de la
                                                                    // primera tabla del dataset y la asignamos al itemssource del datagrid

                DataView datos2 = (DataView)dgArticulos.ItemsSource;// Creamos nuevo DataView

                string ced2 = datos.Table.Rows[dgProveedor.SelectedIndex]["nombre"].ToString();/*Extraemos
                 el nombre del proveedor del DataView datos que representa a itemsource
                 del datagrid dgProveedores */

                laTotal.Content = "Total Libros de la Libreria: " + ced2 + " son " +
                dgArticulos.Items.Count;
            } // y los totales calculados
            else
            {
                MessageBox.Show("Selecciona una libreria"); //Mensaje a mostrar si no hay
                                                        // proveedor seleccionado en el datagrid
            }
        }

        private void btSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wp.IsEnabled = true;
        }

        private void dgProveedor_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dgProveedor.SelectedIndex > -1) //Comprobamos que hay algún proveedor
            { // seleccionado en el dataGrid

                DataView datos = (DataView)dgProveedor.ItemsSource; // Creamos DataView datos
                                                                    // con copia de itemsource del datagrid
                string ced =//Recogemos la celda de código proveedor de la fila seleccionada
                datos.Table.Rows[dgProveedor.SelectedIndex]["codigolibreria"].ToString();

                OleDbDataAdapter adaptador = new
                OleDbDataAdapter("SELECT * FROM libros where codigolibreria=@micod",
                miconexion); /* Creamos dataAdapter con sentencia sql, que permite extraer
                    articulos de la tabla artículos con código del proveedor que indicamos mediante
                    parámetro sql */
                adaptador.SelectCommand.Parameters.AddWithValue("@micod", ced); //Asignamos
                                                                                // valor a parámetro de la sentencia SQL anterior

                DataSet ds = new DataSet(); //Creamos una nueva instancia de la clase dataset
                                            //que representa una memoria caché de datos en memoria.
                adaptador.Fill(ds); /* Devuelve número de filas agregadas o actualizadas
                    correctamente en la clase dataset. No se incluyen las filas afectadas por
                    instrucciones que no devuelven filas.*/
                dgArticulos.ItemsSource = ds.Tables[0].DefaultView; // Obtenemos vista de la
                                                                    // primera tabla del dataset y la asignamos al itemssource del datagrid

                DataView datos2 = (DataView)dgArticulos.ItemsSource;// Creamos nuevo DataView

                string ced2 = datos.Table.Rows[dgProveedor.SelectedIndex]["nombre"].ToString();/*Extraemos
                 el nombre del proveedor del DataView datos que representa a itemsource
                 del datagrid dgProveedores */

                laTotal.Content = "Total Libros de la Libreria: " + ced2 + " son " +
                dgArticulos.Items.Count;
            } // y los totales calculados
        }
    }
}
