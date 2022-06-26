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

namespace GestionDeUsuarios.Ventas
{
    public partial class Venta : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        int aclientes=0;
        string numeroCelular;
        public Venta(string tipoActivo2, string usuIdActivo, string numeroCelu)
        {
            InitializeComponent();
            tipoActivo = tipoActivo2;
            IdUsu = usuIdActivo;
            numeroCelular = numeroCelu;
        }

        private Form formularioActivo = null;
        private void AbrirpanelHijo(Form formularioHijo)
        {
            if (formularioActivo != null)
                formularioActivo.Close();
            formularioActivo = formularioHijo;
            formularioHijo.TopLevel = false;
            formularioHijo.FormBorderStyle = FormBorderStyle.None;
            formularioHijo.Dock = DockStyle.Fill;
            panel1.Controls.Add(formularioHijo);
            panel1.Tag = formularioHijo;
            formularioHijo.BringToFront(); // ESTO POR SI PONEMOS UN LOGO DE FONDO ADELANTE VA EL FORMULARIO
            formularioHijo.Show();
        }

        private void Venta_Load(object sender, EventArgs e)
        {
            cargarEstados();
            textBox1.Enabled = false;
            textBox2.Visible = false; 
            label8.Visible = false;
            textBox3.Visible = false;
            label13.Visible = false;
            radioButton1.Checked = true;
            radioButton4.Checked = true;
            radioButton7.Checked = true;
            numericUpDown2.Enabled = false;
            textBox4.Enabled = false;
            checkBox1.Enabled = false;
            button7.Enabled = false;
            button3.Enabled = false;
            button4.Enabled = false;
            checkBox2.Enabled = false; //WHATSAPP AGRADECER
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
            impedirEnviarWhatsapp(); //WHATSAPP
            cargarcomboBox2();
            cargarcomboBox3();
            label20.Visible = false;
            label21.Visible = false;
            textBox5.Visible = false;
            label21.Text =numeroCelular;
            if (radioButton1.Checked)
            {
                cargarComboBox1Anonimo();
                comboBox1.SelectedIndex = -1;
            }
            ocultarFechasParaConsulta();
        }        

        private void impedirEnviarWhatsapp() //WHATSAPP
        {
            button11.Enabled = false;
            button11.Visible = false;
            pictureBox12.Visible = false;
        }

        private void permitirEnviarWhatsapp() //WHATSAPP
        {
            button11.Enabled = true;
            button11.Visible = true;
            pictureBox12.Visible = true;
        }

        private void cargarEstados()
        {
            conexion.Open();
            string sql = "select COUNT(*) EST_VENTA_NOMBRE from ESTADOVENTA where EST_VENTA_NOMBRE='Sin cobros'";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                string cant = registro["EST_VENTA_NOMBRE"].ToString();
                int cantidadEstado = int.Parse(cant.ToString());

                registro.Close();
                conexion.Close();
                if (cantidadEstado == 0)
                {
                    conexion.Open();
                    string sql1 = "INSERT INTO ESTADOVENTA(EST_VENTA_NOMBRE) VALUES ('Sin cobros')";
                    string sql2 = "INSERT INTO ESTADOVENTA(EST_VENTA_NOMBRE) VALUES ('Cobrada parcialmente')";
                    string sql3 = "INSERT INTO ESTADOVENTA(EST_VENTA_NOMBRE) VALUES ('Cobrada totalmente')";
                    SqlCommand comando1 = new SqlCommand(sql1, conexion);
                    SqlCommand comando2 = new SqlCommand(sql2, conexion);
                    SqlCommand comando3 = new SqlCommand(sql3, conexion);
                    comando1.ExecuteNonQuery();
                    comando2.ExecuteNonQuery();
                    comando3.ExecuteNonQuery();
                    conexion.Close();
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new Clientes(tipoActivo, IdUsu, aclientes));
        }

        private void radioButton1_Click(object sender, EventArgs e) //CONSUMIDOR FINAL
        {
            cargarComboBox1Anonimo();
            textBox1.Text = "";
            button7.Enabled = false;
            comboBox1.SelectedValue = -1;
            checkBox2.Enabled = false; //WHATSAPP AGRADECER
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
            impedirEnviarWhatsapp(); //WHATSAPP
        }

        private void radioButton2_Click(object sender, EventArgs e) //CLIENTE REGISTRADO
        {            
                cargarComboBox1Cliente();
                button7.Enabled = true;
                checkBox2.Enabled = false; //WHATSAPP AGRADECER
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                impedirEnviarWhatsapp(); //WHATSAPP
        }


