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
    public partial class ListElabSoloFecha : Form
    {
        private DateTime _fechadesde;
        private DateTime _fechahasta;

        public DateTime fechadesde
        {
            get { return _fechadesde;  }
            set { _fechadesde = value; }
        }

        public DateTime fechahasta
        {
            get { return _fechahasta; }
            set { _fechahasta = value; }
        }

        public ListElabSoloFecha()
        {
            InitializeComponent();
        }

        private void ListElabSoloFecha_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'DataSetListElabSoloFecha.ListElabSoloFecha' Puede moverla o quitarla según sea necesario.
            try
            {
                this.ListElabSoloFechaTableAdapter.Fill(this.DataSetListElabSoloFecha.ListElabSoloFecha, fechadesde, fechahasta); //AGREGAR ACÁ LO QUE SE ENVÍA

                this.reportViewer1.RefreshReport();
            }
            catch(Exception ex)
            {
                this.reportViewer1.RefreshReport();
            }
        }
    }
}
