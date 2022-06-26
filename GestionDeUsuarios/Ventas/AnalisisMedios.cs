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

namespace GestionDeUsuarios.Ventas
{
    public partial class AnalisisMedios : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        public AnalisisMedios()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void AnalisisMedios_Load(object sender, EventArgs e)
        {
        }

        private void cargarComboBox1()
        {
            if (comboBox6.Text == "Producto Elaborado")
            {
                conexion.Open();
                string sql = "select rubro.rubro_id, rubro_nombre from RUBRO as rubro, PRODUCTOELAB as prod where rubro.RUBRO_ID=prod.RUBRO_ID group by RUBRO_NOMBRE, rubro.RUBRO_ID ORDER BY RUBRO_NOMBRE ASC";
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
            else if (comboBox6.Text == "Producto de Reventa")
            {
                conexion.Open();
                string sql = "select rubro.rubro_id, rubro_nombre from RUBRO as rubro, PRODUCTOREVENTA as pr where rubro.RUBRO_ID = pr.RUBRO_ID group by RUBRO_NOMBRE, rubro.RUBRO_ID ORDER BY RUBRO_NOMBRE ASC";
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

        private void calcularPromedio()
        {
            Promedioefectivo = (Convert.ToDouble(totalefectivo) / Convert.ToDouble(cantidadcobros)) * 100;
            Promediocredito = (Convert.ToDouble(totalcredito) / Convert.ToDouble(cantidadcobros)) * 100;
            Promediodebito = (Convert.ToDouble(totaldebito) / Convert.ToDouble(cantidadcobros)) * 100;
            Promediotransferencia = (Convert.ToDouble(totaltransferencia) / Convert.ToDouble(cantidadcobros)) * 100;
            label18.Text = Promedioefectivo.ToString();
            label17.Text = Promediodebito.ToString();
            label16.Text = Promediocredito.ToString();
            label8.Text = Promediotransferencia.ToString();
            label19.Text = "%";
            label20.Text = "%";
            label21.Text = "%";
            label22.Text = "%";
        }

        private void imprimirEfectivo()
        {
            label32.Text = totalefectivo.ToString();
        }
        private void imprimirCredito()
        {
            label5.Text = totalcredito.ToString();
        }
        private void imprimirDebito()
        {
            label33.Text = totaldebito.ToString();
        }
        private void imprimirTransferencia()
        {
            label1.Text = totaltransferencia.ToString();
        }
        private void imprimirTotalesCobros()
        {
            label15.Text = montototal.ToString();
            label14.Text = cantidadcobros.ToString();
        }
        
        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button4.Enabled = false;
            chart1.Series["Series1"].Points.Clear();
            chart1.Titles.Clear();
            dateTimePicker1.Text = DateTime.Now.Date.ToString();
            dateTimePicker2.Text = DateTime.Now.ToString();
            comboBox1.SelectedIndex = -1;
            comboBox6.SelectedIndex = -1;
            comboBox1.Enabled = true;
            comboBox6.Enabled = true;
            label32.Text = "-";
            label33.Text = "-";
            label5.Text = "-";
            label1.Text = "-";
            label15.Text = "-";
            label14.Text = "-";
            label18.Text = "-";
            label17.Text = "-";
            label16.Text = "-";
            label8.Text = "-";
            Promedioefectivo = 0;
            Promediodebito = 0;
            Promediocredito = 0;
            Promediotransferencia = 0;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (label32.Text == "-" || label33.Text == "-" || label5.Text == "-" || label1.Text == "-")

            {
                Aviso m = new Aviso();
                m.label1.Text = "Para dibujar el gráfico deben tener valores todos los conceptos";
                m.ShowDialog();
            }
            else
            {
                string[] series2 = { "Efectivo", "Tarj. Débito", "Tarj. Crédito", "Transferencia" };
                int[] puntos1 = { int.Parse(label32.Text), int.Parse(label33.Text), (int)float.Parse(label5.Text), (int)float.Parse(label1.Text) };
                chart1.Palette = ChartColorPalette.Pastel;

                chart1.Titles.Add("Cobros: proporciones según medio");
                for (int i = 0; i < series2.Length; i++)
                {
                    chart1.Series["Series1"].Points.AddXY(series2[i], puntos1[i]);
                }
            }
            }


        Double Promedioefectivo, Promediodebito, Promediocredito, Promediotransferencia;

        private void pictureBox5_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "La búsqueda se puede filtrar por:\n" +
                "Rango de fecha.\n" +
                "Rango de fecha, producto elaborado y rubro.\n" +
                "Rango de fecha, producto reventa y rubro.\n";
            m.ShowDialog();
        }

        private void comboBox6_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarComboBox1();
            comboBox1.SelectedIndex = -1;
        }

