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
using System.Windows.Forms.DataVisualization.Charting;

namespace GestionDeUsuarios.Ventas
{
    public partial class AnalisisVentas : Form
    {
        private SqlConnection conexion = new SqlConnection("Data Source=SAM;Initial Catalog=bdSS;Integrated Security=True");
        public AnalisisVentas()
        {
            InitializeComponent();
        }


        private void pictureBox3_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void AnalisisVentas_Load(object sender, EventArgs e)
        {
            cargarcomboBox1();
            cargarcomboBox2();
            cargarcomboBox3();
            cargarComboBox4();
            cargarComboBox5();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox6.SelectedIndex = -1;
            textBox3.Enabled = false;
            button4.Enabled = false;
        }


        private void cargarcomboBox1()
        {
            if (comboBox6.Text == "Producto Elaborado")
            {
                conexion.Open();
                string sql = "select rubro.rubro_id, rubro_nombre from RUBRO as rubro, PRODUCTOELAB as prod where rubro.RUBRO_ID=prod.RUBRO_ID group by RUBRO_NOMBRE, rubro.RUBRO_ID ORDER BY RUBRO_NOMBRE ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox1.DisplayMember = "RUBRO_NOMBRE";
                comboBox1.ValueMember = "RUBRO_ID";
                comboBox1.DataSource = tabla1;
            }
            else if (comboBox6.Text == "Producto de Reventa")
            {
                conexion.Open();
                string sql = "select rubro.rubro_id, rubro_nombre from RUBRO as rubro, PRODUCTOREVENTA as pr where rubro.RUBRO_ID = pr.RUBRO_ID group by RUBRO_NOMBRE, rubro.RUBRO_ID ORDER BY RUBRO_NOMBRE ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox1.DisplayMember = "RUBRO_NOMBRE";
                comboBox1.ValueMember = "RUBRO_ID";
                comboBox1.DataSource = tabla1;
            }
        }



        private void cargarcomboBox2()
        {
            if (comboBox6.Text == "Producto Elaborado")
            {
                conexion.Open();
                string sql = "select prod.MARCA_ID, MARCA_NOMBRE from MARCA as mar, PRODUCTOELAB as prod where mar.MARCA_ID=prod.MARCA_ID group by MARCA_NOMBRE, prod.MARCA_ID ORDER BY MARCA_NOMBRE ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox2.DisplayMember = "MARCA_NOMBRE";
                comboBox2.ValueMember = "MARCA_ID";
                comboBox2.DataSource = tabla1;
            }
            else if (comboBox6.Text == "Producto de Reventa")
            {
                conexion.Open();
                string sql = "select prod.marca_id, marca_nombre from MARCA as marca, PRODUCTOREVENTA as prod where marca.MARCA_ID=prod.MARCA_ID group by MARCA_NOMBRE, prod.marca_id ORDER BY MARCA_NOMBRE ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox2.DisplayMember = "MARCA_NOMBRE";
                comboBox2.ValueMember = "MARCA_ID";
                comboBox2.DataSource = tabla1;
            }
        }



