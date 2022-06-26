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
    public partial class PagoEfectivo : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        public PagoEfectivo()
        {
            InitializeComponent();
        }

        private void PagoEfectivo_Load(object sender, EventArgs e)
        {
            ocultarDetalles();
        }

        private void ocultarDetalles()
        {
            label13.Visible = false;
            label4.Visible = false;
            groupBox1.Visible = false;
            pictureBox2.Visible = false;
            groupBox2.Visible = false;
        }

        private void mostrarDetalles()
        {
            label13.Visible = true;
            label4.Visible = true;
            groupBox1.Visible = true;
            pictureBox2.Visible = true;
            groupBox2.Visible = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
                    if (textBox1.Text == "")
                    {
                     Aviso m = new Aviso();
                     m.label1.Text = "Debe ingresar un número de factura";
                     m.ShowDialog();
                    }
                    else if (!ExisteNumFactura(textBox1.Text))
                    {
                    Aviso m = new Aviso();
                    m.label1.Text = "No existe una factura con ese número";
                    m.ShowDialog();
                    }
                    else if (label10.Text == "- - -" && label12.Text == "- - -")
                    {
                     Aviso m = new Aviso();
                     m.label1.Text = "Debe consultar el monto a pagar de una factura existente";
                     m.ShowDialog();
                    }
                    else if (textBox2.Text == "")
                    {Aviso m = new Aviso();
                    m.label1.Text = "Debe ingresar un monto de pago";
                    m.ShowDialog();
                    }
        else
        {
                    float saldoapagar = 0;
                    saldoapagar = float.Parse(label10.Text);
                    float montoapagar = 0;
                    montoapagar = float.Parse(textBox2.Text);
                    if (montoapagar == 0)
                    {
                        Aviso m = new Aviso();
                        m.label1.Text = "El monto de pago debe ser mayor a 0";
                        m.ShowDialog();
                    }
                    else if (montoapagar > saldoapagar)
                    {
                        Aviso m = new Aviso();
                        m.label1.Text = "El monto de pago no puede ser mayor al monto por pagar";
                        m.ShowDialog();
                    }
                    else if (montoapagar == saldoapagar)
                    {
                    textBox1.Enabled = true;
                    guardarPagoCompra();
                    GuardarDetalleMedio();
                    actualizarEstadoPagoTotal();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    dateTimePicker1.Value = DateTime.Now;
                    label10.Text = "- - -";
                    label12.Text = "- - -";
                    Aviso m = new Aviso();
                    m.label1.Text = "El pago total fue registrado";
                    m.ShowDialog();
                    ocultarDetalles();
                    }
                    else if (montoapagar < saldoapagar)
                {
                    textBox1.Enabled = true;
                    guardarPagoCompra();
                    GuardarDetalleMedio();
                    actualizarEstadoPagoParcial();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    dateTimePicker1.Value = DateTime.Now;
                    label10.Text = "- - -";
                    label12.Text = "- - -";
                    Aviso m = new Aviso();
                    m.label1.Text = "El pago parcial fue registrado";
                    m.ShowDialog();
                    ocultarDetalles();
                }            
        }
      }

        private void actualizarEstadoPagoTotal()
        {
            int compraId = identificadorIdCompra();
            conexion.Open();
            string sql = "update COMPRA set EST_COMPRA_ID='3' where COMPRA_ID=@compraid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@compraid", SqlDbType.Int).Value = compraId;
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        private void actualizarEstadoPagoParcial()
        {
            int compraId = identificadorIdCompra();
            conexion.Open();
            string sql = "update COMPRA set EST_COMPRA_ID='2' where COMPRA_ID=@compraid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@compraid", SqlDbType.Int).Value = compraId;
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        private bool ExisteNumFactura(string numerofactura)
        {
            conexion.Open();
            string sql = "select COMPRA_NUM_FACT from COMPRA where COMPRA_NUM_FACT=@numerofact";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@numerofact", SqlDbType.VarChar).Value = numerofactura;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        public int identificadorIdCompra()
        {
            int idCompra;
            conexion.Open();
            string sql = "select COMPRA_ID from COMPRA where compra_num_fact=@numfact";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@numfact", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idCom = registro["COMPRA_ID"].ToString();
            idCompra = int.Parse(idCom);
            conexion.Close();
            return idCompra;
        }

        private void guardarPagoCompra()
        {
            int compraId = identificadorIdCompra();
            conexion.Open();
            string sql = "insert into PAGOCOMPRA (COMPRA_ID,PAGO_COMPRA_FECHA,PAGO_COMPRA_MONTO) values (@compraid,@comprafecha,@compramonto)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@compraid", SqlDbType.Int).Value = compraId;
            comando.Parameters.Add("@comprafecha", SqlDbType.Date).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@compramonto", SqlDbType.Float).Value = textBox2.Text;
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        private void GuardarDetalleMedio()
        {
            int pagoCompraId = identificadorIdPagoCompra();
            conexion.Open();
            string sql = "insert into DETALLEDEMEDIO (MEDIO_TR_ID, PAGO_COMPRA_ID) values (@medio, @pagocompra)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@medio", SqlDbType.Int).Value = "1";
            comando.Parameters.Add("@pagocompra", SqlDbType.Int).Value = pagoCompraId;
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        public int identificadorIdPagoCompra()
        {
            int compraId = identificadorIdCompra();
            int idPagoCompra;
            conexion.Open();
            string sql = "select PAGO_COMPRA_ID from PAGOCOMPRA where COMPRA_ID=@compraid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@compraid", SqlDbType.VarChar).Value = compraId;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idCom = registro["PAGO_COMPRA_ID"].ToString();
            idPagoCompra = int.Parse(idCom);
            conexion.Close();
            return idPagoCompra;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un número de factura";
                m.ShowDialog();
            }
            else if (!ExisteNumFactura(textBox1.Text))
            {
                Aviso m = new Aviso();
                m.label1.Text = "No existe una factura con ese número";
                m.ShowDialog();
            }
            else
            {
                mostrarDetalles();

                textBox1.Enabled = false;
                int compraId = identificadorIdCompra();
                conexion.Open();
                string sql = "SELECT T1.totalcompra as totalcompra, T1.totalcompra-T2.pagos as saldoapagar FROM (SELECT det.COMPRA_ID, SUM(DET_COMPRA_CANTIDAD*DET_COMPRA_PR_UNIT) as 'totalcompra'  FROM DETALLECOMPRA as det, COMPRA as comp WHERE det.COMPRA_ID=comp.COMPRA_ID and comp.COMPRA_ID=@compraid group by det.COMPRA_ID) T1 LEFT JOIN (SELECT compra.COMPRA_ID, coalesce(sum(PAGOCOMPRA.PAGO_COMPRA_MONTO), 0) as 'pagos'  FROM COMPRA left JOIN PAGOCOMPRA ON COMPRA.COMPRA_ID = PAGOCOMPRA.COMPRA_ID group by COMPRA.COMPRA_ID) T2 on (T1.COMPRA_ID=T2.COMPRA_ID)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@compraid", SqlDbType.Int).Value = compraId;
                SqlDataReader registros = comando.ExecuteReader();
                while (registros.Read())
                {
                    label12.Text = registros["totalcompra"].ToString();
                    label10.Text = registros["saldoapagar"].ToString();
                }
                registros.Close();
                conexion.Close();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }
    }
}
