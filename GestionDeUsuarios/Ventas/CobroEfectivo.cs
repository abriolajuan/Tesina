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
    public partial class CobroEfectivo : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);


        //VARIABLES QUE SE USAN CON WHATSAPP
        String txtCelular = "";
        String totalVenta = "";
        String fechaVenta = "";
        String cobroNuevo = "0";
        float saldoNew = 0;
        String saldoNuevo = "";


        string numeroCelular;
        public CobroEfectivo(string numeroCelu)
        {
            InitializeComponent();
            numeroCelular = numeroCelu;
        }
        private void button2_Click(object sender, EventArgs e)
        {
            Close();
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

        private void CobroEfectivo_Load(object sender, EventArgs e)
        {
            ocultarVentasprevias();
            ocultarNuevocobro();
            if (textBox3.Text != "")
                mostrarVentasprevias();

            textBox1.Visible = false;
            label2.Visible = false;
            textBox5.Visible = false;
            label20.Visible = false;
            label21.Visible = false;

            textBox3.Enabled = false;
            radioButton3.Checked = true;
            cargarComboBox1Cliente();
            textBox2.Enabled = false;
            radioButton1.Checked = true;
            label21.Text = numeroCelular;

            checkBox1.Visible = true; //WHATSAPP
            pictureBox3.Visible = true;
        }

        private void ocultarVentasprevias()
        {
            pictureBox6.Visible = false;
            groupBox2.Visible = false;            
        }

        private void ocultarNuevocobro()
        {
            pictureBox5.Visible = false;
            groupBox1.Visible = false;
        }

        private void mostrarVentasprevias()
        {
            pictureBox6.Visible = true;
            groupBox2.Visible = true;
        }

        private void mostrarNuevocobro()
        {
            pictureBox5.Visible = true;
            groupBox1.Visible = true;
        }

        string uno, dos;
        int aclientes = 1;
        private void button7_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new Clientes(uno,dos, aclientes));
        }


        float monto=0;
        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                mostrarNuevocobro();
                conexion.Open();
                string sql = "SELECT T1.totalventa-t3.descuento as totalventa, T1.NOMBRE_COMPLETO, T1.totalventa-T2.cobros-t3.descuento as saldoacobrar FROM (SELECT (usu.USUARIO_APELLIDO + ', '  +usu.USUARIO_NOMBRE) as NOMBRE_COMPLETO, det.VENTA_ID, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'totalventa' FROM DETALLEVENTA as det, VENTA as ven, usuario as usu WHERE det.VENTA_ID=ven.VENTA_ID and ven.VENTA_ID=@ventaid AND ven.USUARIO_ID=USU.USUARIO_ID  group by det.VENTA_ID, usu.USUARIO_APELLIDO, usu.USUARIO_NOMBRE) T1  LEFT JOIN (SELECT venta.venta_id, coalesce(sum(COBROVENTA.COBRO_VENTA_MONTO), 0) as 'cobros'   FROM VENTA left JOIN COBROVENTA ON VENTA.VENTA_ID = COBROVENTA.VENTA_ID group by VENTA.VENTA_ID) T2 on (T1.VENTA_ID=T2.VENTA_ID) left join (select ven.venta_id, coalesce(ven.VENTA_DTO,0) as 'descuento' from venta as ven, detalleventa as det  where det.venta_id=ven.venta_id and ven.VENTA_ID=@ventaid group by ven.venta_id, ven.VENTA_DTO) t3 ON (t3.VENTA_ID=T2.VENTA_ID)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                while (registros.Read())
                {
                    label6.Text = registros["totalventa"].ToString();
                    label10.Text = registros["NOMBRE_COMPLETO"].ToString();
                    string montoTotal = registros["saldoacobrar"].ToString();
                    monto = float.Parse(montoTotal);
                    label7.Text = registros["saldoacobrar"].ToString();
                }
                registros.Close();
                conexion.Close();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe consultar una venta";
                m.ShowDialog();
            }
        }

        private void radioButton1_Click(object sender, EventArgs e)
        {
            textBox2.Enabled = false;
            textBox2.Text = "";
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            textBox2.Enabled = true;
        }

        DateTime fechaHoy = DateTime.Now;
        private void button1_Click(object sender, EventArgs e) //GUARDAR COBRO
        {
            if (radioButton3.Checked && textBox3.Text=="")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un cliente";
                m.ShowDialog();
            }
            else if(label6.Text == "- - -" && label10.Text == "- - -")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe consultar una venta previa";
                m.ShowDialog();
            }
            else if (radioButton2.Checked && textBox2.Text =="" || textBox2.Text=="0")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un monto parcial, no puede ser igual a 0";
                m.ShowDialog();
            }
            else if (radioButton1.Checked && textBox2.Text == "")
            {
                if (label7.Text == "0")
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se puede ingresar un cobro porque ya está pagado en su totalidad";
                    m.ShowDialog();
                }
                else
                {
                    conexion.Open();
                    string sql = "insert into COBROVENTA (COBRO_VENTA_FECHA, COBRO_VENTA_MONTO, VENTA_ID) values (@ventafecha,@ventamonto,@ventaid)";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@ventamonto", SqlDbType.Float).Value = monto;
                    comando.Parameters.Add("@ventafecha", SqlDbType.DateTime).Value = fechaHoy;
                    comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    guardarDetalleDeMedioEfectivo();
                    actualizarEstadoCobroTotal();
                    if (checkBox1.Checked)
                        whatsappCobro(); //WHATSAPP
                    Aviso m = new Aviso();
                    m.label1.Text = "El cobro total fue registrado.";
                    m.ShowDialog();
                    ocultarVentasprevias();
                    ocultarNuevocobro();
                    textBox3.Text = "";
                    comboBox1.SelectedIndex = -1;
                    label6.Text = "- - -";
                    label10.Text = "- - -";
                    radioButton1.Checked = true;
                    label7.Text = "- - -";
                    radioButton3.Checked = true;
                }
            }
            else if (radioButton2.Checked && textBox2.Text!="")
            {
                float saldoacobrar = 0;
                saldoacobrar = float.Parse(label7.Text);
                float saldoporpagar = 0;
                saldoporpagar = float.Parse(textBox2.Text);
                if (label7.Text == "0")
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se puede ingresar un cobro porque ya está pagado en su totalidad";
                    m.ShowDialog();
                }
                else if (saldoporpagar > saldoacobrar)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "El monto ingresado no puede ser mayor al monto por cobrar";
                    m.ShowDialog();
                }
                else if (saldoporpagar == saldoacobrar)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "El monto ingresado es igual al monto por cobrar, debe seleccionar 'Saldo total'";
                    m.ShowDialog();
                }
                else
                {
                    conexion.Open();
                    string sql = "insert into COBROVENTA (COBRO_VENTA_FECHA, COBRO_VENTA_MONTO, VENTA_ID) values (@ventafecha,@ventamonto,@ventaid)";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@ventamonto", SqlDbType.Float).Value = float.Parse(textBox2.Text);
                    comando.Parameters.Add("@ventafecha", SqlDbType.DateTime).Value = fechaHoy;
                    comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    guardarDetalleDeMedioEfectivo();
                    actualizarEstadoCobroParcial();
                    whatsappCobro(); //WHATSAPP
                    Aviso m = new Aviso();
                    m.label1.Text = "El cobro parcial fue registrado.\nEl saldo actual de esta venta es $" + saldoNuevo + ".";
                    m.ShowDialog();
                    ocultarVentasprevias();
                    ocultarNuevocobro();
                    textBox3.Text = "";
                    textBox2.Text = "";
                    comboBox1.SelectedIndex = -1;
                    label6.Text = "- - -";
                    label10.Text = "- - -";
                    radioButton1.Checked = true;
                    label7.Text = "- - -";
                    radioButton3.Checked = true;
                    checkBox1.Checked = false;
                }
            }
        }


        public void whatsappCobro() //WHATSAPP
        {
            txtCelular = textBox5.Text;
            totalVenta = label6.Text;
            fechaVenta = comboBox1.Text;

            cobroNuevo = "0";
            saldoNew = 0;

            if (radioButton1.Checked)
            {
                cobroNuevo = label7.Text;
                saldoNew = 0;
            }
            else if (radioButton2.Checked)
            {
                cobroNuevo = textBox2.Text;
                saldoNew = (float.Parse(label7.Text)) - (float.Parse(cobroNuevo));
            }            

            saldoNuevo = saldoNew.ToString();

            if(checkBox1.Checked)
                System.Diagnostics.Process.Start("https://web.whatsapp.com/send?phone=54" + txtCelular + "&text=" + "%C2%A1Hola!%20En%20tu%20compra%20por%20$" + totalVenta + "%20del%20" + fechaVenta + "hs,%20hemos%20recibido%20tu%20pago%20en%20efectivo%20por%20$" + cobroNuevo + ",%20quedando%20un%20saldo%20de%20$" + saldoNuevo + "%20%F0%9F%A7%BE%20%C2%A1Muchas%20gracias!%20%F0%9F%98%83%20MTA25%20%F0%9F%A5%AA");
        }


        private void guardarDetalleDeMedioEfectivo()
        {
            int cobroVentaId = identificadorIdCobroVenta();
            conexion.Open();
            string sql = "insert into DETALLEDEMEDIO (MEDIO_TR_ID,COBRO_VENTA_ID) values (@medio,@cobroventa)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cobroventa", SqlDbType.Int).Value = cobroVentaId;
            comando.Parameters.Add("@medio", SqlDbType.Int).Value = "1";
            comando.ExecuteNonQuery();
            conexion.Close();
        }


        public int identificadorIdCobroVenta()
        {
            int idCobroVenta;
            conexion.Open();
            string sql = "select max(COBRO_VENTA_ID) as cobro_venta_id from COBROVENTA where VENTA_ID=@ventaid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@ventaid", SqlDbType.VarChar).Value = comboBox1.SelectedValue;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idVen = registro["COBRO_VENTA_ID"].ToString();
            idCobroVenta = int.Parse(idVen);
            conexion.Close();
            return idCobroVenta;
        }


        private void actualizarEstadoCobroTotal()
        {
            conexion.Open();
            string sql = "update VENTA set EST_VENTA_ID='3' where VENTA_ID=@ventaid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.ExecuteNonQuery();
            conexion.Close();
        }


        private void actualizarEstadoCobroParcial()
        {
            conexion.Open();
            string sql = "update VENTA set EST_VENTA_ID='2' where VENTA_ID=@ventaid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.ExecuteNonQuery();
            conexion.Close();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            cargarComboBox1Anonimo();
            button7.Enabled = false;
            radioButton2.Visible = false;
            textBox2.Visible = false;
            textBox3.Enabled = false;
            textBox3.Text = "";
            comboBox1.SelectedIndex = -1;
            label6.Text = "- - -";
            label10.Text = "- - -";
            radioButton1.Checked = true;
            label7.Text = "- - -";
            textBox2.Text = "";
        }

        public void cargarComboBox1Cliente()
        {
            if (textBox3.Text != "")
            {
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
                conexion.Open();
                string sql = "select t12.venta_fecha, t12.VENTA_ID from(SELECT t1.VENTA_FECHA, t1.VENTA_ID, T1.totalventa-t3.descuento as totalventa, T1.totalventa-T2.cobros-t3.descuento as saldoacobrar FROM (SELECT VENTA_FECHA, ven.VENTA_ID, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'totalventa' FROM DETALLEVENTA as det, VENTA as ven, usuario as usu WHERE det.VENTA_ID=ven.VENTA_ID and ven.USUARIO_ID=USU.USUARIO_ID and CLIENTE_ID=@cliente  group by ven.VENTA_ID, VENTA_FECHA) T1 LEFT JOIN (SELECT venta_fecha, venta.venta_id, coalesce(sum(COBROVENTA.COBRO_VENTA_MONTO), 0) as 'cobros' FROM VENTA left JOIN COBROVENTA ON VENTA.VENTA_ID = COBROVENTA.VENTA_ID where CLIENTE_ID=@cliente group by VENTA.VENTA_ID, VENTA_FECHA) T2 on (T1.VENTA_ID=T2.VENTA_ID) left join (select venta_fecha, ven.venta_id, coalesce(ven.VENTA_DTO,0) as 'descuento' from venta as ven, detalleventa as det where det.venta_id=ven.venta_id and CLIENTE_ID=@cliente  group by ven.venta_id, ven.VENTA_DTO, VENTA_FECHA) t3 ON (t3.VENTA_ID=T2.VENTA_ID))t12 where t12.saldoacobrar>0 ORDER BY T12.VENTA_FECHA DESC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@cliente", SqlDbType.Int).Value = int.Parse(textBox1.Text);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox1.DataSource = tabla1;
                comboBox1.DisplayMember = "VENTA_FECHA";
                comboBox1.ValueMember = "VENTA_ID";
            }
            else if (textBox1.Text == "")
            {
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
            }
        }

        private void radioButton3_Click(object sender, EventArgs e)
        {
            cargarComboBox1Cliente();
            radioButton2.Visible = true;
            textBox2.Visible = true;
            button7.Enabled = true;
            textBox3.Text = "";
            comboBox1.SelectedIndex = -1;
            label6.Text = "- - -";
            label10.Text = "- - -";
            radioButton1.Checked = true;
            label7.Text = "- - -";
            textBox2.Text = "";
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            /*if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }*/
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',' || e.KeyChar == Convert.ToChar(Keys.Back))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void comboBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            label6.Text = "- - -";
            label7.Text = "- - -";
            label10.Text = "- - -";
            textBox2.Text = "";
            radioButton1.Checked = true;
        }

        private void radioButton4_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Visible = false; //WHATSAPP
            pictureBox3.Visible = false;
            mostrarVentasprevias();
            ocultarNuevocobro();
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            checkBox1.Visible = true; //WHATSAPP
            pictureBox3.Visible = true;
        }

        public void cargarComboBox1Anonimo()
        {
            comboBox1.DataSource = null;
            comboBox1.Items.Clear();
            conexion.Open();
            string sql = "select t12.venta_fecha, t12.VENTA_ID from(SELECT t1.VENTA_FECHA, t1.VENTA_ID, T1.totalventa-t3.descuento as totalventa, T1.totalventa-T2.cobros-t3.descuento as saldoacobrar FROM (SELECT VENTA_FECHA, ven.VENTA_ID, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'totalventa' FROM DETALLEVENTA as det, VENTA as ven, usuario as usu WHERE det.VENTA_ID=ven.VENTA_ID and ven.USUARIO_ID=USU.USUARIO_ID and CLIENTE_ID IS NULL  group by ven.VENTA_ID, VENTA_FECHA) T1 LEFT JOIN (SELECT venta_fecha, venta.venta_id, coalesce(sum(COBROVENTA.COBRO_VENTA_MONTO), 0) as 'cobros' FROM VENTA left JOIN COBROVENTA ON VENTA.VENTA_ID = COBROVENTA.VENTA_ID where CLIENTE_ID IS NULL group by VENTA.VENTA_ID, VENTA_FECHA) T2 on (T1.VENTA_ID=T2.VENTA_ID) left join (select venta_fecha, ven.venta_id, coalesce(ven.VENTA_DTO,0) as 'descuento' from venta as ven, detalleventa as det where det.venta_id=ven.venta_id and CLIENTE_ID IS NULL group by ven.venta_id, ven.VENTA_DTO, VENTA_FECHA) t3 ON (t3.VENTA_ID=T2.VENTA_ID))t12 where t12.saldoacobrar>0 ORDER BY T12.VENTA_FECHA DESC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox1.DataSource = tabla1;
            comboBox1.DisplayMember = "VENTA_FECHA";
            comboBox1.ValueMember = "VENTA_ID";
        }
    }
}
