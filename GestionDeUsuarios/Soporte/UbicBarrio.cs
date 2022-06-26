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
    public partial class UbicBarrio : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public UbicBarrio()
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
                m.label1.Text = "Debe cargar algún barrio";
                m.ShowDialog();
            }
            else
            {
                if(existeBarrio())
                    {
                    Aviso m = new Aviso();
                    m.label1.Text = "El barrio ya existe en esa localidad";
                    m.ShowDialog();
                }
                else {
                    conexion.Open();
                    string sql = "insert into BARRIO (LOCALIDAD_ID,BARRIO_NOMBRE) values (@localidadnombre,@barrionombre)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@localidadnombre", SqlDbType.VarChar).Value = comboBox1.SelectedValue.ToString();
                comando.Parameters.Add("@barrionombre", SqlDbType.VarChar).Value = textBox1.Text;
                comando.ExecuteNonQuery();
                textBox1.Text = "";
                conexion.Close();
                    Aviso m = new Aviso();
                    m.label1.Text = "El barrio fue registrado";
                    m.ShowDialog();
                    mostrarGrilla();
                }
            }
        }

        private bool existeBarrio()
        {
            conexion.Open();
            string sql = "select BARRIO_NOMBRE from BARRIO where BARRIO_NOMBRE=@nombreBarrio and LOCALIDAD_ID=@localidadID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@nombreBarrio", SqlDbType.VarChar).Value = textBox1.Text;
            comando.Parameters.Add("@localidadID", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
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
            string sql = "select PROVINCIA_NOMBRE, LOCALIDAD_NOMBRE, BARRIO_NOMBRE from LOCALIDAD as loc join PROVINCIA as prov on prov.PROVINCIA_ID = loc.PROVINCIA_ID join BARRIO as bar on bar.LOCALIDAD_ID = loc.LOCALIDAD_ID ORDER BY PROVINCIA_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PROVINCIA_NOMBRE"].ToString(),
                 registros["LOCALIDAD_NOMBRE"].ToString(), registros["BARRIO_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void UbicBarrio_Load(object sender, EventArgs e)
        {
            mostrarGrilla();
            cargarComboBox2();
            //cargarComboBox1();
        }

        private void cargarComboBox2()
        {
            conexion.Open();
            string sql = "select PROVINCIA_ID, PROVINCIA_NOMBRE from PROVINCIA ORDER BY PROVINCIA_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox2.DisplayMember = "PROVINCIA_NOMBRE";
            comboBox2.ValueMember = "PROVINCIA_ID";
            comboBox2.DataSource = tabla1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarComboBox1();
        }

        private void cargarComboBox1()
        {
            conexion.Open();
            string sql = "select LOCALIDAD_ID, LOCALIDAD_NOMBRE from LOCALIDAD where PROVINCIA_ID=@provinciaid ORDER BY LOCALIDAD_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@provinciaid", SqlDbType.Int).Value = comboBox2.SelectedValue;
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox1.DisplayMember = "LOCALIDAD_NOMBRE";
            comboBox1.ValueMember = "LOCALIDAD_ID";
            comboBox1.DataSource = tabla1;
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