        private void cargarcomboBox3()
        {
            if (comboBox6.Text == "Producto Elaborado" && comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
            {
                conexion.Open();
                string sql = "select PROD_ELAB_ID, PROD_ELAB_DESCR from PRODUCTOELAB where RUBRO_ID=@rubroid and MARCA_ID=@marcaid ORDER BY PROD_ELAB_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox3.DataSource = tabla1;
                comboBox3.DisplayMember = "PROD_ELAB_DESCR";
                comboBox3.ValueMember = "PROD_ELAB_ID";
            }
            else if (comboBox6.Text == "Producto de Reventa" && comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
            {
                conexion.Open();
                string sql = "select PROD_REV_ID, PROD_REV_DESCR from PRODUCTOREVENTA where RUBRO_ID=@rubroid and MARCA_ID=@marcaid ORDER BY PROD_REV_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox3.DataSource = tabla1;
                comboBox3.DisplayMember = "PROD_REV_DESCR";
                comboBox3.ValueMember = "PROD_REV_ID";
            }
        }


        private void cargarComboBox4() // VENDEDOR
        {
            conexion.Open();
            string sql = "select USUARIO_ID, (USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO from USUARIO WHERE TIPO_USU_ID='3' OR TIPO_USU_ID='1' OR TIPO_USU_ID='2' ORDER BY NOMBRE_COMPLETO ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            comboBox4.DisplayMember = "NOMBRE_COMPLETO";
            comboBox4.ValueMember = "USUARIO_ID";
            comboBox4.DataSource = tabla1;
            conexion.Close();
        }


        private void cargarComboBox5() // ESTADO DE COBRO
        {
            conexion.Open();
            string sql = "select EST_VENTA_ID, EST_VENTA_NOMBRE from ESTADOVENTA";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox5.DisplayMember = "EST_VENTA_NOMBRE";
            comboBox5.ValueMember = "EST_VENTA_ID";
            comboBox5.DataSource = tabla1;
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
            panel1.Controls.Add(formularioHijo);
            panel1.Tag = formularioHijo;
            formularioHijo.BringToFront(); // ESTO POR SI PONEMOS UN LOGO DE FONDO ADELANTE VA EL FORMULARIO
            formularioHijo.Show();
        }
        string tipoActivo, IdUsu;
        int aclientes = 6;

        private void comboBox2_SelectedValueChanged_1(object sender, EventArgs e)
        {
            cargarcomboBox3();
            comboBox3.SelectedValue = -1;
        }

        private void comboBox1_SelectedValueChanged_1(object sender, EventArgs e)
        {
            cargarcomboBox3();
            comboBox3.SelectedValue = -1;
        }

        private void comboBox6_SelectedValueChanged_1(object sender, EventArgs e)
        {
            cargarcomboBox1();
            cargarcomboBox2();
            comboBox1.SelectedValue = -1;
            comboBox2.SelectedValue = -1;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                Aviso m = new Aviso();
                m.label1.Text = "La primera fecha y hora deben ser anteriores a la segunda fecha y hora ingresada";
                m.ShowDialog();
            }
            else if (textBox3.Text == "" && comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un dato de las listas desplegables";
                m.ShowDialog();
            }
            else if (comboBox5.SelectedIndex != -1 && comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox6.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un vendedor acompañando al estado de las listas desplegables";
                m.ShowDialog();

            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && textBox3.Text == "" && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && textBox3.Text == "" && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && textBox3.Text != "" && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && textBox3.Text != "" && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && textBox3.Text == "" && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && textBox3.Text == "" && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }


            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text != "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text != "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text != "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text != "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text != "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text != "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text != "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }

            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text != "Producto Elaborado")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && textBox3.Text == "" && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text != "Producto de Reventa")
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (textBox3.Text == "" && comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado") // RUBRO Y MARCA POR MATERIA PRIMA
            {
                button2.Enabled = false;
                CantidadVentasRubroYMarcaProdElab();
                cantidadesUnidadesVendidasProdElab();
                CantidadSinCobrosRubroYMarcaProdElab();
                CantidadCobradaParcialmenteRubroYMarcaProdElab();
                CantidadCobradaTotalmenteRubroYMarcaProdElab();
                cantidadMaximaVentasProdElab();
                cantidadMinimaVentasProdElab();
                cantidadPromedioVentasProdElab();
                fechaMinVendidasProdElab();
                fechaMaxVendidasProdElab();
                button4.Enabled = true;
            }
            else if (textBox3.Text == "" && comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa") // RUBRO Y MARCA POR PRODUCTO DE REVENTA
            {
                button2.Enabled = false;
                CantidadVentasRubroYMarcaProdRev();
                cantidadesUnidadesVendidasProdRev();
                CantidadSinCobrosRubroYMarcaProdRev();
                CantidadCobradaParcialmenteRubroYMarcaProdRev();
                CantidadCobradaTotalmenteRubroYMarcaProdRev();
                cantidadMaximaVentasProdRev();
                cantidadMinimaVentasProdRev();
                cantidadPromedioVentasProdRev();
                fechaMinVendidasProdRev();
                fechaMaxVendidasProdRev();
                button4.Enabled = true;
            }
            else if (textBox3.Text == "" && comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto Elaborado") // RUBRO, MARCA Y NOMBRE DE PRODUCTO DEL ELABORADO
            {
                button2.Enabled = false;
                CantidadVentasRubroNombreMarcaProdElab();
                cantidadesUnidadesVendidasNombreProdElab();
                CantidadSinCobrosRubroNombreMarcaProdElab();
                CantidadCobradaParcialmenteRubroNombreMarcaProdElab();
                CantidadCobradaTotalmenteRubroNombreMarcaProdElab();
                cantidadMaximaVentasNombreProdElab();
                cantidadMinimaVentasNombreProdElab();
                cantidadPromedioVentasNombreProdElab();
                fechaMinVendidasNombreProdElab();
                fechaMaxVendidasNombreProdElab();
                button4.Enabled = true;
            }
            else if (textBox3.Text == "" && comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "Producto de Reventa") // RUBRO Y MARCA POR PRODUCTO DE REVENTA
            {
                button2.Enabled = false;
                CantidadVentasRubroNombreMarcaProdRev();
                cantidadesUnidadesVendidasNombreProdRev();
                CantidadSinCobrosRubroNombreMarcaProdRev();
                CantidadCobradaParcialmenteRubroNombreMarcaProdRev();
                CantidadCobradaTotalmenteRubroNombreMarcaProdRev();
                cantidadMaximaVentasNombreProdRev();
                cantidadMinimaVentasNombreProdRev();
                cantidadPromedioVentasNombreProdRev();
                fechaMinVendidasNombreProdRev();
                fechaMaxVendidasNombreProdRev();
                button4.Enabled = true;
            }
            else if (textBox3.Text == "" && comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "") // POR VENDEDOR
            {
                button2.Enabled = false;
                CantidadVentasPorVendedor();
                cantidadesUnidadesVendidasVendedor();
                CantidadSinCobrosVendedor();
                CantidadCobradaParcialmenteVendedor();
                CantidadCobradaTotalmenteVendedor();
                cantidadMaximaVentasVendedor();
                cantidadMinimaVentasVendedor();
                cantidadPromedioVentasVendedor();
                fechaMinVendidasVendedor();
                fechaMaxVendidasVendedor();
                button4.Enabled = true;
            }
            else if (textBox3.Text == "" && comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "") // POR VENDEDOR Y ESTADO
            {
                button2.Enabled = false;
                CantidadVentasPorVendedorYEstado();
                cantidadesUnidadesVendidasVendedorYEstado();
                CantidadSinCobrosVendedorYEstado();
                CantidadCobradaParcialmenteVendedorYEstado();
                CantidadCobradaTotalmenteVendedorYEstado();
                cantidadMaximaVentasVendedorYEstado();
                cantidadMinimaVentasVendedorYEstado();
                cantidadPromedioVentasVendedorYEstado();
                fechaMinVendidasVendedorYEstado();
                fechaMaxVendidasVendedorYEstado();
                button4.Enabled = true;
            }
            else if (textBox3.Text != "" && comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1 && comboBox6.Text == "") // POR CLIENTE
            {
                button2.Enabled = false;
                CantidadVentasPorCliente();
                cantidadesUnidadesVendidasCliente();
                CantidadSinCobrosCliente();
                CantidadCobradaParcialmenteCliente();
                CantidadCobradaTotalmenteCliente();
                cantidadMaximaVentasCliente();
                cantidadMinimaVentasCliente();
                cantidadPromedioVentasCliente();
                fechaMinVendidasCliente();
                fechaMaxVendidasCliente();
                button4.Enabled = true;
            }
            else if (textBox3.Text != "" && comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto Elaborado") // POR TODO PRODUCTO ELABORADO
            {
                button2.Enabled = false;
                CantidadVentasTodosProdElab();
                cantidadesUnidadesVendidasTodosProdElab();
                CantidadSinCobrosTodosProdElab();
                CantidadCobradaParcialmenteTodosProdElab();
                CantidadCobradaTotalmenteTodosProdElab();
                cantidadMaximaVentasTodosProdElab();
                cantidadMinimaVentasTodosProdElab();
                cantidadPromedioVentasTodosProdElab();
                fechaMinVendidasTodosProdElab();
                fechaMaxVendidasTodosProdElab();
                button4.Enabled = true;
            }
            else if (textBox3.Text != "" && comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1 && comboBox6.Text == "Producto de Reventa") // POR TODO PRODUCTO REVENTA
            {
                button2.Enabled = false;
                CantidadVentasTodosProdRev();
                cantidadesUnidadesVendidasTodosProdRev();
                CantidadSinCobrosTodosProdRev();
                CantidadCobradaParcialmenteTodosProdRev();
                CantidadCobradaTotalmenteTodosProdRev();
                cantidadMaximaVentasTodosProdRev();
                cantidadMinimaVentasTodosProdRev();
                cantidadPromedioVentasTodosProdRev();
                fechaMinVendidasTodosProdRev();
                fechaMaxVendidasTodosProdRev();
                button4.Enabled = true;
            }

        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            AbrirpanelHijo(new Clientes(tipoActivo, IdUsu, aclientes));
        }



        // METODOS PARA LLAMAR EN LAS CONSULTAS


        // ---------------------------------------------- POR RUBRO Y MARCA DE PRODUCTOS ELABORADOS  --------------------------------------------------------------------------

        private void CantidadVentasRubroYMarcaProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT det.VENTA_ID) as 'Cantidad de ventas' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de ventas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "-";
            }
            else
            {
                cantidad = int.Parse(cant);
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadesUnidadesVendidasProdElab()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select SUM(det.DET_VENTA_CANT) as 'Cantidad vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadSinCobrosRubroYMarcaProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad sin cobros' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = det.PROD_ELAB_ID  where VEN.EST_VENTA_ID='1' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad sin cobros"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaParcialmenteRubroYMarcaProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad parcial' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = det.PROD_ELAB_ID  where VEN.EST_VENTA_ID='2' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad parcial"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaTotalmenteRubroYMarcaProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad total' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = det.PROD_ELAB_ID  where VEN.EST_VENTA_ID='3' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad total"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMaximaVentasProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaVentasProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioVentasProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_VENTA_CANT) as 'Cantidad promedio vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinVendidasProdElab()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }

        private void fechaMaxVendidasProdElab()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }




