using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GestionDeUsuarios
{
    public partial class AnalisisCompras : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        SqlDataAdapter DA;
        DataTable DT;
        DataRow DR;
        String Consulta;
        public AnalisisCompras()
        {
            InitializeComponent();
        }

        private void AnalisisCompras_Load(object sender, EventArgs e)
        {
            /*cargarComboBox1();
            cargarComboBox2();*/
            cargarComboBox4();
            comboBox4.SelectedIndex = -1;
            button4.Enabled = false;
        }
        private void cargarComboBox4()
        {
            conexion.Open();
            string sql = "select PROVEE_ID, PROVEE_NOMBRE from PROVEEDOR ORDER BY PROVEE_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox4.DisplayMember = "PROVEE_NOMBRE";
            comboBox4.ValueMember = "PROVEE_ID";
            comboBox4.DataSource = tabla1;
        }
        private void cargarComboBox1()
        {
            if (comboBox5.Text == "Materia Prima")
            {
                conexion.Open();
                string sql = "select mat.rubro_id, rubro_nombre from RUBRO as rubro, MATERIAPRIMA as mat where rubro.RUBRO_ID=mat.RUBRO_ID group by RUBRO_NOMBRE, mat.rubro_id";
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
            else if (comboBox5.Text == "Producto de Reventa")
            {
                conexion.Open();
                string sql = "select prod.rubro_id, rubro_nombre from RUBRO as rubro, PRODUCTOREVENTA as prod where rubro.RUBRO_ID=prod.RUBRO_ID group by RUBRO_NOMBRE, prod.RUBRO_ID";
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
        private void cargarComboBox2()
        {
            if (comboBox5.Text == "Materia Prima")
            {
                conexion.Open();
                string sql = "select mat.marca_id, marca_nombre from MARCA as marca, MATERIAPRIMA as mat where marca.MARCA_ID=mat.MARCA_ID group by MARCA_NOMBRE, mat.marca_id ORDER BY MARCA_NOMBRE ASC";
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
                comboBox2.DisplayMember = "MARCA_NOMBRE";
                comboBox2.ValueMember = "MARCA_ID";
                comboBox2.DataSource = tabla1;
            }
        }
        private void cargarComboBox3()
        {
            if (comboBox5.Text == "Materia Prima" && comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
            {
                conexion.Open();
                string sql = "select MATERIAPR_ID, MATERIAPR_DESCR from MATERIAPRIMA where RUBRO_ID=@rubroid and MARCA_ID=@marcaid ORDER BY MATERIAPR_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                comboBox3.DataSource = tabla1;
                comboBox3.DisplayMember = "MATERIAPR_DESCR";
                comboBox3.ValueMember = "MATERIAPR_ID";
                conexion.Close();
            }
            else if (comboBox5.Text == "Producto de Reventa" && comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1)
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
                comboBox3.DataSource = tabla1;
                comboBox3.DisplayMember = "PROD_REV_DESCR";
                comboBox3.ValueMember = "PROD_REV_ID";
                conexion.Close();
            }
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                Aviso m = new Aviso();
                m.label1.Text = "La primera fecha y hora deben ser anteriores a la segunda fecha y hora ingresada";
                m.ShowDialog();
            }
            else if (comboBox3.SelectedIndex == (-1))
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un artículo";
                m.ShowDialog();
            }
            else if (comboBox4.SelectedIndex == (-1) && comboBox5.Text == "Materia Prima")
            {
                button2.Enabled = false;
                comprasRealizadasMP();
                cantidadesCompradasMP();
                cantidadMaximaMP();
                cantidadMinimaMP();
                cantidadPromedioMP();
                precioMinMP();
                precioMaxMP();
                precioPromMP();
                fechaMinMP();
                fechaMaxMP();
                button4.Enabled = true;
            }
            else if (comboBox4.SelectedIndex == (-1) && comboBox5.Text == "Producto de Reventa")
            {

                button2.Enabled = false;
                comprasRealizadasPR();
                cantidadesCompradasPR();
                cantidadMaximaPR();
                cantidadMinimaPR();
                cantidadPromedioPR();
                precioMinPR();
                precioMaxPR();
                precioPromPR();
                fechaMinPR();
                fechaMaxPR();
                button4.Enabled = true;
            }
            else if (comboBox4.SelectedIndex != (-1) && comboBox5.Text == "Materia Prima")
            {

                button2.Enabled = false;
                comprasRealizadasMPProve();
                cantidadesCompradasMPProve();
                cantidadMaximaMPProve();
                cantidadMinimaMPProve();
                cantidadPromedioMPProve();
                precioMinMPProve();
                precioMaxMPProve();
                precioPromMPProve();
                fechaMinMPProve();
                fechaMaxMPProve();
                button4.Enabled = true;
            }
            else if (comboBox4.SelectedIndex != (-1) && comboBox5.Text == "Producto de Reventa")
            {

                button2.Enabled = false;
                comprasRealizadasPRProve();
                cantidadesCompradasPRProve();
                cantidadMaximaPRProve();
                cantidadMinimaPRProve();
                cantidadPromedioPRProve();
                precioMinPRProve();
                precioMaxPRProve();
                precioPromPRProve();
                fechaMinPRProve();
                fechaMaxPRProve();
                button4.Enabled = true;
            }
        }

        private void comprasRealizadasMP()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(det.COMPRA_ID) as 'Cantidad de compras' from DETALLECOMPRA as det, COMPRA as comp  where MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA <=@fechahasta and det.COMPRA_ID=comp.COMPRA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de compras"].ToString();
            cantidad = int.Parse(cant);
            conexion.Close();
            label6.Text = "Cantidad de compras realizadas: " + cantidad;
        }

        private void comprasRealizadasPR()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(det.COMPRA_ID) as 'Cantidad de compras' from DETALLECOMPRA as det, COMPRA AS comp where PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and det.COMPRA_ID=comp.COMPRA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de compras"].ToString();
            cantidad = int.Parse(cant);
            conexion.Close();
            label6.Text = "Cantidad de compras realizadas: " + cantidad;
        }

        private void comprasRealizadasMPProve()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(det.COMPRA_ID) as 'Cantidad de compras' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROVEE_ID=@proveeid and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de compras"].ToString();
            cantidad = int.Parse(cant);
            conexion.Close();
            label6.Text = "Cantidad de compras realizadas: " + cantidad;
        }

        private void comprasRealizadasPRProve()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(det.COMPRA_ID) as 'Cantidad de compras' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROVEE_ID=@proveeid and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de compras"].ToString();
            cantidad = int.Parse(cant);
            conexion.Close();
            label6.Text = "Cantidad de compras realizadas: " + cantidad;
        }

        private void cantidadesCompradasMP()
        {
            int cantidad;
            conexion.Open();
            string sql = "select SUM(det.DET_COMPRA_CANTIDAD) as 'Cantidad comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label7.Text = "Cantidad de unidades compradas: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label7.Text = "Cantidad de unidades compradas: " + cantidad;
            }
            conexion.Close();
        }

        private void cantidadesCompradasPR()
        {
            int cantidad;
            conexion.Open();
            string sql = "select SUM(det.DET_COMPRA_CANTIDAD) as 'Cantidad comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde AND COMPRA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label7.Text = "Cantidad de unidades compradas: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label7.Text = "Cantidad de unidades compradas: " + cantidad;
            }
            conexion.Close();
        }

        private void cantidadesCompradasMPProve()
        {
            int cantidad =0;
            conexion.Open();
            string sql = "select SUM(det.DET_COMPRA_CANTIDAD) as 'Cantidad comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA <=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label7.Text = "Cantidad de unidades compradas: ---";
            }
            else 
            {
                cantidad = int.Parse(cant);
                label7.Text = "Cantidad de unidades compradas: " + cantidad;
            }
            conexion.Close();
        }

        private void cantidadesCompradasPRProve()
        {
            int cantidad;
            conexion.Open();
            string sql = "select SUM(det.DET_COMPRA_CANTIDAD) as 'Cantidad comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde AND COMPRA_FECHA <=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label7.Text = "Cantidad de unidades compradas: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label7.Text = "Cantidad de unidades compradas: " + cantidad;
            }
            conexion.Close();
        }
        
        private void cantidadMaximaMP()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_COMPRA_CANTIDAD) as 'Cantidad maxima comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label9.Text = "Cantidad máxima comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label9.Text = "Cantidad máxima comprada: ";
                label18.Text= cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMaximaPR()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_COMPRA_CANTIDAD) as 'Cantidad maxima comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde AND COMPRA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label9.Text = "Cantidad máxima comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label9.Text = "Cantidad máxima comprada: ";
                label18.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMaximaMPProve()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_COMPRA_CANTIDAD) as 'Cantidad maxima comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA <=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label9.Text = "Cantidad máxima comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label9.Text = "Cantidad máxima comprada: ";
                label18.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMaximaPRProve()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "SELECT MAX(det.DET_COMPRA_CANTIDAD) as 'Cantidad maxima comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde AND COMPRA_FECHA <=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad maxima comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label9.Text = "Cantidad máxima comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label9.Text = "Cantidad máxima comprada: ";
                label18.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaMP()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_COMPRA_CANTIDAD) as 'Cantidad minima comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label10.Text = "Cantidad mínima comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label10.Text = "Cantidad mínima comprada: ";
                label19.Text= cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaPR()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_COMPRA_CANTIDAD) as 'Cantidad minima comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA <=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label10.Text = "Cantidad mínima comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label10.Text = "Cantidad mínima comprada: ";
                label19.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaMPProve()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_COMPRA_CANTIDAD) as 'Cantidad minima comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID = comp.COMPRA_ID and MATERIAPR_ID = @materia and COMPRA_FECHA>= @fechadesde and COMPRA_FECHA <= @fechahasta and PROVEE_ID = @proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label10.Text = "Cantidad mínima comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label10.Text = "Cantidad mínima comprada: ";
                label19.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadMinimaPRProve()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "SELECT MIN(det.DET_COMPRA_CANTIDAD) as 'Cantidad minima comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA <=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad minima comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label10.Text = "Cantidad mínima comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label10.Text = "Cantidad mínima comprada: ";
                label19.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioMP()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_COMPRA_CANTIDAD) as 'Cantidad promedio comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label11.Text = "Cantidad promedio comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label11.Text = "Cantidad promedio comprada: ";
                label20.Text= cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioPR()
        {
            int cantidad;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_COMPRA_CANTIDAD) as 'Cantidad promedio comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label11.Text = "Cantidad promedio comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label11.Text = "Cantidad promedio comprada: ";
                label20.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioMPProve()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_COMPRA_CANTIDAD) as 'Cantidad promedio comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label11.Text = "Cantidad promedio comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label11.Text = "Cantidad promedio comprada: ";
                label20.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void cantidadPromedioPRProve()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "SELECT AVG(det.DET_COMPRA_CANTIDAD) as 'Cantidad promedio comprada' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad promedio comprada"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label11.Text = "Cantidad promedio comprada: ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label11.Text = "Cantidad promedio comprada: ";
                label20.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioMinMP()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label13.Text = "Precio mínimo ($): ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label13.Text = "Precio mínimo ($):";
                label21.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioMinPR()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label13.Text = "Precio mínimo ($): ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label13.Text = "Precio mínimo ($):";
                label21.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioMinMPProve()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select MIN (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label13.Text = "Precio mínimo ($): ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label13.Text = "Precio mínimo ($):";
                label21.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioMinPRProve()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select MIN (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label13.Text = "Precio mínimo ($): ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label13.Text = "Precio mínimo ($):";
                label21.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioMaxMP()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label12.Text = "Precio máximo ($): ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label12.Text = "Precio máximo ($):";
                label22.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioMaxPR()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label12.Text = "Precio máximo ($): ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label12.Text = "Precio máximo ($):";
                label22.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioMaxMPProve()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select MAX (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label12.Text = "Precio máximo ($): ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label12.Text = "Precio máximo ($):";
                label22.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioMaxPRProve()
        {
            int cantidad = 0;
            conexion.Open();
            string sql = "select MAX (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label12.Text = "Precio máximo ($): ---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label12.Text = "Precio máximo ($):";
                label22.Text= cantidad.ToString();
            }
            conexion.Close();
        }
        private void precioPromMP()
        {
            float cantidad;
            conexion.Open();
            string sql = "select AVG (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label14.Text = "Precio promedio ($): ---";
            }
            else
            {
                cantidad = float.Parse(cant);
                label14.Text = "Precio promedio ($):";
                label23.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioPromPR()
        {
            float cantidad;
            conexion.Open();
            string sql = "select AVG (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label14.Text = "Precio promedio ($): ---";
            }
            else
            {
                cantidad = float.Parse(cant);
                label14.Text = "Precio promedio ($):";
                label23.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioPromMPProve()
        {
            float cantidad = 0;
            conexion.Open();
            string sql = "select AVG (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label14.Text = "Precio promedio ($): ---";
            }
            else
            {
                cantidad = float.Parse(cant);
                label14.Text = "Precio promedio ($):";
                label23.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void precioPromPRProve()
        {
            float cantidad = 0;
            conexion.Open();
            string sql = "select AVG (det.DET_COMPRA_PR_UNIT) as 'Precio' from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and PROVEE_ID=@proveeid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Precio"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label14.Text = "Precio promedio ($): ---";
            }
            else
            {
                cantidad = float.Parse(cant);
                label14.Text = "Precio promedio ($):";
                label23.Text= cantidad.ToString();
            }
            conexion.Close();
        }

        private void fechaMinMP()
        {
            conexion.Open();
            string sql = "select compra_fecha from COMPRA as comp, DETALLECOMPRA as det where det.COMPRA_ID=comp.COMPRA_ID and compra_fecha between @fechadesde AND @fechahasta and det_compra_pr_unit = (select Min(det.DET_COMPRA_PR_UNIT) from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta) group by compra_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["compra_fecha"]).ToString("dd/MM/yyyy");
                label16.Text = "Fecha más reciente de precio mínimo: " + fecha;
            }
            conexion.Close();
        }

        private void fechaMinMPProve()
        {
            conexion.Open();
            string sql = "select compra_fecha from COMPRA as comp, DETALLECOMPRA as det where det.COMPRA_ID=comp.COMPRA_ID and compra_fecha between @fechadesde AND @fechahasta and det_compra_pr_unit = (select Min(det.DET_COMPRA_PR_UNIT) from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and PROVEE_ID=@proveeid) group by compra_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["compra_fecha"]).ToString("dd/MM/yyyy");
                label16.Text = "Fecha más reciente de precio mínimo: " + fecha;
            }
            conexion.Close();
        }
        private void fechaMinPR()
        {
            conexion.Open();
            string sql = "select compra_fecha from COMPRA as comp, DETALLECOMPRA as det where det.COMPRA_ID=comp.COMPRA_ID and compra_fecha between @fechadesde AND @fechahasta and det_compra_pr_unit = (select Min(det.DET_COMPRA_PR_UNIT) from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev ) group by compra_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["compra_fecha"]).ToString("dd/MM/yyyy");
                label16.Text = "Fecha más reciente de precio mínimo: " + fecha;
            }
            conexion.Close();
        }

        private void fechaMinPRProve()
        {
            conexion.Open();
            string sql = "select compra_fecha from COMPRA as comp, DETALLECOMPRA as det where det.COMPRA_ID=comp.COMPRA_ID and compra_fecha between @fechadesde AND @fechahasta and det_compra_pr_unit = (select Min(det.DET_COMPRA_PR_UNIT) from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and PROVEE_ID=@proveeid) group by compra_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["compra_fecha"]).ToString("dd/MM/yyyy");
                label16.Text = "Fecha más reciente de precio mínimo: " + fecha;
            }
            conexion.Close();
        }

        private void fechaMaxPR()
        {
            conexion.Open();
            string sql = "select compra_fecha from COMPRA as comp, DETALLECOMPRA as det where det.COMPRA_ID=comp.COMPRA_ID and compra_fecha between @fechadesde AND @fechahasta and det_compra_pr_unit = (select Max(det.DET_COMPRA_PR_UNIT) from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev ) group by compra_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["compra_fecha"]).ToString("dd/MM/yyyy");
                label15.Text = "Fecha más reciente de precio máximo: " + fecha;
            }
            conexion.Close();
        }

        private void fechaMaxPRProve()
        {
            conexion.Open();
            string sql = "select compra_fecha from COMPRA as comp, DETALLECOMPRA as det where det.COMPRA_ID=comp.COMPRA_ID and compra_fecha between @fechadesde AND @fechahasta and det_compra_pr_unit = (select Max(det.DET_COMPRA_PR_UNIT) from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and PROD_REV_ID=@prodrev and PROVEE_ID=@proveeid) group by compra_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrev", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["compra_fecha"]).ToString("dd/MM/yyyy");
                label15.Text = "Fecha más reciente de precio máximo: " + fecha;
            }
            conexion.Close();
        }

        private void fechaMaxMP()
        {
            
            conexion.Open();
            string sql = "select compra_fecha from COMPRA as comp, DETALLECOMPRA as det where det.COMPRA_ID=comp.COMPRA_ID and compra_fecha between @fechadesde AND @fechahasta and det_compra_pr_unit = (select Max(det.DET_COMPRA_PR_UNIT) from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta) group by compra_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["compra_fecha"]).ToString("dd/MM/yyyy");
                label15.Text = "Fecha más reciente de precio máximo: " + fecha;

            }
            conexion.Close();
        }

        private void fechaMaxMPProve()
        {
            conexion.Open();
            string sql = "select compra_fecha from COMPRA as comp, DETALLECOMPRA as det where det.COMPRA_ID=comp.COMPRA_ID and compra_fecha between @fechadesde AND @fechahasta and det_compra_pr_unit = (select Max(det.DET_COMPRA_PR_UNIT) from DETALLECOMPRA as det, COMPRA as comp where det.COMPRA_ID=comp.COMPRA_ID and MATERIAPR_ID=@materia and COMPRA_FECHA>=@fechadesde and COMPRA_FECHA<=@fechahasta and PROVEE_ID=@proveeid) group by compra_fecha";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materia", SqlDbType.Int).Value = int.Parse(comboBox3.SelectedValue.ToString());
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = int.Parse(comboBox4.SelectedValue.ToString());
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["compra_fecha"]).ToString("dd/MM/yyyy");
                label15.Text = "Fecha más reciente de precio máximo: " + fecha;
            }
            conexion.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button4.Enabled = false;
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart1.Titles.Clear();
            chart2.Titles.Clear();
            comboBox3.SelectedIndex = (-1);
            comboBox4.SelectedIndex = (-1);
            comboBox5.SelectedIndex = (-1);
            comboBox3.DataSource = null;
            comboBox3.Items.Clear();
            dateTimePicker1.Text = DateTime.Now.ToString();
            dateTimePicker2.Text = DateTime.Now.ToString();
            label14.Text = "Precio promedio ($): ";
            label12.Text = "Precio máximo ($): ";
            label13.Text = "Precio mínimo ($): ";
            label11.Text = "Cantidad promedio comprada: ";
            label10.Text = "Cantidad mínima comprada: ";
            label9.Text = "Cantidad máxima comprada: ";
            label7.Text = "Cantidad de unidades compradas: ";
            label6.Text = "Cantidad de compras realizadas: ";
            label16.Text = "Fecha más reciente de precio mínimo: ";
            label15.Text = "Fecha más reciente de precio máximo: ";
            label18.Text = "";
            label19.Text = "";
            label20.Text = "";
            label21.Text = "";
            label22.Text = "";
            label23.Text = "";

        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (label14.Text=="Precio promedio ($): ---")

            {
                Aviso m = new Aviso();
                m.label1.Text = "Para dibujar el gráfico deben tener valores todos los conceptos";
                m.ShowDialog();
            }
            else
            {
                button4.Enabled = false;
                string[] series = { "Cant mín comprada", "Cant máx comprada", "Cant prom comprada" };
                int[] puntos = { int.Parse(label19.Text), int.Parse(label18.Text), int.Parse(label20.Text) };
                chart1.Palette = ChartColorPalette.Pastel;

                chart1.Titles.Add("Cantidades");
                for (int i = 0; i < series.Length; i++)
                {
                    Series serie = chart1.Series.Add(series[i]);
                    serie.Label = puntos[i].ToString();
                    serie.Points.Add(puntos[i]);

                }

                string[] series1 = { "Precio mín", "Precio máx", "Precio prom" };
                int[] puntos1 = { int.Parse(label21.Text), int.Parse(label22.Text), (int)float.Parse(label23.Text) };
                chart1.Palette = ChartColorPalette.Pastel;

                chart2.Titles.Add("Precios");
                for (int i = 0; i < series.Length; i++)
                {
                    Series serie = chart2.Series.Add(series1[i]);
                    serie.Label = "$" + puntos1[i].ToString();
                    serie.Points.Add(puntos1[i]);

                }
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void comboBox5_SelectedValueChanged(object sender, EventArgs e)
        {
            cargarComboBox1();
            cargarComboBox2();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            cargarComboBox3();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            cargarComboBox3();
        }

        private void pictureBox14_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "La búsqueda se puede filtrar por:\n" +
                "Rango de fecha, tipo de producto, rubro, marca y artículo.\n" +
                "Rango de fecha, tipo de producto, rubro, marca, artículo y proveedor.\n";
            m.ShowDialog();
        }
    }
}