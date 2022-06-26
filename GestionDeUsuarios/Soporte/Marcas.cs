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
    public partial class Marcas : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public Marcas()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar alguna marca";
                m.ShowDialog();
            }
            else if (!ExisteMarca(textBox1.Text))
            {
                conexion.Open();
                string sql = "insert into MARCA (MARCA_NOMBRE) values (@marcanombre)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@marcanombre", SqlDbType.VarChar).Value = textBox1.Text;
                comando.ExecuteNonQuery();
                textBox1.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "La Marca fue registrada";
                m.ShowDialog();
                mostrarGrilla();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya cargó una Marca con ese nombre";
                m.ShowDialog();
            }
        }

        private void Marcas_Load(object sender, EventArgs e)
        {
            mostrarGrilla();
            if (!ExisteMTA25())
            {
                conexion.Open();
                string sql = "insert into MARCA (MARCA_NOMBRE) values ('MTA25')";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.ExecuteNonQuery();
                conexion.Close();
            }
        }

        private bool ExisteMTA25()
        {
            conexion.Open();
            string sql = "select MARCA_NOMBRE from MARCA where MARCA_NOMBRE='MTA25'";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select MARCA_NOMBRE from MARCA ORDER BY MARCA_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["MARCA_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private bool ExisteMarca(string marcaExistente)
        {
            conexion.Open();
            string sql = "select MARCA_NOMBRE from MARCA where MARCA_NOMBRE=@marca";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@marca", SqlDbType.VarChar).Value = marcaExistente;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }
    }
}
