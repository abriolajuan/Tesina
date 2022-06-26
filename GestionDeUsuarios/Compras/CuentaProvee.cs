using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionDeUsuarios
{
    public partial class CuentaProvee : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        public CuentaProvee()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CuentaProvee_Load(object sender, EventArgs e)
        {
            cargarComboBox1();
            comboBox1.SelectedIndex = (-1);

            ocultarInfoPrincipal();
        }
       
        private void cargarComboBox1()
        {
            conexion.Open();
            string sql = "select PROVEE_ID, PROVEE_NOMBRE from PROVEEDOR ORDER BY PROVEE_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox1.DisplayMember = "PROVEE_NOMBRE";
            comboBox1.ValueMember = "PROVEE_ID";
            comboBox1.DataSource = tabla1;
        }

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select compra_fecha, sum(det_compra_cantidad*det_compra_pr_unit)as 'total' from COMPRA as comp, DETALLECOMPRA as det where comp.compra_id = det.compra_id and PROVEE_ID=@PROVEE_ID group by COMPRA_FECHA, det.COMPRA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@provee_id", SqlDbType.Int).Value = comboBox1.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["compra_fecha"],
                                  registros["total"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void mostrarGrillaDos()
        {
            conexion.Open();
            string sql = "select PAGO_COMPRA_FECHA, PAGO_COMPRA_MONTO, MEDIO_TR_NOMBRE from PAGOCOMPRA as pag, COMPRA as comp, MEDIOTRANSACCION as med, DETALLEDEMEDIO as det where pag.COMPRA_ID=comp.COMPRA_ID and PROVEE_ID=@proveeid and med.MEDIO_TR_ID=det.MEDIO_TR_ID and pag.PAGO_COMPRA_ID=det.PAGO_COMPRA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = comboBox1.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView2.Rows.Clear();
            while (registros.Read())
            {
                dataGridView2.Rows.Add(registros["pago_compra_fecha"],
                                    registros["medio_tr_nombre"],
                                   registros["pago_compra_monto"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(comboBox1.SelectedIndex==(-1))
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un proveedor para la búsqueda";
                m.ShowDialog();
            }
            else {
                mostrarInfoPrincipal();

                mostrarGrilla();
                mostrarGrillaDos();
                calcularTotalCompras();
                calcularTotalPagos();
                calcularTotalCuenta();
            }
        }

        public void calcularTotalCompras()
        {
            Decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value);
            }
            label5.Text = "Monto total de compras: $" + suma.ToString();
        }

        public void calcularTotalPagos()
        {
            Decimal suma = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView2.Rows[i].Cells[2].Value);
            }
            label6.Text = "Monto total de pagos de compras: $" + suma.ToString();
        }

        public void calcularTotalCuenta()
        {
            Decimal sumaPagos = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; ++i)
            {
                sumaPagos += Convert.ToDecimal(dataGridView2.Rows[i].Cells[2].Value);
            }
            Decimal sumaCompras = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                sumaCompras += Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value);
            }

            Decimal Total = sumaCompras - sumaPagos;
            if (Total > 0)
            {
                label2.ForeColor = Color.FromArgb(255, 0, 0);
                label2.Text = "Saldo de cuenta: $" + Total.ToString();
            }
            else if (Total <= 0)
            {
                label2.ForeColor = Color.FromArgb(0, 128, 0);
                label2.Text = "Saldo de cuenta: $" + Total.ToString();
            }
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void ocultarInfoPrincipal()
        {
            label2.Visible = false;
            label3.Visible = false;
            label4.Visible = false;
            label5.Visible = false;
            label6.Visible = false;
            dataGridView1.Visible = false;
            dataGridView2.Visible = false;
            pictureBox1.Visible = false;
            pictureBox2.Visible = false;
        }

        private void mostrarInfoPrincipal()
        {
            label2.Visible = true;
            label3.Visible = true;
            label4.Visible = true;
            label5.Visible = true;
            label6.Visible = true;
            dataGridView1.Visible = true;
            dataGridView2.Visible = true;
            pictureBox1.Visible = true;
            pictureBox2.Visible = true;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            ocultarInfoPrincipal();
        }
    }
}
