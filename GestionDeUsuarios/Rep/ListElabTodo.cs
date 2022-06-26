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
    public partial class ListElabTodo : Form
    {
        private DateTime _fechadesde;
        private DateTime _fechahasta;
        private int _cocinero;
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

        public int cocinero
        {
            get { return _cocinero; }
            set { _cocinero = value; }
        }

        public int estado
        {
            get { return _estado; }
            set { _estado = value; }
        }

        public ListElabTodo()
        {
            InitializeComponent();
        }

        private void ListElabTodo_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'DataSetListElabTodo.ListElabTodo' Puede moverla o quitarla según sea necesario.
            try
            { 
            this.ListElabTodoTableAdapter.Fill(this.DataSetListElabTodo.ListElabTodo, fechadesde, fechahasta, cocinero, estado); //AGREGAR ACÁ LO QUE SE ENVÍA

                this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                this.reportViewer1.RefreshReport();
            }
        }
    }
}
