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

namespace GestionDeUsuarios.Ventas
{
    public partial class CuentaCliente : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        string numeroCelular;
        public CuentaCliente(string numeroCelu)
        {
            InitializeComponent();
            numeroCelular = numeroCelu;
        }

        public Form formularioActivo = null;
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        string uno, dos;
        decimal suma;

        private void CuentaCliente_Load(object sender, EventArgs e)
        {
            label13.Visible = false;
            textBox1.Visible = false;
            label20.Visible = false;
            label21.Visible = false;
            textBox5.Visible = false;
            textBox3.Enabled = false;

            ocultarInfoPrincipal();

            if (textBox1.Text!="")
            {
                mostrarListadoVentas();
                mostrarListadoCobros();
                calcularSaldoTotalVentas();
                calcularSaldoTotalCobros();
                calcularSaldoTotal();
                label21.Text = numeroCelular;
            }

            if (textBox3.Text != "")
            {
                mostrarInfoPrincipal();
            }
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
            pictureBox12.Visible = false;
            button11.Visible = false;
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
            pictureBox12.Visible = true;
            button11.Visible = true;
        }

        private void mostrarListadoCobros()
        {
            conexion.Open();
            string sql = "select COBRO_VENTA_FECHA, COBRO_VENTA_MONTO, MEDIO_TR_NOMBRE from COBROVENTA as cob, VENTA as ven, MEDIOTRANSACCION as med, DETALLEDEMEDIO as det where cob.VENTA_ID=ven.VENTA_ID and CLIENTE_ID=@clienteid and med.MEDIO_TR_ID=det.MEDIO_TR_ID and cob.COBRO_VENTA_ID=det.COBRO_VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@clienteid", SqlDbType.Int).Value = textBox1.Text;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView2.Rows.Clear();
            while (registros.Read())
            {
                dataGridView2.Rows.Add(registros["COBRO_VENTA_FECHA"],
                                  registros["MEDIO_TR_NOMBRE"],
                                  registros["COBRO_VENTA_MONTO"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void mostrarListadoVentas()
        {
            conexion.Open();
            string sql = "SELECT t1.venta_fecha, T1.totalventa-t3.descuento as total FROM (SELECT venta_fecha, det.VENTA_ID, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'totalventa' FROM DETALLEVENTA as det, VENTA as ven WHERE det.VENTA_ID=ven.VENTA_ID and ven.CLIENTE_ID=@clienteid group by det.VENTA_ID, VENTA_FECHA) T1 LEFT JOIN (select ven.venta_id, coalesce(ven.VENTA_DTO,0) as 'descuento' from venta as ven, detalleventa as det   where det.venta_id=ven.venta_id and ven.CLIENTE_ID=@clienteid group by ven.venta_id, ven.VENTA_DTO) t3 ON (t3.VENTA_ID=T1.VENTA_ID)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@clienteid", SqlDbType.Int).Value = textBox1.Text;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["VENTA_FECHA"],
                                  registros["total"].ToString());
            }
            registros.Close();
            conexion.Close();
        }


        public void calcularSaldoTotalVentas()
        {
            decimal sumatoria = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                sumatoria += Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value);
            }
            label7.Text = sumatoria.ToString();
        }

        public void calcularSaldoTotalCobros()
        {
            decimal sumatoria = 0;
            for (int i = 0; i < dataGridView2.Rows.Count; ++i)
            {
                sumatoria += Convert.ToDecimal(dataGridView2.Rows[i].Cells[2].Value);
            }
            label8.Text = sumatoria.ToString();
        }

        public void calcularSaldoTotal()
        {
            suma = 0;
            decimal totalVentas = Convert.ToDecimal(label7.Text);
            decimal totalCobros = Convert.ToDecimal(label8.Text);
            suma = totalVentas - totalCobros;
            label2.Text = "Saldo de cuenta: $" + suma.ToString();
        }

        int aclientes = 5;

        private void button11_Click(object sender, EventArgs e)
        {
            if(label2.Text== "Saldo de cuenta: $")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe consultar un cliente";
                m.ShowDialog();
            }
            else
            whatsappEstado();
        }

        private void whatsappEstado()
        {
            String txtCelular = textBox5.Text;
            String txtSaldo = suma.ToString();
            System.Diagnostics.Process.Start("https://web.whatsapp.com/send?phone=54" + txtCelular + "&text=" + "%C2%A1Hola!%20Tu%20Cuenta%20Cliente%20posee%20actualmente%20un%20saldo%20de%20$" + txtSaldo + "%20%F0%9F%A7%BE%20Estamos%20a%20tu%20disposici%C3%B3n%20%F0%9F%A4%9D%20MTA25%20%F0%9F%A5%AA");            
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new Clientes(uno, dos, aclientes));
        }
    }
}
