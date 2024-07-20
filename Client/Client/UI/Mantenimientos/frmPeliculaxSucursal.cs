using Client.Models;
using Client.Utils;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client.UI.Mantenimientos
{
    public partial class frmPeliculaxSucursal : Form
    {
        private string _nombreCompleto;
        private SucursalUtils _sucursalUtils;
        private PeliculaUtils _peliculaUtils;
        private PelicuaXSucursalUtils _pelicuaXSucursalUtils;

        public frmPeliculaxSucursal(string nombreCompleto)
        {
            InitializeComponent();
            InitializeDataGridView();
            _nombreCompleto = nombreCompleto;
            _sucursalUtils = new SucursalUtils();
            _peliculaUtils = new PeliculaUtils();
            _pelicuaXSucursalUtils = new PelicuaXSucursalUtils();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmPrincipal frmPrincipal = new frmPrincipal(_nombreCompleto); // Crea una nueva instancia del formulario principal
            frmPrincipal.ShowDialog(); // Muestra el formulario principal como un cuadro de diálogo
            this.Hide(); // Oculta el formulario actual
        }

        private void frmPeliculaxSucursal_Load(object sender, EventArgs e)
        {
            loadCmboSucursales();
            loadDgvPeliculasDisponibles();
            List<object> datos = _pelicuaXSucursalUtils.ObtenerTodos();
            LoadDatos(datos);
        }

        private void loadCmboSucursales()
        {
            cmbSucursales.Items.Clear(); // Limpia los elementos actuales en el combo box
            List<dynamic> sucursales = _sucursalUtils.ObtenerTodos(); // Obtiene todas las sucursales desde el servidor, dynamic es un tipo de dato que puede almacenar cualquier tipo de dato

            if (sucursales != null && sucursales.Count > 0) // Verifica si la lista de sucursales no es null y tiene elementos
            {
                foreach (var sucursal in sucursales) // Itera sobre cada sucursal y la agrega al combo box
                {
                    var displayText = $"{sucursal.IdSucursal} - {sucursal.SucursalNombre}";
                    cmbSucursales.Items.Add(displayText); // Agrega el ID y el nombre de la sucursal al combo box
                }
            }
            else
            {
                MessageBox.Show("No se encontraron sucursales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void InitializeDataGridView()
        {
            dgvPeliculasDisponibles.Columns.Clear(); // Limpia las columnas actuales del DataGridView

            // Agrega las columnas al DataGridView
            dgvPeliculasDisponibles.Columns.Add("IdPelicula", "ID");
            dgvPeliculasDisponibles.Columns.Add("Titulo", "Título");
            dgvPeliculasDisponibles.Columns.Add("NombreCategoria", "Categoría");
            dgvPeliculasDisponibles.Columns.Add("AnoLanzamiento", "Año de Lanzamiento");
            dgvPeliculasDisponibles.Columns.Add("Idioma", "Idioma");

        }


        private void loadDgvPeliculasDisponibles()
        {
            var peliculas = _peliculaUtils.ObtenerTodos(); // Obtiene todas las películas desde el servidor

            if (peliculas != null && peliculas.Count > 0) // Verifica si la lista de películas no es null y tiene elementos
            {
                dgvPeliculasDisponibles.Rows.Clear(); // Limpia las filas actuales del DataGridView

                foreach (var pelicula in peliculas) // Itera sobre cada película y la agrega al DataGridView
                {
                    // Agrega una nueva fila con los datos de la película
                    dgvPeliculasDisponibles.Rows.Add(
                        pelicula.IdPelicula,
                        pelicula.Titulo,
                        pelicula.CategoriaPelicula.NombreCategoria,
                        pelicula.AnoLanzamiento,
                        pelicula.Idioma
                    );
                }
            }
            else // Si no se encontraron películas
            {
                MessageBox.Show("No se encontraron películas.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private class Data
        {
            public int IdSucursal { get; set; }
            public string SucursalNombre { get; set; }
            public string Direccion { get; set; }
            public string Telefono { get; set; }
            public bool Activo { get; set; }
            public int IdPelicula { get; set; }
            public string PeliculaTitulo { get; set; }
            public int AnioLanzamiento { get; set; }
            public int IdCategoria { get; set; }
            public string CategoriaNombre { get; set; }
            public string CategoriaDescripcion { get; set; }
            public int Cantidad { get; set; }
        }

        private void LoadDatos(List<object> datos)
        {

            // Limpia las filas y las columnas existentes
            dgvDatos.Rows.Clear();
            dgvDatos.Columns.Clear();

            // Define las columnas con los nombres de los campos del JSON proporcionado
            dgvDatos.ColumnCount = 12;
            dgvDatos.Columns[0].Name = "IdSucursal";
            dgvDatos.Columns[1].Name = "SucursalNombre";
            dgvDatos.Columns[2].Name = "Direccion";
            dgvDatos.Columns[3].Name = "Telefono";
            dgvDatos.Columns[4].Name = "Activo";
            dgvDatos.Columns[5].Name = "IdPelicula";
            dgvDatos.Columns[6].Name = "PeliculaTitulo";
            dgvDatos.Columns[7].Name = "AnioLanzamiento";
            dgvDatos.Columns[8].Name = "IdCategoria";
            dgvDatos.Columns[9].Name = "CategoriaNombre";
            dgvDatos.Columns[10].Name = "CategoriaDescripcion";
            dgvDatos.Columns[11].Name = "Cantidad";

            foreach (var dato in datos)
            {
                // Deserializa el objeto JSON a una instancia de la clase Data
                var data = JsonConvert.DeserializeObject<Data>(dato.ToString());

                // Añade una fila al DataGridView con los valores correspondientes
                dgvDatos.Rows.Add(
                    data.IdSucursal,
                    data.SucursalNombre,
                    data.Direccion,
                    data.Telefono,
                    data.Activo,
                    data.IdPelicula,
                    data.PeliculaTitulo,
                    data.AnioLanzamiento,
                    data.IdCategoria,
                    data.CategoriaNombre,
                    data.CategoriaDescripcion,
                    data.Cantidad
                );
            }
        }


        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(txtCantidad.Text, out int cantidad)) // Verifica si la cantidad es un número entero
            {
                MessageBox.Show("La cantidad debe ser un número entero.");
                return;
            }

            if (cantidad <= 0)
            {
                MessageBox.Show("La cantidad debe ser mayor a 0.");
                return;
            }

            if (cmbSucursales.SelectedIndex == -1) // Verifica si se seleccionó una sucursal
            {
                MessageBox.Show("Debe seleccionar una sucursal.");
                return;
            }

            if (dgvPeliculasDisponibles.SelectedRows.Count == 0) // Verifica si se seleccionó al menos una película
            {
                MessageBox.Show("Debe seleccionar al menos una película.");
                return;
            }

            string sucursalSeleccionada = cmbSucursales.SelectedItem.ToString();
            string peliculasSeleccionadas = string.Empty;

            foreach (DataGridViewRow row in dgvPeliculasDisponibles.SelectedRows)
            {
                string tituloPelicula = row.Cells["Titulo"].Value.ToString();
                peliculasSeleccionadas += $"{tituloPelicula}, ";
            }

            // Elimina la última coma y espacio
            if (peliculasSeleccionadas.EndsWith(", "))
            {
                peliculasSeleccionadas = peliculasSeleccionadas.Substring(0, peliculasSeleccionadas.Length - 2);
            }

            // Muestra un cuadro de confirmación con la información
            DialogResult result = MessageBox.Show($"Vas a guardar la siguiente información:\n\nCantidad: {cantidad}\nSucursal: {sucursalSeleccionada}\nPelículas: {peliculasSeleccionadas}", "Confirmación", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                // Obtener el ID de la sucursal seleccionada
                int idSucursal = int.Parse(sucursalSeleccionada.Split('-')[0].Trim());

                // Obtener los IDs de las películas seleccionadas
                List<int> idsPeliculasSeleccionadas = new List<int>();
                foreach (DataGridViewRow row in dgvPeliculasDisponibles.SelectedRows)
                {
                    int idPelicula = (int)row.Cells["IdPelicula"].Value;
                    idsPeliculasSeleccionadas.Add(idPelicula);
                }

                // Llamar al método para registrar la relación entre la película y la sucursal
                string response = _pelicuaXSucursalUtils.RegistrarPelicuaXSucursal(idSucursal, idsPeliculasSeleccionadas, cantidad);

                MessageBox.Show(response);

                List<object> datos = _pelicuaXSucursalUtils.ObtenerTodos();
                LoadDatos(datos);

            }
            else
            {
                MessageBox.Show("Operación cancelada.", "Cancelación", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