        string montototal, cantidadcobros, totalefectivo, totalcredito, totaldebito, totaltransferencia;

        private void button2_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                Aviso m = new Aviso();
                m.label1.Text = "La primera fecha y hora ingresadas deben ser anteriores a la segunda fecha y hora ingresadas";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox6.SelectedIndex == -1)
            {
                //Filtrado por rango de fecha
                button4.Enabled = true;
                button2.Enabled = false;
                comboBox1.Enabled = false;
                comboBox6.Enabled = false;

                conexion.Open();
                string sql = "select coalesce(sum(COBRO_VENTA_MONTO), 0) as montototalcobrado, COUNT(DISTINCT COBRO_VENTA_ID) as cantidadcobros from COBROVENTA as cob where COBRO_VENTA_FECHA >= @fechadesde and COBRO_VENTA_FECHA <= @fechahasta";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataReader registros = comando.ExecuteReader();
                while (registros.Read())
                {
                    montototal = registros["montototalcobrado"].ToString();
                    cantidadcobros = registros["cantidadcobros"].ToString();
                }
                registros.Close();
                conexion.Close();
                imprimirTotalesCobros();

                conexion.Open();
                string sql1 = "select isnull (count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosEf from COBROVENTA as cob, DETALLEDEMEDIO as det where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '1' and COBRO_VENTA_FECHA >= @fechadesde1 and COBRO_VENTA_FECHA <= @fechahasta1";
                SqlCommand comando1 = new SqlCommand(sql1, conexion);
                comando1.Parameters.Add("@fechadesde1", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando1.Parameters.Add("@fechahasta1", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataReader registros1 = comando1.ExecuteReader();
                while (registros1.Read())
                {
                    totalefectivo = registros1["totalcobrosEf"].ToString();
                }
                registros1.Close();
                conexion.Close();
                imprimirEfectivo();

                conexion.Open();
                string sql2 = "select isnull (count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosCred from COBROVENTA as cob, DETALLEDEMEDIO as det where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '2' and COBRO_VENTA_FECHA >=@fechadesde3 and COBRO_VENTA_FECHA <= @fechahasta3";
                SqlCommand comando2 = new SqlCommand(sql2, conexion);
                comando2.Parameters.Add("@fechadesde3", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando2.Parameters.Add("@fechahasta3", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataReader registros2 = comando2.ExecuteReader();
                while (registros2.Read())
                {
                    totalcredito = registros2["totalcobrosCred"].ToString();
                }
                registros2.Close();
                conexion.Close();
                imprimirCredito();

                conexion.Open();
                string sql3 = "select isnull(count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosDeb from COBROVENTA as cob, DETALLEDEMEDIO as det where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '3' and COBRO_VENTA_FECHA >= @fechadesde4 and COBRO_VENTA_FECHA <= @fechahasta4";
                SqlCommand comando3 = new SqlCommand(sql3, conexion);
                comando3.Parameters.Add("@fechadesde4", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando3.Parameters.Add("@fechahasta4", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataReader registros3 = comando3.ExecuteReader();
                while (registros3.Read())
                {
                    totaldebito = registros3["totalcobrosDeb"].ToString();
                }
                registros3.Close();
                conexion.Close();
                imprimirDebito();

                conexion.Open();
                string sql4 = "select isnull(count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosTrans from COBROVENTA as cob, DETALLEDEMEDIO as det where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '4' and COBRO_VENTA_FECHA >= @fechadesde5 and COBRO_VENTA_FECHA <= @fechahasta5";
                SqlCommand comando4 = new SqlCommand(sql4, conexion);
                comando4.Parameters.Add("@fechadesde5", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando4.Parameters.Add("@fechahasta5", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataReader registros4 = comando4.ExecuteReader();
                while (registros4.Read())
                {
                    totaltransferencia = registros4["totalcobrosTrans"].ToString();
                }
                registros4.Close();
                conexion.Close();
                imprimirTransferencia();
                calcularPromedio();
            }
            else if (comboBox6.Text== "Producto Elaborado" && comboBox1.SelectedIndex != -1)
            {
                //Filtrado por rango de fecha, producto elaborado y rubro
                button4.Enabled = true;
                button2.Enabled = false;
                comboBox1.Enabled = false;
                comboBox6.Enabled = false;
                conexion.Open();
                string sql = "select coalesce(sum(COBRO_VENTA_MONTO), 0) as montototalcobrado, COUNT(DISTINCT COBRO_VENTA_ID) as cantidadcobros from COBROVENTA as cob, VENTA as ven, DETALLEVENTA as det, PRODUCTOELAB as prod where COBRO_VENTA_FECHA >= @fechadesde and COBRO_VENTA_FECHA <= @fechahasta and cob.VENTA_ID=ven.VENTA_ID and cob.VENTA_ID=det.VENTA_ID and det.PROD_ELAB_ID=prod.PROD_ELAB_ID and prod.RUBRO_ID=@rubro";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                while (registros.Read())
                {
                    montototal = registros["montototalcobrado"].ToString();
                    cantidadcobros = registros["cantidadcobros"].ToString();
                }
                registros.Close();
                conexion.Close();
                imprimirTotalesCobros();

                conexion.Open();
                string sql1 = "select isnull(count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosEf from COBROVENTA as cob, DETALLEDEMEDIO as det, VENTA as vent, DETALLEVENTA as detv, PRODUCTOELAB as prod where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '1' and vent.VENTA_ID = cob.VENTA_ID and COBRO_VENTA_FECHA >= @fechadesde1 and COBRO_VENTA_FECHA <= @fechahasta1 and vent.VENTA_ID = detv.VENTA_ID and detv.PROD_ELAB_ID = prod.PROD_ELAB_ID and prod.RUBRO_ID = @rubro1";
                SqlCommand comando1 = new SqlCommand(sql1, conexion);
                comando1.Parameters.Add("@fechadesde1", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando1.Parameters.Add("@fechahasta1", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando1.Parameters.Add("@rubro1", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros1 = comando1.ExecuteReader();
                while (registros1.Read())
                {
                    totalefectivo = registros1["totalcobrosEf"].ToString();
                }
                registros1.Close();
                conexion.Close();
                imprimirEfectivo();

                conexion.Open();
                string sql2 = "select isnull(count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosCred from COBROVENTA as cob, DETALLEDEMEDIO as det, VENTA as vent, DETALLEVENTA as detv, PRODUCTOELAB as prod where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '2' and vent.VENTA_ID = cob.VENTA_ID and COBRO_VENTA_FECHA >= @fechadesde2 and COBRO_VENTA_FECHA <= @fechahasta2 and vent.VENTA_ID = detv.VENTA_ID and detv.PROD_ELAB_ID = prod.PROD_ELAB_ID and prod.RUBRO_ID = @rubro2";
                SqlCommand comando2 = new SqlCommand(sql2, conexion);
                comando2.Parameters.Add("@fechadesde2", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando2.Parameters.Add("@fechahasta2", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando2.Parameters.Add("@rubro2", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros2 = comando2.ExecuteReader();
                while (registros2.Read())
                {
                    totalcredito = registros2["totalcobrosCred"].ToString();
                }
                registros2.Close();
                conexion.Close();
                imprimirCredito();

                conexion.Open();
                string sql3 = "select isnull(count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosDeb from COBROVENTA as cob, DETALLEDEMEDIO as det, VENTA as vent, DETALLEVENTA as detv, PRODUCTOELAB as prod where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '3' and vent.VENTA_ID = cob.VENTA_ID and COBRO_VENTA_FECHA >= @fechadesde3 and COBRO_VENTA_FECHA <= @fechahasta3 and vent.VENTA_ID = detv.VENTA_ID and detv.PROD_ELAB_ID = prod.PROD_ELAB_ID and prod.RUBRO_ID = @rubro3";
                SqlCommand comando3 = new SqlCommand(sql3, conexion);
                comando3.Parameters.Add("@fechadesde3", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando3.Parameters.Add("@fechahasta3", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando3.Parameters.Add("@rubro3", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros3 = comando3.ExecuteReader();
                while (registros3.Read())
                {
                    totaldebito = registros3["totalcobrosDeb"].ToString();
                }
                registros3.Close();
                conexion.Close();
                imprimirDebito();

                conexion.Open();
                string sql4 = "select isnull(count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosTrans from COBROVENTA as cob, DETALLEDEMEDIO as det, VENTA as vent, DETALLEVENTA as detv, PRODUCTOELAB as prod where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '4' and vent.VENTA_ID = cob.VENTA_ID and COBRO_VENTA_FECHA >= @fechadesde4 and COBRO_VENTA_FECHA <= @fechahasta4 and vent.VENTA_ID = detv.VENTA_ID and detv.PROD_ELAB_ID = prod.PROD_ELAB_ID and prod.RUBRO_ID = @rubro4";
                SqlCommand comando4 = new SqlCommand(sql4, conexion);
                comando4.Parameters.Add("@fechadesde4", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando4.Parameters.Add("@fechahasta4", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando4.Parameters.Add("@rubro4", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros4 = comando4.ExecuteReader();
                while (registros4.Read())
                {
                    totaltransferencia = registros4["totalcobrosTrans"].ToString();
                }
                registros4.Close();
                conexion.Close();
                imprimirTransferencia();
                calcularPromedio();
            }
            else if (comboBox6.Text == "Producto de Reventa" && comboBox1.SelectedIndex != -1)
            {
                //Filtrado por rango de fecha, producto reventa y rubro
                button4.Enabled = true;
                button2.Enabled = false;
                comboBox1.Enabled = false;
                comboBox6.Enabled = false;
                conexion.Open();
                string sql = "select coalesce(sum(COBRO_VENTA_MONTO), 0) as montototalcobrado, COUNT(DISTINCT COBRO_VENTA_ID) as cantidadcobros from COBROVENTA as cob, VENTA as ven, DETALLEVENTA as det, PRODUCTOREVENTA as prod where COBRO_VENTA_FECHA >= @fechadesde and COBRO_VENTA_FECHA <= @fechahasta and cob.VENTA_ID=ven.VENTA_ID and cob.VENTA_ID=det.VENTA_ID and det.PROD_REV_ID=prod.PROD_REV_ID and prod.RUBRO_ID=@rubro";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                while (registros.Read())
                {
                    montototal = registros["montototalcobrado"].ToString();
                    cantidadcobros = registros["cantidadcobros"].ToString();
                }
                registros.Close();
                conexion.Close();
                imprimirTotalesCobros();

                conexion.Open();
                string sql1 = "select isnull(count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosEf from COBROVENTA as cob, DETALLEDEMEDIO as det, VENTA as vent, DETALLEVENTA as detv, PRODUCTOREVENTA as prod where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '1' and vent.VENTA_ID = cob.VENTA_ID and COBRO_VENTA_FECHA >= @fechadesde1 and COBRO_VENTA_FECHA <= @fechahasta1 and vent.VENTA_ID = detv.VENTA_ID and detv.PROD_REV_ID = prod.PROD_REV_ID and prod.RUBRO_ID = @rubro1";
                SqlCommand comando1 = new SqlCommand(sql1, conexion);
                comando1.Parameters.Add("@fechadesde1", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando1.Parameters.Add("@fechahasta1", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando1.Parameters.Add("@rubro1", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros1 = comando1.ExecuteReader();
                while (registros1.Read())
                {
                    totalefectivo = registros1["totalcobrosEf"].ToString();
                }
                registros1.Close();
                conexion.Close();
                imprimirEfectivo();


                conexion.Open();
                string sql2 = "select isnull(count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosCred from COBROVENTA as cob, DETALLEDEMEDIO as det, VENTA as vent, DETALLEVENTA as detv, PRODUCTOREVENTA as prod where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '2' and vent.VENTA_ID = cob.VENTA_ID and COBRO_VENTA_FECHA >= @fechadesde2 and COBRO_VENTA_FECHA <= @fechahasta2 and vent.VENTA_ID = detv.VENTA_ID and detv.PROD_REV_ID = prod.PROD_REV_ID and prod.RUBRO_ID = @rubro2";
                SqlCommand comando2 = new SqlCommand(sql2, conexion);
                comando2.Parameters.Add("@fechadesde2", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando2.Parameters.Add("@fechahasta2", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando2.Parameters.Add("@rubro2", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros2 = comando2.ExecuteReader();
                while (registros2.Read())
                {
                    totalcredito = registros2["totalcobrosCred"].ToString();
                }
                registros2.Close();
                conexion.Close();
                imprimirCredito();

                conexion.Open();
                string sql3 = "select isnull(count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosDeb from COBROVENTA as cob, DETALLEDEMEDIO as det, VENTA as vent, DETALLEVENTA as detv, PRODUCTOREVENTA as prod where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '3' and vent.VENTA_ID = cob.VENTA_ID and COBRO_VENTA_FECHA >= @fechadesde3 and COBRO_VENTA_FECHA <= @fechahasta3 and vent.VENTA_ID = detv.VENTA_ID and detv.PROD_REV_ID = prod.PROD_REV_ID and prod.RUBRO_ID = @rubro3";
                SqlCommand comando3 = new SqlCommand(sql3, conexion);
                comando3.Parameters.Add("@fechadesde3", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando3.Parameters.Add("@fechahasta3", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando3.Parameters.Add("@rubro3", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros3 = comando3.ExecuteReader();
                while (registros3.Read())
                {
                    totaldebito = registros3["totalcobrosDeb"].ToString();
                }
                registros3.Close();
                conexion.Close();
                imprimirDebito();

                conexion.Open();
                string sql4 = "select isnull(count(DISTINCT cob.COBRO_VENTA_ID),0) as totalcobrosTrans from COBROVENTA as cob, DETALLEDEMEDIO as det, VENTA as vent, DETALLEVENTA as detv, PRODUCTOREVENTA as prod where cob.COBRO_VENTA_ID = det.COBRO_VENTA_ID and det.MEDIO_TR_ID = '4' and vent.VENTA_ID = cob.VENTA_ID and COBRO_VENTA_FECHA >= @fechadesde4 and COBRO_VENTA_FECHA <= @fechahasta4 and vent.VENTA_ID = detv.VENTA_ID and detv.PROD_REV_ID = prod.PROD_REV_ID and prod.RUBRO_ID = @rubro4";
                SqlCommand comando4 = new SqlCommand(sql4, conexion);
                comando4.Parameters.Add("@fechadesde4", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando4.Parameters.Add("@fechahasta4", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                comando4.Parameters.Add("@rubro4", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros4 = comando4.ExecuteReader();
                while (registros4.Read())
                {
                    totaltransferencia = registros4["totalcobrosTrans"].ToString();
                }
                registros4.Close();
                conexion.Close();
                imprimirTransferencia();
                calcularPromedio();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox6.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un tipo de producto";
                m.ShowDialog();
            }
            else if (comboBox6.SelectedIndex != -1 && comboBox1.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un rubro";
                m.ShowDialog();
            }
        }
    }
}
