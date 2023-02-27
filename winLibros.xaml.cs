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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ProyectoFinal
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       
        public MainWindow(winPrincipal wp)
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
            llenaCombo("generos"); // cargamos los combobox de generos,
            llenaCombo("autores"); // autor, editorial y librerías
            llenaCombo("editoriales"); // con los datos de las tablas
            llenaCombo("librerias");
            cargardatoslisbox(); //cargamos los nombres de los libros en el listbox
            cambiaventana(true); // Habilitamos botones, listbox, etc que corresponda
        }

        private void cambiaventana(bool modo) //parámetro que recibe true o false
        { //Con esto cambiamos estilo de la ventana a edición o no en los
          // botones,el listbox los textbox, etc. correspondientes.
            lbLibros.IsEnabled = modo;
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
                if ((controlgrid is TextBox) && (controlgrid.Name == "tbTitulo" ||
                controlgrid.Name == "tbISBN" || controlgrid.Name ==
                "tbObservaciones")) //Cambiamos solo los textbox indicados el resto
                {//están siempre como sololectura el usuario no puede cambiar valor
                    ((TextBox)controlgrid).IsReadOnly = modo;
                }
            }
            tbFecha.IsEnabled = !modo; //El datePicker no tiene readonly solo enabled
        }

        private bool camposrequeridos() //Los campos obligatorios tienen un *
        { //comprobamos si los datos obligatorios han sido introducidos
            if (tbFecha.Text == "")
            {
                MessageBox.Show("Falta fecha");
                tbFecha.Focus();
                return false; // devuelve false falta fecha
            }
            foreach (Control controlgrid in gridTexbox.Children)
            {
                string[] requeridos = { "tbTitulo", "tbEditorial", "tbAutor",
                    "tbGenero", "tbLibreria" };
                if ((controlgrid is TextBox) && (((TextBox)controlgrid).Text == ""))
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (controlgrid.Name == requeridos[i]) //compruebo si es uno de
                        { //los 5 texbox requeridos
                            MessageBox.Show("Falta Dato " +
                            controlgrid.Name.Substring(2)); //uso nombre para mensaje
                            controlgrid.Focus();
                            return false; // devuelve false falta algo
                        }
                    }
                }
            }
            return true; // devuelve true porque todo está correcto
        }

        private void limpiartextbox()//vaciamos combobox y textbox correspondientes,
        { //al principio pero también en nuevo, etc
            foreach (Control controlgrid in gridTexbox.Children)
            { // Seleccionamos objetos a limpiar desde la matriz children del grid
                if (controlgrid is ComboBox)
                {
                    ((ComboBox)controlgrid).Text = "";
                }
                if (controlgrid is TextBox)
                {
                    ((TextBox)controlgrid).Clear();
                }
            }
            tbFecha.Text = ""; // También borramos contenido del DatePicker
        }

        private void cargardatoslisbox()
        { //Llenamos listbox con el título desde tabla libros
          //Esta forma de acceso a la BD. Ya ha sido explicada
          // anteriormente. Ver enunciado del proyecto 08AccesoBD
            OleDbDataAdapter adaptador = new OleDbDataAdapter("SELECT * FROM libros", MainWindow.miconexion);

            DataSet ds = new DataSet();
            adaptador.Fill(ds);
            tabla = new DataTable();
            tabla = ds.Tables[0];
            lbLibros.Items.Clear();
            for (int i = 0; i < tabla.Rows.Count; i++)
            {
                fila = tabla.Rows[i];
                lbLibros.Items.Add(fila["titulo"].ToString());
            }
        }

        public void llenaCombo(string latabla) // función compartida para llenar los
        { //combobox correspondientes con datos de las tablas autores, editoriales,
          //generos y librerías, recibida como parámetro
            String consulta = "SELECT nombre FROM " + latabla; // Código Ya explicado.
            OleDbDataAdapter adaptador = new OleDbDataAdapter(consulta,//Conexión a BD
            MainWindow.miconexion); // Definida y abierta en Ventana principal
            DataSet ds = new DataSet(); //Ver proyecto 08AccesoBD y enunciado del
            adaptador.Fill(ds); // proyecto 03Datos de la práctica 09AccesoDatos
            DataTable tabla = new DataTable(); // de WindowsForms
            tabla = ds.Tables[0];
            for (int i = 0; i < tabla.Rows.Count; i++) // Extraemos todas las filas
            {
                switch (latabla) // de la tabla recibida como parámetro
                {
                    case "librerias": //Añadimos dato nombre al comboBox correspondiente
                        cbLibreria.Items.Add(tabla.Rows[i]["nombre"]);
                        break;
                    case "generos":
                        cbGenero.Items.Add(tabla.Rows[i]["nombre"]);
                        break;
                    case "autores":
                        cbAutor.Items.Add(tabla.Rows[i]["nombre"]);
                        break;
                    case "editoriales":
                        cbEditorial.Items.Add(tabla.Rows[i]["nombre"]);
                        break;
                }
            }
        }

        string ponernombre(string latabla, string clave, string parametro)
        // parámetros: nombre tabla, clave tabla, valor de la clave para SQl
        { //Desde el dato código/clave puesto en el textbox correspondiente y
          // recibido como parámetro devuelvo el nombre que me da la sentencia SQL
            string sentenciasql = "SELECT nombre FROM " + latabla + " where " +
            clave + " = @micod";
            OleDbCommand comando = new OleDbCommand(sentenciasql,
            MainWindow.miconexion);
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

        public string poncodigo(string latabla, string elcodigo, string elitem)
        { // La función es compartida para poner código de las 4 tablas
          //Con el dato recibido del ítem seleccionado del combobox
          // correspondiente devuelvo código
            string sentenciasql = "SELECT " + elcodigo + " FROM " + latabla + " WHERE nombre = @micod";
            OleDbCommand comando5 = new OleDbCommand(sentenciasql, MainWindow.miconexion);
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
        private void lbLibros_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (lbLibros.SelectedIndex > -1) // Si hay selección
            {
                fila = tabla.Rows[lbLibros.SelectedIndex];//fila objeto global
                                                          // clase DataRow. tabla objeto global clase DataTable
                tbCodigo.Text = fila["codigolibro"].ToString();// Cargamos los datos
                tbTitulo.Text = fila["titulo"].ToString();// en los textbox
                tbFecha.Text = fila["fechaimpresion"].ToString();// y el datepicker
                tbObservaciones.Text = fila["observacion"].ToString();
                tbISBN.Text = fila["isbn"].ToString();
                tbAutor.Text = fila["codigoautor"].ToString();
                tbEditorial.Text = fila["codigoeditorial"].ToString();
                tbGenero.Text = fila["codigogenero"].ToString();
                tbLibreria.Text = fila["codigoLibreria"].ToString();
                // Con los valores de los textbox que han recibido el código de estas 4
                // datos. Invocamos a la función que pone en el combobox asociado el nombre
                //Ver la función para ver detalle de los parámetros, funcionamiento, etc.
                cbEditorial.Text = ponernombre("Editoriales", "codigoeditorial",
                tbEditorial.Text);
                cbAutor.Text = ponernombre("Autores", "codigoautor", tbAutor.Text);
                cbGenero.Text = ponernombre("generos", "codigogenero", tbGenero.Text);
                cbLibreria.Text = ponernombre("librerias", "codigolibreria",
                tbLibreria.Text);
            }
        }

        private void cbGenero_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbGenero.SelectedIndex > -1) // Si hay selección en el ComboBox
            {
                // invocamos a la función con los parámetros nombre Tabla, nombre campo,
                // ítem seleccionado del comboBox
                tbGenero.Text = poncodigo("generos", "codigogenero",
                cbGenero.Items[cbGenero.SelectedIndex].ToString());
            }
        }

        private void cbAutor_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbAutor.SelectedIndex > -1) // Si hay selección en el ComboBox
            {
                // invocamos a la función con los parámetros nombre Tabla, nombre campo,
                // ítem seleccionado del comboBox
                tbAutor.Text = poncodigo("autores", "codigoautor",
                cbAutor.Items[cbAutor.SelectedIndex].ToString());
            }
        }

        private void cbEditorial_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbEditorial.SelectedIndex > -1) // Si hay selección en el ComboBox
            {
                // invocamos a la función con los parámetros nombre Tabla, nombre campo,
                // ítem seleccionado del comboBox
                tbEditorial.Text = poncodigo("editoriales", "codigoeditorial",
                cbEditorial.Items[cbEditorial.SelectedIndex].ToString());
            }
        }

        private void cbLibreria_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cbLibreria.SelectedIndex > -1) // Si hay selección en el ComboBox
            {
                // invocamos a la función con los parámetros nombre Tabla, nombre campo,
                // ítem seleccionado del comboBox
                tbLibreria.Text = poncodigo("librerias", "codigolibreria",
                cbLibreria.Items[cbLibreria.SelectedIndex].ToString());
            }
        }
        private void btNuevo_Click(object sender, RoutedEventArgs e)
        {
            limpiartextbox();
            lbLibros.SelectedItem = null; // Quitamos selección del listBox
            cambiaventana(false); // Cambiamos de estado a controles
            btActualizar.IsEnabled = false;
            string numFilas = "SELECT TOP 1 codigolibro FROM libros ORDER BY codigolibro DESC "; // Para ver el último código
            OleDbCommand comando1 = new OleDbCommand(numFilas, miconexion);
            int numfil = (int)comando1.ExecuteScalar();// Ver explicación anterior
            tbCodigo.Text = Convert.ToString(numfil + 1); // Nuevo código último + 1
            tbTitulo.Focus();//Enviamos foco para empezar tecleo de nuevos datos


        }

        private void btModificar_Click(object sender, RoutedEventArgs e)
        {
            if (lbLibros.Items.Count > 0) //Si no hay ítems en listbox no hay libros
            {
                if (lbLibros.SelectedItem != null) // No hay ítem seleccionado
                {
                    cambiaventana(false);//cambia a modo edición con botones indicados.
                    btGuardar.IsEnabled = false;
                    tbTitulo.Focus();
                }
                else
                {
                    MessageBox.Show("Tienes que seleccionar un libro de la lista para poder modificarlo.", "Información", MessageBoxButton.OK,

                    MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Actualmente no hay ningún libro en la base de datos.", "Información", MessageBoxButton.OK,

                MessageBoxImage.Information);
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
            string modificar = "UPDATE libros " + "SET titulo = '" + tbTitulo.Text +
            "', observacion = '" + tbObservaciones.Text + "', isbn = '" +
            tbISBN.Text + "', codigoeditorial = '" + tbEditorial.Text
            + "', fechaimpresion = '" + tbFecha.Text + "', codigoautor = '" +
            tbAutor.Text + "', codigogenero = '" + tbGenero.Text + "', codigolibreria = '" + tbLibreria.Text + "' WHERE codigolibro = @mid; ";
            OleDbCommand comando2 = new OleDbCommand(modificar, // instrucción SQL
            MainWindow.miconexion); // conexión definida en winprincipal
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
            lbLibros.Focus(); // Esperamos un nuevo click
            cambiaventana(true); //dejamos controles sin edición
            limpiartextbox();

        }

        private void btEliminar_Click(object sender, RoutedEventArgs e)
        {
            if (lbLibros.Items.Count > 0) // mismas comprobaciones que en modificar
            {
                if (lbLibros.SelectedItem != null)
                {
                    if (MessageBox.Show("¿Realmente desea eliminar el libro " +
                    tbTitulo.Text + " de la base de datos?",
                    "Confirmar Eliminación de Registro", MessageBoxButton.YesNo,
                    MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        string borra = "DELETE FROM libros WHERE codigolibro = @idborra";
                        // Sentencia SQl de borrado con el libro seleccionado
                        OleDbCommand comandoborra = new OleDbCommand(borra, // instrucción
                        MainWindow.miconexion); // SQL conexión definida en winPrincipal
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
                        lbLibros.Items.RemoveAt(lbLibros.SelectedIndex); // Eliminamos
                        lbLibros.SelectedIndex = lbLibros.Items.Count - 1; //el libro del
                        limpiartextbox(); // listbox y borramos contenido de los textbox
                        tbCodigo.Clear();
                    }
                }
                else
                {
                    MessageBox.Show("Selecciona un libro a borrar.");
                    lbLibros.Focus();
                }
            }

        }

        private void btGuardar_Click(object sender, RoutedEventArgs e)
        {
            if (!camposrequeridos())//Comprobamos si están todos los requeridos
            { // Ver detalle en función
                return;
            }
            // Sentencia SQL de inserción del nuevo libro tecleado en los textbox
            String insertar = "INSERT INTO libros(codigolibro, titulo, observacion, isbn," +
                " codigoeditorial, fechaimpresion,codigoautor, codigogenero, codigolibreria)" +
                "VALUES('" + tbCodigo.Text + "', '" + tbTitulo.Text + "', '" + tbObservaciones.Text + "', '" +
                tbISBN.Text + "', '" + tbEditorial.Text + "', '" + tbFecha.Text + "', '" + tbAutor.Text + "', '"
 + tbGenero.Text + "', '" + tbLibreria.Text + "')";
            OleDbCommand comando = new OleDbCommand(insertar, MainWindow.miconexion);
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
            lbLibros.SelectedItem = lbLibros.Items.Count - 1; // lo seleccionamos
            lbLibros.Focus();
            cambiaventana(true); //cambiamos controles para que no estén en edición
            limpiartextbox();
        }

        private void btCancelar_Click(object sender, RoutedEventArgs e)
        {

            lbLibros.SelectedItem = null; //Quitamos selección
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

        private void lbLibros_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

            if (lbLibros.SelectedIndex > -1) // Si hay selección
            {
                fila = tabla.Rows[lbLibros.SelectedIndex];//fila objeto global
                                                          // clase DataRow. tabla objeto global clase DataTable
                tbCodigo.Text = fila["codigolibro"].ToString();// Cargamos los datos
                tbTitulo.Text = fila["titulo"].ToString();// en los textbox
                tbFecha.Text = fila["fechaimpresion"].ToString();// y el datepicker
                tbObservaciones.Text = fila["observacion"].ToString();
                tbISBN.Text = fila["isbn"].ToString();
                tbAutor.Text = fila["codigoautor"].ToString();
                tbEditorial.Text = fila["codigoeditorial"].ToString();
                tbGenero.Text = fila["codigogenero"].ToString();
                tbLibreria.Text = fila["codigoLibreria"].ToString();
                // Con los valores de los textbox que han recibido el código de estas 4
                // datos. Invocamos a la función que pone en el combobox asociado el nombre
                //Ver la función para ver detalle de los parámetros, funcionamiento, etc.
                cbEditorial.Text = ponernombre("Editoriales", "codigoeditorial",
                tbEditorial.Text);
                cbAutor.Text = ponernombre("Autores", "codigoautor", tbAutor.Text);
                cbGenero.Text = ponernombre("generos", "codigogenero", tbGenero.Text);
                cbLibreria.Text = ponernombre("librerias", "codigolibreria",
                tbLibreria.Text);
            }

        }

        private void btSalir_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
