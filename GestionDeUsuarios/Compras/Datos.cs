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
    public partial class Datos : Form
    {
        public string nombre { get; set; }
        public float totCompras { get; set; }
        public float totPagos { get; set; }
        public float saldo { get; set; }
    }
}
