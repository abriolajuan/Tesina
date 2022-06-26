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

namespace GestionDeUsuarios.Producción
{
    public partial class ProdElab : Form
    {
        private SqlConnection conexion = new SqlConnection("Data Source=SAM;Initial Catalog=bdSS;Integrated Security=True");
        public ProdElab()
        {
            InitializeComponent();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            float valor1 = 0;
            float valor2 = 0;
            if (textBox1.Text == "" && textBox2.Text == "" && comboBox1.SelectedValue == null && comboBox2.SelectedValue == null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No puede quedar ningún campo vacío";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedValue == null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un rubro";
                m.ShowDialog();
            }
            else if (comboBox2.SelectedValue == null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar una marca";
                m.ShowDialog();
            }
            else if (textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un nombre / descripción";
                m.ShowDialog();
            }
            else if (textBox2.Text == "0" || textBox2.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un precio mayor a $0";
                m.ShowDialog();
            }
            else if (textBox4.Text != "" && textBox2.Text != "")
            {
                valor1 = float.Parse(textBox2.Text);
                valor2 = float.Parse(textBox4.Text);
                if (valor1 < valor2)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "El monto del descuento no puede ser mayor al precio del producto";
                    m.ShowDialog();
                }
                else if (!ExisteProducto(textBox1.Text))
                {
                    conexion.Open();
                    string sql = "insert into PRODUCTOELAB (PROD_ELAB_DESCR, RUBRO_ID , MARCA_ID , PROD_ELAB_PR_UNIT, PROD_ELAB_DTO) values (@prodesc,@rubroid,@marcaid,@prodelabpr, @descuento)";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@prodesc", SqlDbType.VarChar).Value = textBox1.Text;
                    comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
                    comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox2.SelectedValue.ToString();
                    comando.Parameters.Add("@prodelabpr", SqlDbType.VarChar).Value = textBox2.Text;
                    comando.Parameters.Add("@descuento", SqlDbType.VarChar).Value = textBox4.Text;
                    comando.ExecuteNonQuery();
                    textBox1.Text = "";
                    comboBox1.SelectedValue = "0";
                    comboBox2.SelectedValue = "0";
                    textBox2.Text = "";
                    textBox4.Text = "";
                    conexion.Close();
                    Aviso m = new Aviso();
                    m.label1.Text = "El Producto Elaborado fue registrado";
                    m.ShowDialog();
                    mostrarGrilla();
                }
                else
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Ya existe un Producto Elaborado con esas características";
                    m.ShowDialog();
                }
            }
            else if (!ExisteProducto(textBox1.Text))
            {
                conexion.Open();
                string sql = "insert into PRODUCTOELAB (PROD_ELAB_DESCR, RUBRO_ID , MARCA_ID , PROD_ELAB_PR_UNIT, PROD_ELAB_DTO) values (@prodesc,@rubroid,@marcaid,@prodelabpr, @descuento)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@prodesc", SqlDbType.VarChar).Value = textBox1.Text;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox2.SelectedValue.ToString();
                comando.Parameters.Add("@prodelabpr", SqlDbType.VarChar).Value = textBox2.Text;
                comando.Parameters.Add("@descuento", SqlDbType.VarChar).Value = textBox4.Text;
                comando.ExecuteNonQuery();
                textBox1.Text = "";
                comboBox1.SelectedValue = "0";
                comboBox2.SelectedValue = "0";
                textBox2.Text = "";
                textBox4.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "El Producto Elaborado fue registrado";
                m.ShowDialog();
                mostrarGrilla();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya existe un Producto Elaborado con esas características";
                m.ShowDialog();
            }
        }

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select sum(PROD_ELAB_PR_UNIT - ISNULL(pr.PROD_ELAB_DTO,0)) as totaldesc ,PROD_ELAB_ID, PROD_ELAB_DESCR,PROD_ELAB_PR_UNIT, PR.RUBRO_ID, PR.MARCA_ID, RUBRO_NOMBRE, MARCA_NOMBRE,ISNULL(pr.PROD_ELAB_DTO,0) as PROD_ELAB_DTO from PRODUCTOELAB as pr join RUBRO as rub on rub.RUBRO_ID = pr.RUBRO_ID join MARCA as mar on mar.MARCA_ID = pr.MARCA_ID group by PROD_ELAB_ID, PROD_ELAB_DESCR,PROD_ELAB_PR_UNIT, PR.RUBRO_ID, PR.MARCA_ID, RUBRO_NOMBRE, MARCA_NOMBRE, PROD_ELAB_DTO ORDER BY PROD_ELAB_DESCR ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PROD_ELAB_DESCR"].ToString(),
                       registros["RUBRO_NOMBRE"].ToString(),
                       registros["MARCA_NOMBRE"].ToString(),
                       registros["PROD_ELAB_PR_UNIT"].ToString(),
                       registros["PROD_ELAB_DTO"].ToString(),
                       registros["totaldesc"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private bool ExisteProducto(string ProductoElaborado)
        {
            conexion.Open();
            string sql = "select PROD_ELAB_DESCR from PRODUCTOELAB where PROD_ELAB_DESCR=@desc and RUBRO_ID=@rubro and MARCA_ID=@MARCA";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = ProductoElaborado;
            comando.Parameters.Add("@rubro", SqlDbType.VarChar).Value = comboBox1.SelectedValue.ToString();
            comando.Parameters.Add("@marca", SqlDbType.VarChar).Value = comboBox2.SelectedValue.ToString();
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private bool ExisteProductoModificar()
        {
            conexion.Open();
            string sql = "select PROD_ELAB_DESCR from PRODUCTOELAB where PROD_ELAB_DESCR=@desc and RUBRO_ID=@rubro and MARCA_ID=@marca and PROD_ELAB_ID!=@id";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = textBox1.Text;
            comando.Parameters.Add("@id", SqlDbType.Int).Value = textBox3.Text;
            comando.Parameters.Add("@rubro", SqlDbType.VarChar).Value = comboBox1.SelectedValue.ToString();
            comando.Parameters.Add("@marca", SqlDbType.VarChar).Value = comboBox2.SelectedValue.ToString();
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private void cargarComboBox1()
        {
            conexion.Open();
            string sql = "select RUBRO_ID, RUBRO_NOMBRE from RUBRO ORDER BY RUBRO_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            comboBox1.DisplayMember = "RUBRO_NOMBRE";
            comboBox1.ValueMember = "RUBRO_ID";
            comboBox1.DataSource = tabla1;
            conexion.Close();
        }

        private void cargarComboBox2()
        {
            conexion.Open();
            string sql = "select MARCA_ID, MARCA_NOMBRE from MARCA WHERE MARCA_NOMBRE='MTA25'";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            comboBox2.DisplayMember = "MARCA_NOMBRE";
            comboBox2.ValueMember = "MARCA_ID";
            comboBox2.DataSource = tabla1;
            conexion.Close();
        }

        private void ProdElab_Load(object sender, EventArgs e)
        {
            cargarComboBox1();
            cargarComboBox2();
            button3.Enabled = false;
           // mostrarGrilla();
            label1.Visible = false;
            textBox3.Visible = false;
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;

            pictureBox2.Visible = false;
            button3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.SelectedValue == null || comboBox2.SelectedValue == null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No pueden quedar vacíos los campos de descripción, rubro y marca";
                m.ShowDialog();
            }
            else if (ExisteProducto(textBox1.Text))
            {
                button2.Enabled = false;
                conexion.Open();
                string sql = "select sum(PROD_ELAB_PR_UNIT - ISNULL(pr.PROD_ELAB_DTO,0)) as totaldesc ,PROD_ELAB_ID, PROD_ELAB_DESCR,PROD_ELAB_PR_UNIT, PR.RUBRO_ID, PR.MARCA_ID, RUBRO_NOMBRE, MARCA_NOMBRE,ISNULL(pr.PROD_ELAB_DTO,0) as PROD_ELAB_DTO from PRODUCTOELAB as pr join RUBRO as rub on rub.RUBRO_ID = pr.RUBRO_ID join MARCA as mar on mar.MARCA_ID = pr.MARCA_ID where PROD_ELAB_DESCR=@desc and pr.RUBRO_ID=@RUBRO AND pr.MARCA_ID=@MARCA group by PROD_ELAB_ID, PROD_ELAB_DESCR,PROD_ELAB_PR_UNIT, PR.RUBRO_ID, PR.MARCA_ID, RUBRO_NOMBRE, MARCA_NOMBRE, PROD_ELAB_DTO ORDER BY PROD_ELAB_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = textBox1.Text;
                comando.Parameters.Add("@rubro", SqlDbType.VarChar).Value = comboBox1.SelectedValue.ToString();
                comando.Parameters.Add("@marca", SqlDbType.VarChar).Value = comboBox2.SelectedValue.ToString();
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    textBox3.Text = registros["PROD_ELAB_ID"].ToString();
                    textBox1.Text = registros["PROD_ELAB_DESCR"].ToString();
                    comboBox1.SelectedValue = registros["RUBRO_ID"].ToString();
                    comboBox2.SelectedValue = registros["MARCA_ID"].ToString();
                    textBox2.Text = registros["PROD_ELAB_PR_UNIT"].ToString();
                    textBox4.Text = registros["PROD_ELAB_DTO"].ToString();

                    dataGridView1.Rows.Add(registros["PROD_ELAB_DESCR"].ToString(),
                       registros["RUBRO_NOMBRE"].ToString(),
                       registros["MARCA_NOMBRE"].ToString(),
                       registros["PROD_ELAB_PR_UNIT"].ToString(),
                       registros["PROD_ELAB_DTO"].ToString(),
                       registros["totaldesc"].ToString());

                }
                registros.Close();
                conexion.Close();
                button3.Enabled = true;
                button1.Enabled = false;

                pictureBox2.Visible = true;
                button3.Visible = true;
                pictureBox1.Visible = false;
                button2.Visible = false;
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "No existe un Producto Elaborado con esas características";
                m.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            float valor1 = 0;
            float valor2 = 0;
            if (textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un Nombre / Descripción";
                m.ShowDialog();
            }
            else if (textBox2.Text == "0" || textBox2.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un precio mayor a $0";
                m.ShowDialog();
            }
            else if (textBox4.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar un descuento mayor o igual a $0";
                m.ShowDialog();
            }
            else if (textBox1.Text != "" && textBox2.Text != "")
            {
                valor1 = float.Parse(textBox2.Text);
                valor2 = float.Parse(textBox4.Text);
                if (valor1 < valor2)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "El monto del descuento no puede ser mayor al precio del producto";
                    m.ShowDialog();
                }
                else if (!ExisteProductoModificar())
                {
                    conexion.Open();
                    string sql = "update PRODUCTOELAB set PROD_ELAB_DESCR=@desc, PROD_ELAB_PR_UNIT=@preciounitario, RUBRO_ID=@rubro, MARCA_ID=@marca, PROD_ELAB_DTO=@descuento where PROD_ELAB_ID=@prodelabid";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = textBox1.Text;
                    comando.Parameters.Add("@prodelabid", SqlDbType.Int).Value = textBox3.Text;
                    comando.Parameters.Add("@preciounitario", SqlDbType.Float).Value = textBox2.Text;
                    comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
                    comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
                    comando.Parameters.Add("@descuento", SqlDbType.Float).Value = textBox4.Text;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    comboBox1.SelectedValue = "0";
                    comboBox2.SelectedValue = "0";
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    Aviso m = new Aviso();
                    m.label1.Text = "Se ha modificado el producto";
                    m.ShowDialog();
                    dataGridView1.Rows.Clear();
                    mostrarGrilla();
                    button1.Enabled = true;
                    button2.Enabled = true;
                    button3.Enabled = false;
                    textBox1.Enabled = true;

                    pictureBox2.Visible = false;
                    button3.Visible = false;
                    pictureBox1.Visible = true;
                    button2.Visible = true;
                }
                else
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Ya existe un Producto Elaborado con esas características";
                    m.ShowDialog();
                }
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void mostrarGrillaRubro()
        {
                conexion.Open();
                string sql = "select sum(PROD_ELAB_PR_UNIT - ISNULL(pr.PROD_ELAB_DTO,0)) as totaldesc ,PROD_ELAB_ID, PROD_ELAB_DESCR,PROD_ELAB_PR_UNIT, PR.RUBRO_ID, PR.MARCA_ID, RUBRO_NOMBRE, MARCA_NOMBRE, ISNULL(pr.PROD_ELAB_DTO,0) as PROD_ELAB_DTO from PRODUCTOELAB as pr join RUBRO as rub on rub.RUBRO_ID = pr.RUBRO_ID join MARCA as mar on mar.MARCA_ID = pr.MARCA_ID  WHERE PR.RUBRO_ID= @rubroid group by PROD_ELAB_ID, PROD_ELAB_DESCR,PROD_ELAB_PR_UNIT, PR.RUBRO_ID, PR.MARCA_ID, RUBRO_NOMBRE, MARCA_NOMBRE, PROD_ELAB_DTO ORDER BY PROD_ELAB_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["PROD_ELAB_DESCR"].ToString(),
                           registros["RUBRO_NOMBRE"].ToString(),
                           registros["MARCA_NOMBRE"].ToString(),
                           registros["PROD_ELAB_PR_UNIT"].ToString(),
                           registros["PROD_ELAB_DTO"].ToString(),
                           registros["totaldesc"].ToString());
                }
                registros.Close();
                conexion.Close();
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == -1)
            {
                conexion.Close();
                mostrarGrilla();
            }
            else
            {
                conexion.Close();
                mostrarGrillaRubro();
            }
        }
    }
}
