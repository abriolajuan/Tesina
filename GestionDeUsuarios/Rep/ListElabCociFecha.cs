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
    public partial class ListElabCociFecha : Form
    {
        private DateTime _fechadesde;
        private DateTime _fechahasta;
        private int _cocinero;

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

        public int cocinero
        {
            get { return _cocinero; }
            set { _cocinero = value; }
        }

        public ListElabCociFecha()
        {
            InitializeComponent();
        }

        private void ListElabCociFecha_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'DataSetListElabCociFecha.ListElabCociFecha' Puede moverla o quitarla según sea necesario.
            try
            { 
            this.ListElabCociFechaTableAdapter.Fill(this.DataSetListElabCociFecha.ListElabCociFecha, fechadesde, fechahasta, cocinero); //AGREGAR ACÁ LO QUE SE ENVÍA

                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                this.reportViewer1.RefreshReport();
            }
        }
    }
}
