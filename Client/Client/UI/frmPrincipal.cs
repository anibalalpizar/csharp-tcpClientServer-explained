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
        public frmPrincipal(string nombreCompleto)
        {
            InitializeComponent();
            lblUsuarioLogueado.Text = nombreCompleto;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            frmCategoriaPelicula frmCategoriaPelicula = new frmCategoriaPelicula();
            frmCategoriaPelicula.ShowDialog();
            this.Hide();
        }
    }
}
