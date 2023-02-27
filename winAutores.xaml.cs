using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
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
    /// Lógica de interacción para winAutores.xaml
    /// </summary>
    public partial class winAutores : Window
    {
        public winAutores(winPrincipal wp)
        {
            InitializeComponent();
            this.wp = wp;
        }
        private winPrincipal wp;

        private static OleDbConnection miconexion;
        DataTable tabla; // Guardamos resultado de la ejecución de una sentencia SQL
        DataRow fila; // Guardamos fila de una tabla
        private void Window_Loaded_1(object sender, RoutedEventArgs e)
        {
            string conecta = (@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=.\datos\libros.accdb"); //Acceso con ruta relativa
            miconexion = new OleDbConnection(conecta);
            miconexion.Open();
            limpiartextbox();// Al iniciar limpiamos los textbox
            cargardatoslisbox(); //cargamos los nombres de los libros en el listbox
            cambiaventana(true); // Habilitamos botones, listbox, etc que corresponda
        }
        private void cambiaventana(bool modo) //parámetro que recibe true o false
        { //Con esto cambiamos estilo de la ventana a edición o no en los
          // botones,el listbox los textbox, etc. correspondientes.
            lbAutores.IsEnabled = modo;
            uGridNuevoModificarEliminar.IsEnabled = modo;// Si cambiamos modo al
                                                         // UniformGrid cambiamos también botones contenidos
            btGuardar.IsEnabled = !modo;
            btActualizar.IsEnabled = !modo;
            btCancelar.IsEnabled = !modo;
            texboxsololectura(modo);//otra función para los textbox y combobox
        }
        private void texboxsololectura(bool modo)
        { //cambiamos modo de los textbox, combobox, etc según estén edición o no
            foreach (Control controlgrid in gridTexbox.Children) // Recorremos la
            { //matriz children del grid que tiene los textbox
                if (controlgrid is ComboBox) // cambiamos la propiedad a los combobox
                { // según se indique en el parámetro recibido
                    ((ComboBox)controlgrid).IsReadOnly = modo;
                    ((ComboBox)controlgrid).IsEnabled = !modo;
                }
                if ((controlgrid is TextBox) && (controlgrid.Name != "tbCodigo" )) //Cambiamos solo los textbox indicados el resto
                {//están siempre como sololectura el usuario no puede cambiar valor
                    ((TextBox)controlgrid).IsReadOnly = modo;
                }
            }
        }
        private bool camposrequeridos() //Los campos obligatorios tienen un *
        { //comprobamos si los datos obligatorios han sido introducidos
            if (tbNombre.Text == "")
            {
                MessageBox.Show("Falta Nombre");
                tbNombre.Focus();
                return false; // devuelve false falta fecha
            }
            return true; // devuelve true porque todo está correcto
        }
        private void limpiartextbox()//vaciamos combobox y textbox correspondientes,
        { //al principio pero también en nuevo, etc
            foreach (Control controlgrid in gridTexbox.Children)
            { // Seleccionamos objetos a limpiar desde la matriz children del grid
       
                if (controlgrid is TextBox)
                {
                    ((TextBox)controlgrid).Clear();
                }
            }
        }
        private void cargardatoslisbox()
        { //Llenamos listbox con el título desde tabla libros
          //Esta forma de acceso a la BD. Ya ha sido explicada
          // anteriormente. Ver enunciado del proyecto 08AccesoBD
            OleDbDataAdapter adaptador = new OleDbDataAdapter("SELECT * FROM autores", miconexion);

            DataSet ds = new DataSet();
            adaptador.Fill(ds);
            tabla = new DataTable();
            tabla = ds.Tables[0];
            lbAutores.Items.Clear();
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                fila = tabla.Rows[i];
                lbAutores.Items.Add(fila["nombre"].ToString());
            }
        }
        private void btSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {

            lbAutores.SelectedItem = null; //Quitamos selección
            limpiartextbox();
            tbCodigo.Clear();
            cambiaventana(true); // Dejamos controles sin edición
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            wp.IsEnabled = true;
            if (btGuardar.IsEnabled || btActualizar.IsEnabled)
            {
                MessageBox.Show("Anulamos cierre, Guarda ó Cancela");
                e.Cancel = true;
            }
        }
        private void btGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!camposrequeridos())//Comprobamos si están todos los requeridos
            { // Ver detalle en función
                return;
            }
            // Sentencia SQL de inserción del nuevo libro tecleado en los textbox
            String insertar = "INSERT INTO autores(codigoautor, nombre, ciudad, nacionalidad," +
                " comentario)" +
                "VALUES('" + tbCodigo.Text + "', '" + tbNombre.Text + "', '" + tbCiudad.Text + "', '" +
                tbNacionalidad.Text + "', '" + tbComentario.Text + "')";
            OleDbCommand comando = new OleDbCommand(insertar, miconexion);
            // Representa una instrucción SQL con conexión definida en winPrincipal
            try
            {
                comando.ExecuteNonQuery(); //Método para ejecutar sentencia INSERT
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Datos guardados correctamente.", "Información",
            MessageBoxButton.OK, MessageBoxImage.Information);
            cargardatoslisbox();//mostros nuevo libro
            lbAutores.SelectedItem = lbAutores.Items.Count - 1; // lo seleccionamos
            lbAutores.Focus();
            cambiaventana(true); //cambiamos controles para que no estén en edición
            limpiartextbox();
        }

        private void btEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (lbAutores.Items.Count > 0) // mismas comprobaciones que en modificar
            {
                if (lbAutores.SelectedItem != null)
                {
                    if (MessageBox.Show("¿Realmente desea eliminar el autor " +
                    tbNombre.Text + " de la base de datos?",
                    "Confirmar Eliminación de Registro", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        string borra = "DELETE FROM autores WHERE codigoautor = @idborra";
                        // Sentencia SQl de borrado con el libro seleccionado
                        OleDbCommand comandoborra = new OleDbCommand(borra, // instrucción
                        miconexion); // SQL conexión definida en winPrincipal
                        comandoborra.Parameters.AddWithValue("@idborra", tbCodigo.Text);
                        // Parámetro con código a borrar
                        try
                        {
                            comandoborra.ExecuteNonQuery(); //Método usado para ejecutar
                        } // sentencia de borrado
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                        MessageBox.Show("Datos borrados correctamente.");
                        lbAutores.Items.RemoveAt(lbAutores.SelectedIndex); // Eliminamos
                        lbAutores.SelectedIndex = lbAutores.Items.Count - 1; //el libro del
                        limpiartextbox(); // listbox y borramos contenido de los textbox
                        tbCodigo.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona un autor a borrar.");
                    lbAutores.Focus();
                }
            }
        }

        private void btActualizar_Click(object sender, RoutedEventArgs e)
        {
            if (!camposrequeridos()) //comprobamos campos obligatorios si tienen datos
            {
                return;
            }
            Int32 mid = Convert.ToInt32(tbCodigo.Text);
            //Sentencia SQL de actualización. Modifica datos con los nuevos tecleados.
            string modificar = "UPDATE autores " + "SET nombre = '" + tbNombre.Text +
            "', comentario = '" + tbComentario.Text + "', ciudad = '" +
            tbCiudad.Text + "', nacionalidad = '" + tbNacionalidad.Text
            + "' WHERE codigolibro = @mid; ";
            OleDbCommand comando2 = new OleDbCommand(modificar, // instrucción SQL
            miconexion); // conexión definida en winprincipal
            comando2.Parameters.AddWithValue("@mid", mid); //Parámetro sentencia SQL
            try
            {
                comando2.ExecuteNonQuery(); //Método para ejecutar sentencia UPDATE
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            MessageBox.Show("Datos modificados correctamente.");
            cargardatoslisbox(); //Mostramos nuevo libro en listbox
            lbAutores.Focus(); // Esperamos un nuevo click
            cambiaventana(true); //dejamos controles sin edición
            limpiartextbox();

        }

        private void btModificar_Click(object sender, RoutedEventArgs e)
        {
            if (lbAutores.Items.Count > 0) //Si no hay ítems en listbox no hay libros
            {
                if (lbAutores.SelectedItem != null) // No hay ítem seleccionado
                {
                    cambiaventana(false);//cambia a modo edición con botones indicados.
                    btGuardar.IsEnabled = false;
                    tbNombre.Focus();
                }
                else
                {
                    MessageBox.Show("Tienes que seleccionar un autor de la lista para poder modificarlo.", "Información", MessageBoxButton.OK,

                    MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Actualmente no hay ningún autor en la base de datos.", "Información", MessageBoxButton.OK,

                MessageBoxImage.Information);
            }
        }

        private void btNuevo_Click(object sender, RoutedEventArgs e)
        {
            limpiartextbox();
            lbAutores.SelectedItem = null; // Quitamos selección del listBox
            cambiaventana(false); // Cambiamos de estado a controles
            btActualizar.IsEnabled = false;
            string numFilas = "SELECT TOP 1 codigoautor FROM autores ORDER BY codigoautor DESC "; // Para ver el último código
            OleDbCommand comando1 = new OleDbCommand(numFilas, miconexion);
            object result = comando1.ExecuteScalar();// Ver explicación anterior
            int numfil = result == null ? 0 : Convert.ToInt32(result);
            tbCodigo.Text = Convert.ToString(numfil + 1); // Nuevo código último + 1
            tbNombre.Focus();//Enviamos foco para empezar tecleo de nuevos datos
        }

        private void lbLibros_SelectionChanged(object sender,
 SelectionChangedEventArgs e)
        {
            if (lbAutores.SelectedIndex > -1) // Si hay selección
            {
                fila = tabla.Rows[lbAutores.SelectedIndex];//fila objeto global
                                                          // clase DataRow. tabla objeto global clase DataTable
                tbCodigo.Text = fila["codigoautor"].ToString();// Cargamos los datos
                tbNombre.Text = fila["nombre"].ToString();// en los textbox
                tbCiudad.Text = fila["ciudad"].ToString();// y el datepicker
                tbNacionalidad.Text = fila["nacionalidad"].ToString();
                tbComentario.Text = fila["comentario"].ToString();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            limpiartextbox();// Al iniciar limpiamos los textbox
            cargardatoslisbox(); //cargamos los nombres de los libros en el listbox
            cambiaventana(true); // Habilitamos botones, listbox, etc que corresponda
        }

        public string poncodigo(string latabla, string elcodigo, string elitem)
        { // La función es compartida para poner código de las 4 tablas
          //Con el dato recibido del ítem seleccionado del combobox
          // correspondiente devuelvo código
            string sentenciasql = "SELECT " + elcodigo + " FROM " + latabla + " WHERE nombre = @micod";
            OleDbCommand comando5 = new OleDbCommand(sentenciasql, miconexion);
            string mid = elitem;// extraído de :
                                //cbLibreria.Items[cbLibreria.SelectedIndex].ToString();
            comando5.Parameters.AddWithValue("@midcod", mid);
            OleDbDataReader lector = comando5.ExecuteReader(); // Ejecuta consulta
            if (lector.Read()) // SELECT y en un objeto DataReader.
            {
                return lector[elcodigo].ToString();// Devuelve código
            }
            else
            {
                return ""; // devuelve nada si la consulta está vacía
            }
        }

        string ponernombre(string latabla, string clave, string parametro)
        // parámetros: nombre tabla, clave tabla, valor de la clave para SQl
        { //Desde el dato código/clave puesto en el textbox correspondiente y
          // recibido como parámetro devuelvo el nombre que me da la sentencia SQL
            string sentenciasql = "SELECT nombre FROM " + latabla + " where " +
            clave + " = @micod";
            OleDbCommand comando = new OleDbCommand(sentenciasql,
            miconexion);
            comando.Parameters.AddWithValue("@micod", parametro);
            OleDbDataReader lector = comando.ExecuteReader(); // Ejecuta consulta
            if (lector.Read()) //SELECT y devuelve un objeto DataReader.
            {
                return lector["nombre"].ToString();
            }
            else
            {
                return "";
            }
        }

        

        

       

       

        


    }
}
