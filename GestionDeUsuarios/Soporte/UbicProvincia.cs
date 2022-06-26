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
    public partial class UbicProvincia : Form
    {
        private SqlConnection conexion = new SqlConnection("Data Source=SAM;Initial Catalog=bdSS;Integrated Security=True");

        public UbicProvincia()
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
                m.label1.Text = "Debe cargar alguna provincia";
                m.ShowDialog();
            }
            else if (!ExisteProvincia(textBox1.Text))
            {
                conexion.Open();
                string sql = "insert into PROVINCIA (PROVINCIA_NOMBRE) values (@provincianombre)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@provincianombre", SqlDbType.VarChar).Value = textBox1.Text;
                comando.ExecuteNonQuery();
                textBox1.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "La Provincia fue registrada";
                m.ShowDialog();
                mostrarGrilla();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya cargó una Provincia con ese nombre";
                m.ShowDialog();
            }
        }

        private void UbicProvincia_Load(object sender, EventArgs e)
        {
            mostrarGrilla();
        }

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select PROVINCIA_NOMBRE from PROVINCIA ORDER BY PROVINCIA_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PROVINCIA_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();
        }


        private bool ExisteProvincia (string provinciaExistente)
        {
            conexion.Open();
            string sql = "select PROVINCIA_NOMBRE from PROVINCIA where PROVINCIA_NOMBRE=@provincia";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@provincia", SqlDbType.VarChar).Value = provinciaExistente;
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
