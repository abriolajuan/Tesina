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
    public partial class ListadoVendedores : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        public ListadoVendedores()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mostrarGrillaDeTodos()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.TOTALDEVENTAS, T3.TOTALPESOS, T3.TOTALPESOS-T3.COBROS AS SALDO, T2.TOTALVENDIDOUNIDADES FROM (SELECT  (USUARIO_NOMBRE+' '+USUARIO_APELLIDO) AS NOMBRE_COMPLETO, us.USUARIO_ID, COUNT(ven.VENTA_ID)AS 'TOTALDEVENTAS' FROM USUARIO us, VENTA ven where ven.usuario_id=US.usuario_id group by usuario_nombre, usuario_apellido, US.USUARIO_ID) t1 left join (SELECT US.USUARIO_ID, SUM(DET_VENTA_CANT) AS 'TOTALVENDIDOUNIDADES' FROM USUARIO US, VENTA VEN, DETALLEVENTA DET WHERE VEN.USUARIO_ID=US.USUARIO_ID AND DET.VENTA_ID=VEN.VENTA_ID GROUP BY US.USUARIO_ID) T2 ON (t1.USUARIO_ID=T2.USUARIO_ID) LEFT JOIN (select ta1.USUARIO_ID, TA1.TOTALVENDIDOPESOS-TA3.descuento AS 'TOTALPESOS', TA2.COBROS FROM (SELECT US.USUARIO_ID, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0)))AS 'TOTALVENDIDOPESOS'FROM USUARIO US, DETALLEVENTA DET, VENTA VEN WHERE DET.VENTA_ID=VEN.VENTA_ID AND VEN.USUARIO_ID=US.USUARIO_ID GROUP BY US.USUARIO_ID)TA1 LEFT JOIN (SELECT US.USUARIO_ID, COALESCE(SUM(COBRO_VENTA_MONTO),0) AS 'COBROS'FROM VENTA VEN LEFT JOIN COBROVENTA COB ON VEN.VENTA_ID=COB.VENTA_ID LEFT JOIN USUARIO US ON VEN.USUARIO_ID=US.USUARIO_ID GROUP BY US.USUARIO_ID)TA2 ON (TA1.USUARIO_ID=TA2.USUARIO_ID)LEFT JOIN (SELECT USU.USUARIO_ID, SUM(VENTA_DTO) AS 'DESCUENTO'FROM VENTA AS VEN, USUARIO AS USU WHERE VEN.USUARIO_ID=USU.USUARIO_ID GROUP BY USU.USUARIO_ID) TA3 ON (TA2.USUARIO_ID=TA3.USUARIO_ID))T3 ON (T3.USUARIO_ID=T1.USUARIO_ID)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["TOTALDEVENTAS"].ToString(),
                                    registros["TOTALVENDIDOUNIDADES"].ToString(),
                                  registros["TOTALPESOS"].ToString(),
                                  registros["SALDO"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void filtradoFecha()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.TOTALDEVENTAS, T3.TOTALPESOS, T3.TOTALPESOS-T3.COBROS AS SALDO, T2.TOTALVENDIDOUNIDADES FROM (SELECT  (USUARIO_NOMBRE+' '+USUARIO_APELLIDO) AS NOMBRE_COMPLETO, us.USUARIO_ID, COUNT(ven.VENTA_ID)AS 'TOTALDEVENTAS' FROM USUARIO us, VENTA ven where ven.usuario_id=US.usuario_id and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta group by usuario_nombre, usuario_apellido, US.USUARIO_ID) t1 left join (SELECT US.USUARIO_ID, SUM(DET_VENTA_CANT) AS 'TOTALVENDIDOUNIDADES' FROM USUARIO US, VENTA VEN, DETALLEVENTA DET WHERE VEN.USUARIO_ID=US.USUARIO_ID AND DET.VENTA_ID=VEN.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta GROUP BY US.USUARIO_ID) T2 ON (t1.USUARIO_ID=T2.USUARIO_ID)LEFT JOIN (select ta1.USUARIO_ID, TA1.TOTALVENDIDOPESOS-TA3.descuento AS 'TOTALPESOS', TA2.COBROS FROM (SELECT US.USUARIO_ID, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0)))AS 'TOTALVENDIDOPESOS'FROM USUARIO US, DETALLEVENTA DET, VENTA VEN WHERE DET.VENTA_ID=VEN.VENTA_ID AND VEN.USUARIO_ID=US.USUARIO_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta GROUP BY US.USUARIO_ID)TA1 LEFT JOIN (SELECT US.USUARIO_ID, COALESCE(SUM(COBRO_VENTA_MONTO),0) AS 'COBROS'FROM VENTA VEN LEFT JOIN COBROVENTA COB ON VEN.VENTA_ID=COB.VENTA_ID LEFT JOIN USUARIO US ON VEN.USUARIO_ID=US.USUARIO_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta GROUP BY US.USUARIO_ID)TA2 ON (TA1.USUARIO_ID=TA2.USUARIO_ID)LEFT JOIN (SELECT USU.USUARIO_ID, SUM(VENTA_DTO) AS 'DESCUENTO'FROM VENTA AS VEN, USUARIO AS USU WHERE VEN.USUARIO_ID=USU.USUARIO_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta GROUP BY USU.USUARIO_ID) TA3 ON (TA2.USUARIO_ID=TA3.USUARIO_ID))T3 ON (T3.USUARIO_ID=T1.USUARIO_ID)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["TOTALDEVENTAS"].ToString(),
                                    registros["TOTALVENDIDOUNIDADES"].ToString(),
                                  registros["TOTALPESOS"].ToString(),
                                  registros["SALDO"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void filtradoFechaVendedor()
        {
            conexion.Open();
            string sql = "SELECT T1.NOMBRE_COMPLETO, T1.TOTALDEVENTAS, T3.TOTALPESOS, T3.TOTALPESOS-T3.COBROS AS SALDO, T2.TOTALVENDIDOUNIDADES FROM (SELECT  (USUARIO_NOMBRE+' '+USUARIO_APELLIDO) AS NOMBRE_COMPLETO, us.USUARIO_ID,COUNT(ven.VENTA_ID)AS 'TOTALDEVENTAS' FROM USUARIO us, VENTA ven where ven.usuario_id=US.usuario_id and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta and US.USUARIO_ID=@usuarioid group by usuario_nombre, usuario_apellido, US.USUARIO_ID) t1 left join (SELECT US.USUARIO_ID, SUM(DET_VENTA_CANT) AS 'TOTALVENDIDOUNIDADES' FROM USUARIO US, VENTA VEN, DETALLEVENTA DET WHERE VEN.USUARIO_ID=US.USUARIO_ID AND DET.VENTA_ID=VEN.VENTA_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta and US.USUARIO_ID=@usuarioid GROUP BY US.USUARIO_ID) T2 ON (t1.USUARIO_ID=T2.USUARIO_ID)LEFT JOIN (select ta1.USUARIO_ID, TA1.TOTALVENDIDOPESOS-TA3.descuento AS 'TOTALPESOS', TA2.COBROS FROM (SELECT US.USUARIO_ID, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0)))AS 'TOTALVENDIDOPESOS'FROM USUARIO US, DETALLEVENTA DET,VENTA VEN WHERE DET.VENTA_ID=VEN.VENTA_ID AND VEN.USUARIO_ID=US.USUARIO_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta and US.USUARIO_ID=@usuarioid GROUP BY US.USUARIO_ID)TA1 LEFT JOIN (SELECT US.USUARIO_ID, COALESCE(SUM(COBRO_VENTA_MONTO),0) AS 'COBROS'FROM VENTA VEN LEFT JOIN COBROVENTA COB ON VEN.VENTA_ID=COB.VENTA_ID LEFT JOIN USUARIO US ON VEN.USUARIO_ID=US.USUARIO_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta and US.USUARIO_ID=@usuarioid GROUP BY US.USUARIO_ID)TA2 ON (TA1.USUARIO_ID=TA2.USUARIO_ID)LEFT JOIN (SELECT USU.USUARIO_ID, SUM(VENTA_DTO) AS 'DESCUENTO'FROM VENTA AS VEN, USUARIO AS USU WHERE VEN.USUARIO_ID=USU.USUARIO_ID and ven.VENTA_FECHA>=@fechadesde and ven.VENTA_FECHA<=@fechahasta and USU.USUARIO_ID=@usuarioid GROUP BY USU.USUARIO_ID) TA3 ON (TA2.USUARIO_ID=TA3.USUARIO_ID))T3 ON (T3.USUARIO_ID=T1.USUARIO_ID)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = comboBox4.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"].ToString(),
                                    registros["TOTALDEVENTAS"].ToString(),
                                    registros["TOTALVENDIDOUNIDADES"].ToString(),
                                  registros["TOTALPESOS"].ToString(),
                                  registros["SALDO"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        public void calcularTotalVentas()
        {
            decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[1].Value);
            }
            label1.Text = suma.ToString();
        }

        public void calcularTotalUnidades()
        {
            decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[2].Value);
            }
            label4.Text = suma.ToString();
        }

        public void calcularMontoTotal()
        {
            decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
            }
            label6.Text = suma.ToString();
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

        private void ListadoVendedores_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Value = DateTime.Now.Date;
            cargarComboBox4(); 
            comboBox4.SelectedIndex = -1;
            mostrarGrillaDeTodos();
            calcularMontoTotal();
            calcularSaldoTotal();
            calcularTotalUnidades();
            calcularTotalVentas();
        }

        private void cargarComboBox4()
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

        private void button2_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                Aviso m = new Aviso();
                m.label1.Text = "La primera fecha y hora ingresadas deben ser anteriores a la segunda fecha y hora ingresadas";
                m.ShowDialog();
            }
            else if (comboBox4.SelectedIndex == -1)
            {
                filtradoFecha();
                calcularMontoTotal();
                calcularSaldoTotal();
                calcularTotalUnidades();
                calcularTotalVentas();
                button2.Enabled = false;
                comboBox4.Enabled = false;
            }
            else if (comboBox4.SelectedIndex != -1)
            {
                filtradoFechaVendedor();
                calcularMontoTotal();
                calcularSaldoTotal();
                calcularTotalUnidades();
                calcularTotalVentas();
                button2.Enabled = false;
                comboBox4.Enabled = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            mostrarGrillaDeTodos();
            comboBox4.SelectedIndex = -1;
            dateTimePicker1.Value = DateTime.Now.Date;
            dateTimePicker2.Value = DateTime.Now;
            button2.Enabled = true;
            comboBox4.Enabled = true;
            label1.Text = "-";
            label4.Text = "-";
            label6.Text = "-";
            label9.Text = "-";
        }

        private void pictureBox3_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "La búsqueda se puede filtrar por:\n" +
                "Rango de fecha.\n" +
                "Rango de fecha y vendedor.\n";
            m.ShowDialog();
        }
    }
}
