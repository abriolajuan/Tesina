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
    public partial class AdministrarUsuarios : Form
    {     
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public AdministrarUsuarios()
        {
            InitializeComponent();
        }
        


        private bool ExisteUsuario(string nombreusuario)
        {
            conexion.Open();
            string sql = "select USUARIO_DNI from USUARIO where USUARIO_DNI=@dniusuario";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@dniusuario", SqlDbType.VarChar).Value = nombreusuario;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private void AdministrarUsuarios_Load(object sender, EventArgs e)
        {
            cargarTiposUsuarios();
            cargarComboBox1();
        }

        private void cargarTiposUsuarios()
        {

            conexion.Open();
            string sql = "select COUNT(*) TIPO_USU_NOMBRE from TIPOUSUARIO where TIPO_USU_NOMBRE='Administrador'";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                string cant = registro["TIPO_USU_NOMBRE"].ToString();
                int cantidadEstado = int.Parse(cant.ToString());

                registro.Close();
                conexion.Close();
                if (cantidadEstado == 0)
                {
                    conexion.Open();
                    string sql1 = "insert into TIPOUSUARIO(TIPO_USU_NOMBRE) values ('Administrador')";
                    string sql2 = "insert into TIPOUSUARIO(TIPO_USU_NOMBRE) values ('General')";
                    string sql3 = "insert into TIPOUSUARIO(TIPO_USU_NOMBRE) values ('Vendedor')";
                    string sql4 = "insert into TIPOUSUARIO(TIPO_USU_NOMBRE) values ('Cocinero')";
                    string sql5 = "insert into TIPOUSUARIO(TIPO_USU_NOMBRE) values ('Comprador')";
                //    string sql6 = "insert into TIPOUSUARIO(TIPO_USU_NOMBRE) values ('Cliente')";
                    SqlCommand comando1 = new SqlCommand(sql1, conexion);
                    SqlCommand comando2 = new SqlCommand(sql2, conexion);
                    SqlCommand comando3 = new SqlCommand(sql3, conexion);
                    SqlCommand comando4 = new SqlCommand(sql4, conexion);
                    SqlCommand comando5 = new SqlCommand(sql5, conexion);
                //    SqlCommand comando6 = new SqlCommand(sql6, conexion);
                    comando1.ExecuteNonQuery();
                    comando2.ExecuteNonQuery();
                    comando3.ExecuteNonQuery();
                    comando4.ExecuteNonQuery();
                    comando5.ExecuteNonQuery();
                //    comando6.ExecuteNonQuery();
                    conexion.Close();
                }

            }
        }

        private void cargarComboBox1()
        {
            conexion.Open();
            string sql = "select TIPO_USU_ID, TIPO_USU_NOMBRE from TIPOUSUARIO";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            comboBox1.DisplayMember = "TIPO_USU_NOMBRE";
            comboBox1.ValueMember = "TIPO_USU_ID";
            comboBox1.DataSource = tabla1;
            conexion.Close();
        }

        private Form formularioActivo = null;
        private void AbrirpanelHijo(Form formularioHijo)
        {
            if (formularioActivo != null)
                formularioActivo.Close();
            formularioActivo = formularioHijo;
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;
            panelHijo.Controls.Add(formularioHijo);
            panelHijo.Tag = formularioHijo;
            formularioHijo.BringToFront(); // ESTO POR SI PONEMOS UN LOGO DE FONDO ADELANTE VA EL FORMULARIO
            formularioHijo.Show();
        }

        private void button5_Click_1(object sender, EventArgs e)
        {
            AbrirpanelHijo(new ListadoUsuarios());
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
            Close();
        }


        public Boolean largoAdecuadoClave(string claveDeseada)
        {
            if (claveDeseada.Length >= 8)
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        private void button1_Click_1(object sender, EventArgs e)
        {
            if ((!ExisteUsuario(textBox1.Text)))
            {
                if (textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "")
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Falta completar campos";
                    m.ShowDialog();
                }
                else if (!largoAdecuadoClave(textBox2.Text))
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "La clave debe contener al menos 8 caracteres";
                    m.ShowDialog();
                }
                else if (textBox2.Text != textBox3.Text)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Las contraseñas no coinciden";
                    m.ShowDialog();
                }
                else
                {
                    conexion.Open();
                    string sql = "insert into USUARIO (USUARIO_DNI, USUARIO_CLAVE, USUARIO_MAIL,USUARIO_PREG1,USUARIO_PREG2,USUARIO_PREG3,TIPO_USU_ID,USUARIO_APELLIDO,USUARIO_NOMBRE) values (@dni, @clave, @mail,@pregseg1,@pregseg2,@pregseg3,@tipousuario,@apellido,@nombre)";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@dni", SqlDbType.VarChar).Value = textBox1.Text;
                    comando.Parameters.Add("@clave", SqlDbType.VarChar).Value = textBox2.Text;
                    comando.Parameters.Add("@tipousuario", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
                    comando.Parameters.Add("@pregseg1", SqlDbType.VarChar).Value = textBox4.Text;
                    comando.Parameters.Add("@pregseg2", SqlDbType.VarChar).Value = textBox5.Text;
                    comando.Parameters.Add("@pregseg3", SqlDbType.VarChar).Value = textBox6.Text;
                    comando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = textBox7.Text;
                    comando.Parameters.Add("@apellido", SqlDbType.VarChar).Value = textBox8.Text;
                    comando.Parameters.Add("@mail", SqlDbType.VarChar).Value = textBox9.Text;
                    comando.ExecuteNonQuery();
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    textBox5.Text = "";
                    textBox6.Text = "";
                    textBox7.Text = "";
                    textBox8.Text = "";
                    textBox9.Text = "";
                    conexion.Close();
                    Aviso m = new Aviso();
                    m.label1.Text = "El usuario fue registrado";
                    m.ShowDialog();
                }
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya se ha cargado un usuario con ese DNI";
                m.ShowDialog();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || textBox2.Text == "" || textBox3.Text == "" || textBox4.Text == "" || textBox5.Text == "" || textBox6.Text == "" || textBox7.Text == "" || textBox8.Text == "" || textBox9.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Faltan completar campos";
                m.ShowDialog();
            }
            else if (!largoAdecuadoClave(textBox2.Text))
            {
                Aviso m = new Aviso();
                m.label1.Text = "La clave debe contener al menos 8 caracteres";
                m.ShowDialog();
            }
            else if (textBox2.Text != textBox3.Text)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Las contraseñas no coinciden";
                m.ShowDialog();
            }
            else
            {
                conexion.Open();
                string sql = "update USUARIO set USUARIO_CLAVE=@clave, USUARIO_MAIL=@MAIL, USUARIO_PREG1=@pregseg1, USUARIO_PREG2=@pregseg2, USUARIO_PREG3=@pregseg3,USUARIO_APELLIDO=@apellido, USUARIO_NOMBRE=@nombre, TIPO_USU_ID=@tipousuario where USUARIO_DNI=@usuariodni";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = textBox7.Text; //
                comando.Parameters.Add("@apellido", SqlDbType.VarChar).Value = textBox8.Text; //
                comando.Parameters.Add("@usuariodni", SqlDbType.VarChar).Value = textBox1.Text;
                comando.Parameters.Add("@tipousuario", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
                comando.Parameters.Add("@clave", SqlDbType.VarChar).Value = textBox2.Text; //
                comando.Parameters.Add("@mail", SqlDbType.VarChar).Value = textBox9.Text; //
                comando.Parameters.Add("@pregseg1", SqlDbType.VarChar).Value = textBox4.Text; //
                comando.Parameters.Add("@pregseg2", SqlDbType.VarChar).Value = textBox5.Text; //
                comando.Parameters.Add("@pregseg3", SqlDbType.VarChar).Value = textBox6.Text; //
                int cant = comando.ExecuteNonQuery();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                textBox7.Text = "";
                textBox8.Text = "";
                textBox9.Text = "";
                conexion.Close();

                if (cant == 1)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Los datos fueron actualizados";
                    m.ShowDialog();
                }

                else
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No existe un usuario con ese DNI";
                    m.ShowDialog();
                }
            }
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            /*
            * EliminarUsu m = new EliminarUsu();
           m.ShowDialog();
           */

            if (textBox1.Text == "") 
            {
                Aviso m = new Aviso();
                m.label1.Text = "Falta completar el número de DNI";
                m.ShowDialog();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("¿Confirma la eliminación?", "Eliminar usuario", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    conexion.Open();
                    string sql = "delete from USUARIO where USUARIO_DNI=@usuariodni"; //and reg_clave=@clave and reg_repetirclave=@repetirclave
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@usuariodni", SqlDbType.VarChar).Value = textBox1.Text;
                    //comando.Parameters.Add("@clave", SqlDbType.VarChar).Value = textBox2.Text;
                    //comando.Parameters.Add("@repetirclave", SqlDbType.VarChar).Value = textBox3.Text;
                    int cant = comando.ExecuteNonQuery();
                    conexion.Close();
                    if (cant == 1)
                    {
                        textBox1.Text = "";
                        textBox2.Text = "";
                        textBox3.Text = "";
                        textBox4.Text = "";
                        textBox5.Text = "";
                        textBox6.Text = "";
                        textBox7.Text = "";
                        textBox8.Text = "";
                        textBox9.Text = "";
                        Aviso m = new Aviso();
                        m.label1.Text = "Se ha eliminado el usuario";
                        m.ShowDialog();
                    }
                    else
                    {
                        Aviso m = new Aviso();
                        m.label1.Text = "No existe ese nombre de usuario";
                        m.ShowDialog();
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }

        private void panelHijo_Paint(object sender, PaintEventArgs e)
        {

        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void textBox7_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox8_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }
    }
}
