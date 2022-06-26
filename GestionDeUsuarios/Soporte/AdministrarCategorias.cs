using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionDeUsuarios
{
    public partial class AdministrarCategorias : Form
    {
        public AdministrarCategorias()
        {
            InitializeComponent();
        }

        private Form formularioActivo = null;
        private void AbrirpanelHijo(Form formularioHijo)
        {
            if (formularioActivo != null)
                formularioActivo.Close();
            formularioActivo = formularioHijo;
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;
            panelHijo.Controls.Add(formularioHijo);
            panelHijo.Tag = formularioHijo;
            formularioHijo.BringToFront(); // ESTO POR SI PONEMOS UN LOGO DE FONDO ADELANTE VA EL FORMULARIO
            formularioHijo.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new EntidadesCred());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new Rubros());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new Marcas());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new MediosDeTrans());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new UbicGeogr());
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
