using Client.Models; // Importa los modelos de datos del cliente
using Client.Utils; // Importa utilidades del cliente, como funciones para manejar datos de categorías y películas
using System; // Importa funcionalidades básicas del sistema
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas
using System.Windows.Forms; // Importa funcionalidades para crear aplicaciones de Windows Forms

namespace Client.UI.Mantenimientos // Define el espacio de nombres 'Client.UI.Mantenimientos'
{
    // Define la clase 'frmPelicula' que hereda de 'Form'
    public partial class frmPelicula : Form
    {
        private CategoriaUtils _categoriaUtils; // Utilidad para manejar datos de categorías
        private PeliculaUtils _peliculaUtils; // Utilidad para manejar datos de películas
        private string _nombreCompleto; // Almacena el nombre completo del usuario

        // Constructor que recibe el nombre completo del usuario y inicializa componentes y utilidades
        public frmPelicula(string nombreCompleto)
        {
            InitializeComponent();
            _categoriaUtils = new CategoriaUtils();
            _peliculaUtils = new PeliculaUtils();
            _nombreCompleto = nombreCompleto;
        }

        // Evento que se ejecuta cuando el formulario se carga
        private void frmPelicula_Load(object sender, EventArgs e)
        {
            voidLoadCmb(); // Carga las categorías en el combo box
            List<Pelicula> results = _peliculaUtils.ObtenerTodos(); // Obtiene todas las películas
            LoadData(results); // Carga los datos en el data grid view
        }

        // Método para cargar las categorías en el combo box
        private void voidLoadCmb()
        {
            cmbCategorias.Items.Clear(); // Limpia los elementos actuales en el combo box

            List<CategoriaPelicula> categorias = _categoriaUtils.ObtenerTodos(); // Obtiene todas las categorías desde el servidor

            if (categorias != null && categorias.Count > 0) // Verifica si la lista de categorías no es null y tiene elementos
            {
                foreach (var categoria in categorias) // Itera sobre cada categoría y la agrega al combo box
                {
                    cmbCategorias.Items.Add(categoria.NombreCategoria);
                }
            }
            else
            {
                MessageBox.Show("No se encontraron categorías.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Muestra un mensaje de error si no se encontraron categorías
            }
        }

        // Evento que se ejecuta cuando se hace clic en el botón de la barra de herramientas para ir al formulario principal
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmPrincipal frmPrincipal = new frmPrincipal(_nombreCompleto); // Crea una nueva instancia del formulario principal
            frmPrincipal.ShowDialog(); // Muestra el formulario principal como un cuadro de diálogo
            this.Hide(); // Oculta el formulario actual
        }

        // Evento que se ejecuta cuando se hace clic en el botón 'Agregar'
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            int idPelicula;
            if (!int.TryParse(txtIdPelicula.Text, out idPelicula)) // Verifica si el ID de la película es un número entero
            {
                MessageBox.Show("El ID de la pelicula debe ser un número entero.");
                return;
            }

            int anoLanzamiento;
            if (!int.TryParse(txtAnoLazamiento.Text, out anoLanzamiento)) // Verifica si el año de lanzamiento es un número entero
            {
                MessageBox.Show("El año debe ser un número entero");
                return;
            }

            if (cmbCategorias.SelectedIndex == -1) // Verifica si se seleccionó una categoría
            {
                MessageBox.Show("Debe seleccionar una categoria.");
                return;
            }

            string tituloPelicula = txtTituloPelicula.Text.Trim(); // Obtiene y recorta el título de la película
            string idioma = txtIdioma.Text.Trim(); // Obtiene y recorta el idioma de la película
            int idCategoria = cmbCategorias.SelectedIndex + 1; // Obtiene el ID de la categoría seleccionada

            if (string.IsNullOrEmpty(tituloPelicula) || string.IsNullOrEmpty(idioma)) // Verifica si los campos no están vacíos
            {
                MessageBox.Show("Todos los campos son requeridos.");
                return;
            }

            try
            {
                string result = _peliculaUtils.RegistrarPelicula(idPelicula, tituloPelicula, anoLanzamiento, idioma, idCategoria); // Intenta registrar la película
                MessageBox.Show(result); // Muestra el resultado de la operación

                List<Pelicula> results = _peliculaUtils.ObtenerTodos(); // Obtiene todas las películas actualizadas
                LoadData(results); // Carga los datos actualizados en el data grid view
            }
            catch (Exception ex) // Captura cualquier excepción
            {
                MessageBox.Show($"Error al registrar la pelicula: {ex.Message}"); // Muestra un mensaje de error
            }
        }

        // Método para cargar los datos en el data grid view
        private void LoadData(List<Pelicula> peliculas)
        {
            dgvPeliculas.DataSource = null; // Limpia la fuente de datos actual
            var dataTable = new System.Data.DataTable(); // Crea una nueva tabla de datos

            dataTable.Columns.Add("ID Película", typeof(int)); // Agrega columnas a la tabla de datos
            dataTable.Columns.Add("Título", typeof(string));
            dataTable.Columns.Add("Año de Lanzamiento", typeof(int));
            dataTable.Columns.Add("Idioma", typeof(string));
            dataTable.Columns.Add("Nombre Categoría", typeof(string));

            foreach (var pelicula in peliculas) // Itera sobre cada película y la agrega a la tabla de datos
            {
                dataTable.Rows.Add(pelicula.IdPelicula, pelicula.Titulo, pelicula.AnoLanzamiento, pelicula.Idioma, pelicula.CategoriaPelicula.NombreCategoria);
            }

            dgvPeliculas.DataSource = dataTable; // Establece la fuente de datos del data grid view
        }
    }
}