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
    public partial class Elaboracion : Form
    {
        private SqlConnection conexion = new SqlConnection("Data Source=SAM;Initial Catalog=bdSS;Integrated Security=True");

        string tipoActivo;
        string nombreUsuActivo;
        string apellidoUsuActivo;
        DateTime fechaHoy = DateTime.Now.Date;
        int cantidadElab;
        DateTime fechaEliminar;

        public Elaboracion(string tipoActivo2, string nombreActivo, string apellidoActivo)
        {
            InitializeComponent();
            tipoActivo = tipoActivo2;
            nombreUsuActivo = nombreActivo;
            apellidoUsuActivo = apellidoActivo;
        }


        public void ActualizarEstados2Intento()
        {
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                DateTime fechaObj = Convert.ToDateTime(dataGridView1.Rows[i].Cells[4].Value);
                int cantObj = Convert.ToInt32(dataGridView1.Rows[i].Cells[5].Value);
                int cantElab = Convert.ToInt32(dataGridView1.Rows[i].Cells[6].Value);

                if (cantElab == 0 && fechaHoy <= fechaObj) // PROGRAMADA
                {
                    conexion.Open();
                    string sql = "update PRODUCCION set EST_PROD_ID='1' where PRODUCCION_CANT_ELAB='0' and PRODUCCION_FECHA_OBJ>=@fecha";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@fecha", SqlDbType.Date).Value = fechaHoy;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                }
                else if ((cantElab > 0 || cantElab == cantObj) && fechaHoy <= fechaObj) // EN ELABORACIÓN // 
                {
                    conexion.Open();
                    string sql = "update PRODUCCION set EST_PROD_ID='2' where PRODUCCION_CANT_ELAB>0 and PRODUCCION_FECHA_OBJ>=@fecha";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@fecha", SqlDbType.Date).Value = fechaHoy;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                }
                else if (cantElab < cantObj && fechaHoy > fechaObj) // INCOMPLETA
                {
                    conexion.Open();
                    string sql = "update PRODUCCION set EST_PROD_ID='3' where PRODUCCION_CANT_ELAB< PRODUCCION_CANT_OBJ and PRODUCCION_FECHA_OBJ<@fecha";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@fecha", SqlDbType.Date).Value = fechaHoy;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                }
                else if (cantElab == cantObj && fechaHoy > fechaObj) // COMPLETA
                {
                    conexion.Open();
                    string sql = "update PRODUCCION set EST_PROD_ID='4' where PRODUCCION_CANT_ELAB = PRODUCCION_CANT_OBJ and PRODUCCION_FECHA_OBJ<@fecha";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@fecha", SqlDbType.Date).Value = fechaHoy;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                }
                else if (cantElab > cantObj && fechaHoy > fechaObj) // SUPERADA
                {
                    conexion.Open();
                    string sql = "update PRODUCCION set EST_PROD_ID='5' where PRODUCCION_CANT_ELAB > PRODUCCION_CANT_OBJ and PRODUCCION_FECHA_OBJ<@fecha";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@fecha", SqlDbType.Date).Value = fechaHoy;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                }
            }
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Elaboracion_Load(object sender, EventArgs e)
        {
            cargarcomboBox1();
            cargarcomboBox2();
            cargarcomboBox3();
            cargarComboBox4();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            mostrarGrilla();
            ActualizarEstados2Intento();
            mostrarGrilla();
            button3.Enabled = false;
            label1.Visible = false;
            textBox2.Visible = false;
            button6.Enabled = false;
            cargarEstados();

            button3.Visible = false;
            button6.Visible = false;
            pictureBox5.Visible = false;
        }

        private void cargarEstados()
        {
            conexion.Open();
            string sql = "select COUNT(*) EST_PROD_NOMBRE from ESTADOPROD where EST_PROD_NOMBRE='Programada'";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                string cant = registro["EST_PROD_NOMBRE"].ToString();
                int cantidadEstado = int.Parse(cant.ToString());

                registro.Close();
                conexion.Close();
                if (cantidadEstado == 0)
                {
                    conexion.Open();
                    string sql1 = "insert into ESTADOPROD(EST_PROD_NOMBRE) values ('Programada')";
                    string sql2 = "insert into ESTADOPROD(EST_PROD_NOMBRE) values ('En elaboración')";
                    string sql3 = "insert into ESTADOPROD(EST_PROD_NOMBRE) values ('Incompleta')";
                    string sql4 = "insert into ESTADOPROD(EST_PROD_NOMBRE) values ('Completa')";
                    string sql5 = "insert into ESTADOPROD(EST_PROD_NOMBRE) values ('Superada')";
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
                }
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedValue == null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe elegir un rubro";
                m.ShowDialog();
            }
            else if (comboBox2.SelectedValue == null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe elegir una marca";
                m.ShowDialog();
            }
            else if (comboBox3.SelectedValue == null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe elegir un producto";
                m.ShowDialog();
            }
            else if (comboBox4.SelectedValue == null)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe elegir un cocinero";
                m.ShowDialog();
            }
            else if (numericUpDown1.Value == 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar una cantidad objetivo";
                m.ShowDialog();
            }
            else if (dateTimePicker1.Value <= fechaHoy)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede dar de alta una elaboración con una fecha objetivo \n " +
                    "anterior a " + fechaHoy.ToShortDateString().ToString();
                m.ShowDialog();
            }
            else if (!ExisteElaboracion() && numericUpDown2.Value == 0 && fechaHoy <= dateTimePicker1.Value) 
            {
                conexion.Open();
                string sql = "insert into PRODUCCION (PROD_ELAB_ID, PRODUCCION_FECHA_REG, PRODUCCION_FECHA_OBJ, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB,USUARIO_ID,EST_PROD_ID) values (@prodelabid,@fechareg,@fechaobj,@cantidadobj,@cantidadelab,@usuarioid,@estadoid)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@prodelabid", SqlDbType.Int).Value = comboBox3.SelectedValue.ToString();
                comando.Parameters.Add("@fechareg", SqlDbType.Date).Value = fechaHoy;
                comando.Parameters.Add("@fechaobj", SqlDbType.Date).Value = dateTimePicker1.Value;
                comando.Parameters.Add("@cantidadobj", SqlDbType.Int).Value = numericUpDown1.Value;
                comando.Parameters.Add("@cantidadelab", SqlDbType.Int).Value = numericUpDown2.Value;
                comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = comboBox4.SelectedValue.ToString();
                comando.Parameters.Add("@estadoid", SqlDbType.Int).Value = "1";
                comando.ExecuteNonQuery();
                comboBox1.SelectedValue = "0";
                comboBox2.SelectedValue = "0";
                comboBox3.SelectedValue = "0";
                comboBox4.SelectedValue = "0";
                dateTimePicker1.Value = DateTime.Now;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                textBox2.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "La elaboración fue registrada";
                m.ShowDialog();
                mostrarGrilla();
            }
            else if (!ExisteElaboracion() && numericUpDown2.Value>0 && fechaHoy <= dateTimePicker1.Value)
            {
                conexion.Open();
                string sql = "insert into PRODUCCION (PROD_ELAB_ID, PRODUCCION_FECHA_REG, PRODUCCION_FECHA_OBJ, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB,USUARIO_ID,EST_PROD_ID) values (@prodelabid,@fechareg,@fechaobj,@cantidadobj,@cantidadelab,@usuarioid,@estadoid)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@prodelabid", SqlDbType.Int).Value = comboBox3.SelectedValue.ToString();
                comando.Parameters.Add("@fechareg", SqlDbType.Date).Value = fechaHoy;
                comando.Parameters.Add("@fechaobj", SqlDbType.Date).Value = dateTimePicker1.Value;
                comando.Parameters.Add("@cantidadobj", SqlDbType.Int).Value = numericUpDown1.Value;
                comando.Parameters.Add("@cantidadelab", SqlDbType.Int).Value = numericUpDown2.Value;
                comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = comboBox4.SelectedValue.ToString();
                comando.Parameters.Add("@estadoid", SqlDbType.Int).Value = "2";
                comando.ExecuteNonQuery();
                comboBox1.SelectedValue = "0";
                comboBox2.SelectedValue = "0";
                comboBox3.SelectedValue = "0";
                comboBox4.SelectedValue = "0";
                dateTimePicker1.Value = DateTime.Now;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                textBox2.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "La elaboración fue registrada";
                m.ShowDialog();
                mostrarGrilla();
            }
            else if (!ExisteElaboracion() && numericUpDown2.Value<numericUpDown1.Value && fechaHoy> dateTimePicker1.Value)
            {
                conexion.Open();
                string sql = "insert into PRODUCCION (PROD_ELAB_ID, PRODUCCION_FECHA_REG, PRODUCCION_FECHA_OBJ, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB,USUARIO_ID,EST_PROD_ID) values (@prodelabid,@fechareg,@fechaobj,@cantidadobj,@cantidadelab,@usuarioid,@estadoid)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@prodelabid", SqlDbType.Int).Value = comboBox3.SelectedValue.ToString();
                comando.Parameters.Add("@fechareg", SqlDbType.Date).Value = fechaHoy;
                comando.Parameters.Add("@fechaobj", SqlDbType.Date).Value = dateTimePicker1.Value;
                comando.Parameters.Add("@cantidadobj", SqlDbType.Int).Value = numericUpDown1.Value;
                comando.Parameters.Add("@cantidadelab", SqlDbType.Int).Value = numericUpDown2.Value;
                comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = comboBox4.SelectedValue.ToString();
                comando.Parameters.Add("@estadoid", SqlDbType.Int).Value = "3";
                comando.ExecuteNonQuery();
                comboBox1.SelectedValue = "0";
                comboBox2.SelectedValue = "0";
                comboBox3.SelectedValue = "0";
                comboBox4.SelectedValue = "0";
                dateTimePicker1.Value = DateTime.Now;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                textBox2.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "La elaboración fue registrada";
                m.ShowDialog();
                mostrarGrilla();
            }
            else if (!ExisteElaboracion() && numericUpDown1.Value == numericUpDown2.Value && fechaHoy<dateTimePicker1.Value)
            {
                conexion.Open();
                string sql = "insert into PRODUCCION (PROD_ELAB_ID, PRODUCCION_FECHA_REG, PRODUCCION_FECHA_OBJ, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB,USUARIO_ID,EST_PROD_ID) values (@prodelabid,@fechareg,@fechaobj,@cantidadobj,@cantidadelab,@usuarioid,@estadoid)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@prodelabid", SqlDbType.Int).Value = comboBox3.SelectedValue.ToString();
                comando.Parameters.Add("@fechareg", SqlDbType.Date).Value = fechaHoy;
                comando.Parameters.Add("@fechaobj", SqlDbType.Date).Value = dateTimePicker1.Value;
                comando.Parameters.Add("@cantidadobj", SqlDbType.Int).Value = numericUpDown1.Value;
                comando.Parameters.Add("@cantidadelab", SqlDbType.Int).Value = numericUpDown2.Value;
                comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = comboBox4.SelectedValue.ToString();
                comando.Parameters.Add("@estadoid", SqlDbType.Int).Value = "4";
                comando.ExecuteNonQuery();
                comboBox1.SelectedValue = "0";
                comboBox2.SelectedValue = "0";
                comboBox3.SelectedValue = "0";
                comboBox4.SelectedValue = "0";
                dateTimePicker1.Value = DateTime.Now;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                textBox2.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "La elaboración fue registrada";
                m.ShowDialog();
                mostrarGrilla();
            }
            else if (!ExisteElaboracion() && numericUpDown2.Value > numericUpDown1.Value && fechaHoy>=dateTimePicker1.Value)
            {
                conexion.Open();
                string sql = "insert into PRODUCCION (PROD_ELAB_ID, PRODUCCION_FECHA_REG, PRODUCCION_FECHA_OBJ, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB,USUARIO_ID,EST_PROD_ID) values (@prodelabid,@fechareg,@fechaobj,@cantidadobj,@cantidadelab,@usuarioid,@estadoid)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@prodelabid", SqlDbType.Int).Value = comboBox3.SelectedValue.ToString();
                comando.Parameters.Add("@fechareg", SqlDbType.Date).Value = fechaHoy;
                comando.Parameters.Add("@fechaobj", SqlDbType.Date).Value = dateTimePicker1.Value;
                comando.Parameters.Add("@cantidadobj", SqlDbType.Int).Value = numericUpDown1.Value;
                comando.Parameters.Add("@cantidadelab", SqlDbType.Int).Value = numericUpDown2.Value;
                comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = comboBox4.SelectedValue.ToString();
                comando.Parameters.Add("@estadoid", SqlDbType.Int).Value = "5";
                comando.ExecuteNonQuery();
                comboBox1.SelectedValue = "0";
                comboBox2.SelectedValue = "0";
                comboBox3.SelectedValue = "0";
                comboBox4.SelectedValue = "0";
                dateTimePicker1.Value = DateTime.Now;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                textBox2.Text = "";
                conexion.Close();
                Aviso m = new Aviso();
                m.label1.Text = "La elaboración fue registrada";
                m.ShowDialog();
                mostrarGrilla();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya existe una elaboración con las mismas características \n" +
                    " para esa fecha";
                m.ShowDialog();
            }
        }

        private bool ExisteElaboracion()
        {
            conexion.Open();
            string sql = "select PRODUCCION_ID from PRODUCCION where PROD_ELAB_ID=@PRODUCTOID and USUARIO_ID=@USUARIOID and PRODUCCION_FECHA_OBJ=@FECHAOBJETIVO";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@PRODUCTOID", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@USUARIOID", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@FECHAOBJETIVO", SqlDbType.Date).Value = dateTimePicker1.Value;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private bool ExisteElaboracionModificar()
        {
            conexion.Open();
            string sql = "select PRODUCCION_ID from PRODUCCION where PROD_ELAB_ID=@PRODUCTOID and USUARIO_ID=@USUARIOID and PRODUCCION_FECHA_OBJ=@FECHAOBJETIVO and PRODUCCION_ID!=@ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@ID", SqlDbType.Int).Value = textBox2.Text;
            comando.Parameters.Add("@PRODUCTOID", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@USUARIOID", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@FECHAOBJETIVO", SqlDbType.Date).Value = dateTimePicker1.Value;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }



        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select RUBRO_NOMBRE, MARCA_NOMBRE, PROD_ELAB_DESCR,(USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO, PRODUCCION_FECHA_OBJ, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB, EST_PROD_NOMBRE from PRODUCCION as pr join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pr.PROD_ELAB_ID join USUARIO as usu on usu.USUARIO_ID = pr.USUARIO_ID join RUBRO as rub on rub.RUBRO_ID = prelab.RUBRO_ID  join MARCA as mar on mar.MARCA_ID = prelab.MARCA_ID  join ESTADOPROD as est on est.EST_PROD_ID = pr.EST_PROD_ID ORDER BY PROD_ELAB_DESCR ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"].ToString(),
                 registros["MARCA_NOMBRE"].ToString(), registros["PROD_ELAB_DESCR"].ToString(),
                 registros["NOMBRE_COMPLETO"].ToString(), registros["PRODUCCION_FECHA_OBJ"],
                 registros["PRODUCCION_CANT_OBJ"].ToString(), registros["PRODUCCION_CANT_ELAB"].ToString(),
                 registros["EST_PROD_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();
        }


        private void cargarcomboBox1()
        {
            conexion.Open();
            string sql = "select prod.rubro_id, rubro_nombre from RUBRO as rubro, PRODUCTOELAB as prod where rubro.RUBRO_ID=prod.RUBRO_ID group by RUBRO_NOMBRE, prod.RUBRO_ID ORDER BY RUBRO_NOMBRE ASC";
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


        private void cargarcomboBox2()
        {
            conexion.Open();
            string sql = "select MARCA_ID, MARCA_NOMBRE from MARCA WHERE MARCA_NOMBRE='MTA25'";
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


        private void cargarcomboBox3()
        {
            if (comboBox1.SelectedIndex == -1 || comboBox2.SelectedIndex == -1)
            {
                comboBox3.SelectedIndex = -1;
            }
            else
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
                comboBox3.DisplayMember = "PROD_ELAB_DESCR";
                comboBox3.ValueMember = "PROD_ELAB_ID";
                comboBox3.DataSource = tabla1;
            }
        }




        private void cargarComboBox4()
        {
            if (tipoActivo.Equals("1"))
            {
                conexion.Open();
                string sql = "select USUARIO_ID, (USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO from USUARIO WHERE TIPO_USU_ID='4' OR TIPO_USU_ID='1' OR TIPO_USU_ID='2' ORDER BY NOMBRE_COMPLETO ASC";
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
            else if (tipoActivo.Equals("4")) 
            {

                conexion.Open();
                string sql = "select USUARIO_ID, (USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO from USUARIO WHERE USUARIO_APELLIDO=@apellido AND USUARIO_NOMBRE=@nombre ORDER BY NOMBRE_COMPLETO ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@apellido", SqlDbType.VarChar).Value = apellidoUsuActivo; 
                comando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombreUsuActivo;
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                comboBox4.DisplayMember = "NOMBRE_COMPLETO";
                comboBox4.ValueMember = "USUARIO_ID";
                comboBox4.DataSource = tabla1;
                conexion.Close();
            }
            else if (tipoActivo.Equals("2"))
            {
                conexion.Open();
                string sql = "select USUARIO_ID, (USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO from USUARIO WHERE USUARIO_APELLIDO=@apellido AND USUARIO_NOMBRE=@nombre ORDER BY NOMBRE_COMPLETO ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@apellido", SqlDbType.VarChar).Value = apellidoUsuActivo;
                comando.Parameters.Add("@nombre", SqlDbType.VarChar).Value = nombreUsuActivo;
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                comboBox4.DisplayMember = "NOMBRE_COMPLETO";
                comboBox4.ValueMember = "USUARIO_ID";
                comboBox4.DataSource = tabla1;
                conexion.Close();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex==-1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un rubro";
                m.ShowDialog();
            }
            else if (comboBox2.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar una marca";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1)
            {
                conexion.Open();
                string sql = "select RUBRO_NOMBRE, MARCA_NOMBRE, PROD_ELAB_DESCR,(USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO, PRODUCCION_FECHA_OBJ, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB, EST_PROD_NOMBRE from PRODUCCION as pr join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pr.PROD_ELAB_ID join USUARIO as usu on usu.USUARIO_ID = pr.USUARIO_ID join RUBRO as rub on rub.RUBRO_ID = prelab.RUBRO_ID   join MARCA as mar on mar.MARCA_ID = prelab.MARCA_ID   join ESTADOPROD as est on est.EST_PROD_ID = pr.EST_PROD_ID  where PRODUCCION_FECHA_OBJ=@fecha and prelab.RUBRO_ID=@rubro and prelab.MARCA_ID=@marca ORDER BY PROD_ELAB_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
                comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@fecha", SqlDbType.Date).Value = dateTimePicker1.Value;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"].ToString(),
                     registros["MARCA_NOMBRE"].ToString(), registros["PROD_ELAB_DESCR"].ToString(),
                     registros["NOMBRE_COMPLETO"].ToString(), registros["PRODUCCION_FECHA_OBJ"],
                     registros["PRODUCCION_CANT_OBJ"].ToString(), registros["PRODUCCION_CANT_ELAB"].ToString(),
                     registros["EST_PROD_NOMBRE"].ToString());

                    button5.Enabled = false;
                    button2.Enabled = false;
                }
                registros.Close();
                conexion.Close();
                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1)
            {
                conexion.Open();
                string sql = "select RUBRO_NOMBRE, MARCA_NOMBRE, PROD_ELAB_DESCR,(USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO, PRODUCCION_FECHA_OBJ, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB, EST_PROD_NOMBRE from PRODUCCION as pr join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pr.PROD_ELAB_ID join USUARIO as usu on usu.USUARIO_ID = pr.USUARIO_ID join RUBRO as rub on rub.RUBRO_ID = prelab.RUBRO_ID   join MARCA as mar on mar.MARCA_ID = prelab.MARCA_ID   join ESTADOPROD as est on est.EST_PROD_ID = pr.EST_PROD_ID  where PRODUCCION_FECHA_OBJ=@fecha and prelab.RUBRO_ID=@rubro and prelab.MARCA_ID=@marca and pr.PROD_ELAB_ID=@producto ORDER BY PROD_ELAB_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
                comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@producto", SqlDbType.Int).Value = comboBox3.SelectedValue;
                comando.Parameters.Add("@fecha", SqlDbType.Date).Value = dateTimePicker1.Value;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"].ToString(),
                     registros["MARCA_NOMBRE"].ToString(), registros["PROD_ELAB_DESCR"].ToString(),
                     registros["NOMBRE_COMPLETO"].ToString(), registros["PRODUCCION_FECHA_OBJ"],
                     registros["PRODUCCION_CANT_OBJ"].ToString(), registros["PRODUCCION_CANT_ELAB"].ToString(),
                     registros["EST_PROD_NOMBRE"].ToString());

                    button5.Enabled = false;
                    button2.Enabled = false;
                }
                registros.Close();
                conexion.Close();
                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex != -1)
            {
                conexion.Open();
                string sql = "select RUBRO_NOMBRE, MARCA_NOMBRE, PROD_ELAB_DESCR,(USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO, PRODUCCION_FECHA_OBJ, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB, EST_PROD_NOMBRE from PRODUCCION as pr join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pr.PROD_ELAB_ID join USUARIO as usu on usu.USUARIO_ID = pr.USUARIO_ID join RUBRO as rub on rub.RUBRO_ID = prelab.RUBRO_ID   join MARCA as mar on mar.MARCA_ID = prelab.MARCA_ID   join ESTADOPROD as est on est.EST_PROD_ID = pr.EST_PROD_ID  where PRODUCCION_FECHA_OBJ=@fecha and prelab.RUBRO_ID=@rubro and prelab.MARCA_ID=@marca and pr.USUARIO_ID=@cocinero ORDER BY PROD_ELAB_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
                comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
                comando.Parameters.Add("@fecha", SqlDbType.Date).Value = dateTimePicker1.Value;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"].ToString(),
                     registros["MARCA_NOMBRE"].ToString(), registros["PROD_ELAB_DESCR"].ToString(),
                     registros["NOMBRE_COMPLETO"].ToString(), registros["PRODUCCION_FECHA_OBJ"],
                     registros["PRODUCCION_CANT_OBJ"].ToString(), registros["PRODUCCION_CANT_ELAB"].ToString(),
                     registros["EST_PROD_NOMBRE"].ToString());

                    button5.Enabled = false;
                    button2.Enabled = false;
                }
                registros.Close();
                conexion.Close();
                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1)
            {
                conexion.Open();
                string sql = "select PRODUCCION_ID, RUBRO_NOMBRE, MARCA_NOMBRE, PROD_ELAB_DESCR,(USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO, PRODUCCION_FECHA_OBJ, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB, EST_PROD_NOMBRE from PRODUCCION as pr join PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pr.PROD_ELAB_ID join USUARIO as usu on usu.USUARIO_ID = pr.USUARIO_ID join RUBRO as rub on rub.RUBRO_ID = prelab.RUBRO_ID   join MARCA as mar on mar.MARCA_ID = prelab.MARCA_ID   join ESTADOPROD as est on est.EST_PROD_ID = pr.EST_PROD_ID  where PRODUCCION_FECHA_OBJ=@fecha and prelab.RUBRO_ID=@rubro and prelab.MARCA_ID=@marca and pr.PROD_ELAB_ID=@producto and pr.USUARIO_ID=@usuario ORDER BY PROD_ELAB_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
                comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@producto", SqlDbType.Int).Value = comboBox3.SelectedValue;
                comando.Parameters.Add("@usuario", SqlDbType.Int).Value = comboBox4.SelectedValue;
                comando.Parameters.Add("@fecha", SqlDbType.Date).Value = dateTimePicker1.Value;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"].ToString(),
                     registros["MARCA_NOMBRE"].ToString(), registros["PROD_ELAB_DESCR"].ToString(),
                     registros["NOMBRE_COMPLETO"].ToString(), registros["PRODUCCION_FECHA_OBJ"],
                     registros["PRODUCCION_CANT_OBJ"].ToString(), registros["PRODUCCION_CANT_ELAB"].ToString(),
                     registros["EST_PROD_NOMBRE"].ToString());


                    numericUpDown1.Value = (int)registros["PRODUCCION_CANT_OBJ"];
                    numericUpDown2.Value = (int)registros["PRODUCCION_CANT_ELAB"];
                    string idprod = registros["PRODUCCION_ID"].ToString();
                    int idProdu = int.Parse(idprod);
                    textBox2.Text = idprod;
                    cantidadElab = (int)registros["PRODUCCION_CANT_ELAB"];
                    fechaEliminar = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]);

                    button2.Enabled = false;
                    button3.Enabled = true;
                    button5.Enabled = false;
                    button6.Enabled = true;
                    comboBox1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;

                    button3.Visible = true;
                    button6.Visible = true;
                    pictureBox5.Visible = true;
                    pictureBox1.Visible = false;
                    button5.Visible = false;
                }
                registros.Close();
                conexion.Close();
                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DateTime fechaElaboracion = Convert.ToDateTime(dataGridView1.Rows[0].Cells[4].Value);
            if (fechaHoy > dateTimePicker1.Value)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe ingresar una fecha posterior o igual a " + fechaHoy.ToShortDateString().ToString();
                m.ShowDialog();
            }
            else if (fechaHoy > fechaElaboracion)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede modificar una elaboración ya cerrada";
                m.ShowDialog();
            }
            else if (numericUpDown1.Value == 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede modificar la cantidad objetivo a 0";
                m.ShowDialog();
            }
            else if (ExisteElaboracionModificar())
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya existe una elaboración con las mismas características \n" +
                    " para esa fecha";
                m.ShowDialog();
            }
            else if (!ExisteElaboracionModificar () && fechaHoy <= fechaElaboracion)
            {
                conexion.Open();
                string sql = "update PRODUCCION set PRODUCCION_FECHA_OBJ=@fecha,PRODUCCION_CANT_OBJ=@cantidadobj, PRODUCCION_CANT_ELAB=@cantidadelab where PRODUCCION_ID=@idprodu";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fecha", SqlDbType.Date).Value = dateTimePicker1.Value;
                comando.Parameters.Add("@cantidadobj", SqlDbType.Int).Value = numericUpDown1.Value;
                comando.Parameters.Add("@cantidadelab", SqlDbType.Int).Value = numericUpDown2.Value;
                comando.Parameters.Add("@idprodu", SqlDbType.Int).Value = textBox2.Text;
                comando.ExecuteNonQuery();
                conexion.Close();
                textBox2.Text = "";
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                comboBox1.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
                comboBox4.SelectedIndex = -1;
                dateTimePicker1.Value = DateTime.Now;
                comboBox1.Enabled = true;
                comboBox2.Enabled = true;
                comboBox3.Enabled = true;
                comboBox4.Enabled = true;
                Aviso m = new Aviso();
                m.label1.Text = "Se ha modificado la Elaboración";
                m.ShowDialog();
                dataGridView1.Rows.Clear();
                mostrarGrilla();
                ActualizarEstados2Intento();
                mostrarGrilla();
                button2.Enabled = true;
                button5.Enabled = true;
                button3.Enabled = false;
                button6.Enabled = false;

                button3.Visible = false;
                button6.Visible = false;
                pictureBox5.Visible = false;
                pictureBox1.Visible = true;
                button5.Visible = true;
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            if (cantidadElab != 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede eliminar una elaboración con unidades elaboradas";
                m.ShowDialog();
            }
            else if (fechaEliminar < fechaHoy)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede eliminar una elaboración con una fecha pasada";
                m.ShowDialog();
            }
            else if (cantidadElab == 0 && fechaEliminar >= fechaHoy)
            {
                DialogResult dialogResult = MessageBox.Show("¿Confirma la eliminación?", "Eliminar elaboración", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    conexion.Open();
                    string sql = "delete from PRODUCCION where PRODUCCION_ID=@produid";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@produid", SqlDbType.Int).Value = textBox2.Text;
                    int cant = comando.ExecuteNonQuery();
                    conexion.Close();
                    if (cant == 1)
                    {
                        textBox2.Text = "";
                        numericUpDown1.Value = 0;
                        numericUpDown2.Value = 0;
                        comboBox1.SelectedIndex = -1;
                        comboBox2.SelectedIndex = -1;
                        comboBox3.SelectedIndex = -1;
                        comboBox4.SelectedIndex = -1;
                        dateTimePicker1.Value = DateTime.Now;
                        Aviso m = new Aviso();
                        m.label1.Text = "Se ha eliminado la elaboración";
                        m.ShowDialog();
                        dataGridView1.Rows.Clear();
                        mostrarGrilla();
                        button2.Enabled = true;
                        button5.Enabled = true;
                        button3.Enabled = false;
                        button6.Enabled = false;
                        comboBox1.Enabled = true;
                        comboBox2.Enabled = true;
                        comboBox3.Enabled = true;
                        comboBox4.Enabled = true;

                        button3.Visible = false;
                        button6.Visible = false;
                        pictureBox5.Visible = false;
                        pictureBox1.Visible = true;
                        button5.Visible = true;
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }                
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarcomboBox3();
            comboBox3.SelectedIndex = -1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarcomboBox3();
            comboBox3.SelectedIndex = -1;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedValue = "0";
            comboBox2.SelectedValue = "0";
            comboBox3.SelectedValue = "0";
            comboBox4.SelectedValue = "0";
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            dateTimePicker1.Value = DateTime.Now;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            textBox2.Text = "";
            button2.Enabled = true;
            button5.Enabled = true;
            button3.Enabled = false;
            button6.Enabled = false;
            mostrarGrilla();

            button3.Visible = false;
            button6.Visible = false;
            pictureBox5.Visible = false;
            pictureBox1.Visible = true;
            button5.Visible = true;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "La búsqueda se puede filtrar por:\n" +
                "Rubro, marca y fecha.\n" +
                "Rubro, marca, producto y fecha.\n" +
                "Rubro, marca, cocinero y fecha.\n" +
                "Rubro, marca, producto, cocinero y fecha.\n";
            m.ShowDialog();
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
        }
    }
}
