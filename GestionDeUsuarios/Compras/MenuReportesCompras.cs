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
    public partial class MenuReportesCompras : Form
    {
        public MenuReportesCompras()
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
            panel1.Controls.Add(formularioHijo);
            panel1.Tag = formularioHijo;
            formularioHijo.BringToFront(); // ESTO POR SI PONEMOS UN LOGO DE FONDO ADELANTE VA EL FORMULARIO
            formularioHijo.Show();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            AbrirpanelHijo(new ReportesCompras());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new CuentaGlobal());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new CuentaProvee());
        }

        private void MenuReportesCompras_Load(object sender, EventArgs e)
        {
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new AnalisisCompras());
        }
    }
}
