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

namespace GestionDeUsuarios.Producción
{
    public partial class AnalisisElab : Form
    {
        private SqlConnection conexion = new SqlConnection("Data Source=SAM;Initial Catalog=bdSS;Integrated Security=True");
        string pablo="p",andres="a",juan="j";


        public AnalisisElab()
        {
            InitializeComponent();
        }

        private void AnalisisElab_Load(object sender, EventArgs e)
        {
            Elaboracion elab = new Elaboracion(pablo,andres,juan);
            elab.ActualizarEstados2Intento();
            cargarcomboBox1();
            cargarComboBox4();
            cargarComboBox5();
            comboBox1.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox2.DataSource = null;
            comboBox2.Items.Clear();
            comboBox3.DataSource = null;
            comboBox3.Items.Clear();
            button4.Enabled = false;
            dateTimePicker1.Text = DateTime.Now.Date.ToString();

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


            private void cargarComboBox5()
            {
            conexion.Open();
            string sql = "select EST_PROD_ID, EST_PROD_NOMBRE from ESTADOPROD";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox5.DisplayMember = "EST_PROD_NOMBRE";
            comboBox5.ValueMember = "EST_PROD_ID";
            comboBox5.DataSource = tabla1;
            }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarcomboBox2();
            cargarcomboBox3();
            comboBox3.SelectedIndex = -1;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarcomboBox3();
            comboBox3.SelectedIndex = -1;
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (dateTimePicker1.Value > dateTimePicker2.Value)
            {
                Aviso m = new Aviso();
                m.label1.Text = "La primera fecha y hora deben ser anteriores a la segunda fecha y hora ingresada";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un dato de las listas desplegables";
                m.ShowDialog();
            }
            else if (comboBox5.SelectedIndex != -1 && comboBox1.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar otro dato acompañando al estado de las listas desplegables";
                m.ShowDialog();

            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede realizar esa búsqueda con ese filtro";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un cocinero para poder generar la estadística";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex != -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe seleccionar un cocinero para poder generar la estadística";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1) // RUBRO Y MARCA
            {
                button2.Enabled = false;
                CantidadElaboracionesRealizadasRubroYMarca();
                CantidadUnidadesElaboradasRubroYMarca();
                CantidadEstadosProgramadasRubroYMarca();
                CantidadEstadosEnElaboracionRubroYMarca();
                CantidadEstadosIncompletasRubroYMarca();
                CantidadEstadosCompletasRubroYMarca();
                CantidadEstadosSuperadasRubroYMarca();
                CantidadUnidadesMaxObjetivoRubroYMarca();
                CantidadUnidadesMaxObjetivoRubroYMarca();
                CantidadUnidadesMinObjetivoRubroYMarca();
                CantidadUnidadesPromObjetivoRubroYMarca();
                CantidadUnidadesPromElaboradaRubroYMarca();
                CantidadUnidadesMaxElaboradasRubroYMarca();
                CantidadUnidadesMinElaboradaRubroYMarca();
                CantidadUnidadesPromElaboradaRubroYMarca();
                fechaMinCantidadMaxElaboradaRubroYMarca();
                fechaMinCantidadMinElaboradaRubroYMarca();
                button4.Enabled = true;
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex == -1 && comboBox5.SelectedIndex == -1)
            {
                button2.Enabled = false;
                CantidadElaboracionesRealizadasProducto();
                CantidadUnidadesElaboradasProducto();
                CantidadEstadosProgramadasPorProducto();
                CantidadEstadosEnElaboracionPorProducto();
                CantidadEstadosIncompletasPorProducto();
                CantidadEstadosCompletasPorProducto();
                CantidadEstadosSuperadasPorProducto();
                CantidadUnidadesMaxObjetivoPorProducto();
                CantidadUnidadesMaxObjetivoPorProducto();
                CantidadUnidadesMinObjetivoPorProducto();
                CantidadUnidadesPromObjetivoPorProducto();
                CantidadUnidadesPromElaboradaPorProducto();
                CantidadUnidadesMaxElaboradasPorProducto();
                CantidadUnidadesMinElaboradaPorProducto();
                CantidadUnidadesPromElaboradaPorProducto();
                fechaMinCantidadMaxElaborada();
                fechaMinCantidadMinElaborada();
                button4.Enabled = true;
            }
            else if (comboBox4.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox5.SelectedIndex == -1)
            {
                button2.Enabled = false;
                CantidadElaboracionesRealizadasCocinero();
                CantidadUnidadesElaboradasCocinero();
                CantidadEstadosProgramadasPorCocinero();
                CantidadEstadosEnElaboracionPorCocinero();
                CantidadEstadosIncompletasPorCocinero();
                CantidadEstadosCompletasPorCocinero();
                CantidadEstadosSuperadasPorCocinero();
                CantidadUnidadesMaxObjetivoPorCocinero();
                CantidadUnidadesMaxObjetivoPorCocinero();
                CantidadUnidadesMinObjetivoPorCocinero();
                CantidadUnidadesPromObjetivoPorCocinero();
                CantidadUnidadesPromElaboradaPorCocinero();
                CantidadUnidadesMaxElaboradasPorCocinero();
                CantidadUnidadesMinElaboradaPorCocinero();
                CantidadUnidadesPromElaboradaPorCocinero();
                fechaMinCantidadMaxElaboradaCocinero();
                fechaMinCantidadMinElaboradaCocinero();
                button4.Enabled = true;
            }
            else if (comboBox4.SelectedIndex != -1 && comboBox3.SelectedIndex == -1 && comboBox5.SelectedIndex != -1)
            {
                button2.Enabled = false;
                CantidadElaboracionesRealizadasCocineroYEstado();
                CantidadUnidadesElaboradasCocineroYEstado();
                CantidadEstadosProgramadasPorCocineroYEstado();
                CantidadEstadosEnElaboracionPorCocineroYEstado();
                CantidadEstadosIncompletasPorCocineroYEstado();
                CantidadEstadosCompletasPorCocineroYEstado();
                CantidadEstadosSuperadasPorCocineroYEstado();
                CantidadUnidadesMaxObjetivoPorCocineroYEstado();
                CantidadUnidadesMaxObjetivoPorCocineroYEstado();
                CantidadUnidadesMinObjetivoPorCocineroYEstado();
                CantidadUnidadesPromObjetivoPorCocineroYEstado();
                CantidadUnidadesPromElaboradaPorCocineroYEstado();
                CantidadUnidadesMaxElaboradasPorCocineroYEstado();
                CantidadUnidadesMinElaboradaPorCocineroYEstado();
                CantidadUnidadesPromElaboradaPorCocineroYEstado();
                fechaMinCantidadMaxElaboradaCocineroYEstado();
                fechaMinCantidadMinElaboradaCocineroYEstado();
                button4.Enabled = true;
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex == -1)
            {
                button2.Enabled = false;
                CantidadElaboracionesRealizadasCocineroYProd();
                CantidadUnidadesElaboradasCocineroYProd();
                CantidadEstadosProgramadasPorCocineroYProd();
                CantidadEstadosEnElaboracionPorCocineroYProd();
                CantidadEstadosIncompletasPorCocineroYProd();
                CantidadEstadosCompletasPorCocineroYProd();
                CantidadEstadosSuperadasPorCocineroYProd();
                CantidadUnidadesMaxObjetivoPorCocineroYProd();
                CantidadUnidadesMaxObjetivoPorCocineroYProd();
                CantidadUnidadesMinObjetivoPorCocineroYProd();
                CantidadUnidadesPromObjetivoPorCocineroYProd();
                CantidadUnidadesPromElaboradaPorCocineroYProd();
                CantidadUnidadesMaxElaboradasPorCocineroYProd();
                CantidadUnidadesMinElaboradaPorCocineroYProd();
                CantidadUnidadesPromElaboradaPorCocineroYProd();
                fechaMinCantidadMaxElaboradaCocineroYProd();
                fechaMinCantidadMinElaboradaCocineroYProd();
                button4.Enabled = true;
            }
            else if (comboBox1.SelectedIndex != -1 && comboBox2.SelectedIndex != -1 && comboBox3.SelectedIndex != -1 && comboBox4.SelectedIndex != -1 && comboBox5.SelectedIndex != -1)
            {
                button2.Enabled = false;
                CantidadElaboracionesConEstado();
                CantidadUnidadesElaboradasConEstado();
                CantidadUnidadesMinElaboradaConEstado();
                CantidadEstadosProgramadasConEstado();
                CantidadEstadosEnElaboracionConEstado();
                CantidadEstadosIncompletasConEstado();
                CantidadEstadosCompletasConEstado();
                CantidadEstadosSuperadasConEstado();
                CantidadUnidadesMaxObjetivoConEstado();
                CantidadUnidadesMaxObjetivoConEstado();
                CantidadUnidadesMinObjetivoConEstado();
                CantidadUnidadesPromObjetivoConEstado();
                CantidadUnidadesPromElaboradaConEstado();
                CantidadUnidadesMaxElaboradasConEstado();
                CantidadUnidadesMinElaboradaConEstado();
                CantidadUnidadesPromElaboradaConEstado();
                fechaMinCantidadMaxElaboradaConEstado();
                fechaMinCantidadMinElaboradaConEstado();
                button4.Enabled = true;
            }

        }



        // METODOS PARA LLAMAR EN LAS CONSULTAS


        // ------- ---------------- ------- ---------------- POR RUBRO Y MARCA  ------- ----------------------- ----------------------- ----------------------- ----------------

        private void CantidadElaboracionesRealizadasRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(PRODUCCION_ID) as 'Cantidad de producciones' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de producciones"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label6.Text = "Cantidad de elaboraciones realizadas:";
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesElaboradasRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select SUM(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosProgramadasRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad Programadas' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where EST_PROD_ID='1' and RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad Programadas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosEnElaboracionRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where EST_PROD_ID='2' and RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosIncompletasRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where EST_PROD_ID='3' and RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosCompletasRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where EST_PROD_ID='4' and RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label37.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label37.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosSuperadasRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where EST_PROD_ID='5' and RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label38.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label38.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxObjetivoRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label39.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label39.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesMinObjetivoRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label40.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label40.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromObjetivoRubroYMarca()
        {
            float cantidad = 0;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label41.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label41.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxElaboradasRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMinElaboradaRubroYMarca()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromElaboradaRubroYMarca()
        {
            float cantidad;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinCantidadMaxElaboradaRubroYMarca()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MAX(PRODUCCION_CANT_ELAB) from PRODUCCION where RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }


        private void fechaMinCantidadMinElaboradaRubroYMarca()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION as pro JOIN PRODUCTOELAB as prelab on prelab.PROD_ELAB_ID = pro.PROD_ELAB_ID where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MIN(PRODUCCION_CANT_ELAB) from PRODUCCION where RUBRO_ID=@rubro and MARCA_ID=@marca and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox1.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }








        // ------- ---------------- ------- ---------------- POR PRODUCTO ------- ----------------------- ----------------------- ----------------------- ----------------


        private void CantidadElaboracionesRealizadasProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(PRODUCCION_ID) as 'Cantidad de producciones' from PRODUCCION as pro where PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de producciones"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesElaboradasProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select SUM(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosProgramadasPorProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad Programadas' from PRODUCCION as pro where EST_PROD_ID='1' and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad Programadas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosEnElaboracionPorProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='2' and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosIncompletasPorProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='3' and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosCompletasPorProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='4' and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label37.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label37.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosSuperadasPorProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='5' and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label38.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label38.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxObjetivoPorProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label39.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label39.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesMinObjetivoPorProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label40.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label40.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromObjetivoPorProducto()
        {
            float cantidad=0;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label41.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label41.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxElaboradasPorProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMinElaboradaPorProducto()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromElaboradaPorProducto()
        {
            float cantidad;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinCantidadMaxElaborada()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MAX(PRODUCCION_CANT_ELAB) from PRODUCCION where PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }


        private void fechaMinCantidadMinElaborada()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MIN(PRODUCCION_CANT_ELAB) from PRODUCCION where PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }





        //------ ----------------------- ----------------------- POR COCINERO ------- ----------------------- ----------------------- ----------------------- ----------------

        private void CantidadElaboracionesRealizadasCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(PRODUCCION_ID) as 'Cantidad de producciones' from PRODUCCION as pro where USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de producciones"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesElaboradasCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select SUM(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadEstadosProgramadasPorCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad Programadas' from PRODUCCION as pro where EST_PROD_ID='1' and USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad Programadas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosEnElaboracionPorCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='2' and USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosIncompletasPorCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='3' and USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosCompletasPorCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='4' and USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label37.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label37.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosSuperadasPorCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='5' and USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label38.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label38.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxObjetivoPorCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label39.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label39.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesMinObjetivoPorCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label40.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label40.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromObjetivoPorCocinero()
        {
            float cantidad = 0;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label41.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label41.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxElaboradasPorCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMinElaboradaPorCocinero()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromElaboradaPorCocinero()
        {
            float cantidad;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinCantidadMaxElaboradaCocinero()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MAX(PRODUCCION_CANT_ELAB) from PRODUCCION where USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }


        private void fechaMinCantidadMinElaboradaCocinero()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MIN(PRODUCCION_CANT_ELAB) from PRODUCCION where USUARIO_ID=@cocinero and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }



        //------- ----------------------- ---------------- COCINERO Y ESTADO  ------- ----------------------- ----------------------- ----------------


        private void CantidadElaboracionesRealizadasCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(PRODUCCION_ID) as 'Cantidad de producciones' from PRODUCCION as pro where USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de producciones"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesElaboradasCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select SUM(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadEstadosProgramadasPorCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad Programadas' from PRODUCCION as pro where EST_PROD_ID='1' and USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad Programadas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosEnElaboracionPorCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='2' and USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosIncompletasPorCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='3' and USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosCompletasPorCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='4' and USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label37.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label37.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosSuperadasPorCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='5' and USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label38.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label38.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxObjetivoPorCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label39.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label39.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesMinObjetivoPorCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label40.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label40.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromObjetivoPorCocineroYEstado()
        {
            float cantidad = 0;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label41.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label41.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxElaboradasPorCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMinElaboradaPorCocineroYEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromElaboradaPorCocineroYEstado()
        {
            float cantidad;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinCantidadMaxElaboradaCocineroYEstado()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MAX(PRODUCCION_CANT_ELAB) from PRODUCCION where USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }


        private void fechaMinCantidadMinElaboradaCocineroYEstado()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MIN(PRODUCCION_CANT_ELAB) from PRODUCCION where USUARIO_ID=@cocinero and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }







        //------- ----------------------- ---------------- COCINERO Y PRODUCTO ------- ----------------------- ----------------------- ----------------

        private void CantidadElaboracionesRealizadasCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(PRODUCCION_ID) as 'Cantidad de producciones' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de producciones"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesElaboradasCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select SUM(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadEstadosProgramadasPorCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad Programadas' from PRODUCCION as pro where EST_PROD_ID='1' and USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad Programadas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosEnElaboracionPorCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='2' and USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosIncompletasPorCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='3' and USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosCompletasPorCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='4' and USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label37.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label37.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosSuperadasPorCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='5' and USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label38.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label38.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxObjetivoPorCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label39.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label39.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesMinObjetivoPorCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label40.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label40.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromObjetivoPorCocineroYProd()
        {
            float cantidad = 0;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label41.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label41.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxElaboradasPorCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMinElaboradaPorCocineroYProd()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromElaboradaPorCocineroYProd()
        {
            float cantidad;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinCantidadMaxElaboradaCocineroYProd()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MAX(PRODUCCION_CANT_ELAB) from PRODUCCION where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }


        private void fechaMinCantidadMinElaboradaCocineroYProd()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MIN(PRODUCCION_CANT_ELAB) from PRODUCCION where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }



        // ------- -----------------------      TODAS LAS COMBINACIONES INCLUYENDO ESTADO ------- ----------------------- ---------------- ------- ----------------

        private void CantidadElaboracionesConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(PRODUCCION_ID) as 'Cantidad de producciones' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de producciones"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label32.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label32.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesElaboradasConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select SUM(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label33.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label33.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadEstadosProgramadasConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad Programadas' from PRODUCCION as pro where EST_PROD_ID='1' and USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad Programadas"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label34.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label34.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosEnElaboracionConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='2' and USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label35.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label35.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosIncompletasConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='3' and USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label36.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label36.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosCompletasConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='4' and USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label37.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label37.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadEstadosSuperadasConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select COUNT(EST_PROD_ID) as 'Cantidad' from PRODUCCION as pro where EST_PROD_ID='5' and USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label38.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label38.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxObjetivoConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label39.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label39.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesMinObjetivoConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label40.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label40.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromObjetivoConEstado()
        {
            float cantidad = 0;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_OBJ) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label41.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label41.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMaxElaboradasConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MAX(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label42.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label42.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void CantidadUnidadesMinElaboradaConEstado()
        {
            int cantidad;
            conexion.Open();
            string sql = "select MIN(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label43.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label43.Text = cantidad.ToString();
            }
            conexion.Close();
        }

        private void CantidadUnidadesPromElaboradaConEstado()
        {
            float cantidad;
            conexion.Open();
            string sql = "select AVG(PRODUCCION_CANT_ELAB) as 'Cantidad de Unidades' from PRODUCCION as pro where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            registros.Read();
            string cant = registros["Cantidad de Unidades"].ToString();
            if ((cant == null) || (cant.Equals(String.Empty)))
            {
                label44.Text = "---";
            }
            else
            {
                cantidad = int.Parse(cant);
                conexion.Close();
                label44.Text = cantidad.ToString();
            }
            conexion.Close();
        }


        private void fechaMinCantidadMaxElaboradaConEstado()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MAX(PRODUCCION_CANT_ELAB) from PRODUCCION where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label45.Text = fecha;
            }
            conexion.Close();
        }


