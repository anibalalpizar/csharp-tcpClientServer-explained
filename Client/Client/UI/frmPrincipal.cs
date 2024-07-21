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
            frmCategoriaPelicula frmCategoriaPelicula = new frmCategoriaPelicula(this, _nombreCompleto, _idUsuario);
            frmCategoriaPelicula.ShowDialog();       
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            frmPelicula frmPelicula = new frmPelicula(this, _nombreCompleto, _idUsuario);
            frmPelicula.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            frmEncargado frmEncargado = new frmEncargado(this, _nombreCompleto, _idUsuario);
            frmEncargado.ShowDialog();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            frmSucursal frmSucursal = new frmSucursal(this,_nombreCompleto, _idUsuario);
            frmSucursal.ShowDialog();
        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            frmCliente frmCliente = new frmCliente(this,_nombreCompleto, _idUsuario);
            frmCliente.ShowDialog();
        }

        private void toolStripButton6_Click(object sender, EventArgs e)
        {
            frmPeliculaxSucursal frmPeliculaxSucursal = new frmPeliculaxSucursal(this,_nombreCompleto, _idUsuario);
            frmPeliculaxSucursal.ShowDialog();
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            frmPrestamo frmPrestamo = new frmPrestamo(this,_nombreCompleto, _idUsuario);
            frmPrestamo.ShowDialog();
        }
    }
}
