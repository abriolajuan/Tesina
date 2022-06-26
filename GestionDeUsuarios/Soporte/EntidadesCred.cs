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
    public partial class EntidadesCred : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public EntidadesCred()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar alguna entidad";
                m.ShowDialog();
            }
            else if (!ExisteEntidad(textBox1.Text))
            {
                conexion.Open();
                string sql = "insert into ENTIDADCREDITICIA (ENTIDAD_NOMBRE) values (@entidadnombre)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@entidadnombre", SqlDbType.VarChar).Value = textBox1.Text;
                comando.ExecuteNonQuery();
                textBox1.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "La entidad fue registrada";
                m.ShowDialog();
                MostrarGrilla();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya cargó una entidad crediticia con ese nombre";
                m.ShowDialog();
            }

        }

        private void EntidadesCred_Load(object sender, EventArgs e)
        {
            button1.Enabled = false;
            MostrarGrilla();

            label3.Visible = false;
            textBox2.Visible = false;
            pictureBox2.Visible = false;
            button1.Visible = false;
        }

        private void MostrarGrilla()
        {
            conexion.Open();
            string sql = "select ENTIDAD_NOMBRE from ENTIDADCREDITICIA ORDER BY ENTIDAD_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["ENTIDAD_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private bool ExisteEntidad(string entidadExistente)
        {
            conexion.Open();
            string sql = "select ENTIDAD_NOMBRE from ENTIDADCREDITICIA where ENTIDAD_NOMBRE=@entidad";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@entidad", SqlDbType.VarChar).Value = entidadExistente;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (!ExisteEntidad(textBox1.Text))
            {
                Aviso m = new Aviso();
                m.label1.Text = "No existe una entidad crediticia con ese nombre";
                m.ShowDialog();
            }
            else
            {
                button2.Enabled = false;
                textBox1.Enabled = false;
                button1.Enabled = true;

                pictureBox1.Visible = false;
                button2.Visible = false;
                label3.Visible = true;
                textBox2.Visible = true;
                pictureBox2.Visible = true;
                button1.Visible = true;

                conexion.Open();
                string sql = "select ENTIDAD_NOMBRE from ENTIDADCREDITICIA WHERE ENTIDAD_NOMBRE=@entidad";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@entidad", SqlDbType.VarChar).Value = textBox1.Text;
                SqlDataReader registros = comando.ExecuteReader();
                while (registros.Read())
                {
                    textBox1.Text = registros["ENTIDAD_NOMBRE"].ToString();
                    textBox2.Text = registros["ENTIDAD_NOMBRE"].ToString();
                }
                conexion.Close();
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para actualizar la entidad crediticia tiene que cargar un nuevo nombre";
                m.ShowDialog();
            }
            else
            {
                conexion.Open();
                string sql = "update ENTIDADCREDITICIA set ENTIDAD_NOMBRE=@nuevonombre WHERE ENTIDAD_NOMBRE=@anteriornombre";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@anteriornombre", SqlDbType.VarChar).Value = textBox1.Text;
                comando.Parameters.Add("@nuevonombre", SqlDbType.VarChar).Value = textBox2.Text;
                comando.ExecuteNonQuery();
                conexion.Close();
                textBox1.Text = "";
                textBox2.Text = "";
                Aviso m = new Aviso();
                m.label1.Text = "Se ha modificado la entidad";
                m.ShowDialog();
                dataGridView1.Rows.Clear();
                MostrarGrilla();
                button1.Enabled = false;
                button2.Enabled = true;
                textBox1.Enabled = true;

                pictureBox1.Visible = true;
                button2.Visible = true;
                label3.Visible = false;
                textBox2.Visible = false;
                pictureBox2.Visible = false;
                button1.Visible = false;
            }
        }
    }
}