        private void fechaMinCantidadMinElaboradaConEstado()
        {
            conexion.Open();
            string sql = "select PRODUCCION_FECHA_OBJ from PRODUCCION where PRODUCCION_FECHA_OBJ between @fechadesde and @fechahasta and PRODUCCION_CANT_ELAB= (select MIN(PRODUCCION_CANT_ELAB) from PRODUCCION where USUARIO_ID=@cocinero and PROD_ELAB_ID=@productoelaborado and EST_PROD_ID=@estado and PRODUCCION_FECHA_OBJ>=@fechadesde and PRODUCCION_FECHA_OBJ<=@fechahasta) group by PRODUCCION_FECHA_OBJ";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@cocinero", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
            comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
            comando.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@estado", SqlDbType.Int).Value = comboBox5.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                String fecha = Convert.ToDateTime(registros["PRODUCCION_FECHA_OBJ"]).ToString("dd/MM/yyyy");
                label46.Text = fecha;
            }
            conexion.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void pictureBox20_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "La búsqueda se puede filtrar por:\n" +
                "Rango de fecha, rubro y marca.\n" +
                "Rango de fecha, rubro, marca y producto.\n" +
                "Rango de fecha y cocinero.\n" +
                "Rango de fecha, cocinero y estado.\n" +
                "Rango de fecha, cocinero, rubro, marca y producto. \n" +
                "Rango de fecha, cocinero, rubro, marca, producto y estado.";
            m.ShowDialog();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            button2.Enabled = true;
            button4.Enabled = false;
            chart1.Titles.Clear();
            chart1.Series.Clear();
            chart2.Series.Clear();
            chart2.Titles.Clear();
            comboBox1.SelectedIndex = (-1);
            comboBox2.SelectedIndex = (-1);
            comboBox3.SelectedIndex = (-1);
            comboBox4.SelectedIndex = (-1);
            comboBox5.SelectedIndex = (-1);
            comboBox2.DataSource = null;
            comboBox2.Items.Clear();
            comboBox3.DataSource = null;
            comboBox3.Items.Clear();
            dateTimePicker1.Text = DateTime.Now.Date.ToString();
            dateTimePicker2.Text = DateTime.Now.ToString();
            label6.Text = "Cantidad de elaboraciones realizadas:";
            label7.Text = "Cantidad de unidades elaboradas:";
            label13.Text = "Estado \"Programada\":";
            label12.Text = "Estado \"En Elaboración\":";
            label14.Text = "Estado \"Incompleta\":";
            label31.Text = "Estado \"Completa\":";
            label30.Text = "Estado \"Superada\":";
            label9.Text = "Cantidad máxima objetivo:";
            label10.Text = "Cantidad mínima objetivo:";
            label11.Text = "Cantidad promedio objetivo:";
            label25.Text = "Cantidad máxima elaborada:";
            label24.Text = "Cantidad mínima elaborada:";
            label27.Text = "Cantidad promedio elaborada:";
            label15.Text = "Fecha mas reciente de cantidad máxima elaborada:";
            label16.Text = "Fecha mas reciente de cantidad mínima elaborada:";
            label32.Text = "";
            label33.Text = "";
            label34.Text = "";
            label35.Text = "";
            label36.Text = "";
            label37.Text = "";
            label38.Text = "";
            label39.Text = "";
            label40.Text = "";
            label41.Text = "";
            label42.Text = "";
            label43.Text = "";
            label44.Text = "";
            label45.Text = "";
            label46.Text = "";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (label32.Text == "---" || label32.Text=="0")

