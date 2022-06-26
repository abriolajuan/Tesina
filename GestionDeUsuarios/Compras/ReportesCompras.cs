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

namespace GestionDeUsuarios
{
    public partial class ReportesCompras : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        SqlDataAdapter DA;
        DataTable DT;
        DataRow DR;
        String Consulta;

        public ReportesCompras()
        {
            InitializeComponent();
        }

        private void ReportesCompras_Load(object sender, EventArgs e)
        {
            Consulta= "select SUM(DET_COMPRA_CANTIDAD * DET_COMPRA_PR_UNIT) as total, PROVEE_NOMBRE FROM COMPRA as comp join PROVEEDOR as prov on prov.PROVEE_ID = comp.PROVEE_ID join DETALLECOMPRA as det on det.COMPRA_ID = comp.COMPRA_ID  GROUP BY PROVEE_NOMBRE";
            DA = new SqlDataAdapter(Consulta, conexion);
            DT = new DataTable();
            DA.Fill(DT);
            this.chart1.Palette = ChartColorPalette.Pastel;
            this.chart1.Titles.Add("Montos totales de compras por proveedor");
            if (DT.Rows.Count>0)
            {
                    foreach (DataRow row in DT.Rows)
                    {
                        Series series = this.chart1.Series.Add(row.ItemArray[1].ToString());
                        series.Points.Add(Convert.ToDouble(row.ItemArray[0]));
                        series.Label = "$" +  row[0].ToString();
                    }
            }

            cargarComboBox1();
            comboBox1.SelectedIndex = (-1);
            mostrarGrillaTodos();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void cargarComboBox1()
        {
            conexion.Open();
            string sql = "select PROVEE_ID, PROVEE_NOMBRE from PROVEEDOR ORDER BY PROVEE_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox1.DisplayMember = "PROVEE_NOMBRE";
            comboBox1.ValueMember = "PROVEE_ID";
            comboBox1.DataSource = tabla1;
        }

        public int identificadorIdCompra()
        {
            int idCompra;
            conexion.Open();
            string sql = "SELECT COMPRA_ID FROM COMPRA WHERE PROVEE_ID= @proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = comboBox1.SelectedValue;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idCom = registro["COMPRA_ID"].ToString();
            idCompra = int.Parse(idCom);
            conexion.Close();
            return idCompra;
        }

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "select  SUM (DET_COMPRA_CANTIDAD * DET_COMPRA_PR_UNIT) as total, COMPRA_NUM_FACT, PROVEE_NOMBRE, COMPRA_FECHA from COMPRA as comp join PROVEEDOR as prov on prov.PROVEE_ID = comp.PROVEE_ID join DETALLECOMPRA as det on det.COMPRA_ID = comp.COMPRA_ID  WHERE comp.PROVEE_ID = @proveeid GROUP BY COMPRA_NUM_FACT, prov.PROVEE_NOMBRE, COMPRA_FECHA";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = comboBox1.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["COMPRA_NUM_FACT"].ToString(),
                                  registros["PROVEE_NOMBRE"].ToString(), registros["COMPRA_FECHA"],
                                  registros["total"].ToString()); 
            }
            registros.Close();
            conexion.Close();
        }

        private void mostrarGrillaTodos()
        {
            conexion.Open();
            string sql = "select  SUM (DET_COMPRA_CANTIDAD * DET_COMPRA_PR_UNIT) as total, COMPRA_NUM_FACT, PROVEE_NOMBRE, COMPRA_FECHA from COMPRA" +
                " as comp join PROVEEDOR as prov on prov.PROVEE_ID = comp.PROVEE_ID join DETALLECOMPRA as det on det.COMPRA_ID = comp.COMPRA_ID " +
                "GROUP BY COMPRA_NUM_FACT, prov.PROVEE_NOMBRE, COMPRA_FECHA";
            SqlCommand comando = new SqlCommand(sql, conexion);            
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["COMPRA_NUM_FACT"].ToString(),
                                  registros["PROVEE_NOMBRE"].ToString(),registros["COMPRA_FECHA"],
                                  registros["total"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void mostrarGrillaTodosFecha()
        {
            conexion.Open();
            string sql = "select  SUM (DET_COMPRA_CANTIDAD * DET_COMPRA_PR_UNIT) as total, COMPRA_NUM_FACT, PROVEE_NOMBRE, COMPRA_FECHA from COMPRA as comp join PROVEEDOR as prov on prov.PROVEE_ID = comp.PROVEE_ID join DETALLECOMPRA as det on det.COMPRA_ID = comp.COMPRA_ID  WHERE COMPRA_FECHA >= @fechadesde and COMPRA_FECHA<= @fechahasta GROUP BY COMPRA_NUM_FACT, prov.PROVEE_NOMBRE, COMPRA_FECHA";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["COMPRA_NUM_FACT"].ToString(),
                                  registros["PROVEE_NOMBRE"].ToString(), registros["COMPRA_FECHA"],
                                  registros["total"].ToString());
            }
            registros.Close();
            conexion.Close();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex == (-1))
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar algún proveedor";
                m.ShowDialog();
            }
            else
            {
                mostrarGrilla();
                comboBox1.Enabled = false;
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if(dateTimePicker1.Value > dateTimePicker2.Value)
            {
                Aviso m = new Aviso();
                m.label1.Text = "La primera fecha y hora deben ser anteriores a la segunda fecha y hora ingresada";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == (-1))
            {
                mostrarGrillaTodosFecha();
            }
            else
            {
                dateTimePicker1.Enabled = false;
                dateTimePicker2.Enabled = false;
                conexion.Open();
                string sql = "select  SUM (DET_COMPRA_CANTIDAD * DET_COMPRA_PR_UNIT) as total, COMPRA_NUM_FACT, PROVEE_NOMBRE, COMPRA_FECHA from COMPRA as comp join PROVEEDOR as prov on prov.PROVEE_ID = comp.PROVEE_ID join DETALLECOMPRA as det on det.COMPRA_ID = comp.COMPRA_ID  WHERE comp.PROVEE_ID = @proveeid and COMPRA_FECHA >= @fechadesde and COMPRA_FECHA<= @fechahasta  GROUP BY COMPRA_NUM_FACT, prov.PROVEE_NOMBRE, COMPRA_FECHA";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    dataGridView1.Rows.Add(registros["COMPRA_NUM_FACT"].ToString(),
                                      registros["PROVEE_NOMBRE"].ToString(), registros["COMPRA_FECHA"],
                                      registros["total"].ToString());
                }
                registros.Close();
                conexion.Close();
            }
            comboBox1.Enabled = false;
            dateTimePicker1.Enabled = false;
            dateTimePicker2.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = (-1);
            comboBox1.Enabled = true;
            dateTimePicker1.Enabled = true;
            dateTimePicker2.Enabled = true;
            mostrarGrillaTodos();
            //dataGridView1.Rows.Clear();
            dateTimePicker1.Text = DateTime.Now.ToString();
            dateTimePicker2.Text = DateTime.Now.ToString();
        }

        private void cancelar_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            SaveFileDialog saveFileDialog1 = new SaveFileDialog();
            saveFileDialog1.Filter = "JPeg Imagen|*.jpg|Bitmap Imagen|*.bmp|PNG Imagen|*.png";
            saveFileDialog1.Title = "Guardar Grafico en Imagen";
            saveFileDialog1.ShowDialog();
            if (saveFileDialog1.FileName != "")
            {
                System.IO.FileStream fs =
                (System.IO.FileStream)saveFileDialog1.OpenFile();
                switch (saveFileDialog1.FilterIndex)
                {
                    case 1:
                        this.chart1.SaveImage(fs, ChartImageFormat.Jpeg);
                        break;
                    case 2:
                        this.chart1.SaveImage(fs, ChartImageFormat.Bmp);
                        break;
                    case 3:
                        this.chart1.SaveImage(fs, ChartImageFormat.Png);
                        break;
                }
                fs.Close();
            }
        }
    }
}
