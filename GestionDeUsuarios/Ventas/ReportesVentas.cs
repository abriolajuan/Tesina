using GestionDeUsuarios.Ventas;
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
    public partial class ReportesVentas : Form
    {
        public ReportesVentas()
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

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new ListadoClientes());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new ListadoDeudores());
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new ListadoProductos());
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new ListadoVendedores());
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new AnalisisVentas());
        }

        string numeroCelu = "";
        private void button7_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new CuentaCliente(numeroCelu));
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new AnalisisMedios());
        }
    }
}
