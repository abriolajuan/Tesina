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
    public partial class ProdReventaMain : Form

    {
        private SqlConnection conexion = new SqlConnection("Data Source=SAM;Initial Catalog=bdSS;Integrated Security=True");

        public ProdReventaMain()
        {
            InitializeComponent();
        }

        private void ProdReventaMain_Load(object sender, EventArgs e)
        {
            textBox3.Enabled = false;
            cargarComboBox1();
            cargarComboBox2();
            mostrarGrilla();
            button3.Enabled = false;
            label2.Visible = false;
            textBox3.Visible = false;

            pictureBox2.Visible = false;
            button3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            float valor1 = 0;
            float valor2 = 0;
            if (textBox1.Text == "" || textBox2.Text == "" || comboBox1.SelectedValue == null || comboBox2.SelectedValue == null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No puede quedar ningún campo vacío";
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
                    string sql = "insert into PRODUCTOREVENTA (PROD_REV_DESCR, RUBRO_ID , MARCA_ID , PROD_REV_PR_UNIT, PROD_REV_CANT, PROD_REV_DTO) values (@prodesc,@rubroid,@marcaid,@prodrevpr,@prodrevcant, @descuento)";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@prodesc", SqlDbType.VarChar).Value = textBox1.Text;
                    comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
                    comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox2.SelectedValue.ToString();
                    comando.Parameters.Add("@prodrevpr", SqlDbType.VarChar).Value = textBox2.Text;
                    comando.Parameters.Add("@prodrevcant", SqlDbType.Int).Value = numericUpDown1.Value;
                    comando.Parameters.Add("@descuento", SqlDbType.VarChar).Value = textBox4.Text;
                    comando.ExecuteNonQuery();
                    textBox1.Text = "";
                    comboBox1.SelectedValue = "0";
                    comboBox2.SelectedValue = "0";
                    textBox2.Text = "";
                    textBox4.Text = "";
                    numericUpDown1.Value = 0;
                    conexion.Close();
                    Aviso m = new Aviso();
                    m.label1.Text = "El Producto de Reventa fue registrado";
                    m.ShowDialog();
                    mostrarGrilla();
                }
                else
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Ya existe un producto de reventa con esas características";
                    m.ShowDialog();
                }
            }
            else if (!ExisteProducto(textBox1.Text))
            {
                conexion.Open();
                string sql = "insert into PRODUCTOREVENTA (PROD_REV_DESCR, RUBRO_ID , MARCA_ID , PROD_REV_PR_UNIT, PROD_REV_CANT, PROD_REV_DTO) values (@prodesc,@rubroid,@marcaid,@prodrevpr,@prodrevcant, @descuento)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@prodesc", SqlDbType.VarChar).Value = textBox1.Text;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox2.SelectedValue.ToString();
                comando.Parameters.Add("@prodrevpr", SqlDbType.VarChar).Value = textBox2.Text;
                comando.Parameters.Add("@prodrevcant", SqlDbType.Int).Value = numericUpDown1.Value;
                comando.Parameters.Add("@descuento", SqlDbType.VarChar).Value = textBox4.Text;
                comando.ExecuteNonQuery();
                textBox1.Text = "";
                comboBox1.SelectedValue = "0";
                comboBox2.SelectedValue = "0";
                textBox2.Text = "";
                textBox4.Text = "";
                numericUpDown1.Value = 0;
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "El Producto de Reventa fue registrado";
                m.ShowDialog();
                mostrarGrilla();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya existe un producto de reventa con esas características";
                m.ShowDialog();
            }
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
            string sql = "select MARCA_ID, MARCA_NOMBRE from MARCA ORDER BY MARCA_NOMBRE ASC";
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

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select sum(PROD_REV_PR_UNIT - ISNULL(pr.PROD_REV_DTO,0)) as totaldesc, PROD_REV_ID, PROD_REV_DESCR,PROD_REV_PR_UNIT,PROD_REV_CANT, ISNULL(pr.PROD_REV_DTO,0) as PROD_REV_DTO, PR.RUBRO_ID, PR.MARCA_ID, RUBRO_NOMBRE, MARCA_NOMBRE from PRODUCTOREVENTA as pr join RUBRO as rub on rub.RUBRO_ID = pr.RUBRO_ID join MARCA as mar on mar.MARCA_ID = pr.MARCA_ID group by PROD_REV_ID, PROD_REV_DESCR,PROD_REV_PR_UNIT,PROD_REV_CANT, PROD_REV_DTO,PR.RUBRO_ID, PR.MARCA_ID, RUBRO_NOMBRE, MARCA_NOMBRE ORDER BY PROD_REV_DESCR ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PROD_REV_DESCR"].ToString(),
                       registros["RUBRO_NOMBRE"].ToString(),
                       registros["MARCA_NOMBRE"].ToString(),
                       registros["PROD_REV_PR_UNIT"],
                       registros["PROD_REV_DTO"],
                       registros["totaldesc"],
                       registros["PROD_REV_CANT"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        public int identificadorIdProducto()
        {
                int idProducto;
                conexion.Open();
                string sql = "select PROD_REV_ID from PRODUCTOREVENTA where PROD_REV_DESCR=@descripcion";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@descripcion", SqlDbType.VarChar).Value = textBox1.Text;
                SqlDataReader registro = comando.ExecuteReader();
                registro.Read();
                string idProd = registro["PROD_REV_ID"].ToString();
                idProducto = int.Parse(idProd);
                conexion.Close();
                return idProducto;
        }

        private bool ExisteProducto(string ProductoReventa)
        {
            conexion.Open();
            string sql = "select PROD_REV_DESCR from PRODUCTOREVENTA where PROD_REV_DESCR=@desc and RUBRO_ID=@rubro and MARCA_ID=@MARCA";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = ProductoReventa;
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
            string sql = "select PROD_REV_DESCR from PRODUCTOREVENTA where PROD_REV_DESCR=@desc and RUBRO_ID=@rubro and MARCA_ID=@MARCA and PROD_REV_ID!=@id";
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

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.SelectedValue==null || comboBox2.SelectedValue==null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No pueden quedar vacíos los campos de descripción, rubro y marca";
                m.ShowDialog();
            }
            else if (ExisteProducto(textBox1.Text))
            {
                button2.Enabled = false;
                conexion.Open();
                string sql = "select sum(PROD_REV_PR_UNIT - ISNULL(pr.PROD_REV_DTO,0)) as totaldesc, PROD_REV_ID, PROD_REV_DESCR,PROD_REV_PR_UNIT,PROD_REV_CANT, ISNULL(pr.PROD_REV_DTO,0) as PROD_REV_DTO, PR.RUBRO_ID, PR.MARCA_ID, RUBRO_NOMBRE, MARCA_NOMBRE from PRODUCTOREVENTA as pr join RUBRO as rub on rub.RUBRO_ID = pr.RUBRO_ID join MARCA as mar on mar.MARCA_ID = pr.MARCA_ID  where PROD_REV_DESCR=@desc and pr.RUBRO_ID=@RUBRO AND pr.MARCA_ID=@MARCA group by PROD_REV_ID, PROD_REV_DESCR,PROD_REV_PR_UNIT,PROD_REV_CANT, PROD_REV_DTO,PR.RUBRO_ID, PR.MARCA_ID, RUBRO_NOMBRE, MARCA_NOMBRE ORDER BY PROD_REV_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = textBox1.Text;
                comando.Parameters.Add("@rubro", SqlDbType.VarChar).Value = comboBox1.SelectedValue.ToString();
                comando.Parameters.Add("@marca", SqlDbType.VarChar).Value = comboBox2.SelectedValue.ToString();
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    textBox3.Text = registros["PROD_REV_ID"].ToString();
                    textBox1.Text = registros["PROD_REV_DESCR"].ToString();
                    comboBox1.SelectedValue = registros["RUBRO_ID"].ToString();
                    comboBox2.SelectedValue = registros["MARCA_ID"].ToString();
                    textBox2.Text = registros["PROD_REV_PR_UNIT"].ToString();
                    textBox4.Text = registros["PROD_REV_DTO"].ToString();
                    string cantidad = registros["PROD_REV_CANT"].ToString();
                    int cant = int.Parse(cantidad);
                    numericUpDown1.Value = cant;

                    dataGridView1.Rows.Add(registros["PROD_REV_DESCR"].ToString(),
                       registros["RUBRO_NOMBRE"].ToString(),
                       registros["MARCA_NOMBRE"].ToString(),
                       registros["PROD_REV_PR_UNIT"],
                       registros["PROD_REV_DTO"],
                       registros["totaldesc"],
                       registros["PROD_REV_CANT"].ToString());

                }
                registros.Close();
                conexion.Close();
                button3.Enabled = true;

                pictureBox2.Visible = true;
                button3.Visible = true;
                pictureBox1.Visible = false;
                button2.Visible = false;
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "No existe un producto de reventa con esas características";
                m.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            float valor1 = 0;
            float valor2 = 0;
            if (textBox1.Text == "" || textBox2.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para modificar un producto tienen que estar todos los campos cargados";
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
                else if (!ExisteProductoModificar())
                {
                    conexion.Open();
                    string sql = "update PRODUCTOREVENTA set PROD_REV_DESCR=@desc, PROD_REV_PR_UNIT=@preciounitario, PROD_REV_DTO=@descuento , PROD_REV_CANT=@cantidad, RUBRO_ID=@rubro, MARCA_ID=@marca where PROD_REV_ID=@prodrevid";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = textBox1.Text;
                    comando.Parameters.Add("@prodrevid", SqlDbType.Int).Value = textBox3.Text;
                    comando.Parameters.Add("@preciounitario", SqlDbType.VarChar).Value = textBox2.Text;
                    comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
                    comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
                    comando.Parameters.Add("@cantidad", SqlDbType.Int).Value = numericUpDown1.Value;
                    comando.Parameters.Add("@descuento", SqlDbType.VarChar).Value = textBox4.Text;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    comboBox1.SelectedValue = "0";
                    comboBox2.SelectedValue = "0";
                    textBox1.Text = "";
                    textBox2.Text = "";
                    textBox3.Text = "";
                    textBox4.Text = "";
                    numericUpDown1.Value = 0;
                    Aviso m = new Aviso();
                    m.label1.Text = "Se ha modificado el producto";
                    m.ShowDialog();
                    dataGridView1.Rows.Clear();
                    mostrarGrilla();
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
                    m.label1.Text = "Ya existe un Producto de reventa con esas características";
                    m.ShowDialog();
                }
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }
    }
}
