using Client.Models;
using Client.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Client.UI.Mantenimientos
{
    public partial class frmSucursal : Form
    {
        private string _nombreCompleto; // Nombre completo del usuario
        private string _idUsuario; // ID del usuario
        private EncargadoUtils _encargadoUtils; // Utilidades de encargado
        private SucursalUtils _sucursalUtils; // Utilidades de sucursal

        public frmSucursal(string nombreCompleto, string idUsuario) // Constructor del formulario
        {
            InitializeComponent();
            _nombreCompleto = nombreCompleto; // Asigna el nombre completo del usuario
            _idUsuario = idUsuario; // Asigna el ID del usuario
            _encargadoUtils = new EncargadoUtils(); // Inicializa las utilidades de encargado
            _sucursalUtils = new SucursalUtils(); // Inicializa las utilidades de sucursal
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmPrincipal frmPrincipal = new frmPrincipal(_nombreCompleto, _idUsuario); // Crea una instancia del formulario principal
            frmPrincipal.ShowDialog();
            this.Hide();
        }

        private void frmSucursal_Load(object sender, EventArgs e)
        {
            LoadCmbEncargados();
            LoadCmbActivo();
            List<object> sucursales = _sucursalUtils.ObtenerTodos();
            LoadData(sucursales);
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            int idSucursal;
            if (!int.TryParse(txtIdSucursal.Text, out idSucursal)) // Verifica si el ID de la sucursal es un número entero
            {
                MessageBox.Show("El ID de la sucursal debe ser un número entero.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbEncargados.SelectedIndex == -1) // Verifica si se ha seleccionado un encargado
            {
                MessageBox.Show("Debe seleccionar un encargado.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (cmbActivo.SelectedIndex == -1) // Verifica si se ha seleccionado si la sucursal está activa o no
            {
                MessageBox.Show("Debe seleccionar si la sucursal está activa o no.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string nombreSucursal = txtNombreSucursal.Text.Trim(); // Obtiene y recorta el nombre de la sucursal
            string direccion = txtDireccion.Text.Trim(); // Obtiene y recorta la dirección de la sucursal
            string telefono = txtTelefono.Text.Trim(); // Obtiene y recorta el teléfono de la sucursal
            int idEncargado = int.Parse(cmbEncargados.Text); // Obtiene el ID del encargado seleccionado
            bool activo = cmbActivo.SelectedIndex == 0; // Obtiene si la sucursal está activa o no

            if (string.IsNullOrEmpty(nombreSucursal) || string.IsNullOrEmpty(direccion) || string.IsNullOrEmpty(telefono)) // Verifica si los campos no están vacíos
            {
                MessageBox.Show("Todos los campos son requeridos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // el nombre debe ser menor a 50 caracteres
            if (nombreSucursal.Length > 50)
            {
                MessageBox.Show("El nombre de la sucursal debe ser menor a 50 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // la dirección debe ser menor a 150 caracteres
            if (direccion.Length > 150)
            {
                MessageBox.Show("La dirección de la sucursal debe ser menor a 150 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // el teléfono debe ser menor a 10 caracteres
            if (telefono.Length > 10)
            {
                MessageBox.Show("El teléfono de la sucursal debe ser menor a 10 caracteres.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                string result = _sucursalUtils.RegistrarSucursal(idSucursal, nombreSucursal, direccion, telefono,
                    idEncargado, activo); // Intenta registrar la sucursal
                MessageBox.Show(result, "Información", MessageBoxButtons.OK, MessageBoxIcon.Information); // Muestra el resultado de la operación

                List<object> sucursales = _sucursalUtils.ObtenerTodos(); // Obtiene todas las sucursales actualizadas
                LoadData(sucursales); // Carga los datos actualizados en el data grid view
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Muestra un mensaje de error
            }
        }

        private void LoadCmbEncargados()
        {
            cmbEncargados.Items.Clear(); // Limpia el combo box de encargados
            List<Encargado> encargados = _encargadoUtils.ObtenerTodos(); // Obtiene todos los encargados

            if (encargados != null && encargados.Count > 0) // Verifica si la lista de encargados no es null y tiene elementos
            {
                foreach (var encargado in encargados) // Itera sobre cada encargado y lo agrega al combo box
                {
                    cmbEncargados.Items.Add(encargado.IdEncargado); // Agrega el encargado al combo box
                }
            }
            else
            {
                MessageBox.Show("No se encontraron encargados.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); // Muestra un mensaje de error si no se encontraron encargados
            }
        }

        private void LoadData(List<object> sucursales)
        {
            // Limpiar las filas y las columnas existentes
            dgvDatos.Rows.Clear();
            dgvDatos.Columns.Clear();

            // Definir las columnas con los nombres de los campos del JSON proporcionado
            dgvDatos.ColumnCount = 10;
            dgvDatos.Columns[0].Name = "IdSucursal";
            dgvDatos.Columns[1].Name = "IdEncargado";
            dgvDatos.Columns[2].Name = "SucursalNombre";
            dgvDatos.Columns[3].Name = "Direccion";
            dgvDatos.Columns[4].Name = "Telefono";
            dgvDatos.Columns[5].Name = "Activo";
            dgvDatos.Columns[6].Name = "EncargadoIdentificacion";
            dgvDatos.Columns[7].Name = "EncargadoFechaIngreso";
            dgvDatos.Columns[8].Name = "PersonaNombre";
            dgvDatos.Columns[9].Name = "PrimerApellido";

            foreach (var sucursal in sucursales)
            {
                // Castear el objeto sucursal al tipo correspondiente
                var data = JsonConvert.DeserializeObject<JObject>(sucursal.ToString());

                // Añadir una fila al DataGridView con los valores correspondientes
                dgvDatos.Rows.Add(
                    data["IdSucursal"].ToString(),
                    data["IdEncargado"].ToString(),
                    data["SucursalNombre"].ToString(),
                    data["Direccion"].ToString(),
                    data["Telefono"].ToString(),
                    data["Activo"].ToString(),
                    data["EncargadoIdentificacion"].ToString(),
                    data["EncargadoFechaIngreso"].ToString(),
                    data["PersonaNombre"].ToString(),
                    data["PrimerApellido"].ToString()
                );
            }
        }




        private void LoadCmbActivo()
        {
            cmbActivo.Items.Clear(); // Limpia el combo box de activo
            cmbActivo.Items.Add("Si"); // Agrega 'Si' al combo box
            cmbActivo.Items.Add("No"); // Agrega 'No' al combo box
        }
    }
}