        public void cargarComboBox1Cliente()
        {
            if (textBox1.Text != "")
            {
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
                conexion.Open();
                string sql = "select TOP 10 VENTA_FECHA, VENTA_ID from VENTA where CLIENTE_ID=@cliente ORDER BY VENTA_FECHA DESC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@cliente", SqlDbType.Int).Value = int.Parse(textBox3.Text);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox1.DataSource = tabla1;
                comboBox1.DisplayMember = "VENTA_FECHA";
                comboBox1.ValueMember = "VENTA_ID";
                comboBox1.SelectedValue = -1;
            }
            else if (textBox1.Text=="")
            {
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
            }
        }


        public void cargarComboBox1Anonimo()
        {            
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
                conexion.Open();
                string sql = "select TOP 10 VENTA_FECHA, VENTA_ID from VENTA where CLIENTE_ID IS NULL ORDER BY VENTA_FECHA DESC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox1.DataSource = tabla1;
                comboBox1.DisplayMember = "VENTA_FECHA";
                comboBox1.ValueMember = "VENTA_ID";
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


        private void calcularPrecioPRREV ()
        {
            textBox2.Text = "";
            conexion.Open();
            string sql = "select PROD_REV_PR_UNIT from PRODUCTOREVENTA where RUBRO_ID=@rubroid and MARCA_ID=@marcaid and PROD_REV_ID=@productoid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@productoid", SqlDbType.Int).Value = comboBox4.SelectedValue;
            SqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
                textBox2.Text = registro["PROD_REV_PR_UNIT"].ToString();
            registro.Close();
            conexion.Close();
        }


        private void calcularPrecioPRELAB ()
        {
            textBox2.Text = "";
            conexion.Open();
            string sql = "select PROD_ELAB_PR_UNIT from PRODUCTOELAB where RUBRO_ID=@rubroid and MARCA_ID=@marcaid and PROD_ELAB_ID=@productoid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
            comando.Parameters.Add("@productoid", SqlDbType.Int).Value = comboBox4.SelectedValue;
            SqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
                textBox2.Text = registro["PROD_ELAB_PR_UNIT"].ToString();
            registro.Close();
            conexion.Close();
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarcomboBox4();
        }

        private void comboBox3_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarcomboBox4();
        }

        private void comboBox5_SelectedIndexChanged(object sender, EventArgs e)
        {
            cargarcomboBox2();
            cargarcomboBox3(); 
        }

        private void button8_Click(object sender, EventArgs e) //LIMPIAR DETALLE DE UNA FILA
        {
            if (dataGridView1.Rows.Count == 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "La lista de artículos ya está vacía";
                m.ShowDialog();
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("¿Está seguro que desea eliminar la filada seleccionada?", "Eliminar Fila", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    int selectedrowindex = dataGridView1.SelectedCells[0].RowIndex;
                    DataGridViewRow selectedRow = dataGridView1.Rows[selectedrowindex];
                    string cellValue = Convert.ToString(selectedRow.Cells["Producto"].Value);
                    listArt.Remove(cellValue);
                    if (dataGridView1.Rows.Count > 0)
                    {
                        dataGridView1.Rows.RemoveAt(dataGridView1.CurrentRow.Index);
                        calcularTotal();
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
        }



        List<string> listArt = new List<string>();
        private void button6_Click(object sender, EventArgs e) //AGREGAR A LA VENTA
        {
            int cantFilas = dataGridView1.Rows.Count;
            if (cantFilas == 0)
            {
                if ( numericUpDown1.Value == 0 && comboBox5.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se puede agregar a la factura si están todos los campos vacíos";
                    m.ShowDialog();
                }
                else if (comboBox5.SelectedIndex == -1)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Debe seleccionar un tipo de producto";
                    m.ShowDialog();
                }
                else if (comboBox2.SelectedIndex == -1)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Debe seleccionar un rubro";
                    m.ShowDialog();
                }
                else if (comboBox3.SelectedIndex == -1)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Debe seleccionar una marca";
                    m.ShowDialog();
                }
                else if (comboBox4.SelectedIndex == -1)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Debe cargar la descripción del producto";
                    m.ShowDialog();
                }
                else if (numericUpDown1.Value == 0)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Debe cargar alguna cantidad";
                    m.ShowDialog();
                }
                else if (comboBox5.Text == "Producto Elaborado")
                {
                    calcularPrecioPRELAB();
                    guardarDetalleDeVentaProductoElaborado();
                    calcularTotal();
                    listArt.Add(comboBox4.Text);
                    numericUpDown1.Value = 0;
                    if(radioButton2.Checked)
                    { 
                        checkBox2.Enabled = true; //WHATSAPP AGRADECER
                        checkBox3.Enabled = true;
                        checkBox4.Enabled = true;
                    }
                    ocultarTrasAgregar();
                }
                else if (comboBox5.Text == "Producto de Reventa")
                {
                    calcularPrecioPRREV();
                    guardarDetalleDeVentaProductoReventa();
                    calcularTotal();
                    listArt.Add(comboBox4.Text);
                    numericUpDown1.Value = 0;
                    if (radioButton2.Checked)
                    {
                        checkBox2.Enabled = true; //WHATSAPP AGRADECER
                        checkBox3.Enabled = true;
                        checkBox4.Enabled = true;
                    }
                    ocultarTrasAgregar();
                }
            }
            else
            {
                string nombreArt = comboBox4.Text;
                string artMarcaConcatenado = comboBox4.Text + comboBox3.Text;
                string verificar = "";
                bool presente = false;
                for (int i = 0; i < listArt.Count; i++)
                {
                    verificar = dataGridView1.Rows[i].Cells["Producto"].Value.ToString() + dataGridView1.Rows[i].Cells["Marca"].Value.ToString();
                    if (artMarcaConcatenado == verificar)
                    {
                        presente = true;
                    }
                }
                if (presente == true)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "El artículo seleccionado ya fue agregado, no puede repetirse";
                    m.ShowDialog();
                }
                else
                {
                    if (comboBox5.Text == "Producto Elaborado")
                    {
                        if (numericUpDown1.Value == 0)
                        {
                            Aviso m = new Aviso();
                            m.label1.Text = "Debe cargar alguna cantidad";
                            m.ShowDialog();
                        }
                        else
                        {
                            calcularPrecioPRELAB();
                            guardarDetalleDeVentaProductoElaborado();
                            calcularTotal();
                            listArt.Add(comboBox4.Text);
                            numericUpDown1.Value = 0;
                        }
                    }
                    else if (comboBox5.Text == "Producto de Reventa")
                    {
                        if (numericUpDown1.Value == 0)
                        {
                            Aviso m = new Aviso();
                            m.label1.Text = "Debe cargar alguna cantidad";
                            m.ShowDialog();
                        }
                        else
                        {
                            calcularPrecioPRREV();
                            guardarDetalleDeVentaProductoReventa();
                            calcularTotal();
                            listArt.Add(comboBox4.Text);
                            numericUpDown1.Value = 0;
                        }
                    }
                }
            }
        }

        private void ocultarTrasAgregar()
        {
            button5.Enabled = false;
            button10.Enabled = false;
            button5.Visible = false;
            button10.Visible = false;
            pictureBox11.Visible = false;
            label5.Visible = false;
            comboBox1.Visible = false;
            pictureBox1.Visible = false;
            groupBox3.Visible = false;
            pictureBox2.Visible = false;
            pictureBox5.Visible = false;
            button4.Visible = false;
            button3.Visible = false;
            label1.Visible = false;
            label17.Visible = false;
            pictureBox9.Visible = false;
            pictureBox10.Visible = false;
            label3.Visible = false;
            label16.Visible = false;
            if (radioButton1.Checked)
            {
                groupBox4.Visible = false;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;
            }
        }

        public void calcularTotal()
        {
            Decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value);
            }
                label15.Text = suma.ToString();
        }

        public void calcularSubTotal()
        {
            Decimal suma = 0;
            Decimal cantidadSub, preciounit, descuento, preciofinal;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
               cantidadSub = Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
               preciounit = Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value);
               descuento = Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
               preciofinal = preciounit - descuento;
               suma = preciofinal * cantidadSub;

                dataGridView1.Rows[i].Cells[6].Value = suma.ToString();
            }
        }

        decimal descuentoBD=0;
        public void calcularTotalConDescuentoPorcentaje()
        {
            if (radioButton5.Checked && numericUpDown2.Value == 0) // PORCENTAJE
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un valor de descuento";
                m.ShowDialog();
                checkBox1.Checked = false;
            }
            else
            {
                Decimal suma = 0;
                Decimal porcentaje = numericUpDown2.Value;
                Decimal descuento = 0;
                Decimal total = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value);
                }
                if (radioButton5.Checked && checkBox1.Checked)
                {
                    descuento = suma * porcentaje / 100;
                    total = suma - descuento;
                    descuentoBD = descuento;
                    label15.Text = total.ToString();
                }
            }
        }


        public void calcularTotalConDescuentoMonto()
        {
                Decimal suma = 0;
                Decimal monto = Convert.ToDecimal(textBox4.Text);
                Decimal total = 0;
                for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                {
                    suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value);
                }
                if (radioButton3.Checked && checkBox1.Checked)
                {
                    total = suma - monto;
                    descuentoBD = monto;
                    label15.Text = total.ToString();
                }
        }



        public void guardarDetalleDeVentaProductoElaborado()
        {
                conexion.Open();
                string sql = "select ISNULL(pr.PROD_ELAB_DTO,0) as totaldesc from PRODUCTOELAB as pr where RUBRO_ID=@rubro and MARCA_ID=@marca and PROD_ELAB_ID=@id";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@id", SqlDbType.Int).Value = comboBox4.SelectedValue;
                comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox3.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                while (registros.Read())
                {
                    Decimal subtotalfilaParcial, subtotalfila = 0;
                    Decimal preciounitariofila = 0;
                    Decimal cantidadDesc;
                    int cantidadfila = 0;
                    preciounitariofila = Convert.ToDecimal(textBox2.Text);
                    cantidadfila = (int)numericUpDown1.Value;
                    cantidadDesc = Convert.ToDecimal(registros["totaldesc"]);
                    subtotalfilaParcial = preciounitariofila - cantidadDesc;
                    subtotalfila = subtotalfilaParcial * cantidadfila;
                    dataGridView1.Rows.Add(comboBox5.Text, comboBox4.Text, comboBox3.Text, numericUpDown1.Value, textBox2.Text, registros["totaldesc"], subtotalfila);
                }
                registros.Close();
                conexion.Close();
        }

        public void guardarDetalleDeVentaProductoReventa()
        {
            conexion.Open();
            string sql = "select ISNULL(pr.PROD_REV_DTO,0) as totaldesc from PRODUCTOREVENTA as pr where RUBRO_ID=@rubro and MARCA_ID=@marca and PROD_REV_ID=@id";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@id", SqlDbType.Int).Value = comboBox4.SelectedValue;
            comando.Parameters.Add("@rubro", SqlDbType.Int).Value = comboBox2.SelectedValue;
            comando.Parameters.Add("@marca", SqlDbType.Int).Value = comboBox3.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                Decimal subtotalfilaParcial, subtotalfila = 0;
                Decimal preciounitariofila = 0;
                Decimal cantidadDesc;
                int cantidadfila = 0;
                preciounitariofila = Convert.ToDecimal(textBox2.Text);
                cantidadfila = (int)numericUpDown1.Value;
                cantidadDesc = Convert.ToDecimal(registros["totaldesc"]);
                subtotalfilaParcial = preciounitariofila - cantidadDesc;
                subtotalfila = subtotalfilaParcial * cantidadfila;
                dataGridView1.Rows.Add(comboBox5.Text, comboBox4.Text, comboBox3.Text, numericUpDown1.Value, textBox2.Text, registros["totaldesc"], subtotalfila);

            }
            registros.Close();
            conexion.Close();
        }

        private void button9_Click(object sender, EventArgs e) //LIMPIAR DETALLE COMPLETO
        {
            if (dataGridView1.Rows.Count != 0)
            {
                DialogResult dialogResult = MessageBox.Show("¿Está seguro que desea limpiar toda la lista?", "Eliminar todos las filas", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    dataGridView1.Rows.Clear();
                    calcularTotal();
                    listArt.Clear();
                }
                else if (dialogResult == DialogResult.No)
                {
                    return;
                }
            }
            else 
            {
                Aviso m = new Aviso();
                m.label1.Text = "La lista de artículos ya está vacía";
                m.ShowDialog();
            }
        }

        private void radioButton5_Click(object sender, EventArgs e) //SE APLICA PORCENTAJE DE DESCUENTO
        {
            int cantFilas = dataGridView1.Rows.Count;
            if (cantFilas == 0)
            {
                radioButton4.Checked = true;
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un artículo para aplicar descuento";
                m.ShowDialog();
            }
            else
            {
                checkBox1.Enabled = true;
                numericUpDown2.Enabled = true;
                textBox4.Enabled = false;
                textBox4.Text = "";
                calcularTotal();
                checkBox1.Checked = false;
            }
        }

        private void radioButton4_Click(object sender, EventArgs e) //NO SE APLICA DESCUENTO
        {
            button6.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            radioButton3.Enabled = true;
            radioButton5.Enabled = true;
            numericUpDown2.Enabled = true;
            textBox4.Enabled = true;
            checkBox1.Enabled = false;
            checkBox1.Checked = false;
            numericUpDown2.Value = 0;
            numericUpDown2.Enabled = false;
            textBox4.Enabled = false;
            textBox4.Text = "";
            calcularTotal();
        }

        private void radioButton3_Click(object sender, EventArgs e) //SE APLICA DESCUENTO POR MONTO ESPECÍFICO
        {
            int cantFilas = dataGridView1.Rows.Count;
            if (cantFilas == 0)
            {
                radioButton4.Checked = true;
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un artículo para aplicar descuento";
                m.ShowDialog();
            }
            else
            {
                checkBox1.Enabled = true;
                checkBox1.Checked = false;
                textBox4.Enabled = true;
                numericUpDown2.Enabled = false;
                numericUpDown2.Value = 0;
                calcularTotal();
                checkBox1.Checked = false;
            }
        }

        private void checkBox1_Click(object sender, EventArgs e) //IMPACTAR DESCUENTO
        {
            if (radioButton5.Checked) //PORCENTAJE
            {
                calcularTotalConDescuentoPorcentaje();
            }
            else if (radioButton3.Checked) //MONTO
            {
                if (radioButton3.Checked && (textBox4.Text == "0" || textBox4.Text == "")) //MONTO
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Debe cargar un valor de descuento";
                    m.ShowDialog();
                    checkBox1.Checked = false;
                }else{
                        float montoDescuento = float.Parse(textBox4.Text);
                        float montoTotal = float.Parse(label15.Text);
                        if (montoDescuento > montoTotal)
                        {
                            Aviso m = new Aviso();
                            m.label1.Text = "El descuento no puede ser mayor al total";
                            m.ShowDialog();
                            checkBox1.Checked = false;
                        }
                        else
                        {
                            calcularTotalConDescuentoMonto();
                        }
                    }
            }
            if (checkBox1.Checked==true)
            {                
                button6.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
                radioButton3.Enabled = false;
                radioButton5.Enabled = false;
                numericUpDown2.Enabled = false;
                textBox4.Enabled = false;
                checkBox1.Enabled = false;
            }            
        }

        private void button1_Click(object sender, EventArgs e) // GRABAR VENTA
        {
            listArt.Clear();
            if (radioButton2.Checked && textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Por favor cargue un nombre de cliente";
                m.ShowDialog();
            }
            else if (dataGridView1.Rows.Count == 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Por favor cargue artículos en la venta";
                m.ShowDialog();
            }
            else
            {
                guardarVenta();
                int ventaId = identificadorIdVenta();

                try
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {
                        String tipo = "";
                        tipo = (string)row.Cells[0].Value;
                        String Name = (string)row.Cells[1].Value;
                        float Precio = float.Parse((string)row.Cells["PrecioUnitario"].Value);

                        if (tipo == "Producto Elaborado")
                        {
                            int prodElabId = identificadorIdProductoElaborado(Name);
                            conexion.Open();
                            SqlCommand agregar = new SqlCommand("insert into DETALLEVENTA (DET_VENTA_CANT, PROD_ELAB_ID, VENTA_ID, DET_VENTA_PR_UNIT, DET_VENTA_DESC_UNIT) values (@cantidad,@productoelaborado,@ventaid, @precio, @descuentounitario)", conexion);
                            agregar.Parameters.Clear();
                            agregar.Parameters.Add("@ventaid", SqlDbType.Int).Value = ventaId;
                            agregar.Parameters.AddWithValue("@cantidad", Convert.ToString(row.Cells["Cantidad"].Value));
                            agregar.Parameters.AddWithValue("@precio", SqlDbType.Float).Value = Precio;
                            agregar.Parameters.Add("@productoelaborado", SqlDbType.Int).Value = prodElabId;
                            agregar.Parameters.AddWithValue("@descuentounitario", Convert.ToString(row.Cells["descuento"].Value));
                            agregar.ExecuteNonQuery();
                            conexion.Close();
                        }
                        else if (tipo == "Producto de Reventa")
                        {
                            int prodrevId = identificadorIdProductoReventa(Name);
                            conexion.Open();
                            SqlCommand agregar = new SqlCommand("insert into DETALLEVENTA (DET_VENTA_CANT, PROD_REV_ID, VENTA_ID, DET_VENTA_PR_UNIT, DET_VENTA_DESC_UNIT) values (@cantidad, @productoreventa,@ventaid, @precio, @descuentounitario)", conexion);
                            agregar.Parameters.Clear();
                            agregar.Parameters.Add("@ventaid", SqlDbType.Int).Value = ventaId;
                            agregar.Parameters.AddWithValue("@cantidad", Convert.ToString(row.Cells["Cantidad"].Value));
                            agregar.Parameters.AddWithValue("@precio", SqlDbType.Float).Value = Precio;
                            agregar.Parameters.Add("@productoreventa", SqlDbType.Int).Value = prodrevId;
                            agregar.Parameters.AddWithValue("@descuentounitario", Convert.ToString(row.Cells["descuento"].Value));
                            agregar.ExecuteNonQuery();
                            conexion.Close();
                        }
                    }
                }
                finally
                {
                    guardarCobro();
                    guardarDetalleDeMedioEfectivo();
                }
                dataGridView1.Rows.Clear();
                label3.Text = "Sin cobros";
                label15.Text = "";
                textBox1.Text = "";
                textBox2.Text = "";
                textBox3.Text = "";
                textBox4.Text = "";
                radioButton1.Checked = true;
                radioButton4.Checked = true;
                radioButton7.Checked = true;
                comboBox1.SelectedIndex = -1;
                comboBox5.SelectedIndex = -1;
                comboBox4.SelectedIndex = -1;
                comboBox3.SelectedIndex = -1;
                comboBox2.SelectedIndex = -1;
                numericUpDown1.Value = 0;
                numericUpDown2.Value = 0;
                button3.Enabled = false;
                button4.Enabled = false;
                cargarComboBox1Anonimo();
                comboBox1.SelectedValue = -1;
            }
        }


        DateTime fechaHoy = DateTime.Now;
        string tipoActivo;
        string IdUsu;

        private void guardarVenta()
        {
            if (radioButton2.Checked) // CLIENTE REGISTRADO
            {
                if (radioButton6.Checked) //OTRO MEDIO Y/U OTRO MONTO
                {
                    conexion.Open();
                    string sql = "insert into VENTA (CLIENTE_ID, VENTA_FECHA, USUARIO_ID,EST_VENTA_ID, VENTA_DTO) values (@clienteid,@ventafecha,@usuarioid,@estadoventaid,@descuento)";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@clienteid", SqlDbType.Int).Value = int.Parse(textBox3.Text);
                    comando.Parameters.Add("@ventafecha", SqlDbType.DateTime).Value = fechaHoy;
                    comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = IdUsu;
                    comando.Parameters.Add("@estadoventaid", SqlDbType.Int).Value = "1";
                    comando.Parameters.Add("@descuento", SqlDbType.Float).Value = descuentoBD;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    Aviso m = new Aviso();
                    m.label1.Text = "La venta fue registrada";
                    m.ShowDialog();
                    mostrarTrasGrabar();
                }
                else if (radioButton7.Checked) //EFECTIVO
                {
                    conexion.Open();
                    string sql = "insert into VENTA (CLIENTE_ID, VENTA_FECHA, USUARIO_ID,EST_VENTA_ID, VENTA_DTO) values (@clienteid,@ventafecha,@usuarioid,@estadoventaid,@descuento)";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@clienteid", SqlDbType.Int).Value = int.Parse(textBox3.Text);
                    comando.Parameters.Add("@ventafecha", SqlDbType.DateTime).Value = fechaHoy;
                    comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = IdUsu;
                    comando.Parameters.Add("@estadoventaid", SqlDbType.Int).Value = "3";
                    comando.Parameters.Add("@descuento", SqlDbType.Float).Value = descuentoBD;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    Aviso m = new Aviso();
                    m.label1.Text = "La venta fue registrada";
                    m.ShowDialog();
                    mostrarTrasGrabar();
                }
                whatsappGuardar(); //WHATSAPP
                radioButton1.Enabled = true;
                checkBox2.Enabled = false; //WHATSAPP AGRADECER
                checkBox3.Enabled = false;
                checkBox4.Enabled = false;
                impedirEnviarWhatsapp(); //WHATSAPP
            }
            else if (radioButton1.Checked) // CLIENTE ANÓNIMO
            {
                if (radioButton6.Checked)
                {
                    conexion.Open();
                    string sql = "insert into VENTA (VENTA_FECHA,USUARIO_ID,EST_VENTA_ID, VENTA_DTO) values (@ventafecha,@usuarioid,@estadoventaid,@descuento)";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@ventafecha", SqlDbType.DateTime).Value = fechaHoy;
                    comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = IdUsu;
                    comando.Parameters.Add("@estadoventaid", SqlDbType.Int).Value = "1";
                    comando.Parameters.Add("@descuento", SqlDbType.Float).Value = descuentoBD;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    Aviso m = new Aviso();
                    m.label1.Text = "La venta fue registrada";
                    m.ShowDialog();
                    mostrarTrasGrabar();
                }
                else if (radioButton7.Checked)
                {
                    conexion.Open();
                    string sql = "insert into VENTA (VENTA_FECHA,USUARIO_ID,EST_VENTA_ID, VENTA_DTO) values (@ventafecha,@usuarioid,@estadoventaid,@descuento)";
                    SqlCommand comando = new SqlCommand(sql, conexion);
                    comando.Parameters.Add("@ventafecha", SqlDbType.DateTime).Value = fechaHoy;
                    comando.Parameters.Add("@usuarioid", SqlDbType.Int).Value = IdUsu;
                    comando.Parameters.Add("@estadoventaid", SqlDbType.Int).Value = "3";
                    comando.Parameters.Add("@descuento", SqlDbType.Float).Value = descuentoBD;
                    comando.ExecuteNonQuery();
                    conexion.Close();
                    Aviso m = new Aviso();
                    m.label1.Text = "La venta fue registrada";
                    m.ShowDialog();
                    mostrarTrasGrabar();
                }
            }
        }

        private void whatsappGuardar()
        {
            if (checkBox3.Checked == true && checkBox4.Checked == true) //ESTE IF EXISTE POR SI FALLAN LAS EXCLUSIONES MUTUAS AUTOMÁTICAS DE LOS CHECKBOX
            { 
                Aviso m = new Aviso();
                m.label1.Text = "Para notificar por Whatsapp, puede seleccionar Retiro o Delivery, no ambas al mismo tiempo";
                m.ShowDialog();
            }
            else if (checkBox2.Checked == true && checkBox3.Checked == false && checkBox4.Checked == false)
                whatsappSoloAgradecer();
            else if (checkBox2.Checked == true && checkBox3.Checked == true && checkBox4.Checked == false)
                whatsappAgradecerRetiro();
            else if (checkBox2.Checked == true && checkBox3.Checked == false && checkBox4.Checked == true)
                whatsappAgradecerDelivery(); ;
        }

        private void guardarCobro()
        {
            if (radioButton7.Checked) // COBRO POR EFECTIVO
            {
                int ventaId = identificadorIdVenta();
                conexion.Open();
                string sql = "insert into COBROVENTA (COBRO_VENTA_FECHA, COBRO_VENTA_MONTO, VENTA_ID) values (@ventafecha,@monto,@ventaid)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@ventafecha", SqlDbType.DateTime).Value = fechaHoy;
                comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = ventaId;
                comando.Parameters.Add("@monto", SqlDbType.Float).Value = label15.Text;
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            else if (radioButton6.Checked) // COBRO POR OTRO MEDIO
            {
                MenuCobro m = new MenuCobro();
                m.ShowDialog();
            }
        }


        private void guardarDetalleDeMedioEfectivo()
        {
            if (radioButton7.Checked)
            {
                int cobroVentaId = identificadorIdCobroVenta();
                conexion.Open();
                string sql = "insert into DETALLEDEMEDIO (MEDIO_TR_ID,COBRO_VENTA_ID) values (@medio,@cobroventa)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@cobroventa", SqlDbType.Int).Value = cobroVentaId;
                comando.Parameters.Add("@medio", SqlDbType.Int).Value = "1";
                comando.ExecuteNonQuery();
                conexion.Close();
            }
            else
            {
                // NADA
            }
        }

        public int identificadorIdCobroVenta()
        {
            int ventaId = identificadorIdVenta();
            int idCobroVenta;
            conexion.Open();
            string sql = "select max(COBRO_VENTA_ID) as cobro_venta_id from COBROVENTA where VENTA_ID=@ventaid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = ventaId;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idVen = registro["COBRO_VENTA_ID"].ToString();
            idCobroVenta = int.Parse(idVen);
            conexion.Close();
            return idCobroVenta;
        }


        public int identificadorIdVenta()
        {
            int idVenta;
            conexion.Open();
            string sql = "select MAX(VENTA_ID) as MaximoId FROM VENTA";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idVen = registro["MaximoId"].ToString();
            idVenta = int.Parse(idVen);
            conexion.Close();
            return idVenta;
        }

        public int identificadorIdProductoElaborado(String Name)
        {
            int idElaborado;
            String nombre = "";
            nombre = Name;
            conexion.Open();
            string sql = "select PROD_ELAB_ID from PRODUCTOELAB where PROD_ELAB_DESCR=@desc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@desc", SqlDbType.VarChar).Value = nombre;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idElab = registro["PROD_ELAB_ID"].ToString();
            idElaborado = int.Parse(idElab);
            conexion.Close();
            return idElaborado;
        }

        public int identificadorIdProductoReventa(String Name)
        {
            int idProducto;
            String nombre = "";
            nombre = Name;
            conexion.Open();
            string sql = "select PROD_REV_ID from PRODUCTOREVENTA where PROD_REV_DESCR=@prodrevdesc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@prodrevdesc", SqlDbType.VarChar).Value = nombre;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idCom = registro["PROD_REV_ID"].ToString();
            idProducto = int.Parse(idCom);
            conexion.Close();
            return idProducto;
        }

        int b = 0;
        List<string> listModifCant2 = new List<string>();
        private void button4_Click(object sender, EventArgs e) // MODIFICAR VENTA
        {
            if (ExisteCobro((int)comboBox1.SelectedValue))
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede modificar la factura porque se ingresó un cobro";
                m.ShowDialog();
            }
            else
            {
                listModifCant2.Clear();
                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    listModifCant2.Add(dataGridView1.Rows[i].Cells["Cantidad"].Value.ToString());
                }
                if (listModifCant.SequenceEqual(listModifCant2))
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se verifican cambios en las cantidades ingresadas, modifique las mismas";
                    m.ShowDialog();
                }
                else if (b == 1)
                {
                    try
                    {
                        bool bandera = false;
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            String tipo = "";
                            tipo = (string)row.Cells[0].Value;
                            String Name = (string)row.Cells[1].Value;
                            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                            {
                                if (Convert.ToString(dataGridView1.Rows[i].Cells["Cantidad"].Value) == "0" || Convert.ToString(dataGridView1.Rows[i].Cells["Cantidad"].Value).Length == 0)
                                {
                                    bandera = true;
                                }
                            }
                            if (bandera == false && tipo == "Producto Elaborado")
                            {
                                int prdelabId = identificadorIdProductoElaborado(Name);
                                conexion.Open();
                                SqlCommand agregar = new SqlCommand("update DETALLEVENTA set DET_VENTA_CANT=@cantidad where VENTA_ID=@ventaid and PROD_ELAB_ID=@prelabid", conexion);
                                agregar.Parameters.Clear();
                                agregar.Parameters.AddWithValue("@cantidad", Convert.ToString(row.Cells["Cantidad"].Value));
                                agregar.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                                agregar.Parameters.Add("@prelabid", SqlDbType.Int).Value = prdelabId;
                                agregar.ExecuteNonQuery();
                                conexion.Close();
                                quitarFiltrosModificar();                                
                            }
                            else if (bandera == false && tipo == "Producto de Reventa")
                            {
                                int prodrevId = identificadorIdProductoReventa(Name);
                                conexion.Open();
                                SqlCommand agregar = new SqlCommand("update DETALLEVENTA set DET_VENTA_CANT=@cantidad where VENTA_ID=@ventaid and PROD_REV_ID=@prodrevid", conexion);
                                agregar.Parameters.Clear();
                                agregar.Parameters.AddWithValue("@cantidad", Convert.ToString(row.Cells["Cantidad"].Value));
                                agregar.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
                                agregar.Parameters.Add("@prodrevid", SqlDbType.Int).Value = prodrevId;
                                agregar.ExecuteNonQuery();
                                conexion.Close();
                                quitarFiltrosModificar();                                
                            }
                        }
                        if (bandera == true)
                        {
                            Aviso m = new Aviso();
                            m.label1.Text = "Verifique la cantidad ingresada, no puede ser 0";
                            m.ShowDialog();
                        }
                    }
                    finally
                    {

                    }
                }
            }
        }


        private void quitarFiltrosModificar()
        {
            Aviso m = new Aviso();
            m.label1.Text = "Se ha modificado la cantidad en la venta";
            m.ShowDialog();
            dataGridView1.Rows.Clear();
            label3.Text = "Sin cobros";
            label16.Text = "- - -";
            label15.Text = "";
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            radioButton1.Checked = true;
            radioButton4.Checked = true;
            radioButton7.Checked = true;
            comboBox1.SelectedIndex = -1;
            comboBox5.SelectedIndex = -1;
            comboBox4.SelectedIndex = -1;
            comboBox3.SelectedIndex = -1;
            comboBox2.SelectedIndex = -1;
            numericUpDown1.Value = 0;
            numericUpDown2.Value = 0;
            cargarComboBox1Anonimo();
            button1.Enabled = true;
            button3.Enabled = false;
            button6.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button4.Enabled = false;
            comboBox1.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox5.Enabled = true;
            radioButton1.Enabled = true;
            radioButton2.Enabled = true;
            radioButton3.Enabled = true;
            radioButton5.Enabled = true;
            radioButton4.Enabled = true;
            checkBox1.Enabled = true;
            checkBox1.Checked = false;
            numericUpDown1.Enabled = true;
            checkBox2.Enabled = false; //WHATSAPP AGRADECER
            checkBox3.Enabled = false;
            checkBox4.Enabled = false;
            radioButton1.Enabled = true;
            impedirEnviarWhatsapp(); //WHATSAPP
            mostrarTrasModEli();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) // CONSULTAR POR VENTAS PREVIAS
        {
            
        }

        private void mostrarTrasGrabar()
        {
            button5.Enabled = true;
            button10.Enabled = true;
            button5.Visible = true;
            button10.Visible = true;
            pictureBox11.Visible = true;
            label5.Visible = true;
            comboBox1.Visible = true;
            pictureBox1.Visible = true;
            groupBox3.Visible = true;
            pictureBox2.Visible = true;
            pictureBox5.Visible = true;
            button4.Visible = true;
            button3.Visible = true;
            label1.Visible = true;
            label17.Visible = true;
            pictureBox9.Visible = true;
            pictureBox10.Visible = true;
            label3.Visible = true;
            label16.Visible = true;
        }

        private void selectVenta()
        {
            if (comboBox1.SelectedIndex != -1)
            {
                conexion.Open();
                string sql = "SELECT EST_VENTA_NOMBRE, VENTA_DTO from VENTA ven JOIN ESTADOVENTA est on est.EST_VENTA_ID = ven.EST_VENTA_ID WHERE VENTA_ID=@idventa";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@idventa", SqlDbType.Int).Value = comboBox1.SelectedValue;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    label3.Text = "";
                    label3.Text = registros["EST_VENTA_NOMBRE"].ToString();
                    textBox4.Text = registros["VENTA_DTO"].ToString();
                }
                registros.Close();
                conexion.Close();
            }
        }

        private void selectDetalleVenta()
        {
            string PR = "Producto de Reventa";
            string PE = "Producto Elaborado";
            conexion.Open();
            string sql = "select SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as total, PROD_ELAB_DESCR, MARCA_NOMBRE, DET_VENTA_CANT, DET_VENTA_PR_UNIT,ISNULL(DET_VENTA_DESC_UNIT,0) as PROD_ELAB_DTO,det.PROD_ELAB_ID, PROD_REV_ID from DETALLEVENTA as det   join VENTA as ven on ven.VENTA_ID = det.VENTA_ID join PRODUCTOELAB as prelab on det.PROD_ELAB_ID = prelab.PROD_ELAB_ID  join MARCA as marc on marc.MARCA_ID = prelab.MARCA_ID WHERE det.VENTA_ID = @idventa GROUP BY PROD_ELAB_DESCR, MARCA_NOMBRE, DET_VENTA_CANT, DET_VENTA_PR_UNIT, DET_VENTA_DESC_UNIT, det.PROD_ELAB_ID, PROD_REV_ID UNION ALL select SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as total, PROD_REV_DESCR ,MARCA_NOMBRE, DET_VENTA_CANT, DET_VENTA_PR_UNIT,ISNULL(DET_VENTA_DESC_UNIT,0) AS PROD_REV_DTO, PROD_ELAB_ID, det.PROD_REV_ID from DETALLEVENTA as det join VENTA as ven on ven.VENTA_ID = det.VENTA_ID join PRODUCTOREVENTA as pr on det.PROD_REV_ID = pr.PROD_REV_ID   join MARCA as marc on marc.MARCA_ID = pr.MARCA_ID WHERE det.VENTA_ID = @idventa GROUP BY PROD_REV_DESCR, MARCA_NOMBRE, DET_VENTA_CANT, DET_VENTA_PR_UNIT, DET_VENTA_DESC_UNIT,PROD_ELAB_ID, det.PROD_REV_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@idventa", SqlDbType.Int).Value = comboBox1.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                decimal ProductoElab = 0;
                decimal ProdRev = 0;
                Decimal.TryParse(registros["PROD_ELAB_ID"].ToString(), out ProductoElab);
                Decimal.TryParse(registros["PROD_REV_ID"].ToString(), out ProdRev);

                if (ProductoElab != 0)
                {
                    dataGridView1.Rows.Add(PE, registros["PROD_ELAB_DESCR"].ToString(),
                    registros["MARCA_NOMBRE"].ToString(), registros["DET_VENTA_CANT"].ToString(),
                    registros["DET_VENTA_PR_UNIT"].ToString(), registros["PROD_ELAB_DTO"].ToString(),
                    registros["total"].ToString());
                }
                else
                if (ProdRev != 0)
                {

                    dataGridView1.Rows.Add(PR, registros["PROD_ELAB_DESCR"].ToString(),
                    registros["MARCA_NOMBRE"].ToString(), registros["DET_VENTA_CANT"].ToString(),
                    registros["DET_VENTA_PR_UNIT"].ToString(), registros["PROD_ELAB_DTO"].ToString(), 
                    registros["total"].ToString());
                }

            }
            registros.Close();
            conexion.Close();
        }

        

        List<string> listModifCant = new List<string>();

        private void button5_Click(object sender, EventArgs e) //CONSULTAR VENTA
        {
            listModifCant.Clear();
            if (comboBox1.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe elegir una venta previa";
                m.ShowDialog();
            }
            else
            {
                selectVenta();
                selectDetalleVenta();
                saldoPendienteDeCobro();
                button4.Enabled = true;
                button3.Enabled = true;
                button1.Enabled = false;
                if(radioButton1.Checked)
                { 
                    checkBox2.Enabled = false; //WHATSAPP AGRADECER
                    checkBox3.Enabled = false;
                    checkBox4.Enabled = false;
                    impedirEnviarWhatsapp(); //WHATSAPP
                }
                else if (radioButton2.Checked)
                {
                    checkBox2.Enabled = false; //WHATSAPP AGRADECER
                    checkBox3.Enabled = true;
                    checkBox4.Enabled = true;
                    permitirEnviarWhatsapp(); //WHATSAPP
                }
                foreach (var col in dataGridView1.Columns.Cast<DataGridViewColumn>())
                {
                    col.ReadOnly = true;
                    dataGridView1.Columns["Cantidad"].ReadOnly = false;
                }
                if (textBox4.Text == "0")
                {
                    numericUpDown1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    button6.Enabled = false;
                    button8.Enabled = false;
                    button9.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;
                    radioButton4.Checked = true;
                    radioButton3.Enabled = false;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton6.Enabled = false;
                    radioButton7.Enabled = false;
                    checkBox1.Enabled = false;
                    calcularTotal();
                    b = 1;
                    foreach (var col in dataGridView1.Columns.Cast<DataGridViewColumn>())
                    {
                        col.ReadOnly = true;
                        dataGridView1.Columns["Cantidad"].ReadOnly = false;
                    }
                }
                else
                {
                    numericUpDown1.Enabled = false;
                    comboBox2.Enabled = false;
                    comboBox3.Enabled = false;
                    comboBox4.Enabled = false;
                    comboBox5.Enabled = false;
                    button6.Enabled = false;
                    button8.Enabled = false;
                    button9.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton5.Enabled = false;
                    radioButton4.Enabled = false;
                    radioButton3.Checked = true;
                    radioButton1.Enabled = false;
                    radioButton2.Enabled = false;
                    radioButton3.Enabled = false;
                    radioButton6.Enabled = false;
                    radioButton7.Enabled = false;
                    checkBox1.Enabled = false;
                    checkBox1.Checked = true;
                    calcularTotalConDescuentoMonto();
                    b = 1;
                    foreach (var col in dataGridView1.Columns.Cast<DataGridViewColumn>())
                    {
                        col.ReadOnly = true;
                        dataGridView1.Columns["Cantidad"].ReadOnly = false;
                    }
                }

                for (int i = 0; i < dataGridView1.Rows.Count; i++)
                {
                    listModifCant.Add(dataGridView1.Rows[i].Cells["Cantidad"].Value.ToString());
                }

                ocultarTrasConsultar();
            }
        }

        private void ocultarTrasConsultar()
        {
            label6.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label12.Visible = false;
            label7.Visible = false;
            comboBox5.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            numericUpDown1.Visible = false;
            button6.Visible = false;
            pictureBox6.Visible = false;
            pictureBox7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
            groupBox1.Visible = false;
            radioButton7.Visible = false;
            radioButton6.Visible = false;
            label19.Visible = false;
            label18.Visible = false;
            label14.Visible = false;
            pictureBox1.Visible = false;
            button1.Visible = false;
            if (radioButton1.Checked)
            {//OCULTAR WHATSAPP
                groupBox4.Visible = false;
                checkBox2.Visible = false;
                checkBox3.Visible = false;
                checkBox4.Visible = false;
            }
        }

        private void mostrarTrasModEli() //MOSTRAR TRAS MODIFICAR O ELIMINAR
        {
            if (radioButton1.Checked)
            {//MOSTRAR WHATSAPP
                groupBox4.Visible = true;
                checkBox2.Visible = true;
                checkBox3.Visible = true;
                checkBox4.Visible = true;
            }
            label6.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label12.Visible = true;
            label7.Visible = true;
            comboBox5.Visible = true;
            comboBox2.Visible = true;
            comboBox3.Visible = true;
            comboBox4.Visible = true;
            numericUpDown1.Visible = true;
            button6.Visible = true;
            pictureBox6.Visible = true;
            pictureBox7.Visible = true;
            button8.Visible = true;
            button9.Visible = true;
            groupBox1.Visible = true;
            radioButton7.Visible = true;
            radioButton6.Visible = true;
            label19.Visible = true;
            label18.Visible = true;
            label14.Visible = true;
            pictureBox1.Visible = true;
            button1.Visible = true;
        }

        private void saldoPendienteDeCobro()
        {
            conexion.Open();
            string sql = "SELECT T1.totalventa-T2.cobros-t3.descuento as saldoacobrar FROM (SELECT det.VENTA_ID, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'totalventa' FROM DETALLEVENTA as det, VENTA as ven WHERE det.VENTA_ID=ven.VENTA_ID and ven.VENTA_ID=@ventaid group by det.VENTA_ID) T1 LEFT JOIN (SELECT venta.venta_id, coalesce(sum(COBROVENTA.COBRO_VENTA_MONTO), 0) as 'cobros' FROM VENTA left JOIN COBROVENTA ON VENTA.VENTA_ID = COBROVENTA.VENTA_ID group by VENTA.VENTA_ID) T2 on (T1.VENTA_ID=T2.VENTA_ID) left join (select ven.venta_id, coalesce(ven.VENTA_DTO,0) as 'descuento' from venta as ven, detalleventa as det  where det.venta_id=ven.venta_id and ven.VENTA_ID=@ventaid group by ven.venta_id, ven.VENTA_DTO) t3 ON (t3.VENTA_ID=T2.VENTA_ID)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@ventaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                string montoTotal = registros["saldoacobrar"].ToString();
                label16.Text = registros["saldoacobrar"].ToString();
            }
            registros.Close();
            conexion.Close();
        }
        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (b == 1 && dataGridView1.Rows.Count!=0)
            {
                calcularSubTotal();
                calcularTotalConDescuentoMonto();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (b == 1 && dataGridView1.Rows.Count != 0)
            {
                calcularSubTotal();
                calcularTotalConDescuentoMonto();
            }
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            /*if (b == 1)
            {
                calcularSubTotal();
                calcularTotalConDescuentoMonto();
            }*/
        }

        private void button3_Click(object sender, EventArgs e) // ELIMINAR VENTA
        {
              if (comboBox1.SelectedIndex == -1)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Debe cargar alguna venta previa";
                    m.ShowDialog();
                }
                else if (ExisteCobro((int)comboBox1.SelectedValue))
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se puede eliminar la factura porque se ingresó un cobro";
                    m.ShowDialog();
                }
                else if (b==1)
                {
                    DialogResult dialogResult = MessageBox.Show("¿Confirma la eliminación?", "Eliminar venta", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        eliminarDetalleVenta();
                        conexion.Open();
                        string sql = "delete from VENTA where VENTA_ID=@idventa"; 
                        SqlCommand comando = new SqlCommand(sql, conexion);
                        comando.Parameters.Add("@idventa", SqlDbType.VarChar).Value = comboBox1.SelectedValue;
                        int cant = comando.ExecuteNonQuery();
                        conexion.Close();
                        if (cant == 1)
                        {
                            Aviso m = new Aviso();
                            m.label1.Text = "Se ha eliminado la venta";
                            m.ShowDialog();
                            dataGridView1.Rows.Clear();
                            label3.Text = "Sin cobros";
                            label15.Text = "";
                            label16.Text = "- - -";
                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";
                            textBox4.Text = "";
                            radioButton1.Checked = true;
                            radioButton4.Checked = true;
                            radioButton7.Checked = true;
                            comboBox1.SelectedIndex = -1;
                            comboBox5.SelectedIndex = -1;
                            comboBox4.SelectedIndex = -1;
                            comboBox3.SelectedIndex = -1;
                            comboBox2.SelectedIndex = -1;
                            numericUpDown1.Value = 0;
                            numericUpDown2.Value = 0;
                            cargarComboBox1Anonimo();
                            button1.Enabled = true;
                            button6.Enabled = true;
                            button8.Enabled = true;
                            button9.Enabled = true;
                            button4.Enabled = false;
                            comboBox1.Enabled = true;
                            comboBox2.Enabled = true;
                            comboBox3.Enabled = true;
                            comboBox4.Enabled = true;
                            comboBox5.Enabled = true;
                            radioButton1.Enabled = true;
                            radioButton2.Enabled = true;
                            radioButton3.Enabled = true;
                            radioButton5.Enabled = true;
                            radioButton4.Enabled = true;
                            checkBox1.Enabled = true;
                            checkBox1.Checked = false;
                            numericUpDown1.Enabled = true;
                            button3.Enabled = false;
                            b = 0;
                            checkBox2.Enabled = false; //WHATSAPP AGRADECER
                            checkBox3.Enabled = false;
                            checkBox4.Enabled = false;
                            radioButton1.Enabled = true;
                            impedirEnviarWhatsapp(); //WHATSAPP
                            mostrarTrasModEli();
                        }  
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
        }


        private bool ExisteCobro(int idVenta)
        {
            conexion.Open();
            string sql = "select COBRO_VENTA_ID from COBROVENTA where VENTA_ID=@idventa";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@idventa", SqlDbType.Int).Value = idVenta;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private void eliminarDetalleVenta()
        {
                conexion.Open();
                string sql = "delete from DETALLEVENTA where VENTA_ID=@idventa";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@idventa", SqlDbType.VarChar).Value = comboBox1.SelectedValue;
                int cant = comando.ExecuteNonQuery();
                conexion.Close();            
        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            // radioButton1.Enabled = false; VER MAS ADELANTE SI AL PRESIONAR EL RADIOBUTTON 2 HAY QUE INHABILITAR EL RADIO BUTTON 1
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }


        private void whatsappSoloAgradecer() //WHATSAPP
        {
            System.Diagnostics.Process.Start("https://web.whatsapp.com/send?phone=54" + textBox5.Text + "&text=" + "%C2%A1Hola!%20%C2%A1Muchas%20gracias%20por%20tu%20compra!%20%F0%9F%98%83%20Seguimos%20disponibles%20para%20vos%20%F0%9F%98%89%20MTA25%20%F0%9F%A5%AA");
        }

        private void button11_Click(object sender, EventArgs e) //BOTÓN ENVIAR, ESPECÍFICO DE GROUPBOX DE WHATSAPP
        {
            if (checkBox2.Checked == false && checkBox3.Checked == false && checkBox4.Checked == false)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Seleccione la novedad a notificar: Listo para retiro o Delivery iniciado";
                m.ShowDialog();
            }
            else
                whatsappPostConsulta();
        }

        private void whatsappPostConsulta()
        {
            if (checkBox2.Checked==false && checkBox3.Checked==true && checkBox4.Checked==false)
                whatsappRetiro();
            else if (checkBox2.Checked==false && checkBox3.Checked==false && checkBox4.Checked==true)
                whatsappDelivery();
        }

        private void whatsappRetiro()
        {
            System.Diagnostics.Process.Start("https://web.whatsapp.com/send?phone=54" + textBox5.Text + "&text=" + "%C2%A1Hola!%20Tu%20compra%20del%20" + comboBox1.Text + "hs,%20est%C3%A1%20lista%20para%20retiro%20%F0%9F%98%83%20Seguimos%20disponibles%20para%20vos%20%F0%9F%98%89%20MTA25%20%F0%9F%A5%AA");
        }

        private void whatsappDelivery()
        {
            System.Diagnostics.Process.Start("https://web.whatsapp.com/send?phone=54" + textBox5.Text + "&text=" + "%C2%A1Hola!%20Tu%20compra%20del%20" + comboBox1.Text + "hs,%20ya%20est%C3%A1%20en%20manos%20del%20delivery%20%F0%9F%98%83%20Seguimos%20disponibles%20para%20vos%20%F0%9F%98%89%20MTA25%20%F0%9F%A5%AA");
        }

        private void whatsappAgradecerRetiro()
        {
            System.Diagnostics.Process.Start("https://web.whatsapp.com/send?phone=54" + textBox5.Text + "&text=" + "%C2%A1Hola!%20Tu%20compra%20est%C3%A1%20lista%20para%20retiro%20%F0%9F%98%83%20%C2%A1Muchas%20gracias!%20%F0%9F%98%89%20MTA25%20%F0%9F%A5%AA");
        }

        private void whatsappAgradecerDelivery()
        {
            System.Diagnostics.Process.Start("https://web.whatsapp.com/send?phone=54" + textBox5.Text + "&text=" + "%C2%A1Hola!%20Tu%20compra%20ya%20est%C3%A1%20en%20manos%20del%20delivery%20%F0%9F%98%83%20%C2%A1Muchas%20gracias!%20%F0%9F%98%89%20MTA25%20%F0%9F%A5%AA");
        }

        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox4.Enabled == true && button11.Enabled == false) //PARA UNA NUEVA VENTA
            {
                checkBox4.Enabled = false;
                checkBox2.Enabled = false;
                checkBox2.Checked = true;
            }
            else if (checkBox4.Enabled==false && button11.Enabled == false) //PARA UNA NUEVA VENTA
            { 
                checkBox4.Enabled = true;
                checkBox2.Enabled = true;
                checkBox2.Checked = true;
            }
            else if (checkBox4.Enabled == true && button11.Enabled == true) //POSTCONSULTA OK DE UNA VENTA YA EXISTENTE
            {
                checkBox4.Enabled = false;
            }
            else if (checkBox4.Enabled == false && button11.Enabled == true) //POSTCONSULTA OK DE UNA VENTA YA EXISTENTE
            {
                checkBox4.Enabled = true;
            }
        }

        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox3.Enabled == true && button11.Enabled == false) //PARA UNA NUEVA VENTA
            {
                checkBox3.Enabled = false;
                checkBox2.Enabled = false;
                checkBox2.Checked = true;
            }
            else if (checkBox3.Enabled == false && button11.Enabled == false) //PARA UNA NUEVA VENTA
            {
                checkBox3.Enabled = true;
                checkBox2.Enabled = true;
                checkBox2.Checked = true;
            }
            else if (checkBox3.Enabled == true && button11.Enabled == true) //POSTCONSULTA OK DE UNA VENTA YA EXISTENTE
            {
                checkBox3.Enabled = false;
            }
            else if (checkBox3.Enabled == false && button11.Enabled == true) //POSTCONSULTA OK DE UNA VENTA YA EXISTENTE
            {
                checkBox3.Enabled = true;
            }
        }

        private void button10_Click(object sender, EventArgs e)
        {
            ocultarTrasConsultar();

            if(button10.Text=="Ver más")
            {
                mostrarFechasParaConsulta();
            }
            else if (button10.Text == "<< OK")
            {
                ocultarFechasParaConsulta();
                llenarFechasDeVentas();
            }
        }

        private void ocultarFechasParaConsulta()
        {
            groupBox5.Visible = false;
            button10.Text = "Ver más";
        }

        private void mostrarFechasParaConsulta()
        {
            groupBox5.Visible = true;
            button10.Text = "<< OK";
        }

        private void llenarFechasDeVentas()
        {
            if (radioButton1.Checked)
                cargarComboBox1AnonimoFechas();
            else if (radioButton2.Checked)
                cargarComboBox1ClienteFechas();
        }

        public void cargarComboBox1AnonimoFechas()
        {
            if (textBox1.Text == "")
            {
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
                conexion.Open();
                string sql = "select VENTA_FECHA, VENTA_ID from VENTA where CLIENTE_ID IS NULL and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta ORDER BY VENTA_FECHA DESC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox1.DataSource = tabla1;
                comboBox1.DisplayMember = "VENTA_FECHA";
                comboBox1.ValueMember = "VENTA_ID";
                comboBox1.SelectedValue = -1;
            }
        }

        public void cargarComboBox1ClienteFechas()
        {
            if (textBox1.Text != "")
            {
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
                conexion.Open();
                string sql = "select VENTA_FECHA, VENTA_ID from VENTA where CLIENTE_ID=@cliente and VENTA_FECHA>=@fechadesde and VENTA_FECHA<=@fechahasta ORDER BY VENTA_FECHA DESC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@cliente", SqlDbType.Int).Value = int.Parse(textBox3.Text);
                comando.Parameters.Add("@fechadesde", SqlDbType.DateTime).Value = dateTimePicker1.Value.Date;
                comando.Parameters.Add("@fechahasta", SqlDbType.DateTime).Value = dateTimePicker2.Value;
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox1.DataSource = tabla1;
                comboBox1.DisplayMember = "VENTA_FECHA";
                comboBox1.ValueMember = "VENTA_ID";
                comboBox1.SelectedValue = -1;
            }
            else if (textBox1.Text == "")
            {
                comboBox1.DataSource = null;
                comboBox1.Items.Clear();
            }
        }
    }     
}