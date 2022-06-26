﻿using GestionDeUsuarios.Producción;
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
    public partial class ReportesElab : Form
    {
        string tipoActivo;
        string nombreUsuActivo;
        string apellidoUsuActivo;
        public ReportesElab(string tipoActivo2, string nombreActivo, string apellidoActivo)
        {
            InitializeComponent(); 
            tipoActivo = tipoActivo2;
            nombreUsuActivo = nombreActivo;
            apellidoUsuActivo = apellidoActivo;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
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
        public void fijarTipo(string tipo)
        {
            tipoActivo = tipo;
        }



        private void button3_Click(object sender, EventArgs e)
        {
           /* Elaboracion m = new Elaboracion(tipoActivo, nombreActivo2, apellidoActivo2);
            m.ShowDialog();
            m.Close();*/
            AbrirpanelHijo(new Elaboracion(tipoActivo, nombreUsuActivo, apellidoUsuActivo));
            AbrirpanelHijo(new AnalisisElab());
        }
        private void button2_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new Elaboracion(tipoActivo, nombreUsuActivo, apellidoUsuActivo));
            AbrirpanelHijo(new ListadoElab());
        }


        private void ReportesElab_Load(object sender, EventArgs e)
        {
            
            
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox12_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
