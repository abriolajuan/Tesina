using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionDeUsuarios.Ventas
{
    public partial class MenuCobro : Form
    {
        public MenuCobro()
        {
            InitializeComponent();
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        string numeroCelu = "";
        private void button1_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new CobroEfectivo(numeroCelu));
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
            panel1.Controls.Add(formularioHijo);
            panel1.Tag = formularioHijo;
            formularioHijo.BringToFront(); // ESTO POR SI PONEMOS UN LOGO DE FONDO ADELANTE VA EL FORMULARIO
            formularioHijo.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new CobroTarjetas(numeroCelu));
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new CobroTransferencia(numeroCelu));
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new CobroCheque());
        }
    }
}
