using Client.Models;
using Client.Utils;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Client.UI.Mantenimientos
{
    public partial class frmCliente : Form
    {
        private ClienteUtils _clienteUtils; // Instancia de la utilidad de clientes
        private string _nombreCompleto; // Almacena el nombre completo del usuario

        public frmCliente(string nombreCompleto)
        {
            InitializeComponent();
            _clienteUtils = new ClienteUtils(); // Inicializa la utilidad de clientes
            _nombreCompleto = nombreCompleto; // Establece el nombre completo del usuariokd
        }

        // Evento que se dispara al hacer clic en el botón 'Registrar'
        private void button1_Click(object sender, EventArgs e)
        {
            // Declaramos una variable para 
            int idCliente;

            if (!int.TryParse(txtIdCliente.Text, out idCliente))
            {
                // Si la conversión falla, mostramos un mensaje de error y salimos del método
                MessageBox.Show("El ID del cliente debe ser un número entero.");
                return;

            }

            string identificacion = txtIdentifiacion.Text.Trim(); // Obtiene y limpia el campo de identificación
            string nombre = txtNombre.Text.Trim(); // Obtiene y limpia el campo de nombre
            string apellido1 = txtprimerApellido.Text.Trim(); // Obtiene y limpia el campo de primer apellido
            string apellido2 = txtSegundoApellido.Text.Trim(); // Obtiene y limpia el campo de segundo apellido
            DateTime fechaNacimiento = dtpFechaNacimiento.Value; // Obtiene la fecha de nacimiento
            DateTime fechaIngreso = dtpFechaIngreso.Value; // Obtiene la fecha de ingreso
            bool activo = cmbActivo.Text == "Si"; // Obtiene el valor del combo box de activo

            // La fecha de ingreso no puede ser mayor a la fecha actual
            if (DateTime.Compare(dtpFechaIngreso.Value, DateTime.Now) > 0)
            {
                MessageBox.Show("La fecha de ingreso no puede ser mayor a la fecha actual.");
                return;
            }

            // La fecha de nacimiento no puede ser mayor a la fecha de ingreso ni a la fecha actual
            if (DateTime.Compare(dtpFechaNacimiento.Value, DateTime.Now) > 0 || DateTime.Compare(dtpFechaNacimiento.Value, dtpFechaIngreso.Value) > 0)
            {
                MessageBox.Show("La fecha de nacimiento no puede ser mayor a la fecha de ingreso ni a la fecha actual.");
                return;
            }

            // La identificación no puede ser mayor a 12 caracteres
            if (identificacion.Length > 12)
            {
                MessageBox.Show("La identificación no puede ser mayor a 12 caracteres.");
                return;
            }

            // El nombre, primer apellido y segundo apellido no pueden ser mayores de 25 caracteres
            if (nombre.Length > 25 || apellido1.Length > 25 || apellido2.Length > 25)
            {
                MessageBox.Show("El nombre, primer apellido y segundo apellido no pueden ser mayores de 25 caracteres.");
                return;
            }

            // Vericamos si los campos obligatorios están vacíos
            if (string.IsNullOrEmpty(identificacion) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido1) || string.IsNullOrEmpty(apellido2) || string.IsNullOrEmpty(fechaNacimiento.ToString()) || string.IsNullOrEmpty(fechaIngreso.ToString()))
            {
                // Si están vacíos, mostramos un mensaje de error y salimos del método
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            try
            {
                string result = _clienteUtils.RegistrarCliente(idCliente, identificacion, nombre, apellido1, apellido2, fechaNacimiento, fechaIngreso, activo);

                MessageBox.Show(result);

                List<Cliente> results = _clienteUtils.ObtenerTodos();
                LoadData(results);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al registrar el cliente.");
            }


        }

        private void LoadCmbActivo()
        {
            cmbActivo.Items.Clear(); // Limpia el combo box de activo
            cmbActivo.Items.Add("Si"); // Agrega 'Si' al combo box
            cmbActivo.Items.Add("No"); // Agrega 'No' al combo box
        }

        private void LoadData(List<Cliente> clientes)
        {
            dgvDatos.DataSource = null; // Limpia el origen de datos del DataGridView
            var dataTable = new System.Data.DataTable(); // Crea una nueva tabla de datos

            // Define las columnas de la tabla de datos
            dataTable.Columns.Add("ID Cliente", typeof(int));
            dataTable.Columns.Add("Identificación", typeof(string));
            dataTable.Columns.Add("Nombre", typeof(string));
            dataTable.Columns.Add("Primer Apellido", typeof(string));
            dataTable.Columns.Add("Segundo Apellido", typeof(string));
            dataTable.Columns.Add("Fecha de Nacimiento", typeof(DateTime));
            dataTable.Columns.Add("Fecha de Ingreso", typeof(DateTime));
            dataTable.Columns.Add("Activo", typeof(bool));

            // Recorre la lista de clientes
            foreach (var cliente in clientes)
            {
                // Agrega una nueva fila a la tabla de datos con los datos del cliente actual
                dataTable.Rows.Add(cliente.IdCliente, cliente.Identificacion, cliente.Nombre, cliente.Apellido1, cliente.Apellido2, cliente.FechaNacimiento, cliente.FechaIngreso, cliente.Activo);
            }

            // Asigna la tabla de datos como origen de datos del DataGridView
            dgvDatos.DataSource = dataTable;
        }

        private void frmCliente_Load(object sender, EventArgs e)
        {
            LoadCmbActivo();
            List<Cliente> results = _clienteUtils.ObtenerTodos();
            LoadData(results);
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmPrincipal frmPrincipal = new frmPrincipal(_nombreCompleto); // Crea una nueva instancia del formulario principal
            frmPrincipal.ShowDialog(); // Muestra el formulario principal como un cuadro de diálogo
            this.Hide(); // Oculta el formulario actual
        }
    }
}
