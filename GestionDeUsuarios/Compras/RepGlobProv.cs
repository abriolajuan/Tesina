using Microsoft.Reporting.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionDeUsuarios.Compras
{
    public partial class RepGlobProv : Form
    {
        public List<Datos> Datos = new List<Datos>();
        public RepGlobProv()
        {
            InitializeComponent();
        }

        private void RepGlobProv_Load(object sender, EventArgs e)
        {
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(new ReportDataSource("DataSet1", Datos));
            this.reportViewer1.RefreshReport();
        }
    }
}
