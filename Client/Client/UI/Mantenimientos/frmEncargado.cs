using Client.Models; // Importa los modelos de datos del cliente
using Client.Utils; // Importa las utilidades del cliente
using System; // Importa funcionalidades básicas del sistema
using System.Collections.Generic; // Importa funcionalidades para trabajar con colecciones genéricas
using System.Windows.Forms; // Importa funcionalidades para la creación de interfaces de usuario en Windows

namespace Client.UI.Mantenimientos // Define el espacio de nombres 'Client.UI.Mantenimientos'
{
    // Define la clase 'frmEncargado' que hereda de 'Form'
    public partial class frmEncargado : Form
    {
        private EncargadoUtils _encargadoUtils; // Instancia de la utilidad de encargados
        private string _nombreCompleto; // Almacena el nombre completo del usuario

        // Constructor de la clase, inicializa componentes y establece el nombre completo del usuario
        public frmEncargado(string nombreCompleto)
        {
            InitializeComponent(); // Inicializa los componentes de la interfaz
            _encargadoUtils = new EncargadoUtils(); // Inicializa la utilidad de encargados
            _nombreCompleto = nombreCompleto; // Establece el nombre completo del usuario
        }

        // Evento que se dispara al hacer clic en el botón 'Registrar'
        private void btnRegistrar_Click(object sender, EventArgs e)
        {
            // Declaramos una variable para el ID del encargado
            int idEncargado;
            // Intentamos convertir el texto del campo 'txtIdEncargado' a un número entero
            if (!int.TryParse(txtIdEncargado.Text, out idEncargado))
            {
                // Si la conversión falla, mostramos un mensaje de error y salimos del método
                MessageBox.Show("El ID del encargado debe ser un número entero.");
                return;
            }

            string identificacion = txtIdentificacion.Text.Trim(); // Obtiene y limpia el campo de identificación
            string nombre = txtNombre.Text.Trim(); // Obtiene y limpia el campo de nombre
            string apellido1 = txtApellido1.Text.Trim(); // Obtiene y limpia el campo de primer apellido
            string apellido2 = txtApellido2.Text.Trim(); // Obtiene y limpia el campo de segundo apellido
            DateTime fechaNacimiento = dtpFechaNacimiento.Value; // Obtiene la fecha de nacimiento
            DateTime fechaIngreso = dtpFechaIngreso.Value; // Obtiene la fecha de ingreso

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

            // Verificamos si los campos obligatorios están vacíos
            if (string.IsNullOrEmpty(identificacion) || string.IsNullOrEmpty(nombre) || string.IsNullOrEmpty(apellido1) || string.IsNullOrEmpty(apellido2) || string.IsNullOrEmpty(fechaNacimiento.ToString()) || string.IsNullOrEmpty(fechaIngreso.ToString()))
            {
                // Si están vacíos, mostramos un mensaje de error y salimos del método
                MessageBox.Show("Todos los campos son obligatorios.");
                return;
            }

            try
            {
                // Intentamos registrar el encargado con los datos proporcionados
                string result = _encargadoUtils.RegistrarEncargado(idEncargado, identificacion, nombre, apellido1, apellido2, fechaNacimiento, fechaIngreso);
                // Mostramos el resultado de la operación
                MessageBox.Show(result);
                // Obtenemos todos los encargados y los cargamos en la interfaz
                List<Encargado> results = _encargadoUtils.ObtenerTodos();
                LoadData(results);
            }
            catch (Exception ex)
            {
                // Mostramos un mensaje de error si ocurre una excepción
                MessageBox.Show($"Error al registrar el encargado: {ex.Message}");
            }
        }

        // Evento que se dispara al hacer clic en el botón del menú
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmPrincipal frmPrincipal = new frmPrincipal(_nombreCompleto); // Crea una nueva instancia del formulario principal
            frmPrincipal.ShowDialog(); // Muestra el formulario principal como un cuadro de diálogo
            this.Hide(); // Oculta el formulario actual
        }

        // Evento que se dispara al cargar el formulario
        private void frmEncargado_Load(object sender, EventArgs e)
        {
            List<Encargado> results = _encargadoUtils.ObtenerTodos(); // Obtiene todos los encargados
            LoadData(results); // Carga los datos de los encargados en la interfaz
        }

        // Método para cargar datos en el DataGridView
        private void LoadData(List<Encargado> encargados)
        {
            dgvDatos.DataSource = null; // Limpia el DataGridView
            var dataTable = new System.Data.DataTable(); // Crea una nueva tabla de datos

            // Define las columnas de la tabla
            dataTable.Columns.Add("ID Encargado", typeof(int));
            dataTable.Columns.Add("Identificación", typeof(string));
            dataTable.Columns.Add("Nombre", typeof(string));
            dataTable.Columns.Add("Primer Apellido", typeof(string));
            dataTable.Columns.Add("Segundo Apellido", typeof(string));
            dataTable.Columns.Add("Fecha de Nacimiento", typeof(DateTime));
            dataTable.Columns.Add("Fecha de Ingreso", typeof(DateTime));

            // Llena la tabla con los datos de los encargados
            foreach (var encargado in encargados)
            {
                dataTable.Rows.Add(encargado.IdEncargado, encargado.Identificacion, encargado.Nombre, encargado.Apellido1, encargado.Apellido2, encargado.FechaNacimiento, encargado.FechaIngreso);
            }

            dgvDatos.DataSource = dataTable; // Establece la fuente de datos del DataGridView
        }
    }
}
