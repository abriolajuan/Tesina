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
    public partial class MediosDeTrans : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public MediosDeTrans()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Close();
        }

        private void MediosDeTrans_Load(object sender, EventArgs e)
        {
            mostrarGrilla();
            conexion.Open();
            string sql = "select COUNT(*) MEDIO_TR_NOMBRE from MEDIOTRANSACCION where MEDIO_TR_NOMBRE='Efectivo'";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                string cant = registro["MEDIO_TR_NOMBRE"].ToString();
                int cantidadEfectivo = int.Parse(cant.ToString());

                registro.Close();
                conexion.Close();
                if (cantidadEfectivo == 0)
                {
                    conexion.Open();
                    string sql1 = "insert into MEDIOTRANSACCION (MEDIO_TR_NOMBRE) values ('Efectivo')";
                    string sql2 = "insert into MEDIOTRANSACCION (MEDIO_TR_NOMBRE) values ('Tarjeta de Crédito')";
                    string sql3 = "insert into MEDIOTRANSACCION (MEDIO_TR_NOMBRE) values ('Tarjeta de Débito')";
                    string sql4 = "insert into MEDIOTRANSACCION (MEDIO_TR_NOMBRE) values ('Transferencia Bancaria')";
                    string sql5 = "insert into MEDIOTRANSACCION (MEDIO_TR_NOMBRE) values ('Cheque')";
                    SqlCommand comando1 = new SqlCommand(sql1, conexion);
                    SqlCommand comando2 = new SqlCommand(sql2, conexion);
                    SqlCommand comando3 = new SqlCommand(sql3, conexion);
                    SqlCommand comando4 = new SqlCommand(sql4, conexion);
                    SqlCommand comando5 = new SqlCommand(sql5, conexion);
                    comando1.ExecuteNonQuery();
                    comando2.ExecuteNonQuery();
                    comando3.ExecuteNonQuery();
                    comando4.ExecuteNonQuery();
                    comando5.ExecuteNonQuery();
                    conexion.Close();
                    mostrarGrilla();
                }

            }
        }


        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select MEDIO_TR_NOMBRE from MEDIOTRANSACCION ORDER BY MEDIO_TR_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["MEDIO_TR_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private bool ExisteMedio(string medioExistente)
        {
            conexion.Open();
            string sql = "select MEDIO_TR_NOMBRE from MEDIOTRANSACCION where MEDIO_TR_NOMBRE=@mediotrans";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@mediotrans", SqlDbType.VarChar).Value = medioExistente;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

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
