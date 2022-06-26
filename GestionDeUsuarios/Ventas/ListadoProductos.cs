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
    public partial class ListadoProductos : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        public ListadoProductos()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ListadoProductos_Load(object sender, EventArgs e)
        {
            cargarComboBox5();
            comboBox5.SelectedIndex = -1; 
        }

        private void cargarcomboBox2()
        {
            if (comboBox1.Text == "Producto Elaborado")
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
            else if (comboBox1.Text == "Producto de Reventa")
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
            if (comboBox1.Text == "Producto Elaborado")
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
            else if (comboBox1.Text == "Producto de Reventa")
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
            if (comboBox1.Text == "Producto Elaborado" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
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
            else if (comboBox1.Text == "Producto de Reventa" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1)
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


        private void cargarComboBox5()
        {
            conexion.Open();
            string sql = "select USUARIO_ID, (USUARIO_APELLIDO + ', '  +USUARIO_NOMBRE) as NOMBRE_COMPLETO from USUARIO WHERE TIPO_USU_ID='3' OR TIPO_USU_ID='1' OR TIPO_USU_ID='2' ORDER BY NOMBRE_COMPLETO ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            comboBox5.DisplayMember = "NOMBRE_COMPLETO";
            comboBox5.ValueMember = "USUARIO_ID";
            comboBox5.DataSource = tabla1;
            conexion.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
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
        public void calcularCantidadTotalVendida()
        {
            decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
            }
            label6.Text = suma.ToString();
        }

        public void calcularMontoTotalVendido()
        {
            decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value);
            }
            label9.Text = suma.ToString();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                Aviso m = new Aviso();
                m.label1.Text = "La primera fecha y hora ingresadas deben ser anteriores a la segunda fecha y hora ingresadas";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un tipo de producto";
                m.ShowDialog();
            }
            else if (comboBox1.Text == "Producto Elaborado" && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1)
            {
                //FILTRADO POR TIPO DE PRODUCTO (PRODUCTO ELABORADO), RANGO DE FECHA
                conexion.Open();
                string sql = "select prod_elab_descr, MARCA_NOMBRE, RUBRO_NOMBRE, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'MONTOTOTALVENDIDO', SUM(DET_VENTA_CANT) AS 'CANTIDADTOTALVENDIDA' from PRODUCTOELAB as prodelab, MARCA as mar, RUBRO as rub, DETALLEVENTA as det, VENTA as ven where mar.MARCA_ID=prodelab.MARCA_ID and  rub.RUBRO_ID=prodelab.RUBRO_ID and det.PROD_ELAB_ID=prodelab.PROD_ELAB_ID and ven.VENTA_ID=det.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta group by PROD_ELAB_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"],
                                      registros["MARCA_NOMBRE"].ToString(),
                                      registros["PROD_ELAB_DESCR"].ToString(),
                                      registros["CANTIDADTOTALVENDIDA"].ToString(),
                                      registros["MONTOTOTALVENDIDO"].ToString());
                }
                registros.Close();
                conexion.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }
                calcularCantidadTotalVendida();
                calcularMontoTotalVendido();
                bloquear();
            }
            else if (comboBox1.Text == "Producto de Reventa" && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1)
            {
                //FILTRADO POR TIPO DE PRODUCTO (PRODUCTO REVENTA), RANGO DE FECHA
                conexion.Open();
                string sql = "select PROD_REV_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'MONTOTOTALVENDIDO', SUM(DET_VENTA_CANT) AS 'CANTIDADTOTALVENDIDA' from PRODUCTOREVENTA as prodrev, MARCA as mar, RUBRO as rub, DETALLEVENTA as det, VENTA as ven where det.PROD_REV_ID=prodrev.PROD_REV_ID and mar.MARCA_ID=prodrev.MARCA_ID and rub.RUBRO_ID=prodrev.RUBRO_ID and ven.VENTA_ID=det.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta group by PROD_REV_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"],
                                      registros["MARCA_NOMBRE"].ToString(),
                                      registros["PROD_REV_DESCR"].ToString(),
                                      registros["CANTIDADTOTALVENDIDA"].ToString(),
                                      registros["MONTOTOTALVENDIDO"].ToString());
                }
                registros.Close();
                conexion.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }

                calcularCantidadTotalVendida();
                calcularMontoTotalVendido();
                bloquear();
            }
            else if (comboBox1.Text == "Producto Elaborado" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1)
            {
                //FILTRADO POR TIPO DE PRODUCTO (PRODUCTO ELABORADO), RANGO DE FECHA, RUBRO
                conexion.Open();
                string sql = "select prod_elab_descr, MARCA_NOMBRE, RUBRO_NOMBRE, SUM(DET_VENTA_CANT*DET_VENTA_PR_UNIT) as 'MONTOTOTALVENDIDO', SUM(DET_VENTA_CANT) AS 'CANTIDADTOTALVENDIDA' from PRODUCTOELAB as prodelab, MARCA as mar, RUBRO as rub,DETALLEVENTA as det, VENTA as ven where mar.MARCA_ID=prodelab.MARCA_ID and  rub.RUBRO_ID=prodelab.RUBRO_ID and det.PROD_ELAB_ID=prodelab.PROD_ELAB_ID and ven.VENTA_ID=det.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta AND prodelab.RUBRO_ID=@rubroid group by PROD_ELAB_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"],
                                      registros["MARCA_NOMBRE"].ToString(),
                                      registros["PROD_ELAB_DESCR"].ToString(),
                                      registros["CANTIDADTOTALVENDIDA"].ToString(),
                                      registros["MONTOTOTALVENDIDO"].ToString());
                }
                registros.Close();
                conexion.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }

                calcularCantidadTotalVendida();
                calcularMontoTotalVendido();
                bloquear();
            }
            else if (comboBox1.Text == "Producto de Reventa" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1)
            {
                //FILTRADO POR TIPO DE PRODUCTO (PRODUCTO REVENTA), RANGO DE FECHA, RUBRO
                conexion.Open();
                string sql = "select PROD_REV_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'MONTOTOTALVENDIDO', SUM(DET_VENTA_CANT) AS 'CANTIDADTOTALVENDIDA' from PRODUCTOREVENTA as prodrev, MARCA as mar, RUBRO as rub, DETALLEVENTA as det, VENTA as ven where det.PROD_REV_ID=prodrev.PROD_REV_ID and mar.MARCA_ID=prodrev.MARCA_ID and rub.RUBRO_ID=prodrev.RUBRO_ID and ven.VENTA_ID=det.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta and prodrev.RUBRO_ID=@rubroid group by PROD_REV_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"],
                                      registros["MARCA_NOMBRE"].ToString(),
                                      registros["PROD_REV_DESCR"].ToString(),
                                      registros["CANTIDADTOTALVENDIDA"].ToString(),
                                      registros["MONTOTOTALVENDIDO"].ToString());
                }
                registros.Close();
                conexion.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }

                calcularCantidadTotalVendida();
                calcularMontoTotalVendido();
                bloquear();
            }
            else if (comboBox1.Text == "Producto Elaborado" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1)
            {
                //FILTRADO POR TIPO DE PRODUCTO (PRODUCTO ELABORADO), RANGO DE FECHA, RUBRO, MARCA
                conexion.Open();
                string sql = "select prod_elab_descr, MARCA_NOMBRE, RUBRO_NOMBRE, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'MONTOTOTALVENDIDO', SUM(DET_VENTA_CANT) AS 'CANTIDADTOTALVENDIDA' from PRODUCTOELAB as prodelab, MARCA as mar, RUBRO as rub, DETALLEVENTA as det, VENTA as ven where mar.MARCA_ID=prodelab.MARCA_ID and  rub.RUBRO_ID=prodelab.RUBRO_ID and det.PROD_ELAB_ID=prodelab.PROD_ELAB_ID and ven.VENTA_ID=det.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta AND prodelab.RUBRO_ID=@rubroid AND prodelab.MARCA_ID=@marcaid group by PROD_ELAB_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"],
                                      registros["MARCA_NOMBRE"].ToString(),
                                      registros["PROD_ELAB_DESCR"].ToString(),
                                      registros["CANTIDADTOTALVENDIDA"].ToString(),
                                      registros["MONTOTOTALVENDIDO"].ToString());
                }
                registros.Close();
                conexion.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }

                calcularCantidadTotalVendida();
                calcularMontoTotalVendido();
                bloquear();
            }
            else if (comboBox1.Text == "Producto de Reventa" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1)
            {
                //FILTRADO POR TIPO DE PRODUCTO (PRODUCTO REVENTA), RANGO DE FECHA, RUBRO, MARCA
                conexion.Open();
                string sql = "select PROD_REV_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'MONTOTOTALVENDIDO', SUM(DET_VENTA_CANT) AS 'CANTIDADTOTALVENDIDA' from PRODUCTOREVENTA as prodrev, MARCA as mar, RUBRO as rub, DETALLEVENTA as det, VENTA as ven where det.PROD_REV_ID=prodrev.PROD_REV_ID and mar.MARCA_ID=prodrev.MARCA_ID and rub.RUBRO_ID=prodrev.RUBRO_ID and ven.VENTA_ID=det.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta and prodrev.RUBRO_ID=@rubroid and prodrev.MARCA_ID=@marcaid group by PROD_REV_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"],
                                      registros["MARCA_NOMBRE"].ToString(),
                                      registros["PROD_REV_DESCR"].ToString(),
                                      registros["CANTIDADTOTALVENDIDA"].ToString(),
                                      registros["MONTOTOTALVENDIDO"].ToString());
                }
                registros.Close();
                conexion.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }

                calcularCantidadTotalVendida();
                calcularMontoTotalVendido();
                bloquear();
            }
            else if (comboBox1.Text == "Producto Elaborado" && comboBox2.SelectedIndex !=-1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1)
            {
                //FILTRADO POR TIPO DE PRODUCTO (PRODUCTO ELABORADO), RANGO DE FECHA, RUBRO, MARCA, PRODUCTO
                conexion.Open();
                string sql = "select prod_elab_descr, MARCA_NOMBRE, RUBRO_NOMBRE, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'MONTOTOTALVENDIDO', SUM(DET_VENTA_CANT) AS 'CANTIDADTOTALVENDIDA' from PRODUCTOELAB as prodelab, MARCA as mar, RUBRO as rub, DETALLEVENTA as det, VENTA as ven where mar.MARCA_ID=prodelab.MARCA_ID and  rub.RUBRO_ID=prodelab.RUBRO_ID and det.PROD_ELAB_ID=prodelab.PROD_ELAB_ID and ven.VENTA_ID=det.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta AND prodelab.RUBRO_ID=@rubroid AND prodelab.MARCA_ID=@marcaid AND prodelab.PROD_ELAB_ID=@PRODELAB group by PROD_ELAB_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
                comando.Parameters.Add("@prodelab", SqlDbType.Int).Value = comboBox4.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"],
                                      registros["MARCA_NOMBRE"].ToString(),
                                      registros["PROD_ELAB_DESCR"].ToString(),
                                      registros["CANTIDADTOTALVENDIDA"].ToString(),
                                      registros["MONTOTOTALVENDIDO"].ToString());
                }
                registros.Close();
                conexion.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }

                calcularCantidadTotalVendida();
                calcularMontoTotalVendido();
                bloquear();
            }
            else if (comboBox1.Text == "Producto de Reventa" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1)
            {
                //FILTRADO POR TIPO DE PRODUCTO (PRODUCTO REVENTA), RANGO DE FECHA, RUBRO, MARCA, PRODUCTO
                conexion.Open();
                string sql = "select PROD_REV_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'MONTOTOTALVENDIDO', SUM(DET_VENTA_CANT) AS 'CANTIDADTOTALVENDIDA' from PRODUCTOREVENTA as prodrev, MARCA as mar, RUBRO as rub, DETALLEVENTA as det, VENTA as ven where det.PROD_REV_ID=prodrev.PROD_REV_ID and mar.MARCA_ID=prodrev.MARCA_ID and rub.RUBRO_ID=prodrev.RUBRO_ID and ven.VENTA_ID=det.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta and prodrev.RUBRO_ID=@rubroid and prodrev.MARCA_ID=@marcaid AND prodrev.PROD_REV_ID=@PRODREV group by PROD_REV_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
                comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = comboBox4.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"],
                                      registros["MARCA_NOMBRE"].ToString(),
                                      registros["PROD_REV_DESCR"].ToString(),
                                      registros["CANTIDADTOTALVENDIDA"].ToString(),
                                      registros["MONTOTOTALVENDIDO"].ToString());
                }
                registros.Close();
                conexion.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }

                calcularCantidadTotalVendida();
                calcularMontoTotalVendido();
                bloquear();
            }
            else if (comboBox1.Text == "Producto Elaborado" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1)
            {
                //FILTRADO POR TIPO DE PRODUCTO (PRODUCTO ELABORADO), RANGO DE FECHA, RUBRO, MARCA, PRODUCTO, VENDEDOR
                conexion.Open();
                string sql = "select prod_elab_descr, MARCA_NOMBRE, RUBRO_NOMBRE, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'MONTOTOTALVENDIDO', SUM(DET_VENTA_CANT) AS 'CANTIDADTOTALVENDIDA' from PRODUCTOELAB as prodelab, MARCA as mar, RUBRO as rub, DETALLEVENTA as det, VENTA as ven where mar.MARCA_ID=prodelab.MARCA_ID and  rub.RUBRO_ID=prodelab.RUBRO_ID and det.PROD_ELAB_ID=prodelab.PROD_ELAB_ID and ven.VENTA_ID=det.VENTA_ID and ven.VENTA_FECHA>=@FECHADESDE and VENTA_FECHA<=@FECHAHASTA AND prodelab.RUBRO_ID=@RUBROID AND prodelab.MARCA_ID=@MARCAID AND prodelab.PROD_ELAB_ID=@PRODELAB AND ven.USUARIO_ID=@USUARIOID group by PROD_ELAB_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
                comando.Parameters.Add("@prodelab", SqlDbType.Int).Value = comboBox4.SelectedValue;
                comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = comboBox5.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"],
                                      registros["MARCA_NOMBRE"].ToString(),
                                      registros["PROD_ELAB_DESCR"].ToString(),
                                      registros["CANTIDADTOTALVENDIDA"].ToString(),
                                      registros["MONTOTOTALVENDIDO"].ToString());
                }
                registros.Close();
                conexion.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }

                calcularCantidadTotalVendida();
                calcularMontoTotalVendido();
                bloquear();
            }
            else if (comboBox1.Text == "Producto de Reventa" && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1)
            {
                //FILTRADO POR TIPO DE PRODUCTO (PRODUCTO REVENTA), RANGO DE FECHA, RUBRO, MARCA, PRODUCTO, VENDEDOR
                conexion.Open();
                string sql = "select PROD_REV_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'MONTOTOTALVENDIDO', SUM(DET_VENTA_CANT) AS 'CANTIDADTOTALVENDIDA' from PRODUCTOREVENTA as prodrev, MARCA as mar, RUBRO as rub, DETALLEVENTA as det, VENTA as ven where det.PROD_REV_ID=prodrev.PROD_REV_ID and mar.MARCA_ID=prodrev.MARCA_ID and rub.RUBRO_ID=prodrev.RUBRO_ID and ven.VENTA_ID=det.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta and prodrev.RUBRO_ID=@rubroid and prodrev.MARCA_ID=@marcaid AND prodrev.PROD_REV_ID=@PRODREV AND ven.USUARIO_ID=@USUARIOID group by PROD_REV_DESCR, MARCA_NOMBRE, RUBRO_NOMBRE";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
                comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = comboBox4.SelectedValue;
                comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = comboBox5.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["RUBRO_NOMBRE"],
                                      registros["MARCA_NOMBRE"].ToString(),
                                      registros["PROD_REV_DESCR"].ToString(),
                                      registros["CANTIDADTOTALVENDIDA"].ToString(),
                                      registros["MONTOTOTALVENDIDO"].ToString());
                }
                registros.Close();
                conexion.Close();

                if (dataGridView1.Rows.Count == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se encontraron registros para esta búsqueda";
                    m.ShowDialog();
                }

                calcularCantidadTotalVendida();
                calcularMontoTotalVendido();
                bloquear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker2.Value = DateTime.Now;
            button2.Enabled = true;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox5.Enabled = true;
            label6.Text = "";
            label9.Text = "";
        }

        public void bloquear()
        {
            button2.Enabled = false;
            comboBox1.Enabled = false;
            comboBox2.Enabled = false;
            comboBox3.Enabled = false;
            comboBox4.Enabled = false;
            comboBox5.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "La búsqueda se puede filtrar por:\n" +
                "Tipo de producto y rango de fecha.\n" +
                "Tipo de producto, rango de fecha y rubro.\n" +
                "Tipo de producto, rango de fecha, rubro y marca.\n" +
                "Tipo de producto, rango de fecha, rubro, marca y producto.\n" +
                "Tipo de producto, rango de fecha, rubro, marca, producto y vendedor.";
            m.ShowDialog();
        }
    }
}
