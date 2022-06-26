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
    public partial class Sesiones : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public Sesiones()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void MostrarGrilla()
        {
            conexion.Open();
            string sql = "select USUARIO_APELLIDO, USUARIO_NOMBRE, TIPO_USU_NOMBRE, SESION_FECHA, SESION_EVENTO from SESION as ses join USUARIO as usu on usu.USUARIO_ID = SES.USUARIO_ID join TIPOUSUARIO as tipousu on tipousu.TIPO_USU_ID = usu.TIPO_USU_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(
                    registros["USUARIO_APELLIDO"].ToString(),
                registros["USUARIO_NOMBRE"].ToString(),
                registros["TIPO_USU_NOMBRE"].ToString(),
                registros["SESION_FECHA"].ToString(),
                registros["SESION_EVENTO"].ToString()
                );
            }
            registros.Close();
            conexion.Close();
        }

        private void Sesiones_Load(object sender, EventArgs e)
        {
            MostrarGrilla();
            cargarComboBox1();
            cargarComboBox2();            
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox1.Text = "Seleccione una opción";
            comboBox2.Text = "Seleccione una opción";
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePicker2.Format = DateTimePickerFormat.Custom;
            dateTimePicker2.CustomFormat = "dd/MM/yyyy HH:mm";
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
            button3.Enabled = true;


        }


        public void cargarComboBox1()
        {
            conexion.Open();

            string sql = "select USUARIO_ID, USUARIO_APELLIDO, USUARIO_NOMBRE, CONCAT(USUARIO_APELLIDO, ', ', USUARIO_NOMBRE) full_name from USUARIO ORDER BY full_name ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox1.DisplayMember = "full_name";
            comboBox1.ValueMember = "USUARIO_ID";
            comboBox1.DataSource = tabla1;
            comboBox1.SelectedIndex = (-1);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            
            MostrarGrilla();
            
            if (comboBox1.SelectedIndex == (-1))
            {
                

            }
            else
            {
                comboBox2.Enabled = false;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                button3.Enabled = false;
                conexion.Open();
                string sql = "select USUARIO_APELLIDO, USUARIO_NOMBRE, TIPO_USU_NOMBRE, SESION_FECHA, SESION_EVENTO from SESION as ses join USUARIO as usu on usu.USUARIO_ID = SES.USUARIO_ID join TIPOUSUARIO as tipousu on tipousu.TIPO_USU_ID = usu.TIPO_USU_ID where usu.USUARIO_ID=@usuIdClickeado";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@usuIdClickeado", SqlDbType.Int).Value = int.Parse(comboBox1.SelectedValue.ToString());
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(
                        registros["USUARIO_APELLIDO"].ToString(),
                    registros["USUARIO_NOMBRE"].ToString(),
                    registros["TIPO_USU_NOMBRE"].ToString(),
                    registros["SESION_FECHA"].ToString(),
                    registros["SESION_EVENTO"].ToString()
                    );
                }
                registros.Close();
                conexion.Close();
                
            }
        }
        public void cargarComboBox2()
        {
                conexion.Open();
                /*string sql = "SELECT USUARIO_ID, TIPO_USU_ID, TIPO_USU_NOMBRE FROM USUARIO AS USU JOIN TIPOUSUARIO AS TIPOUSU ON TIPOUSU.TIPO_USU_ID= USU.TIPO_USU_ID ORDER BY TIPO_USU_NOMBRE ASC";*/
                string sql = "Select tipo_usu_id, tipo_usu_nombre from tipousuario";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox2.DisplayMember = "TIPO_USU_NOMBRE";
                comboBox2.ValueMember = "TIPO_USU_ID";
                comboBox2.DataSource = tabla1;
                comboBox2.SelectedIndex = (-1);

            
        }



        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            MostrarGrilla();
           
            if (comboBox2.SelectedIndex == (-1))
            {
             
            }
            else
            {
                comboBox1.Enabled = false;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                button3.Enabled = false;
                conexion.Open();
                string sql = "select USUARIO_APELLIDO, USUARIO_NOMBRE, TIPO_USU_NOMBRE, SESION_FECHA, SESION_EVENTO from SESION as ses join USUARIO as usu on usu.USUARIO_ID = SES.USUARIO_ID join TIPOUSUARIO as tipousu on tipousu.TIPO_USU_ID = usu.TIPO_USU_ID where usu.TIPO_USU_ID=@TipousuIdClickeado";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@TipousuIdClickeado", SqlDbType.Int).Value = int.Parse(comboBox2.SelectedValue.ToString());
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(
                        registros["USUARIO_APELLIDO"].ToString(),
                    registros["USUARIO_NOMBRE"].ToString(),
                    registros["TIPO_USU_NOMBRE"].ToString(),
                    registros["SESION_FECHA"].ToString(),
                    registros["SESION_EVENTO"].ToString()
                    );
                }
                registros.Close();
                conexion.Close();
            }
            

        }


        private void button2_Click(object sender, EventArgs e) //quitar filtros
        {
            MostrarGrilla();
            radioButton1.Checked = false;
            if (radioButton1.Checked == false) 
            {
                MostrarGrilla();
            }
            radioButton2.Checked = false;  
            if (radioButton2.Checked == false)
            {
                MostrarGrilla();
            }
            comboBox1.SelectedIndex = (-1);
            comboBox2.SelectedIndex = (-1);
            comboBox1.Text = "Seleccione una opción";
            comboBox2.Text = "Seleccione una opción";
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            dateTimePicker1.Text = DateTime.Now.ToString();
            dateTimePicker2.Text = DateTime.Now.ToString();
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
            button3.Enabled = true;
        }


        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex < 0)
            {
                comboBox1.Text = "Seleccione una opción";
            }
            else
            {
                comboBox1.Text = comboBox1.SelectedText;
            }
        }
        private void comboBox2_TextChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex < 0)
            {
                comboBox2.Text = "Seleccione una opción";
            }
            else
            {
                comboBox2.Text = comboBox2.SelectedText;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            MostrarGrilla();
            radioButton2.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            button3.Enabled = false;
            conexion.Open();
            string sql = "select USUARIO_APELLIDO, USUARIO_NOMBRE, TIPO_USU_NOMBRE, SESION_FECHA, SESION_EVENTO from SESION as ses join USUARIO as usu on usu.USUARIO_ID = SES.USUARIO_ID join TIPOUSUARIO as tipousu on tipousu.TIPO_USU_ID = usu.TIPO_USU_ID where ses.SESION_EVENTO=@evento";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@evento", SqlDbType.VarChar).Value ="Ingreso";
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(
                    registros["USUARIO_APELLIDO"].ToString(),
                registros["USUARIO_NOMBRE"].ToString(),
                registros["TIPO_USU_NOMBRE"].ToString(),
                registros["SESION_FECHA"].ToString(),
                registros["SESION_EVENTO"].ToString()
                );
            }
            registros.Close();
            conexion.Close();
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            MostrarGrilla();
            radioButton1.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
            button3.Enabled = false;
            conexion.Open();
            string sql = "select USUARIO_APELLIDO, USUARIO_NOMBRE, TIPO_USU_NOMBRE, SESION_FECHA, SESION_EVENTO from SESION as ses join USUARIO as usu on usu.USUARIO_ID = SES.USUARIO_ID join TIPOUSUARIO as tipousu on tipousu.TIPO_USU_ID = usu.TIPO_USU_ID where ses.SESION_EVENTO=@evento";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@evento", SqlDbType.VarChar).Value = "Salida";
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(
                    registros["USUARIO_APELLIDO"].ToString(),
                registros["USUARIO_NOMBRE"].ToString(),
                registros["TIPO_USU_NOMBRE"].ToString(),
                registros["SESION_FECHA"].ToString(),
                registros["SESION_EVENTO"].ToString()
                );
            }
            registros.Close();
            conexion.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (dateTimePicker1.Value >= dateTimePicker2.Value)
            {
                Aviso m = new Aviso();
                m.label1.Text = "La primera fecha y hora deben ser anteriores a la segunda \n fecha y hora ingresada";
                m.ShowDialog();
            }
            else
            {
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                conexion.Open();
                string sql = "select USUARIO_APELLIDO, USUARIO_NOMBRE, TIPO_USU_NOMBRE, SESION_FECHA, SESION_EVENTO from SESION as ses join USUARIO as usu on usu.USUARIO_ID = SES.USUARIO_ID join TIPOUSUARIO as tipousu on tipousu.TIPO_USU_ID = usu.TIPO_USU_ID where ses.SESION_FECHA >= @fechadesde and ses.SESION_FECHA <= @fechahasta";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(
                        registros["USUARIO_APELLIDO"].ToString(),
                    registros["USUARIO_NOMBRE"].ToString(),
                    registros["TIPO_USU_NOMBRE"].ToString(),
                    registros["SESION_FECHA"].ToString(),
                    registros["SESION_EVENTO"].ToString()
                    );
                }
                registros.Close();
                conexion.Close();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}