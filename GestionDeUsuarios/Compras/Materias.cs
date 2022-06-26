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
    public partial class Materias : Form
    {
        private SqlConnection conexion = new SqlConnection("Data Source=SAM;Initial Catalog=bdSS;Integrated Security=True");
        public Materias()
        {
            InitializeComponent();
        }

        private void Materias_Load(object sender, EventArgs e)
        {
            cargarComboBox1();
            cargarComboBox2();
            mostrarGrilla();
            button3.Enabled = false;
            textBox2.Enabled = false;
            textBox2.Visible = false;
            label1.Visible = false;

            pictureBox2.Visible = false;
            button3.Visible = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.SelectedValue==null || comboBox2.SelectedValue==null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No puede quedar ningún campo vacío";
                m.ShowDialog();
            }
            else if (!ExisteMateria(textBox1.Text))
            {
                conexion.Open();
                string sql = "insert into MATERIAPRIMA (MATERIAPR_DESCR, RUBRO_ID , MARCA_ID , MATERIAPR_FECHAVENC , MATERIAPR_STOCKMIN) values (@materiadesc,@rubroid,@marcaid,@materiafechavenc,@materiastockmin)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@materiadesc", SqlDbType.VarChar).Value = textBox1.Text;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox2.SelectedValue.ToString();
                comando.Parameters.Add("@materiafechavenc", SqlDbType.DateTime).Value = dateTimePicker1.Value;
                comando.Parameters.Add("@materiastockmin", SqlDbType.Int).Value = numericUpDown1.Value;
                comando.ExecuteNonQuery();
                textBox1.Text = "";
                comboBox1.SelectedValue ="0";
                comboBox2.SelectedValue = "0";
                dateTimePicker1.Value = DateTime.Now;
                numericUpDown1.Value = 0;
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "La Materia Prima fue registrada";
                m.ShowDialog();
                mostrarGrilla();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya existe una Materia Prima con esas características";
                m.ShowDialog();
            }
        }

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select MATERIAPR_DESCR,RUBRO_NOMBRE,MARCA_NOMBRE, MATERIAPR_FECHAVENC, MATERIAPR_STOCKMIN from MATERIAPRIMA as mat join RUBRO as rub on rub.RUBRO_ID = mat.RUBRO_ID join MARCA as mar on mar.MARCA_ID = mat.MARCA_ID ORDER BY MATERIAPR_DESCR ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["MATERIAPR_DESCR"].ToString(),
                 registros["RUBRO_NOMBRE"].ToString(), registros["MARCA_NOMBRE"].ToString(),
                 registros["MATERIAPR_FECHAVENC"], registros["MATERIAPR_STOCKMIN"].ToString());
            }
            registros.Close();
            conexion.Close();
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

        private void button3_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "" || comboBox1.SelectedValue==null || comboBox2.SelectedValue==null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No puede estar vacío el nombre o descripción";
                m.ShowDialog();
            }
            else if (!ExisteMateriaModificar())
            {
             conexion.Open();
             string sql = "update MATERIAPRIMA set MATERIAPR_DESCR=@desc, RUBRO_ID=@rubroid, MARCA_ID=@marcaid, MATERIAPR_FECHAVENC=@materiafechavenc, MATERIAPR_STOCKMIN=@materiastockmin where MATERIAPR_ID=@materiaid";
             SqlCommand comando = new SqlCommand(sql, conexion);
             comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = textBox1.Text;
             comando.Parameters.Add("@materiaid", SqlDbType.Int).Value = textBox2.Text;
             comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
             comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox2.SelectedValue.ToString();
             comando.Parameters.Add("@materiafechavenc", SqlDbType.DateTime).Value = dateTimePicker1.Value;
             comando.Parameters.Add("@materiastockmin", SqlDbType.Int).Value = numericUpDown1.Value;
             comando.ExecuteNonQuery();
             conexion.Close();
             textBox1.Text = "";
             textBox2.Text = "";
             comboBox1.SelectedValue = "0";
             comboBox2.SelectedValue = "0";
             dateTimePicker1.Value = DateTime.Now;
             numericUpDown1.Value = 0;
                Aviso m = new Aviso();
                m.label1.Text = "Se ha modificado la Materia Prima";
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
                m.label1.Text = "Ya existe una Materia prima con esas características";
                m.ShowDialog();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private bool ExisteMateria(string MateriaPrima)
        {
            conexion.Open();
            string sql = "select MATERIAPR_DESCR from MATERIAPRIMA where MATERIAPR_DESCR=@desc and RUBRO_ID=@rubro and MARCA_ID=@marca";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = MateriaPrima;
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

        private bool ExisteMateriaModificar()
        {
            conexion.Open();
            string sql = "select MATERIAPR_DESCR from MATERIAPRIMA where MATERIAPR_DESCR=@desc and RUBRO_ID=@rubro and MARCA_ID=@marca and MATERIAPR_ID!=@id";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = textBox1.Text;
            comando.Parameters.Add("@id", SqlDbType.Int).Value = textBox2.Text;
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
                m.label1.Text = "No pueden quedar vacíos descripción, rubro y marca";
                m.ShowDialog();
            }
            else if (ExisteMateria(textBox1.Text))
            {
                button2.Enabled = false;
                conexion.Open();
                string sql = "select MATERIAPR_ID,MATERIAPR_DESCR,RUBRO_NOMBRE,mat.RUBRO_ID,mat.MARCA_ID,MARCA_NOMBRE, MATERIAPR_FECHAVENC, MATERIAPR_STOCKMIN from MATERIAPRIMA as mat join RUBRO as rub on rub.RUBRO_ID = mat.RUBRO_ID join MARCA as mar on mar.MARCA_ID = mat.MARCA_ID where MATERIAPR_DESCR=@desc and mat.RUBRO_ID=@rubro and mat.MARCA_ID=@marca";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = textBox1.Text;
                comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
                comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue.ToString();
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    textBox2.Text = registros["MATERIAPR_ID"].ToString();
                    comboBox1.SelectedValue = registros["RUBRO_ID"].ToString();
                    comboBox2.SelectedValue = registros["MARCA_ID"].ToString();
                    dateTimePicker1.Value = Convert.ToDateTime(registros["MATERIAPR_FECHAVENC"]);
                    string cantidad = registros["MATERIAPR_STOCKMIN"].ToString();
                    int cant = int.Parse(cantidad);
                    numericUpDown1.Value = cant;

                    dataGridView1.Rows.Add(registros["MATERIAPR_DESCR"].ToString(),
                        registros["RUBRO_NOMBRE"].ToString(), 
                        registros["MARCA_NOMBRE"].ToString(),
                        registros["MATERIAPR_FECHAVENC"],
                        registros["MATERIAPR_STOCKMIN"].ToString());
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
                m.label1.Text = "No existe una Materia Prima con ese nombre";
                m.ShowDialog();
            }
        }
    }
}
