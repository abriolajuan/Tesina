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
    public partial class RecuperarPorMail : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public RecuperarPorMail()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un mail";
                m.ShowDialog();
            }
            else
            {
                recuperarContraseñaPorMail();
            }
        }

        public void recuperarContraseñaPorMail()
        {
            conexion.Open();
            string sql = "select * from USUARIO where USUARIO_MAIL=@mail";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@mail", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                label2.Text = "";
                int dniUsuario = registro.GetInt32(1);
                string clave = registro.GetString(2);
                string userMail = textBox1.Text;

                var mailService = new SystemSupportMail();
                mailService.sendMail(
                    subject: "Soporte SandSys - Recuperación de contraseña",
                    body: "Hola, " + dniUsuario + ".\n\nSolicitaste recuperar tu contraseña.\n"
                    + "Tu contraseña actual es: " + clave + "\nPara mayor seguridad, te recomendamos cambiar tu contraseña tras ingresar nuevamente al sistema.",
                    recipientMail: new List<string> { userMail }
                    );
                Aviso m = new Aviso();
                m.label1.Text = "Tu contraseña actual fue enviada a tu casilla registrada,\n aguardá 1 minuto por favor.\nA su vez, para mayor seguridad, te recomendamos:\n cambiar tu contraseña tras ingresar nuevamente al sistema.";
                m.ShowDialog();
            }
            else
            {
               label2.Text="Lo sentimos, la casilla de mail ingresada no se relaciona con un usuario registrado.";
            }
            registro.Close();
            conexion.Close();

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void RecuperarPorMail_Load(object sender, EventArgs e)
        {

        }
    }
}
