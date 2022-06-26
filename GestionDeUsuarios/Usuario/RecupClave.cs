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
    public partial class RecupClave : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public RecupClave()
        {
            InitializeComponent();
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ingrese Usuario (DNI)";
                m.ShowDialog();
            }
            else if (textBox2.Text == "" || textBox3.Text=="" || textBox4.Text=="")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ingrese las preguntas de seguridad";
                m.ShowDialog();
            }
            else if (!largoAdecuadoClave(textBox5.Text))
            {
                Aviso m = new Aviso();
                m.label1.Text = "La clave debe contener al menos 8 caracteres";
                m.ShowDialog();
            }
            else if (textBox5.Text != textBox6.Text)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Las contraseñas no coinciden";
                m.ShowDialog();
            }
            else
            {
                conexion.Open();
                string sql = "update USUARIO set USUARIO_CLAVE=@clave where USUARIO_DNI=@nombreusuario and USUARIO_PREG1=@pregseg1 and USUARIO_PREG2=@pregseg2 and USUARIO_PREG3=@pregseg3";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@nombreusuario", SqlDbType.VarChar).Value = textBox1.Text;
                comando.Parameters.Add("@clave", SqlDbType.VarChar).Value = textBox5.Text;
                comando.Parameters.Add("@pregseg1", SqlDbType.VarChar).Value = textBox3.Text;
                comando.Parameters.Add("@pregseg2", SqlDbType.VarChar).Value = textBox2.Text;
                comando.Parameters.Add("@pregseg3", SqlDbType.VarChar).Value = textBox4.Text;
                int cant = comando.ExecuteNonQuery();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                conexion.Close();
                if (cant == 1)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Se ha modificado la contraseña";
                    m.ShowDialog();
                }
                else
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Los datos ingresados no fueron correctos";
                    m.ShowDialog();
                }
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void label10_Click(object sender, EventArgs e)
        {

        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void capturarEnter()
        {
                conexion.Open();
                string sql = "update USUARIO set USUARIO_CLAVE=@clave where USUARIO_DNI=@nombreusuario and USUARIO_PREG1=@pregseg1 and USUARIO_PREG2=@pregseg2 and USUARIO_PREG3=@pregseg3";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@nombreusuario", SqlDbType.VarChar).Value = textBox1.Text;
                comando.Parameters.Add("@clave", SqlDbType.VarChar).Value = textBox5.Text;
                comando.Parameters.Add("@pregseg1", SqlDbType.VarChar).Value = textBox2.Text;
                comando.Parameters.Add("@pregseg2", SqlDbType.VarChar).Value = textBox3.Text;
                comando.Parameters.Add("@pregseg3", SqlDbType.VarChar).Value = textBox4.Text;
                int cant = comando.ExecuteNonQuery();
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                textBox5.Text = "";
                textBox6.Text = "";
                conexion.Close();
            if (cant == 1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Se ha modificado la contraseña";
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Los datos ingresados no fueron correctos";
                m.ShowDialog();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                capturarEnter();
            }
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                capturarEnter();
            }
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                capturarEnter();
            }
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                capturarEnter();
            }
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                capturarEnter();
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == Convert.ToChar(Keys.Enter))
            {
                capturarEnter();
            }
        }

        private void label4_Click(object sender, EventArgs e)
        {
            RecuperarPorMail r = new RecuperarPorMail();
            r.ShowDialog();
        }

     
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            RecuperarPorMail r = new RecuperarPorMail();
            r.ShowDialog();
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void RecupClave_Load(object sender, EventArgs e)
        {

        }
    }
}

