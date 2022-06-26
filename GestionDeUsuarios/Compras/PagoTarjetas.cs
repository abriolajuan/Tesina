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
    public partial class PagoTarjetas : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        public PagoTarjetas()
        {
            InitializeComponent();
        }

        private void cargarComboBox2()
        {
            conexion.Open();
            string sql = "select ENTIDAD_ID, ENTIDAD_NOMBRE from ENTIDADCREDITICIA ORDER BY ENTIDAD_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox2.DisplayMember = "ENTIDAD_NOMBRE";
            comboBox2.ValueMember = "ENTIDAD_ID";
            comboBox2.DataSource = tabla1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void PagoTarjetas_Load(object sender, EventArgs e)
        {
            cargarComboBox2();
            pictureBox5.Visible = false;

            ocultarDetalles();
        }

        private void ocultarDetalles()
        {
            label13.Visible = false;
            label15.Visible = false;
            groupBox1.Visible = false;
            pictureBox3.Visible = false;
            groupBox2.Visible = false;
        }

        private void mostrarDetalles()
        {
            label13.Visible = true;
            label15.Visible = true;
            groupBox1.Visible = true;
            pictureBox3.Visible = true;
            groupBox2.Visible = true;
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
            else if (label12.Text == "- - -" && label14.Text == "- - -")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe consultar el monto a pagar de una factura existente";
                m.ShowDialog();
            }
            else if (textBox2.Text == "")
                {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un monto de pago";
                m.ShowDialog();
                }
                else if (comboBox1.SelectedIndex == -1)
                {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un tipo de tarjeta";
                m.ShowDialog();
                }
                else if (comboBox2.SelectedIndex == -1)
                {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar una entidad crediticia";
                m.ShowDialog();
                }
                else if (textBox3.Text == "")
                {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un nombre de titular";
                m.ShowDialog();
                }
                else if (textBox4.Text == "")
                {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un apellido de titular";
                m.ShowDialog();
                }
                else if (textBox5.Text == "")
                {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un DNI de titular";
                m.ShowDialog();
                }
                else if (textBox6.Text == "")
                {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar los últimos 4 dígitos de tarjeta";
                m.ShowDialog();
                }
                else if (textBox6.TextLength<4)
                {
                Aviso m = new Aviso();
                m.label1.Text = "Verifique la cantidad de dígitos ingresados, deben ser 4";
                m.ShowDialog();
                }
            else if (comboBox1.Text == "Tarjeta de Débito")
            {
                float saldoapagar = 0;
                saldoapagar = float.Parse(label14.Text);
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
                    guardarDetalleDeMedioDebito();
                    actualizarEstadoPagoTotal();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    dateTimePicker1.Value = DateTime.Now;
                    dateTimePicker2.Value = DateTime.Now;
                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;
                    label14.Text = "- - -";
                    label12.Text = "- - -";
                    Aviso m = new Aviso();
                    m.label1.Text = "El Pago total fue registrado";
                    m.ShowDialog();
                    ocultarDetalles();
                }
                else if (montoapagar < saldoapagar)
                {
                    textBox1.Enabled = true;
                    guardarPagoCompra();
                    guardarDetalleDeMedioDebito();
                    actualizarEstadoPagoParcial();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    dateTimePicker1.Value = DateTime.Now;
                    dateTimePicker2.Value = DateTime.Now;
                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;
                    label14.Text = "- - -";
                    label12.Text = "- - -";
                    Aviso m = new Aviso();
                    m.label1.Text = "El Pago parcial fue registrado";
                    m.ShowDialog();
                    ocultarDetalles();
                }
            }
            else if (comboBox1.Text == "Tarjeta de Crédito")
            {
                float saldoapagar = 0;
                saldoapagar = float.Parse(label14.Text);
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
                    guardarDetalleDeMedioCredito();
                    actualizarEstadoPagoTotal();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    dateTimePicker1.Value = DateTime.Now;
                    dateTimePicker2.Value = DateTime.Now;
                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;
                    label14.Text = "- - -";
                    label12.Text = "- - -";
                    Aviso m = new Aviso();
                    m.label1.Text = "El Pago total fue registrado";
                    m.ShowDialog();
                    ocultarDetalles();
                }
                else if (montoapagar < saldoapagar)
                {

                    textBox1.Enabled = true;
                    guardarPagoCompra();
                    guardarDetalleDeMedioCredito();
                    actualizarEstadoPagoParcial();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    dateTimePicker1.Value = DateTime.Now;
                    dateTimePicker2.Value = DateTime.Now;
                    comboBox1.SelectedIndex = -1;
                    comboBox2.SelectedIndex = -1;
                    label14.Text = "- - -";
                    label12.Text = "- - -";
                    Aviso m = new Aviso();
                    m.label1.Text = "El Pago parcial fue registrado";
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

        private void guardarDetalleDeMedioDebito()
        {
            int pagoCompraId = identificadorIdPagoCompra();
            conexion.Open();
            string sql = "insert into DETALLEDEMEDIO (MEDIO_TR_ID, ENTIDAD_ID, DETMEDIO_APELLIDOTIT, DETMEDIO_NOMBRETIT, DETMEDIO_DNITIT, DETMEDIO_FECHAVENC, DETMEDIO_ULTIMOSDIG, PAGO_COMPRA_ID) values (@medio,@entidad,@apellidotitular,@nombretitular,@dnititular, @fechanac, @ultimosdig, @pagocompra)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@pagocompra", SqlDbType.Int).Value = pagoCompraId;
            comando.Parameters.Add("@medio", SqlDbType.Int).Value = "3";
            comando.Parameters.Add("@entidad", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@apellidotitular", SqlDbType.VarChar).Value = textBox4.Text;
            comando.Parameters.Add("@nombretitular", SqlDbType.VarChar).Value = textBox3.Text;
            comando.Parameters.Add("@dnititular", SqlDbType.VarChar).Value = textBox5.Text;
            comando.Parameters.Add("@fechanac", SqlDbType.Date).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@ultimosdig", SqlDbType.VarChar).Value = textBox6.Text;
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        private void guardarDetalleDeMedioCredito()
        {
            int pagoCompraId = identificadorIdPagoCompra();
            conexion.Open();
            string sql = "insert into DETALLEDEMEDIO (MEDIO_TR_ID, ENTIDAD_ID, DETMEDIO_APELLIDOTIT, DETMEDIO_NOMBRETIT, DETMEDIO_DNITIT, DETMEDIO_FECHAVENC, DETMEDIO_ULTIMOSDIG, PAGO_COMPRA_ID) values (@medio,@entidad,@apellidotitular,@nombretitular,@dnititular, @fechanac, @ultimosdig, @pagocompra)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@pagocompra", SqlDbType.Int).Value = pagoCompraId;
            comando.Parameters.Add("@medio", SqlDbType.Int).Value = "2";
            comando.Parameters.Add("@entidad", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@apellidotitular", SqlDbType.VarChar).Value = textBox4.Text;
            comando.Parameters.Add("@nombretitular", SqlDbType.VarChar).Value = textBox3.Text;
            comando.Parameters.Add("@dnititular", SqlDbType.VarChar).Value = textBox5.Text;
            comando.Parameters.Add("@fechanac", SqlDbType.Date).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@ultimosdig", SqlDbType.VarChar).Value = textBox6.Text;
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
                    label14.Text = registros["saldoapagar"].ToString();
                }
                registros.Close();
                conexion.Close();
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            pictureBox5.Visible = true;
        }

        private void textBox6_Click(object sender, EventArgs e)
        {
            pictureBox5.Visible = true;
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

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }
    }
}
