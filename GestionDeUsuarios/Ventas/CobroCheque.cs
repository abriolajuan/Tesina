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
    public partial class CobroCheque : Form
    {

        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public CobroCheque()
        {
            InitializeComponent();
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

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void CobroCheque_Load(object sender, EventArgs e)
        {
            radioButton3.Checked = true;
            textBox1.Enabled = false;
            radioButton1.Checked = true;
            cargarComboBox1Cliente();
            textBox7.Enabled = false;
            textBox2.Visible = false;
            label16.Visible = false;
            cargarComboBox2();
        }


        public void cargarComboBox1Cliente()
        {
            if (textBox1.Text != "")
            {
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
                conexion.Open();
                string sql = "select VENTA_FECHA, VENTA_ID from VENTA where CLIENTE_ID=@cliente ORDER BY VENTA_FECHA DESC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@cliente", SqlDbType.Int).Value = int.Parse(textBox2.Text);
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

        private void radioButton3_Click(object sender, EventArgs e)
        {
            cargarComboBox1Cliente();
            radioButton2.Visible = true;
            textBox7.Visible = true;
            button7.Enabled = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            comboBox2.SelectedIndex = -1;
            textBox7.Enabled = false;
            label2.Text = "- - -";
            label10.Text = "- - -";
            label14.Text = "- - -";
            dateTimePicker2.Value = DateTime.Now;
        }

        private void radioButton4_Click(object sender, EventArgs e)
        {
            cargarComboBox1Anonimo();
            button7.Enabled = false;
            radioButton2.Visible = false;
            textBox7.Visible = false;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox7.Text = "";
            comboBox2.SelectedIndex = -1;
            textBox7.Enabled = false;
            label2.Text = "- - -";
            label10.Text = "- - -";
            label14.Text = "- - -";
            dateTimePicker2.Value = DateTime.Now;
        }


        public void cargarComboBox1Anonimo()
        {
            comboBox1.DataSource = null;
            comboBox1.Items.Clear();
            conexion.Open();
            string sql = "select VENTA_FECHA, VENTA_ID from VENTA where CLIENTE_ID IS NULL ORDER BY VENTA_FECHA DESC";
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

        float monto = 0;

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                conexion.Open();
                string sql = "SELECT T1.totalventa-t3.descuento as totalventa, T1.NOMBRE_COMPLETO, T1.totalventa-T2.cobros-t3.descuento as saldoacobrar FROM (SELECT (usu.USUARIO_APELLIDO + ', '  +usu.USUARIO_NOMBRE) as NOMBRE_COMPLETO, det.VENTA_ID, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'totalventa'  FROM DETALLEVENTA as det, VENTA as ven, usuario as usu WHERE det.VENTA_ID=ven.VENTA_ID and ven.VENTA_ID=@ventaid AND ven.USUARIO_ID=USU.USUARIO_ID group by det.VENTA_ID, usu.USUARIO_APELLIDO, usu.USUARIO_NOMBRE) T1 LEFT JOIN (SELECT venta.venta_id, coalesce(sum(COBROVENTA.COBRO_VENTA_MONTO), 0) as 'cobros'  FROM VENTA left JOIN COBROVENTA ON VENTA.VENTA_ID = COBROVENTA.VENTA_ID group by VENTA.VENTA_ID) T2 on (T1.VENTA_ID=T2.VENTA_ID) left join (select ven.venta_id, coalesce(ven.VENTA_DTO,0) as 'descuento' from venta as ven, detalleventa as det  where det.venta_id=ven.venta_id and ven.VENTA_ID=@ventaid group by ven.venta_id, ven.VENTA_DTO) t3 ON (t3.VENTA_ID=T2.VENTA_ID)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                while (registros.Read())
                {
                    label2.Text = registros["totalventa"].ToString();
                    label10.Text = registros["NOMBRE_COMPLETO"].ToString();
                    string montoTotal = registros["saldoacobrar"].ToString();
                    monto = float.Parse(montoTotal);
                    label14.Text = registros["saldoacobrar"].ToString();
                }
                registros.Close();
                conexion.Close();
            }
            else 
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe consultar un cliente";
                m.ShowDialog();
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (radioButton3.Checked && textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un cliente";
                m.ShowDialog();
            }
            else if (label2.Text == "- - -" && label10.Text == "- - -")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe consultar una venta previa";
                m.ShowDialog();
            }
            else if (radioButton2.Checked && textBox7.Text == "" || textBox7.Text == "0")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un monto parcial, no puede ser igual a 0";
                m.ShowDialog();
            }
            else if (comboBox2.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar una entidad destino";
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
                m.label1.Text = "Debe ingresar un N° de cheque";
                m.ShowDialog();
            }
            else if (radioButton1.Checked)
            {
                if (label14.Text == "0")
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se puede ingresar un cobro porque ya está pagado en su totalidad";
                    m.ShowDialog();
                }
                else
                {
                    cobroTotal();
                    guardarDetalleDeMedio();
                    actualizarEstadoCobroTotal();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox7.Text = "";
                    comboBox2.SelectedIndex = -1;
                    radioButton3.Checked = true;
                    radioButton1.Checked = true;
                    textBox7.Enabled = false;
                    comboBox1.DataSource = null;
                    comboBox1.Items.Clear();
                    label2.Text = "- - -";
                    label10.Text = "- - -";
                    label14.Text = "- - -";
                    dateTimePicker2.Value = DateTime.Now;
                }
            }
            else if (radioButton2.Checked && textBox7.Text != "")
            {
                float saldoacobrar = 0;
                saldoacobrar = float.Parse(label14.Text);
                float saldoporpagar = 0;
                saldoporpagar = float.Parse(textBox7.Text);
                if (label14.Text == "0")
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
                    cobroParcial();
                    guardarDetalleDeMedio();
                    actualizarEstadoCobroParcial();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox7.Text = "";
                    comboBox2.SelectedIndex = -1;
                    radioButton3.Checked = true;
                    radioButton1.Checked = true;
                    textBox7.Enabled = false;
                    comboBox1.DataSource = null;
                    comboBox1.Items.Clear();
                    label2.Text = "- - -";
                    label10.Text = "- - -";
                    label14.Text = "- - -";
                    dateTimePicker2.Value = DateTime.Now;
                }

            }

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
        private void cobroParcial()
        {
            conexion.Open();
            string sql = "insert into COBROVENTA (COBRO_VENTA_FECHA, COBRO_VENTA_MONTO, VENTA_ID) values (@ventafecha,@ventamonto,@ventaid)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@ventamonto", SqlDbType.Float).Value = float.Parse(textBox7.Text);
            comando.Parameters.Add("@ventafecha", SqlDbType.DateTime).Value = fechaHoy;
            comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.ExecuteNonQuery();
            conexion.Close();
            Aviso m = new Aviso();
            m.label1.Text = "El cobro parcial fue registrado";
            m.ShowDialog();
        }

        DateTime fechaHoy = DateTime.Now;
        private void cobroTotal()
        {
            conexion.Open();
            string sql = "insert into COBROVENTA (COBRO_VENTA_FECHA, COBRO_VENTA_MONTO, VENTA_ID) values (@ventafecha,@ventamonto,@ventaid)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@ventamonto", SqlDbType.Float).Value = monto;
            comando.Parameters.Add("@ventafecha", SqlDbType.DateTime).Value = fechaHoy;
            comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.ExecuteNonQuery();
            conexion.Close();
            Aviso m = new Aviso();
            m.label1.Text = "El cobro total fue registrado";
            m.ShowDialog();
        }

        private void guardarDetalleDeMedio()
        {
            int cobroVentaId = identificadorIdCobroVenta();
            conexion.Open();
            string sql = "insert into DETALLEDEMEDIO(MEDIO_TR_ID, ENTIDAD_ID, DETMEDIO_APELLIDOTIT, DETMEDIO_NOMBRETIT, DETMEDIO_NUMCHEQUE, DETMEDIO_FECHACHEQUE, COBRO_VENTA_ID) values(@medio, @entidad, @apellidotitular, @nombretitular, @numcheque, @fechacheque, @cobroventa)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cobroventa", SqlDbType.Int).Value = cobroVentaId;
            comando.Parameters.Add("@medio", SqlDbType.Int).Value = "5";
            comando.Parameters.Add("@entidad", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@apellidotitular", SqlDbType.VarChar).Value = textBox4.Text;
            comando.Parameters.Add("@nombretitular", SqlDbType.VarChar).Value = textBox3.Text;
            comando.Parameters.Add("@numcheque", SqlDbType.VarChar).Value = textBox5.Text;
            comando.Parameters.Add("@fechacheque", SqlDbType.Date).Value = dateTimePicker2.Value;
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

        private void radioButton1_Click(object sender, EventArgs e)
        {
            textBox7.Text = "";
            textBox7.Enabled = false;
        }

        private void radioButton2_Click(object sender, EventArgs e)
        {
            textBox7.Enabled = true;
        }

        string uno, dos;
        int aclientes = 4;

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
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

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            /* if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
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

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new Clientes(uno, dos, aclientes));
        }
    }
}
