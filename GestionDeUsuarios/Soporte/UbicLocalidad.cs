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
    public partial class UbicLocalidad : Form
    {
        private SqlConnection conexion = new SqlConnection("Data Source=SAM;Initial Catalog=bdSS;Integrated Security=True");

        public UbicLocalidad()
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
                m.label1.Text = "Debe cargar alguna localidad";
                m.ShowDialog();
            }
            else if (!ExisteLocalidad(textBox1.Text))
            {
                conexion.Open();
                string sql = "insert into LOCALIDAD (PROVINCIA_ID,LOCALIDAD_NOMBRE) values (@provincianombre,@localidadnombre)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@provincianombre", SqlDbType.VarChar).Value = comboBox1.SelectedValue.ToString();
                comando.Parameters.Add("@localidadnombre", SqlDbType.VarChar).Value = textBox1.Text;
                comando.ExecuteNonQuery();
                textBox1.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "La Localidad fue registrada";
                m.ShowDialog();
                mostrarGrilla();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya cargó una localidad con ese nombre para esa provincia";
                m.ShowDialog();
            }
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
            comboBox1.DisplayMember = "PROVINCIA_NOMBRE";
            comboBox1.ValueMember = "PROVINCIA_ID";
            comboBox1.DataSource = tabla1;
            conexion.Close();
        }

        private void UbicLocalidad_Load(object sender, EventArgs e)
        {
            mostrarGrilla();
            cargarComboBox1();
        }

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select PROVINCIA_NOMBRE, LOCALIDAD_NOMBRE from LOCALIDAD as loc join PROVINCIA as prov on prov.PROVINCIA_ID = loc.PROVINCIA_ID ORDER BY PROVINCIA_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PROVINCIA_NOMBRE"].ToString(),
                 registros["LOCALIDAD_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private bool ExisteLocalidad(string localidadExistente)
        {
            conexion.Open();
            string sql = "select LOCALIDAD_NOMBRE from LOCALIDAD where LOCALIDAD_NOMBRE=@localidad and PROVINCIA_ID=@provinciaID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@localidad", SqlDbType.VarChar).Value = localidadExistente;
            comando.Parameters.Add("@provinciaID", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
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
