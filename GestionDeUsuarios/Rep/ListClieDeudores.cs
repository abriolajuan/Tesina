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
    public partial class ListClieDeudores : Form
    {
        public ListClieDeudores()
        {
            InitializeComponent();
        }

        private void ListClieDeudores_Load(object sender, EventArgs e)
        {
            try
            {
                this.ListClieDeudoresTableAdapter.Fill(this.DataSetListClieDeudores.ListClieDeudores);
                this.reportViewer1.RefreshReport();
            }
            catch(Exception ex)
            {
                this.reportViewer1.RefreshReport();
            }
        }
    }
}
