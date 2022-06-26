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
    public partial class ListadoUsuarios : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public ListadoUsuarios()
        {
            InitializeComponent();
        }

        private void MostrarGrilla()
        {
            conexion.Open();
            string sql = "select USUARIO_DNI, USUARIO_NOMBRE, USUARIO_APELLIDO, TIPO_USU_NOMBRE from USUARIO as usu join TIPOUSUARIO as tipousu on tipousu.TIPO_USU_ID = usu.TIPO_USU_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["USUARIO_DNI"].ToString(),
                registros["USUARIO_NOMBRE"].ToString(), registros["USUARIO_APELLIDO"].ToString(), registros["TIPO_USU_NOMBRE"].ToString());


            }
            registros.Close();
            conexion.Close();
        }

        private void ListadoUsuarios_Load(object sender, EventArgs e)
        {
            MostrarGrilla();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
