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
    public partial class GlobalProvee : Form
    {
        public GlobalProvee()
        {
            InitializeComponent();
        }

        private void GlobalProvee_Load(object sender, EventArgs e)
        {
            // TODO: esta línea de código carga datos en la tabla 'DataSetGlobalProvee.GobalProvee' Puede moverla o quitarla según sea necesario.
            try { 
            this.GobalProveeTableAdapter.Fill(this.DataSetGlobalProvee.GobalProvee);

            this.reportViewer1.RefreshReport();
            }
            catch (Exception ex)
            {
                this.reportViewer1.RefreshReport();
            }
        }
    }
}