            {
                Aviso m = new Aviso();
                m.label1.Text = "Para dibujar el gráfico deben tener valores todos los conceptos";
                m.ShowDialog();
            }
            else
            {
                button4.Enabled = false;

                string[] series2 = { "Cant Max Obj", "Cant Min Obj", "Cant Prom Obj", "Cant Max Elab", "Cant Min Elab", "Cant Prom Elab" };
                int[] puntos1 = { int.Parse(label39.Text), int.Parse(label40.Text), (int)float.Parse(label41.Text), int.Parse(label42.Text), int.Parse(label43.Text), int.Parse(label44.Text) };
                chart2.Palette = ChartColorPalette.Pastel;

                chart2.Titles.Add("Cantidades");
                for (int i = 0; i < series2.Length -1; i++)
                {
                    Series serie = chart2.Series.Add(series2[i]);
                    serie.Label = puntos1[i].ToString();
                    serie.Points.Add(puntos1[i]);

                }


                // GRAFICO 1
                string[] series = { "Programada", "En Elaboración", "Incompleta", "Completa", "Superada" };
                int[] puntos = { int.Parse(label34.Text), int.Parse(label35.Text), (int)float.Parse(label36.Text), int.Parse(label37.Text), int.Parse(label38.Text) };

                chart1.Titles.Add("Estados");
                for (int i = 0; i < series.Length; i++)
                {
                    Series serie = chart1.Series.Add(series[i]);
                    serie.Label = puntos[i].ToString();
                    serie.Points.Add(puntos[i]);
                }

            }

        }

    }
}
