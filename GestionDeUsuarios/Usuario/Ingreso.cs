using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionDeUsuarios
{
    public partial class Ingreso : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
(
    int nLeftRect, // x-coordinate of upper-left corner
    int nTopRect, // y-coordinate of upper-left corner
    int nRightRect, // x-coordinate of lower-right corner
    int nBottomRect, // y-coordinate of lower-right corner
    int nWidthEllipse, // height of ellipse
    int nHeightEllipse // width of ellipse
);

        public Ingreso()
        {
            InitializeComponent();
            textBox1.MaxLength = 8;
            textBox2.MaxLength = 16; 
            textBox1.ForeColor = SystemColors.GrayText;
            textBox2.ForeColor = SystemColors.GrayText;
            textBox1.Text = "Ingrese usuario";
            textBox2.Text = "Ingrese contraseña"; 
            this.textBox1.Leave += new System.EventHandler(this.textBox1_SinTexto);
           this.textBox2.Leave += new System.EventHandler(this.textBox2_SinTexto);
            this.textBox2.Enter += new System.EventHandler(this.textBox2_BorradoDeMarca);
           this.textBox1.Enter += new System.EventHandler(this.textBox1_BorradoDeMarca);
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }
        
        
          private void textBox1_SinTexto(object sender, EventArgs e)
        {
            if (textBox1.Text.Length == 0 || textBox1.Text == "" || textBox1.Text == null)
            {
                textBox1.Text = "Ingrese usuario";
                textBox1.ForeColor = SystemColors.GrayText;
            }
        }
          
        private void textBox1_BorradoDeMarca(object sender, EventArgs e)
        {
            if (textBox1.Text == "Ingrese usuario")
            {
                textBox1.Text = "";
                textBox1.ForeColor = SystemColors.WindowText;
            }
        }
        private void textBox2_SinTexto(object sender, EventArgs e)
        {
            if (textBox2.Text.Length == 0 || textBox2.Text == "" || textBox2.Text == null)
            {
                textBox2.Text = "Ingrese contraseña";
                textBox2.UseSystemPasswordChar = false;    
                textBox2.ForeColor = SystemColors.GrayText;
            }
        }

        private void textBox2_BorradoDeMarca(object sender, EventArgs e)
        {
            if (textBox2.Text == "Ingrese contraseña")
            {
                textBox2.Text = "";
                textBox2.UseSystemPasswordChar = true;
                textBox2.ForeColor = SystemColors.WindowText;
            }
        }

        private void validarTextBox()
        {
            String campouno = textBox1.Text;
            String campodos = textBox2.Text;
            if (textBox1.Text.Length == 0 || textBox1.Text == "" || textBox1.Text == "Ingrese usuario" || textBox1.Text == null)
            {

                Aviso m = new Aviso();
                m.label1.Text = "Por favor revise el usuario";
                m.ShowDialog();
                return;
            }
            else if (textBox2.Text.Length == 0 || textBox2.Text == "" || textBox2.Text == null || textBox2.Text == "Ingrese contraseña")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Por favor revise la contraseña";
                m.ShowDialog();
                return;
            }
            else
            {
                if ((ExisteUsuario(textBox1.Text)))
                {
                    if (ExisteClave(textBox2.Text))
                    {
                        validarUsuario();
                        textBox1.Text = "";
                        textBox2.Text = "";
                    }
                    else if (textBox2.Text!="")
                    {
                        Aviso m = new Aviso();
                        m.label1.Text = "La contraseña es incorrecta";
                        m.ShowDialog();
                    }
                }
                else
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No existe dicho usuario";
                    m.ShowDialog();
                }
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            validarTextBox();            
        }
            

        private void validarUsuario()
        {
            conexion.Open();
            string sql = "select (r.USUARIO_NOMBRE +  ' '  + r.USUARIO_APELLIDO ) as NOMBRE_COMPLETO, count (*) as 'usuario', r.USUARIO_ID, r.TIPO_USU_ID, r.USUARIO_NOMBRE, r.USUARIO_APELLIDO from USUARIO r where r.USUARIO_DNI = @dniusuario and r.USUARIO_CLAVE = @clave group by r.USUARIO_DNI, r.USUARIO_CLAVE, r.TIPO_USU_ID, r.USUARIO_ID, r.USUARIO_NOMBRE, r.USUARIO_APELLIDO";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@clave", SqlDbType.VarChar).Value = textBox2.Text;
            comando.Parameters.Add("@dniusuario", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataReader registro = comando.ExecuteReader();

            if (registro.Read())
            {
                string usu = registro["usuario"].ToString();
                string usuId = registro["USUARIO_ID"].ToString();
                string tipo = registro["TIPO_USU_ID"].ToString();
                string nombre = registro["USUARIO_NOMBRE"].ToString();
                string apellido = registro["USUARIO_APELLIDO"].ToString();
                string usuario = registro["NOMBRE_COMPLETO"].ToString();
                int cantidadUsu = int.Parse(usu.ToString());
                int NumTipo = int.Parse(tipo.ToString());
                if (cantidadUsu > 0) 
                {
                    string tipoActivo = tipo;
                    string tipoActivo2 = tipo;
                    string dniActivo = textBox1.Text;
                    string nombreActivo = nombre;
                    string apellidoActivo = apellido;
                    string UsuIdActivo = usuId;
                    string usuarioActivo = usuario;
                    int aclientes = 0;
                    string numeroCelu = "";
                    Compras comp = new Compras (tipoActivo2);
                    Elaboracion elab = new Elaboracion(tipoActivo2,nombreActivo,apellidoActivo);
                    ReportesElab elabo = new ReportesElab(tipoActivo2, nombreActivo, apellidoActivo);
                    Ventas.Venta v = new Ventas.Venta(tipoActivo2,UsuIdActivo,numeroCelu);
                    Clientes c = new Clientes(tipoActivo2,UsuIdActivo, aclientes);

                    Menu m = new Menu();
                    m.fijarTipo(tipoActivo);
                    m.fijarDni(dniActivo);
                    m.fijarUsuario(usuarioActivo);
                    m.fijarNombre(nombreActivo);
                    m.fijarApellido(apellidoActivo);
                    m.fijarIdUsu(UsuIdActivo);
                    this.Hide();
                    m.ShowDialog();
                    this.Show();

                   /* Compras c = new Compras();
                    c.fijarTipo(tipoActivo2);
                    this.Hide();
                    c.ShowDialog();
                    this.Show();*/
                }                
            }
            registro.Close();
            conexion.Close();
            
        }

        
        private bool ExisteUsuario(string usuarioExistente)
        {
            conexion.Open();
            string sql = "select USUARIO_DNI from USUARIO where USUARIO_DNI=@dniusuario";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@dniusuario", SqlDbType.VarChar).Value = usuarioExistente;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
            existe = true;
            registro.Close();
            conexion.Close();
            return existe;

        }

        private bool ExisteClave(string claveExistente)
        {
            conexion.Open();
            string sql = "select USUARIO_CLAVE from USUARIO where USUARIO_CLAVE=@clave and USUARIO_DNI=@dniusuario";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@clave", SqlDbType.VarChar).Value = claveExistente;
            comando.Parameters.Add("@dniusuario", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            RecupClave m = new RecupClave();
            m.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            conexion.Open();
            string sql = "select count (*) TIPO_USU_ID from USUARIO where TIPO_USU_ID='1'";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                string cant = registro["TIPO_USU_ID"].ToString();
                int cantidadAdministrador = int.Parse(cant.ToString());


                if (cantidadAdministrador == 0)
                {
                    AdministrarUsuarios a = new AdministrarUsuarios();
                    a.ShowDialog();
                }
                registro.Close();
                conexion.Close();

            }

        }



        private void pictureBox5_Click(object sender, EventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar==Convert.ToChar(Keys.Enter))
            {
                validarTextBox();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                validarTextBox();
            }
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
    
