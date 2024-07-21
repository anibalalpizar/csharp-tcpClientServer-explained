using Client.UI.Mantenimientos;
using Client.UI.Proceso;
using System;
using System.Windows.Forms;

namespace Client.UI
{
    public partial class frmPrincipal : Form
    {
        private string _nombreCompleto;
        private string _idUsuario;


        public frmPrincipal(string nombreCompleto, string idUsuario)
        {
            InitializeComponent();
            _nombreCompleto = nombreCompleto;
            _idUsuario = idUsuario;
            lblUsuarioLogueado.Text = nombreCompleto;
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmCategoriaPelicula frmCategoriaPelicula = new frmCategoriaPelicula(_nombreCompleto, _idUsuario);
            frmCategoriaPelicula.ShowDialog();
            this.Hide();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            frmPelicula frmPelicula = new frmPelicula(_nombreCompleto, _idUsuario);
            frmPelicula.ShowDialog();
            this.Hide();

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            frmEncargado frmEncargado = new frmEncargado(_nombreCompleto, _idUsuario);
            frmEncargado.ShowDialog();
            this.Hide();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            frmSucursal frmSucursal = new frmSucursal(_nombreCompleto, _idUsuario);
            frmSucursal.ShowDialog();
            this.Hide();

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            frmCliente frmCliente = new frmCliente(_nombreCompleto, _idUsuario);
            frmCliente.ShowDialog();
            this.Hide();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            frmPeliculaxSucursal frmPeliculaxSucursal = new frmPeliculaxSucursal(_nombreCompleto, _idUsuario);
            frmPeliculaxSucursal.ShowDialog();
            this.Hide();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            frmPrestamo frmPrestamo = new frmPrestamo(_nombreCompleto, _idUsuario);
            frmPrestamo.ShowDialog();
            this.Hide();
        }
    }
}
