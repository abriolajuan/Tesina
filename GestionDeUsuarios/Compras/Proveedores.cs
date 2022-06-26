using AventStack.ExtentReports.Gherkin.Model;
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
    public partial class Proveedores : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        public Proveedores()
        {
            InitializeComponent();
        }

        private void Proveedores_Load(object sender, EventArgs e)
        {
            cargarComboBox1();
            mostrarGrilla();
            button3.Enabled = false;

            pictureBox2.Visible = false;
            button3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private Form formularioActivo = null;
        private void AbrirpanelHijo(Form formularioHijo)
        {
            if (formularioActivo != null)
                formularioActivo.Close();
            formularioActivo = formularioHijo;
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;
            panelHijo.Controls.Add(formularioHijo);
            panelHijo.Tag = formularioHijo;
            formularioHijo.BringToFront(); // ESTO POR SI PONEMOS UN LOGO DE FONDO ADELANTE VA EL FORMULARIO
            formularioHijo.Show();
        }



        private void button3_Click_1(object sender, EventArgs e)
        {
            AbrirpanelHijo(new CuentaProvee());
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new CuentaGlobal());
        }

        private bool ExisteProveedor(string Proveedor)
        {
            conexion.Open();
            string sql = "select PROVEE_NOMBRE from PROVEEDOR where PROVEE_NOMBRE=@nombre";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = Proveedor;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1 || comboBox3.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ningun campo puede estar vacío";
                m.ShowDialog();
            }
            else if (!ExisteProveedor(textBox1.Text))
            {
                guardarDomicilioProveedor();
                guardarProveedor();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya existe un proveedor con ese nombre";
                m.ShowDialog();
            }
        }

        private void guardarDomicilioProveedor()
        {
            conexion.Open();
            string sql = "insert into DOMICILIO (BARRIO_ID, DOMIC_CALLE_NOMBRE , DOMIC_CALLE_ALTURA) values (@barrioid,@domcalle,@domaltura)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@domcalle", SqlDbType.VarChar).Value = textBox4.Text;
            comando.Parameters.Add("@barrioid", SqlDbType.Int).Value = comboBox3.SelectedValue.ToString();
            comando.Parameters.Add("@domaltura", SqlDbType.Int).Value = textBox5.Text;
            comando.ExecuteNonQuery();
            conexion.Close();
            mostrarGrilla();
        }

        public int identificadorDomicilio()
        {
            int idDomicilio;

            conexion.Open();
            string sql = "select DOMIC_ID from DOMICILIO where domic_calle_nombre=@domicnombre and domic_calle_altura=@domicaltura";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@domicnombre", SqlDbType.VarChar).Value = textBox4.Text;
            comando.Parameters.Add("@domicaltura", SqlDbType.Int).Value = textBox5.Text;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idDomic = registro["DOMIC_ID"].ToString();
            idDomicilio = int.Parse(idDomic);
            conexion.Close();
            return idDomicilio;
        }

        private void guardarProveedor()
        {
            int domId = identificadorDomicilio();
            conexion.Open();
            string sql = "insert into PROVEEDOR (DOMIC_ID, PROVEE_NOMBRE, PROVEE_TEL , PROVEE_CUIT) values (@domid,@provenom,@proveetel,@proveecuit)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@domid", SqlDbType.Int).Value = domId;
            comando.Parameters.Add("@provenom", SqlDbType.VarChar).Value = textBox1.Text;
            comando.Parameters.Add("@proveetel", SqlDbType.VarChar).Value = textBox2.Text;
            comando.Parameters.Add("@proveecuit", SqlDbType.VarChar).Value = textBox3.Text;
            comando.ExecuteNonQuery();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            conexion.Close();
            Aviso m = new Aviso();
            m.label1.Text = "El proveedor fue registrado";
            m.ShowDialog();
            mostrarGrilla();
        }

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select PROVEE_NOMBRE,PROVEE_TEL,PROVEE_CUIT, PROVINCIA_NOMBRE, LOCALIDAD_NOMBRE,BARRIO_NOMBRE, DOMIC_CALLE_NOMBRE, DOMIC_CALLE_ALTURA from PROVEEDOR as prov join DOMICILIO as dom on dom.DOMIC_ID = prov.DOMIC_ID join BARRIO as bar on bar.BARRIO_ID = dom.BARRIO_ID  join LOCALIDAD as loc on loc.LOCALIDAD_ID = bar.LOCALIDAD_ID join PROVINCIA as provinc on provinc.PROVINCIA_ID= loc.PROVINCIA_ID ORDER BY PROVEE_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PROVEE_NOMBRE"].ToString(),
                 registros["PROVEE_TEL"].ToString(), registros["PROVEE_CUIT"].ToString(),
                 registros["PROVINCIA_NOMBRE"].ToString(), registros["LOCALIDAD_NOMBRE"].ToString(),
                 registros["BARRIO_NOMBRE"].ToString(), registros["DOMIC_CALLE_NOMBRE"].ToString(),
                 registros["DOMIC_CALLE_ALTURA"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void cargarComboBox1()
        {
            conexion.Open();
            string sql = "select PROVINCIA_ID, PROVINCIA_NOMBRE from PROVINCIA ORDER BY PROVINCIA_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox1.DisplayMember = "PROVINCIA_NOMBRE";
            comboBox1.ValueMember = "PROVINCIA_ID";
            comboBox1.DataSource = tabla1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarComboBox2();
        }

        private void cargarComboBox2()
        {
            conexion.Open();
            string sql = "select LOCALIDAD_ID, LOCALIDAD_NOMBRE from LOCALIDAD where PROVINCIA_ID=@provinciaid ORDER BY LOCALIDAD_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@provinciaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox2.DisplayMember = "LOCALIDAD_NOMBRE";
            comboBox2.ValueMember = "LOCALIDAD_ID";
            comboBox2.DataSource = tabla1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarComboBox3();
        }

        private void cargarComboBox3()
        {
            conexion.Open();
            string sql = "select BARRIO_ID, BARRIO_NOMBRE from BARRIO where LOCALIDAD_ID=@localidadid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@localidadid", SqlDbType.Int).Value = comboBox2.SelectedValue;
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox3.DisplayMember = "BARRIO_NOMBRE";
            comboBox3.ValueMember = "BARRIO_ID";
            comboBox3.DataSource = tabla1;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            guardarProveedor();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click_3(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar algún nombre de proveedor";
                m.ShowDialog();
            }
            else if (ExisteProveedor(textBox1.Text))
            {
                button3.Enabled = true;
                textBox1.Enabled = false;
                button2.Enabled = false;
                button1.Enabled = false;

                pictureBox2.Visible = true;
                button3.Visible = true;
                pictureBox1.Visible = false;
                button2.Visible = false;

                conexion.Open();
                string sql = "SELECT PROVEE_NOMBRE, PROVEE_TEL, PROVEE_CUIT , BARRIO_NOMBRE, DOMIC_CALLE_NOMBRE, DOMIC_CALLE_ALTURA, LOCALIDAD_NOMBRE, PROVINCIA_NOMBRE from PROVEEDOR p, DOMICILIO d, LOCALIDAD l, BARRIO b, PROVINCIA pro WHERE p.DOMIC_ID= d.DOMIC_ID and b.LOCALIDAD_ID=l.LOCALIDAD_ID and d.BARRIO_ID=b.BARRIO_ID and pro.PROVINCIA_ID=l.PROVINCIA_ID and PROVEE_NOMBRE=@nombre";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = textBox1.Text;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                if (registros.Read())
                {
                dataGridView1.Rows.Add(registros["PROVEE_NOMBRE"].ToString(),
                registros["PROVEE_TEL"].ToString(), registros["PROVEE_CUIT"].ToString(),
                registros["PROVINCIA_NOMBRE"].ToString(), registros["LOCALIDAD_NOMBRE"].ToString(),
                registros["BARRIO_NOMBRE"].ToString(), registros["DOMIC_CALLE_NOMBRE"].ToString(),
                registros["DOMIC_CALLE_ALTURA"].ToString());

                textBox2.Text = registros["PROVEE_TEL"].ToString();
                textBox3.Text = registros["PROVEE_CUIT"].ToString();
                textBox4.Text = registros["DOMIC_CALLE_NOMBRE"].ToString();
                textBox5.Text = registros["DOMIC_CALLE_ALTURA"].ToString();
                }

                registros.Close();
                conexion.Close();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "No existe un proveedor con ese nombre";
                m.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para modificar un proovedor tienen que estar todos los campos cargados";
                m.ShowDialog();
            }
            else
            {
                button1.Enabled = true;
                button3.Enabled = false;
                modificarDomicilioProveedor();
                modificarProveedor();

                pictureBox2.Visible = false;
                button3.Visible = false;
                pictureBox1.Visible = true;
                button2.Visible = true;
            }
        }

        private void modificarDomicilioProveedor()
        {
            int domId = identificadorDomicilioModificar();
            conexion.Open();
            string sql = "update DOMICILIO set DOMIC_CALLE_NOMBRE=@domcalle, DOMIC_CALLE_ALTURA=@domaltura, BARRIO_ID=@barrioid  where DOMIC_ID=@domid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@domcalle", SqlDbType.VarChar).Value = textBox4.Text;
            comando.Parameters.Add("@barrioid", SqlDbType.Int).Value = comboBox3.SelectedValue.ToString();
            comando.Parameters.Add("@domaltura", SqlDbType.Int).Value = textBox5.Text;
            comando.Parameters.Add("@domid", SqlDbType.Int).Value = domId;
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        private void modificarProveedor()
        {
            int provId = identificadorIdProveedor();
            int domId = identificadorDomicilio();
            conexion.Open();
            string sql = "update PROVEEDOR set  PROVEE_TEL=@proveetel, PROVEE_CUIT= @proveecuit where PROVEE_ID=@proveid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@proveid", SqlDbType.Int).Value = provId;
            comando.Parameters.Add("@proveetel", SqlDbType.VarChar).Value = textBox2.Text;
            comando.Parameters.Add("@proveecuit", SqlDbType.VarChar).Value = textBox3.Text;
            comando.ExecuteNonQuery();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            conexion.Close();
            Aviso m = new Aviso();
            m.label1.Text = "El proveedor se modificó correctamente";
            m.ShowDialog();
            mostrarGrilla();
            button2.Enabled = true;
            textBox1.Enabled = true;
        }

        public int identificadorDomicilioModificar()
        {
            int idDomicilio;
            conexion.Open();
            string sql = "select DOMIC_ID from PROVEEDOR where PROVEE_NOMBRE=@proveenombre";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@proveenombre", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idDomic = registro["DOMIC_ID"].ToString();
            idDomicilio = int.Parse(idDomic);
            conexion.Close();
            return idDomicilio;
        }

        public int identificadorIdProveedor()
        {
            int idProvee;
            conexion.Open();
            string sql = "SELECT PROVEE_ID FROM PROVEEDOR WHERE PROVEE_NOMBRE=@proveenombre";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@proveenombre", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idDomic = registro["PROVEE_ID"].ToString();
            idProvee = int.Parse(idDomic);
            conexion.Close();
            return idProvee;
        }

        private void panelHijo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
