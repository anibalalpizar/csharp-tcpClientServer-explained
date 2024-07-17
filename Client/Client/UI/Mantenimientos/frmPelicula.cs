using Client.Models;
using Client.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client.UI.Mantenimientos
{
    public partial class frmPelicula : Form
    {
        private CategoriaUtils _categoriaUtils;
        private PeliculaUtils _peliculaUtils;
        private string _nombreCompleto;

        public frmPelicula(string nombreCompleto)
        {
            InitializeComponent();
            _categoriaUtils = new CategoriaUtils();
            _peliculaUtils = new PeliculaUtils();
            _nombreCompleto = nombreCompleto;
        }

        private void frmPelicula_Load(object sender, EventArgs e)
        {
            voidLoadCmb();
            List<Pelicula> results = _peliculaUtils.ObtenerTodos();
            LoadData(results);
        }

        private void voidLoadCmb()
        {
            // Limpia los elementos actuales en el combo box
            cmbCategorias.Items.Clear();

            // Obtiene todas las categorías desde el servidor
            List<CategoriaPelicula> categorias = _categoriaUtils.ObtenerTodos();

            // Verifica si la lista de categorías no es null y tiene elementos
            if (categorias != null && categorias.Count > 0)
            {
                // Itera sobre cada categoría y la agrega al combo box
                foreach (var categoria in categorias)
                {
                    cmbCategorias.Items.Add(categoria.NombreCategoria);
                }
            }
            else
            {
                MessageBox.Show("No se encontraron categorías.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmPrincipal frmPrincipal = new frmPrincipal(_nombreCompleto);
            frmPrincipal.ShowDialog();
            this.Hide();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            int idPelicula;
            if (!int.TryParse(txtIdPelicula.Text, out idPelicula))
            {
                MessageBox.Show("El ID de la pelicula debe ser un número entero.");
                return;
            }

            int anoLanzamiento;
            if (!int.TryParse(txtAnoLazamiento.Text, out anoLanzamiento))
            {
                MessageBox.Show("El año debe ser un número entero");
                return;
            }

            if (cmbCategorias.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una categoria.");
                return;
            }

            string tituloPelicula = txtTituloPelicula.Text.Trim();
            string idioma = txtIdioma.Text.Trim();
            int idCategoria = cmbCategorias.SelectedIndex + 1;

            if (string.IsNullOrEmpty(tituloPelicula) || string.IsNullOrEmpty(idioma))
            {
                MessageBox.Show("Todos los campos son requeridos.");
                return;
            }

            try
            {
                string result = _peliculaUtils.RegistrarPelicula(idPelicula, tituloPelicula, anoLanzamiento, idioma, idCategoria);
                MessageBox.Show(result);

                List<Pelicula> results = _peliculaUtils.ObtenerTodos();
                LoadData(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al registrar la pelicula: {ex.Message}");
            }
        }

        private void LoadData(List<Pelicula> peliculas)
        {
            dgvPeliculas.DataSource = null;
            var dataTable = new System.Data.DataTable();

            dataTable.Columns.Add("ID Película", typeof(int));
            dataTable.Columns.Add("Título", typeof(string));
            dataTable.Columns.Add("Año de Lanzamiento", typeof(int));
            dataTable.Columns.Add("Idioma", typeof(string));
            dataTable.Columns.Add("Nombre Categoría", typeof(string));

            foreach (var pelicula in peliculas)
            {
                dataTable.Rows.Add(pelicula.IdPelicula, pelicula.Titulo, pelicula.AnoLanzamiento, pelicula.Idioma, pelicula.CategoriaPelicula.NombreCategoria);
            }

            dgvPeliculas.DataSource = dataTable;

        }
    }
}
