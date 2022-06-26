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
    public partial class ListadoElab : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        public ListadoElab()
        {
            InitializeComponent();
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
                m.label1.Text = "La primera fecha y hora ingresadas deben ser anteriores a la segunda fecha y hora ingresadas";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex==-1 && comboBox2.SelectedIndex == -1)
            {
                button4.Enabled = true;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                BuscarSoloPorFecha();
                calcularTotalObjetivo();
                calcularTotalElaborada();
                calcularDiferencia();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex == -1)
            {
                button4.Enabled = true;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                BuscarCocineroYFecha();
                calcularTotalObjetivo();
                calcularTotalElaborada();
                calcularDiferencia();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex != -1)
            {
                button4.Enabled = true;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                BuscarEstadoYFecha();
                calcularTotalObjetivo();
                calcularTotalElaborada();
                calcularDiferencia();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
            {
                button4.Enabled = true;
                comboBox1.Enabled = false;
                comboBox2.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                BuscarPorTodo();
                calcularTotalObjetivo();
                calcularTotalElaborada();
                calcularDiferencia();
            }
        }



        // ---------------------------- ---------------------------------------METODOS --------------------------------------------------------------------------------------------------------

        string tipoPdf = ""; //SE USA COMO BANDERA PARA DEFINIR CUÁL PDF SE GENERA CON EL BOTÓN DESCARGAR (HAY 4 ESCENARIOS POSIBLES)

        private void BuscarSoloPorFecha()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ, PROD_ELAB_DESCR, (USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB, PRODUCCION_CANT_ELAB - PRODUCCION_CANT_OBJ as Diferencia, EST_PROD_NOMBRE from PRODUCCION as pro  JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID=pro.PROD_ELAB_ID JOIN USUARIO as usu on usu.USUARIO_ID = pro.USUARIO_ID JOIN ESTADOPROD as est on est.EST_PROD_ID = pro.EST_PROD_ID where PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta ORDER BY PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PRODUCCION_FECHA_OBJ"],
                                  registros["PROD_ELAB_DESCR"].ToString(),
                                  registros["NOMBRE_COMPLETO"].ToString(),
                                  registros["PRODUCCION_CANT_OBJ"].ToString(),
                                  registros["PRODUCCION_CANT_ELAB"].ToString(),
                                  registros["Diferencia"].ToString(),
                                  registros["EST_PROD_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();

            tipoPdf = "SoloFecha";


            if (dataGridView1.Rows.Count == 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se encontraron registros para esta búsqueda";
                m.ShowDialog();
            }
        }

        private void BuscarCocineroYFecha()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ, PROD_ELAB_DESCR, (USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB, PRODUCCION_CANT_ELAB - PRODUCCION_CANT_OBJ as Diferencia, EST_PROD_NOMBRE from PRODUCCION as pro  JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID=pro.PROD_ELAB_ID JOIN USUARIO as usu on usu.USUARIO_ID = pro.USUARIO_ID JOIN ESTADOPROD as est on est.EST_PROD_ID = pro.EST_PROD_ID where pro.USUARIO_ID=@COCINERO and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta ORDER BY PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox1.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PRODUCCION_FECHA_OBJ"],
                                  registros["PROD_ELAB_DESCR"].ToString(),
                                  registros["NOMBRE_COMPLETO"].ToString(),
                                  registros["PRODUCCION_CANT_OBJ"].ToString(),
                                  registros["PRODUCCION_CANT_ELAB"].ToString(),
                                  registros["Diferencia"].ToString(),
                                  registros["EST_PROD_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();

            tipoPdf = "CociFecha";


            if (dataGridView1.Rows.Count == 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se encontraron registros para esta búsqueda";
                m.ShowDialog();
            }
        }


        private void BuscarEstadoYFecha()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ, PROD_ELAB_DESCR, (USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB, PRODUCCION_CANT_ELAB - PRODUCCION_CANT_OBJ as Diferencia, EST_PROD_NOMBRE from PRODUCCION as pro  JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID=pro.PROD_ELAB_ID JOIN USUARIO as usu on usu.USUARIO_ID = pro.USUARIO_ID JOIN ESTADOPROD as est on est.EST_PROD_ID = pro.EST_PROD_ID where pro.EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta ORDER BY PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox2.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PRODUCCION_FECHA_OBJ"],
                                  registros["PROD_ELAB_DESCR"].ToString(),
                                  registros["NOMBRE_COMPLETO"].ToString(),
                                  registros["PRODUCCION_CANT_OBJ"].ToString(),
                                  registros["PRODUCCION_CANT_ELAB"].ToString(),
                                  registros["Diferencia"].ToString(),
                                  registros["EST_PROD_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();

            tipoPdf = "EstadoFecha";


            if (dataGridView1.Rows.Count == 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se encontraron registros para esta búsqueda";
                m.ShowDialog();
            }
        }

        private void BuscarPorTodo()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ, PROD_ELAB_DESCR, (USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO, PRODUCCION_CANT_OBJ, PRODUCCION_CANT_ELAB, PRODUCCION_CANT_ELAB - PRODUCCION_CANT_OBJ as Diferencia, EST_PROD_NOMBRE from PRODUCCION as pro  JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID=pro.PROD_ELAB_ID JOIN USUARIO as usu on usu.USUARIO_ID = pro.USUARIO_ID JOIN ESTADOPROD as est on est.EST_PROD_ID = pro.EST_PROD_ID where pro.USUARIO_ID=@COCINERO and pro.EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta ORDER BY PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox2.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PRODUCCION_FECHA_OBJ"],
                                  registros["PROD_ELAB_DESCR"].ToString(),
                                  registros["NOMBRE_COMPLETO"].ToString(),
                                  registros["PRODUCCION_CANT_OBJ"].ToString(),
                                  registros["PRODUCCION_CANT_ELAB"].ToString(),
                                  registros["Diferencia"].ToString(),
                                  registros["EST_PROD_NOMBRE"].ToString());
            }
            registros.Close();
            conexion.Close();

            tipoPdf = "Todo";


            if (dataGridView1.Rows.Count == 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se encontraron registros para esta búsqueda";
                m.ShowDialog();
            }
        }


        public void calcularTotalObjetivo()
        {
            int suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToInt32(dataGridView1.Rows[i].Cells[3].Value);
            }
            label8.Text = suma.ToString();
        }

        public void calcularTotalElaborada()
        {
            int suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToInt32(dataGridView1.Rows[i].Cells[4].Value);
            }
            label9.Text = suma.ToString();
        }

        public void calcularDiferencia()
        {
            int suma = 0;
            int n1 = int.Parse(label8.Text);
            int n2 = int.Parse(label9.Text);
            suma = n2 - n1;
            label10.Text = suma.ToString();
        }


        private void cargarComboBox1()
        {
            conexion.Open();
            string sql = "select USUARIO_ID, (USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO from USUARIO WHERE TIPO_USU_ID='4' OR TIPO_USU_ID='1' OR TIPO_USU_ID='2' ORDER BY NOMBRE_COMPLETO ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            comboBox1.DisplayMember = "NOMBRE_COMPLETO";
            comboBox1.ValueMember = "USUARIO_ID";
            comboBox1.DataSource = tabla1;
            conexion.Close();
        }

        private void cargarComboBox2()
        {
            conexion.Open();
            string sql = "select EST_PROD_ID, EST_PROD_NOMBRE from ESTADOPROD";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox2.DisplayMember = "EST_PROD_NOMBRE";
            comboBox2.ValueMember = "EST_PROD_ID";
            comboBox2.DataSource = tabla1;
        }

        private void ListadoElab_Load_1(object sender, EventArgs e)
        {
            cargarComboBox1();
            cargarComboBox2();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            button4.Enabled = false;
            dateTimePicker1.Value = DateTime.Now.Date;

            tipoPdf = "";
        }

        private void button3_Click(object sender, EventArgs e)
        {            
            dataGridView1.Rows.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker2.Value = DateTime.Now;
            label8.Text = "";
            label9.Text = "";
            label10.Text = "";
            button4.Enabled = false;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;

            tipoPdf = "";
        }

        private void button4_Click(object sender, EventArgs e) //BOTÓN DESCARGAR PDF
        {
            if (dataGridView1.Rows.Count == 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "El listado está vacío, no es posible su descarga";
                m.ShowDialog();
            }
            else if (tipoPdf=="SoloFecha")
            { 
            ListElabSoloFecha form = new ListElabSoloFecha();
            form.fechadesde = Convert.ToDateTime(dateTimePicker1.Value.Date);//ENVÍO DATO PARAMETRIZADO
            form.fechahasta = Convert.ToDateTime(dateTimePicker2.Value.Date);//ENVÍO DATO PARAMETRIZADO
            form.ShowDialog();
            }
            else if (tipoPdf == "CociFecha")
            {
                ListElabCociFecha form = new ListElabCociFecha();
                form.fechadesde = Convert.ToDateTime(dateTimePicker1.Value.Date);//ENVÍO DATO PARAMETRIZADO
                form.fechahasta = Convert.ToDateTime(dateTimePicker2.Value.Date);//ENVÍO DATO PARAMETRIZADO
                form.cocinero = Convert.ToInt32(comboBox1.SelectedValue);//ENVÍO DATO PARAMETRIZADO
                form.ShowDialog();
            }
            else if (tipoPdf == "EstadoFecha")
            {
                ListElabEstadoFecha form = new ListElabEstadoFecha();
                form.fechadesde = Convert.ToDateTime(dateTimePicker1.Value.Date);//ENVÍO DATO PARAMETRIZADO
                form.fechahasta = Convert.ToDateTime(dateTimePicker2.Value.Date);//ENVÍO DATO PARAMETRIZADO
                form.estado = Convert.ToInt32(comboBox2.SelectedValue);//ENVÍO DATO PARAMETRIZADO
                form.ShowDialog();
            }
            else if (tipoPdf == "Todo")
            {
                ListElabTodo form = new ListElabTodo();
                form.fechadesde = Convert.ToDateTime(dateTimePicker1.Value.Date);//ENVÍO DATO PARAMETRIZADO
                form.fechahasta = Convert.ToDateTime(dateTimePicker2.Value.Date);//ENVÍO DATO PARAMETRIZADO
                form.cocinero = Convert.ToInt32(comboBox1.SelectedValue);//ENVÍO DATO PARAMETRIZADO
                form.estado = Convert.ToInt32(comboBox2.SelectedValue);//ENVÍO DATO PARAMETRIZADO
                form.ShowDialog();
            }
        }

        private void pictureBox6_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "La búsqueda se puede filtrar por:\n" +
                "Rango de fecha.\n" +
                "Rango de fecha y cocinero.\n" +
                "Rango de fecha y estado.\n" +
                "Rango de fecha, cocinero y estado.\n";
            m.ShowDialog();
        }
    }
}
