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
    public partial class UbicGeogr : Form
    {
        public UbicGeogr()
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            AbrirpanelHijo(new UbicProvincia());
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new UbicLocalidad());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new UbicBarrio());
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
