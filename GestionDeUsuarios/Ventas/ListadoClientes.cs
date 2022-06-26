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

namespace GestionDeUsuarios.Ventas
{
    public partial class ListadoClientes : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        public ListadoClientes()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ListadoClientes_Load(object sender, EventArgs e)
        {
            mostrarGrillaDeTodosPorMonto();
            calcularTotalUnidades();
            calcularMontoTotal();
            dateTimePicker1.Value = DateTime.Now.Date;
        }


        private void mostrarGrillaDeTodosPorMonto()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.CLIENTE_TEL, T1.TOTALUNIDADES, T1.TOTAL-T2.DESCUENTO as MONTOTOTAL FROM ( SELECT(CLIENTE_APELLIDO + ', ' + CLIENTE_NOMBRE) AS NOMBRE_COMPLETO, CLIENTE_TEL, SUM(DET_VENTA_CANT) AS 'Totalunidades', SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'total' from CLIENTE as cl, DETALLEVENTA as det, VENTA as vent where det.VENTA_ID = vent.VENTA_ID and cl.CLIENTE_ID = vent.CLIENTE_ID group by CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_TEL )t1 left join (select CLIENTE_TEL, SUM(VENTA_DTO) as 'descuento' from CLIENTE as cl, VENTA as ven where cl.CLIENTE_ID=ven.CLIENTE_ID group by cl.CLIENTE_TEL)t2 on(t2.CLIENTE_TEL= t1.CLIENTE_TEL) order by MONTOTOTAL desc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["CLIENTE_TEL"].ToString(),
                                  registros["TOTALUNIDADES"].ToString(),
                                  registros["MONTOTOTAL"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void filtradoRangoFecha()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.CLIENTE_TEL, T1.TOTALUNIDADES, T1.TOTAL-T2.DESCUENTO as MONTOTOTAL FROM (SELECT (CLIENTE_APELLIDO+', '+CLIENTE_NOMBRE) AS NOMBRE_COMPLETO, CLIENTE_TEL, SUM(DET_VENTA_CANT) AS 'Totalunidades', SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'total' from CLIENTE as cl, DETALLEVENTA as det, VENTA as vent where det.VENTA_ID=vent.VENTA_ID and cl.CLIENTE_ID=vent.CLIENTE_ID and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta group by CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_TEL) t1 left join (SELECT CLIENTE_TEL, SUM(VENTA_DTO) as 'descuento' from CLIENTE as cl, VENTA as vent where cl.CLIENTE_ID=vent.CLIENTE_ID and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta group by CLIENTE_TEL)t2 on (t2.CLIENTE_TEL=t1.CLIENTE_TEL) order by MONTOTOTAL desc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["CLIENTE_TEL"].ToString(),
                                  registros["TOTALUNIDADES"].ToString(),
                                  registros["MONTOTOTAL"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        // //RANGO DE FECHA, PRODUCTO ELABORADO, RUBRO
        private void filtradoElabRubro()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.CLIENTE_TEL, T1.TOTALUNIDADES, T1.TOTAL as MONTOTOTAL FROM ( SELECT (CLIENTE_APELLIDO+', '+CLIENTE_NOMBRE) AS NOMBRE_COMPLETO, CLIENTE_TEL, SUM(DET_VENTA_CANT) AS 'Totalunidades', SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'total' from CLIENTE as cl, DETALLEVENTA as det, VENTA as vent , RUBRO as rub, PRODUCTOELAB AS PRODELAB where det.VENTA_ID=vent.VENTA_ID and cl.CLIENTE_ID=vent.CLIENTE_ID and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta and rub.RUBRO_ID=PRODELAB.RUBRO_ID and PRODELAB.PROD_ELAB_ID=det.PROD_ELAB_ID and RUB.RUBRO_ID=@rubro group by CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_TEL)t1 order by MONTOTOTAL desc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox2.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["CLIENTE_TEL"].ToString(),
                                  registros["TOTALUNIDADES"].ToString(),
                                  registros["MONTOTOTAL"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        // //RANGO DE FECHA, PRODUCTO REVENTA, RUBRO
        private void filtradoRevRubro()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.CLIENTE_TEL, T1.TOTALUNIDADES, T1.TOTAL as MONTOTOTAL FROM (SELECT (CLIENTE_APELLIDO+', '+CLIENTE_NOMBRE) AS NOMBRE_COMPLETO, CLIENTE_TEL, SUM(DET_VENTA_CANT) AS 'Totalunidades', SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'total' from CLIENTE as cl, DETALLEVENTA as det, VENTA as vent , RUBRO as rub, PRODUCTOREVENTA AS PRODREV where det.VENTA_ID=vent.VENTA_ID and cl.CLIENTE_ID=vent.CLIENTE_ID and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta and PRODREV.PROD_REV_ID=det.PROD_REV_ID and rub.RUBRO_ID=PRODREV.RUBRO_ID and RUB.RUBRO_ID=@rubro group by CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_TEL)t1 order by MONTOTOTAL desc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox2.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["CLIENTE_TEL"].ToString(),
                                  registros["TOTALUNIDADES"].ToString(),
                                  registros["MONTOTOTAL"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        // //RANGO DE FECHA, PRODUCTO ELABORADO, RUBRO, MARCA
        private void filtradoElabRubroMarca()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.CLIENTE_TEL, T1.TOTALUNIDADES, T1.TOTAL as MONTOTOTAL FROM (SELECT (CLIENTE_APELLIDO+', '+CLIENTE_NOMBRE) AS NOMBRE_COMPLETO, CLIENTE_TEL, SUM(DET_VENTA_CANT) AS 'Totalunidades',SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'total' from CLIENTE as cl, DETALLEVENTA as det, VENTA as vent , RUBRO as rub, PRODUCTOELAB AS PRODELAB, MARCA AS mar where det.VENTA_ID=vent.VENTA_ID and cl.CLIENTE_ID=vent.CLIENTE_ID and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta and rub.RUBRO_ID=PRODELAB.RUBRO_ID and PRODELAB.PROD_ELAB_ID=det.PROD_ELAB_ID and RUB.RUBRO_ID=@rubro and mar.MARCA_ID=PRODELAB.MARCA_ID and mar.MARCA_ID=@marca group by CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_TEL)t1 order by MONTOTOTAL desc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["CLIENTE_TEL"].ToString(),
                                  registros["TOTALUNIDADES"].ToString(),
                                  registros["MONTOTOTAL"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        // //RANGO DE FECHA, PRODUCTO REVENTA, RUBRO, MARCA
        private void filtradoRevRubroMarca()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.CLIENTE_TEL, T1.TOTALUNIDADES, T1.TOTAL as MONTOTOTAL FROM (SELECT (CLIENTE_APELLIDO+', '+CLIENTE_NOMBRE) AS NOMBRE_COMPLETO, CLIENTE_TEL, SUM(DET_VENTA_CANT) AS 'Totalunidades', SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'total' from CLIENTE as cl, DETALLEVENTA as det, VENTA as vent , RUBRO as rub, PRODUCTOREVENTA AS PRODREV,MARCA as mar where det.VENTA_ID=vent.VENTA_ID and cl.CLIENTE_ID=vent.CLIENTE_ID and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta and PRODREV.PROD_REV_ID=det.PROD_REV_ID and rub.RUBRO_ID=PRODREV.RUBRO_ID and RUB.RUBRO_ID=@rubro and mar.MARCA_ID=PRODREV.MARCA_ID and mar.MARCA_ID=@marca group by CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_TEL)t1 order by MONTOTOTAL desc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["CLIENTE_TEL"].ToString(),
                                  registros["TOTALUNIDADES"].ToString(),
                                  registros["MONTOTOTAL"].ToString());
            }
            registros.Close();
            conexion.Close();
        }
        //RANGO DE FECHA, PRODUCTO ELABORADO, RUBRO, MARCA Y NOMBRE PRODUCTO ELABORADO
        private void filtradoTodoElaborado()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.CLIENTE_TEL, T1.TOTALUNIDADES, T1.TOTAL as MONTOTOTAL FROM (SELECT (CLIENTE_APELLIDO+', '+CLIENTE_NOMBRE) AS NOMBRE_COMPLETO, CLIENTE_TEL, SUM(DET_VENTA_CANT) AS 'Totalunidades', SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'total' from CLIENTE as cl, DETALLEVENTA as det, VENTA as vent, RUBRO as rub, PRODUCTOELAB AS PRODELAB, MARCA AS mar where det.VENTA_ID=vent.VENTA_ID and cl.CLIENTE_ID=vent.CLIENTE_ID and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta and rub.RUBRO_ID=PRODELAB.RUBRO_ID and PRODELAB.PROD_ELAB_ID=det.PROD_ELAB_ID and RUB.RUBRO_ID=@rubro and mar.MARCA_ID=PRODELAB.MARCA_ID and mar.MARCA_ID=@marca and PRODELAB.PROD_ELAB_ID=@PRODELAB group by CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_TEL)t1 order by MONTOTOTAL desc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@prodelab", SqlDbType.Int).Value = comboBox4.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["CLIENTE_TEL"].ToString(),
                                  registros["TOTALUNIDADES"].ToString(),
                                  registros["MONTOTOTAL"].ToString());
            }
            registros.Close();
            conexion.Close();
        }
        //RANGO DE FECHA, PRODUCTO ELABORADO, RUBRO, MARCA Y NOMBRE PRODUCTO REVENTA
        private void filtradoTodoReventa()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.CLIENTE_TEL, T1.TOTALUNIDADES, T1.TOTAL as MONTOTOTAL FROM (SELECT (CLIENTE_APELLIDO+', '+CLIENTE_NOMBRE) AS NOMBRE_COMPLETO, CLIENTE_TEL, SUM(DET_VENTA_CANT) AS 'Totalunidades',SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'total' from CLIENTE as cl, DETALLEVENTA as det, VENTA as vent , RUBRO as rub, PRODUCTOREVENTA AS PRODREV, MARCA as mar where det.VENTA_ID=vent.VENTA_ID and cl.CLIENTE_ID=vent.CLIENTE_ID and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta and PRODREV.PROD_REV_ID=det.PROD_REV_ID and rub.RUBRO_ID=PRODREV.RUBRO_ID and RUB.RUBRO_ID=@rubro and mar.MARCA_ID=PRODREV.MARCA_ID and mar.MARCA_ID=@marca and PRODREV.PROD_REV_ID=@PRODREV group by CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_TEL)t1 order by MONTOTOTAL desc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = comboBox4.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["CLIENTE_TEL"].ToString(),
                                  registros["TOTALUNIDADES"].ToString(),
                                  registros["MONTOTOTAL"].ToString());
            }
            registros.Close();
            conexion.Close();
        }
        public void calcularTotalUnidades()
        {
            decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value);
            }
            label6.Text = suma.ToString();
        }

        public void calcularMontoTotal()
        {
            decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
            }
            label9.Text = suma.ToString();
        }
        public void calcularSaldoTotal()
        {
            decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value);
            }
            label9.Text = suma.ToString();
        }


        private void cargarcomboBox2()
        {
            if (comboBox5.Text == "Producto Elaborado")
            {
                conexion.Open();
                string sql = "select rubro.rubro_id, rubro_nombre from RUBRO as rubro, PRODUCTOELAB as prod where rubro.RUBRO_ID=prod.RUBRO_ID group by RUBRO_NOMBRE, rubro.RUBRO_ID ORDER BY RUBRO_NOMBRE ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox2.DisplayMember = "RUBRO_NOMBRE";
                comboBox2.ValueMember = "RUBRO_ID";
                comboBox2.DataSource = tabla1;
            }
            else if (comboBox5.Text == "Producto de Reventa")
            {
                conexion.Open();
                string sql = "select rubro.rubro_id, rubro_nombre from RUBRO as rubro, PRODUCTOREVENTA as pr where rubro.RUBRO_ID = pr.RUBRO_ID group by RUBRO_NOMBRE, rubro.RUBRO_ID ORDER BY RUBRO_NOMBRE ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox2.DisplayMember = "RUBRO_NOMBRE";
                comboBox2.ValueMember = "RUBRO_ID";
                comboBox2.DataSource = tabla1;
            }
        }

        private void cargarcomboBox3()
        {
            if (comboBox5.Text == "Producto Elaborado")
            {
                conexion.Open();
                string sql = "select prod.MARCA_ID, MARCA_NOMBRE from MARCA as mar, PRODUCTOELAB as prod where mar.MARCA_ID=prod.MARCA_ID group by MARCA_NOMBRE, prod.MARCA_ID ORDER BY MARCA_NOMBRE ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox3.DisplayMember = "MARCA_NOMBRE";
                comboBox3.ValueMember = "MARCA_ID";
                comboBox3.DataSource = tabla1;
            }
            else if (comboBox5.Text == "Producto de Reventa")
            {
                conexion.Open();
                string sql = "select prod.marca_id, marca_nombre from MARCA as marca, PRODUCTOREVENTA as prod where marca.MARCA_ID=prod.MARCA_ID group by MARCA_NOMBRE, prod.marca_id ORDER BY MARCA_NOMBRE ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox3.DisplayMember = "MARCA_NOMBRE";
                comboBox3.ValueMember = "MARCA_ID";
                comboBox3.DataSource = tabla1;
            }
        }

        private void cargarcomboBox4()
        {
            if (comboBox5.Text == "Producto Elaborado" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
            {
                conexion.Open();
                string sql = "select PROD_ELAB_ID, PROD_ELAB_DESCR from PRODUCTOELAB where RUBRO_ID=@rubroid and MARCA_ID=@marcaid ORDER BY PROD_ELAB_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox4.DataSource = tabla1;
                comboBox4.DisplayMember = "PROD_ELAB_DESCR";
                comboBox4.ValueMember = "PROD_ELAB_ID";
            }
            else if (comboBox5.Text == "Producto de Reventa" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
            {
                conexion.Open();
                string sql = "select PROD_REV_ID, PROD_REV_DESCR from PRODUCTOREVENTA where RUBRO_ID=@rubroid and MARCA_ID=@marcaid ORDER BY PROD_REV_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox4.DataSource = tabla1;
                comboBox4.DisplayMember = "PROD_REV_DESCR";
                comboBox4.ValueMember = "PROD_REV_ID";
            }
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarcomboBox2();
            cargarcomboBox3();
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarcomboBox4();
            comboBox4.SelectedIndex = -1;
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarcomboBox4();
            comboBox4.SelectedIndex = -1;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                Aviso m = new Aviso();
                m.label1.Text = "La primera fecha y hora ingresadas deben ser anteriores a la segunda fecha y hora ingresadas";
                m.ShowDialog();
            }
            else if (comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Por favor seleccione un Rubro, Marca y/o Producto";
                m.ShowDialog();
            }
            else if (comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1)
            {
                filtradoRangoFecha();
                button2.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                calcularMontoTotal();
                calcularTotalUnidades();
            }
            else if (comboBox5.Text == "Producto Elaborado" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1)
            {
                filtradoElabRubro();
                button2.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                calcularMontoTotal();
                calcularTotalUnidades();
            }
            else if (comboBox5.Text == "Producto de Reventa" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1)
            {
                filtradoRevRubro();
                button2.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                calcularMontoTotal();
                calcularTotalUnidades();
            }
            else if (comboBox5.Text == "Producto Elaborado" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1)
            {
                filtradoElabRubroMarca();
                button2.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                calcularMontoTotal();
                calcularTotalUnidades();
            }
            else if (comboBox5.Text == "Producto de Reventa" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1)
            {
                filtradoRevRubroMarca();
                button2.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                calcularMontoTotal();
                calcularTotalUnidades();
            }
            else if (comboBox5.Text == "Producto Elaborado" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1)
            {
                filtradoTodoElaborado();
                button2.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                calcularMontoTotal();
                calcularTotalUnidades();
            }
            else if (comboBox5.Text == "Producto de Reventa" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1)
            {
                filtradoTodoReventa();
                button2.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                calcularMontoTotal();
                calcularTotalUnidades();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            mostrarGrillaDeTodosPorMonto();
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker2.Value = DateTime.Now;
            button2.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox5.Enabled = true;
            label6.Text = "";
            label9.Text = "";
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "La búsqueda se puede filtrar por:\n" +
                "Rango de fecha, tipo de producto y rubro.\n" +
                "Rango de fecha, tipo de producto, rubro y marca.\n" +
                "Rango de fecha, tipo de producto, rubro, marca y producto.\n";
            m.ShowDialog();
        }
    }
}