        // ------------------------------------------------------ POR RUBRO Y MARCA PRODUCTOS DE REVENTA ----------------------------------------


        private void CantidadVentasRubroYMarcaProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT det.VENTA_ID) as 'Cantidad de ventas' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de ventas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void cantidadesUnidadesVendidasProdRev()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select SUM(det.DET_VENTA_CANT) as 'Cantidad vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID  where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadSinCobrosRubroYMarcaProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad sin cobros' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID where VEN.EST_VENTA_ID='1' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad sin cobros"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadCobradaParcialmenteRubroYMarcaProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad parcial' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID  where VEN.EST_VENTA_ID='2' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad parcial"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaTotalmenteRubroYMarcaProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad total' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID  where VEN.EST_VENTA_ID='3' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad total"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void cantidadMaximaVentasProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaVentasProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioVentasProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_VENTA_CANT) as 'Cantidad promedio vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void fechaMinVendidasProdRev()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }

        private void fechaMaxVendidasProdRev()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }



        // ---------------------------------------------- POR RUBRO, MARCA Y NOMBRE DEL PRODUCTO DE PRODUCTOS ELABORADOS  --------------------------------------------------------------------------

        private void CantidadVentasRubroNombreMarcaProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT det.VENTA_ID) as 'Cantidad de ventas' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de ventas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadesUnidadesVendidasNombreProdElab()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select SUM(det.DET_VENTA_CANT) as 'Cantidad vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID where det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadSinCobrosRubroNombreMarcaProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad sin cobros' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = det.PROD_ELAB_ID  where det.PROD_ELAB_ID=@productoelaborado and VEN.EST_VENTA_ID='1' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad sin cobros"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaParcialmenteRubroNombreMarcaProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad parcial' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = det.PROD_ELAB_ID  where det.PROD_ELAB_ID=@productoelaborado and VEN.EST_VENTA_ID='2' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad parcial"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaTotalmenteRubroNombreMarcaProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad total' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = det.PROD_ELAB_ID  where det.PROD_ELAB_ID=@productoelaborado and VEN.EST_VENTA_ID='3' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad total"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMaximaVentasNombreProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaVentasNombreProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioVentasNombreProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_VENTA_CANT) as 'Cantidad promedio vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinVendidasNombreProdElab()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }

        private void fechaMaxVendidasNombreProdElab()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }



        // ------------------------------------------------------ POR RUBRO, NOMBRE Y MARCA PRODUCTOS DE REVENTA ----------------------------------------


        private void CantidadVentasRubroNombreMarcaProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT det.VENTA_ID) as 'Cantidad de ventas' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de ventas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void cantidadesUnidadesVendidasNombreProdRev()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select SUM(det.DET_VENTA_CANT) as 'Cantidad vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID  where det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadSinCobrosRubroNombreMarcaProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad sin cobros' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID where det.PROD_REV_ID=@productoreventa and VEN.EST_VENTA_ID='1' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad sin cobros"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadCobradaParcialmenteRubroNombreMarcaProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad parcial' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID  where det.PROD_REV_ID=@productoreventa and VEN.EST_VENTA_ID='2' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad parcial"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaTotalmenteRubroNombreMarcaProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad total' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID  where det.PROD_REV_ID=@productoreventa and VEN.EST_VENTA_ID='3' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad total"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void cantidadMaximaVentasNombreProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaVentasNombreProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioVentasNombreProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_VENTA_CANT) as 'Cantidad promedio vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void fechaMinVendidasNombreProdRev()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }

        private void fechaMaxVendidasNombreProdRev()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }




        // ---------------------------------------------- POR VENDEDOR  --------------------------------------------------------------------------

        private void CantidadVentasPorVendedor()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT det.VENTA_ID) as 'Cantidad de ventas' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de ventas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadesUnidadesVendidasVendedor()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select SUM(det.DET_VENTA_CANT) as 'Cantidad vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadSinCobrosVendedor()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad sin cobros' from VENTA as ven where VEN.EST_VENTA_ID='1' and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad sin cobros"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaParcialmenteVendedor()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad parcial' from VENTA as ven where VEN.EST_VENTA_ID='2' and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad parcial"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaTotalmenteVendedor()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad total' from VENTA as ven where VEN.EST_VENTA_ID='3' and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad total"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMaximaVentasVendedor()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaVentasVendedor()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioVentasVendedor()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_VENTA_CANT) as 'Cantidad promedio vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinVendidasVendedor()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }

        private void fechaMaxVendidasVendedor()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }



        // ---------------------------------------------- POR VENDEDOR Y ESTADO --------------------------------------------------------------------------

        private void CantidadVentasPorVendedorYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT det.VENTA_ID) as 'Cantidad de ventas' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de ventas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadesUnidadesVendidasVendedorYEstado()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select SUM(det.DET_VENTA_CANT) as 'Cantidad vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadSinCobrosVendedorYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad sin cobros' from VENTA as ven where VEN.EST_VENTA_ID='1' and ven.EST_VENTA_ID=@estado and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad sin cobros"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaParcialmenteVendedorYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad parcial' from VENTA as ven where VEN.EST_VENTA_ID='2' and ven.EST_VENTA_ID=@estado and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad parcial"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaTotalmenteVendedorYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad total' from VENTA as ven where VEN.EST_VENTA_ID='3' and ven.EST_VENTA_ID=@estado and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad total"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMaximaVentasVendedorYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaVentasVendedorYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioVentasVendedorYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_VENTA_CANT) as 'Cantidad promedio vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinVendidasVendedorYEstado()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }

        private void fechaMaxVendidasVendedorYEstado()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.USUARIO_ID=@usuario and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }



        // ---------------------------------------------- POR CLIENTE --------------------------------------------------------------------------

        private void CantidadVentasPorCliente()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT det.VENTA_ID) as 'Cantidad de ventas' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de ventas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadesUnidadesVendidasCliente()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select SUM(det.DET_VENTA_CANT) as 'Cantidad vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadSinCobrosCliente()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad sin cobros' from VENTA as ven where VEN.EST_VENTA_ID='1' and ven.CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad sin cobros"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaParcialmenteCliente()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad parcial' from VENTA as ven where VEN.EST_VENTA_ID='2' and ven.CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad parcial"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaTotalmenteCliente()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad total' from VENTA as ven where VEN.EST_VENTA_ID='3' and ven.CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad total"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMaximaVentasCliente()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaVentasCliente()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioVentasCliente()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_VENTA_CANT) as 'Cantidad promedio vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinVendidasCliente()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }

        private void fechaMaxVendidasCliente()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }



        // ---------------------------------------------- POR TODOS PRODUCTOS ELABORADOS  --------------------------------------------------------------------------

        private void CantidadVentasTodosProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT det.VENTA_ID) as 'Cantidad de ventas' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de ventas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadesUnidadesVendidasTodosProdElab()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select SUM(det.DET_VENTA_CANT) as 'Cantidad vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadSinCobrosTodosProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad sin cobros' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = det.PROD_ELAB_ID  where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_ELAB_ID=@productoelaborado and VEN.EST_VENTA_ID='1' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad sin cobros"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaParcialmenteTodosProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad parcial' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = det.PROD_ELAB_ID  where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_ELAB_ID=@productoelaborado and VEN.EST_VENTA_ID='2' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad parcial"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaTotalmenteTodosProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad total' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = det.PROD_ELAB_ID  where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_ELAB_ID=@productoelaborado and VEN.EST_VENTA_ID='3' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad total"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMaximaVentasTodosProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaVentasTodosProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioVentasTodosProdElab()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_VENTA_CANT) as 'Cantidad promedio vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinVendidasTodosProdElab()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }

        private void fechaMaxVendidasTodosProdElab()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID= det.PROD_ELAB_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_ELAB_ID=@productoelaborado and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }



        // ------------------------------------------------------ POR TODO PRODUCTOS DE REVENTA ----------------------------------------


        private void CantidadVentasTodosProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT det.VENTA_ID) as 'Cantidad de ventas' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de ventas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void cantidadesUnidadesVendidasTodosProdRev()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select SUM(det.DET_VENTA_CANT) as 'Cantidad vendida' from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID= det.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID  where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadSinCobrosTodosProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad sin cobros' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_REV_ID=@productoreventa and VEN.EST_VENTA_ID='1' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad sin cobros"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadCobradaParcialmenteTodosProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad parcial' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID  where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_REV_ID=@productoreventa and VEN.EST_VENTA_ID='2' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad parcial"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadCobradaTotalmenteTodosProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(DISTINCT VEN.VENTA_ID) as 'Cantidad total' from VENTA as ven JOIN DETALLEVENTA as det on det.VENTA_ID=ven.VENTA_ID join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID  where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_REV_ID=@productoreventa and VEN.EST_VENTA_ID='3' and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad total"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void cantidadMaximaVentasTodosProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaVentasTodosProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioVentasTodosProdRev()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_VENTA_CANT) as 'Cantidad promedio vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio vendida"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "0";
            }
            else
            {
                cantidad = int.Parse(cant);
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void fechaMinVendidasTodosProdRev()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MIN(det.DET_VENTA_CANT) as 'Cantidad minima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }


        private void fechaMaxVendidasTodosProdRev()
        {
            conexion.Open();
            string sql = "select venta_fecha from VENTA as ven, DETALLEVENTA as det where det.VENTA_ID=ven.VENTA_ID and venta_fecha between @fechadesde AND @fechahasta and DET_VENTA_CANT = (SELECT MAX(det.DET_VENTA_CANT) as 'Cantidad maxima vendida' from DETALLEVENTA as det join PRODUCTOREVENTA as prev on prev.PROD_REV_ID= det.PROD_REV_ID join VENTA as ven on ven.VENTA_ID= det.VENTA_ID where ven.EST_VENTA_ID=@estado and ven.CLIENTE_ID=@cliente and ven.USUARIO_ID=@usuario and det.PROD_REV_ID=@productoreventa and RUBRO_ID=@rubro and MARCA_ID=@marca and VENTA_FECHA>=@fechadesde and VENTA_FECHA <=@fechahasta and det.VENTA_ID=ven.VENTA_ID) group by venta_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@cliente", SqlDbType.Int).Value = textBox1.Text;
            comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            comando.Parameters.Add("@productoreventa", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["venta_fecha"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }

        // QUITAR FILTROS

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button4.Enabled = false;
            chart1.Titles.Clear();
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart2.Titles.Clear();
            comboBox1.SelectedIndex = (-1);
            comboBox2.SelectedIndex = (-1);
            comboBox3.SelectedIndex = (-1);
            comboBox4.SelectedIndex = (-1);
            comboBox5.SelectedIndex = (-1);
            comboBox6.SelectedIndex = (-1);
            textBox1.Text = "";
            textBox3.Text = "";
            comboBox2.DataSource = null;
            comboBox2.Items.Clear();
            comboBox3.DataSource = null;
            comboBox3.Items.Clear();
            dateTimePicker1.Text = DateTime.Now.Date.ToString();
            dateTimePicker2.Text = DateTime.Now.ToString();
            label32.Text = "-";
            label33.Text = "-";
            label34.Text = "-";
            label35.Text = "-";
            label36.Text = "-";
            label42.Text = "-";
            label43.Text = "-";
            label44.Text = "-";
            label45.Text = "-";
            label46.Text = "-";
        }

        private void pictureBox3_Click_1(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "La búsqueda se puede filtrar por:\n" +
                "Rango de fecha, tipo de producto, rubro y marca.\n" +
                "Rango de fecha, tipo de producto, rubro, marca y producto.\n" +
                "Rango de fecha y vendedor.\n" +
                "Rango de fecha, vendedor y estado.\n" +
                "Rango de fecha y cliente. \n" + 
                "Rango de fecha, tipo de producto, rubro, marca, producto, vendedor, cliente y estado.";
            m.ShowDialog();
        }


        // GRAFICOS

        private void button4_Click(object sender, EventArgs e)
        {
            if (label32.Text == "---" || label32.Text == "0" || label32.Text=="-")

            {
                Aviso m = new Aviso();
                m.label1.Text = "Para dibujar el gráfico deben tener valores todos los conceptos";
                m.ShowDialog();
            }
            else
            {
                button4.Enabled = false;

                string[] series2 = { "Cant Sin Cobros", "Cant Cobrada Parcialmente", "Cant Cobrada Totalmente" };
                int[] puntos1 = { int.Parse(label34.Text), int.Parse(label35.Text), (int)float.Parse(label36.Text) };
                chart1.Palette = ChartColorPalette.Pastel;

                chart1.Titles.Add("Cantidades Estados de Cobro");
                for (int i = 0; i < series2.Length; i++)
                {
                    Series serie = chart1.Series.Add(series2[i]);
                    serie.Label = puntos1[i].ToString();
                    serie.Points.Add(puntos1[i]);

                }


                // GRAFICO 2
                string[] series = { "Cant Máxima", "Cant Mínima", " Cant Promedio"};
                int[] puntos = { int.Parse(label42.Text), int.Parse(label43.Text), (int)float.Parse(label44.Text)};

                chart2.Titles.Add("Cantidades Vendidas");
                for (int i = 0; i < series.Length; i++)
                {
                    Series serie = chart2.Series.Add(series[i]);
                    serie.Label = puntos[i].ToString();
                    serie.Points.Add(puntos[i]);
                }

            }
        }


    }
}
