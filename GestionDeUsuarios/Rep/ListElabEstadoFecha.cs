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
    public partial class ListElabEstadoFecha : Form
    {
        private DateTime _fechadesde;
        private DateTime _fechahasta;
        private int _estado;

        public DateTime fechadesde
        {
            get { return _fechadesde; }
            set { _fechadesde = value; }
        }

        public DateTime fechahasta
        {
            get { return _fechahasta; }
            set { _fechahasta = value; }
        }

        public int estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        public ListElabEstadoFecha()
        {
            InitializeComponent();
        }

        private void ListElabEstadoFecha_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'DataSetListElabEstadoFecha.ListElabEstadoFecha' Puede moverla o quitarla según sea necesario.
            try
            { 
            this.ListElabEstadoFechaTableAdapter.Fill(this.DataSetListElabEstadoFecha.ListElabEstadoFecha, fechadesde, fechahasta, estado); //AGREGAR ACÁ LO QUE SE ENVÍA

                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                this.reportViewer1.RefreshReport();
            }
        }
    }
}
