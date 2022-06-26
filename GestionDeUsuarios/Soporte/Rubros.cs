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
    public partial class Rubros : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public Rubros()
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
                m.label1.Text = "Debe cargar algun rubro";
                m.ShowDialog();
            }
            else if (!ExisteRubro(textBox1.Text))
            {
                conexion.Open();
                string sql = "insert into RUBRO (RUBRO_NOMBRE) values (@rubronombre)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubronombre", SqlDbType.VarChar).Value = textBox1.Text;
                comando.ExecuteNonQuery();
                textBox1.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "El rubro fue registrado";
                m.ShowDialog();
                mostrarGrilla();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya cargó un rubro con ese nombre";
                m.ShowDialog();
            }
        }

        private void Rubros_Load(object sender, EventArgs e)
        {
            mostrarGrilla();
        }

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select RUBRO_NOMBRE from RUBRO ORDER BY RUBRO_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private bool ExisteRubro(string rubroExistente)
        {
            conexion.Open();
            string sql = "select RUBRO_NOMBRE from RUBRO where RUBRO_NOMBRE=@rubro";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.VarChar).Value = rubroExistente;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }
    }
}
