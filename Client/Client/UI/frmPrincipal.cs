using Client.UI.Mantenimientos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client.UI
{
    public partial class frmPrincipal : Form
    {
        private string _nombreCompleto;

        public frmPrincipal(string nombreCompleto)
        {
            InitializeComponent();
            _nombreCompleto = nombreCompleto;
            lblUsuarioLogueado.Text = nombreCompleto;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmCategoriaPelicula frmCategoriaPelicula = new frmCategoriaPelicula(_nombreCompleto);
            frmCategoriaPelicula.ShowDialog();
            this.Hide();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            frmPelicula frmPelicula = new frmPelicula(_nombreCompleto);
            frmPelicula.ShowDialog();
            this.Hide();

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            frmEncargado frmEncargado = new frmEncargado(_nombreCompleto);
            frmEncargado.ShowDialog();
            this.Hide();
        }
    }
}
