// Importamos los modelos necesarios desde el espacio de nombres Client.Models
using Client.Models;
// Importamos utilidades desde el espacio de nombres Client.Utils
using Client.Utils;
// Importamos la biblioteca estándar de C# para el manejo de fechas y horas
using System;
// Importamos la biblioteca estándar de C# para la creación de interfaces gráficas
using System.Windows.Forms;
// Importamos la biblioteca estándar de C# para el manejo de colecciones genéricas
using System.Collections.Generic;

// Definimos un espacio de nombres específico para las interfaces de usuario de mantenimiento
namespace Client.UI.Mantenimientos
{
    // Definimos la clase 'frmCategoriaPelicula' que hereda de 'Form' (un formulario de Windows Forms)
    public partial class frmCategoriaPelicula : Form
    {
        // Declaramos una variable privada '_categoriaUtils' para manejar las utilidades de categoría
        private CategoriaUtils _categoriaUtils;
        private string _nombreCompleto;
        private string _idUsuario;
        private frmPrincipal _frmPrincipal;



        // Constructor de la clase 'frmCategoriaPelicula'
        public frmCategoriaPelicula(frmPrincipal frmPrincipal, string nombreCompleto, string idUsuario)
        {
            InitializeComponent();
            _categoriaUtils = new CategoriaUtils();
            _nombreCompleto = nombreCompleto;
            _idUsuario = idUsuario;
            _frmPrincipal = frmPrincipal; // Asigna la instancia del formulario principal
        }


        // Método que se ejecuta cuando se hace clic en el botón 'Agregar'
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Declaramos una variable para el ID de la categoría
            int idCategoria;
            // Intentamos convertir el texto del campo 'txtId' a un número entero
            if (!int.TryParse(txtId.Text, out idCategoria))
            {
                // Si la conversión falla, mostramos un mensaje de error y salimos del método
                MessageBox.Show("El ID de la categoría debe ser un número entero.");
                return;
            }

            // Obtenemos y limpiamos los valores de los campos de texto para nombre y descripción
            string nombreCategoria = txtNombre.Text.Trim();
            string descripcion = txtDescripcion.Text.Trim();

            // Verificamos si los campos de nombre y descripción están vacíos
            if (string.IsNullOrEmpty(nombreCategoria) || string.IsNullOrEmpty(descripcion))
            {
                // Si están vacíos, mostramos un mensaje de error y salimos del método
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            // nombre cateogria no puede ser mayor a 25 caracteres
            if (nombreCategoria.Length > 25)
            {
                MessageBox.Show("El nombre de la categoría no puede ser mayor a 25 caracteres.");
                return;
            }

            // descripción no puede ser mayor a 150 caracteres
            if (descripcion.Length > 150)
            {
                MessageBox.Show("La descripción de la categoría no puede ser mayor a 150 caracteres.");
                return;
            }

            try
            {
                // Intentamos registrar la categoría con los datos proporcionados
                string result = _categoriaUtils.RegistrarCategoria(idCategoria, nombreCategoria, descripcion);
                // Mostramos el resultado de la operación
                MessageBox.Show(result);
                // Obtenemos todas las categorías y las cargamos en la interfaz
                List<CategoriaPelicula> results = _categoriaUtils.ObtenerTodos();
                LoadData(results);
            }
            catch (Exception ex)
            {
                // Si ocurre un error durante el registro, mostramos un mensaje de error
                MessageBox.Show($"Error al registrar la categoría: {ex.Message}");
            }
        }

        // Método que se ejecuta cuando el formulario se carga
        private void frmCategoriaPelicula_Load(object sender, EventArgs e)
        {
            // Obtenemos todas las categorías y las cargamos en la interfaz
            List<CategoriaPelicula> results = _categoriaUtils.ObtenerTodos();
            LoadData(results);
        }

        // Método para cargar los datos de las categorías en la interfaz
        private void LoadData(List<CategoriaPelicula> categorias)
        {
            // Establecemos la fuente de datos del DataGridView a null
            dgvData.DataSource = null;

            // Creamos una nueva tabla de datos
            var dataTable = new System.Data.DataTable();

            // Añadimos las columnas necesarias a la tabla de datos
            dataTable.Columns.Add("ID Categoría", typeof(int));
            dataTable.Columns.Add("Nombre", typeof(string));
            dataTable.Columns.Add("Descripción", typeof(string));

            // Iteramos sobre la lista de categorías y añadimos cada una a la tabla de datos
            foreach (var categoria in categorias)
            {
                dataTable.Rows.Add(categoria.IdCategoria, categoria.NombreCategoria, categoria.Descripcion);
            }

            // Establecemos la fuente de datos del DataGridView a la tabla de datos creada
            dgvData.DataSource = dataTable;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Muestra la instancia original del formulario principal
            _frmPrincipal.Show();
            this.Close(); // Cierra el formulario actual
        }
    }
}
