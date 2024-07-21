using Client.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client.UI.Proceso
{
    public partial class frmPrestamo : Form
    {
        private string _nombreCompleto;
        private string _idUsuario;
        private SucursalUtils _sucursalUtils;
        private PeliculaUtils _peliculaUtils;
        private frmPrincipal _frmPrincipal;

        public frmPrestamo(frmPrincipal frmPrincipal,string nombreCompleto, string idUsuario)
        {
            InitializeComponent();
            _nombreCompleto = nombreCompleto;
            _idUsuario = idUsuario;
            _sucursalUtils = new SucursalUtils();
            _peliculaUtils = new PeliculaUtils();
            _frmPrincipal = frmPrincipal;
        }

        private void frmPrestamo_Load(object sender, EventArgs e)
        {
            loadCmbSucursales();
            List<object> datos = _peliculaUtils.MisPeliculas(Int32.Parse(_idUsuario));
            loadDatos(datos);

        }

        private void loadCmbSucursales()
        {
            cmbSucursales.Items.Clear();
            List<dynamic> sucursales = _sucursalUtils.ObtenerTodos();

            if (sucursales != null && sucursales.Count > 0)
            {
                foreach (var sucursal in sucursales)
                {
                    bool activo = (bool)((JValue)sucursal.Activo).Value;

                    if (activo)
                    {
                        var displayText = $"{sucursal.IdSucursal} - {sucursal.SucursalNombre}";
                        cmbSucursales.Items.Add(displayText);
                    }
                }

                if (cmbSucursales.Items.Count == 0)
                {
                    MessageBox.Show("No se encontraron sucursales activas.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                MessageBox.Show("No se encontraron sucursales.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private class Data
        {
            public int IdPrestamo { get; set; }
            public DateTime FechaPrestamo { get; set; }
            public bool PendienteDevolucion { get; set; }
            public int IdPelicula { get; set; }
            public string Titulo { get; set; }
            public int AnioLanzamiento { get; set; }
            public string Idioma { get; set; }
            public string NombreCategoria { get; set; }
        }

        private void loadDatos(List<object> datos)
        {
            // limpiar las filas y columnas existentes
            dgvDatos.Rows.Clear();
            dgvDatos.Columns.Clear();

            // define las columnas con los nombres de los campos del json proporcionado
            dgvDatos.Columns.Add("IdPrestamo", "ID Préstamo");
            dgvDatos.Columns.Add("FechaPrestamo", "Fecha de Préstamo");
            dgvDatos.Columns.Add("PendienteDevolucion", "Pendiente de Devolución");
            dgvDatos.Columns.Add("IdPelicula", "ID Película");
            dgvDatos.Columns.Add("Titulo", "Título");
            dgvDatos.Columns.Add("AnioLanzamiento", "Año de Lanzamiento");
            dgvDatos.Columns.Add("Idioma", "Idioma");
            dgvDatos.Columns.Add("NombreCategoria", "Nombre de la Categoría");

            // recorre los datos y los agrega a las filas del dgv
            foreach (var dato in datos)
            {
                var data = JsonConvert.DeserializeObject<Data>(dato.ToString());
                dgvDatos.Rows.Add(
                    data.IdPrestamo,
                    data.FechaPrestamo,
                    data.PendienteDevolucion,
                    data.IdPelicula,
                    data.Titulo,
                    data.AnioLanzamiento,
                    data.Idioma,
                    data.NombreCategoria
                );
            }


        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            // Muestra la instancia original del formulario principal
            _frmPrincipal.Show();
            this.Close(); // Cierra el formulario actual
        }

        private void cmbSucursales_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbSucursales.SelectedIndex != -1)
            {
                string selectedText = cmbSucursales.SelectedItem.ToString();
                string[] selectedParts = selectedText.Split('-');
                string idSucursal = selectedParts[0].Trim();
                string nombreSucursal = selectedParts[1].Trim();

                MessageBox.Show($"Mostrando peliculas para ID: {idSucursal}\nNombre: {nombreSucursal}", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

                var peliculasResponse = _sucursalUtils.ObtenerPorId(Int32.Parse(idSucursal));

                if (peliculasResponse != null)
                {
                    List<Peliculadgv> peliculas = JsonConvert.DeserializeObject<List<Peliculadgv>>(peliculasResponse.ToString());
                    LlenarDgvPeliculas(peliculas);
                }
                else
                {
                    MessageBox.Show("No se encontraron películas para la sucursal seleccionada.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void LlenarDgvPeliculas(List<Peliculadgv> peliculas)
        {
            dgvPeliculas.Rows.Clear();

            dgvPeliculas.Columns.Add("IdPelicula", "ID Película");
            dgvPeliculas.Columns.Add("Titulo", "Título");
            dgvPeliculas.Columns.Add("AnioLanzamiento", "Año de Lanzamiento");
            dgvPeliculas.Columns.Add("Idioma", "Idioma");
            dgvPeliculas.Columns.Add("NombreCategoria", "Nombre de la Categoría");
            dgvPeliculas.Columns.Add("Descripcion", "Descripción");
            dgvPeliculas.Columns.Add("Cantidad", "Cantidad");

            foreach (var pelicula in peliculas)
            {
                dgvPeliculas.Rows.Add(
                    pelicula.IdPelicula,
                    pelicula.Titulo,
                    pelicula.AnioLanzamiento,
                    pelicula.Idioma,
                    pelicula.NombreCategoria,
                    pelicula.Descripcion,
                    pelicula.Cantidad
                );
            }
        }
        private class Peliculadgv
        {
            public int IdSucursal { get; set; }
            public int IdPelicula { get; set; }
            public int Cantidad { get; set; }
            public int IdCategoria { get; set; }
            public string Titulo { get; set; }
            public int AnioLanzamiento { get; set; }
            public string Idioma { get; set; }
            public string NombreCategoria { get; set; }
            public string Descripcion { get; set; }
        }

        private void btnCrear_Click(object sender, EventArgs e)
        {
            // validar que se haya seleccionado algo del combo
            if (cmbSucursales.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una sucursal.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // validar que se haya seleccionado algo del dgv
            if (dgvPeliculas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Debe seleccionar una película.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // si se selecionan mas de una fila, lanzar mensaje de error
            if (dgvPeliculas.SelectedRows.Count > 1)
            {
                MessageBox.Show("Debe seleccionar solo una película.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // obtener la fila seleccionada
            DataGridViewRow selectedRow = dgvPeliculas.SelectedRows[0];

            // obtener el id de la pelicula
            int idPelicula = Int32.Parse(selectedRow.Cells["IdPelicula"].Value.ToString());

            // el id del cliente que solicita el prestamo
            int idCliente = Int32.Parse(_idUsuario);

            // Obtener el id de la sucursal
            string selectedText = cmbSucursales.SelectedItem.ToString();
            string[] selectedParts = selectedText.Split('-');
            string idSucursal = selectedParts[0].Trim();



            var response = _peliculaUtils.PrestarPelicula(idCliente, Int32.Parse(idSucursal), idPelicula);

            MessageBox.Show(response, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);

            List<object> datos = _peliculaUtils.MisPeliculas(Int32.Parse(_idUsuario));
            loadDatos(datos);



        }
    }
}

