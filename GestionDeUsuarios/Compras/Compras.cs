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
    public partial class Compras : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        string tipoActivo;

        public Compras(string tipoActivo2)
        {
            InitializeComponent();
            tipoActivo = tipoActivo2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Compras_Load(object sender, EventArgs e)
        {
            listArt.Clear();
            foreach (var col in dataGridView1.Columns.Cast<DataGridViewColumn>())
            {
                col.ReadOnly = true;
            }
            cargarComboBox1();
            //cargarComboBox2();
            //cargarComboBox3();
            button4.Enabled = false;
            cargarEstados();
        }

        private void cargarEstados()
        {
            conexion.Open();
            string sql = "select COUNT(*) EST_COMPRA_NOMBRE from ESTADOCOMPRA where EST_COMPRA_NOMBRE='Sin pagos'";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registro = comando.ExecuteReader();
            if (registro.Read())
            {
                string cant = registro["EST_COMPRA_NOMBRE"].ToString();
                int cantidadEstado = int.Parse(cant.ToString());

                registro.Close();
                conexion.Close();
                if (cantidadEstado == 0)
                {
                    conexion.Open();
                    string sql1 = "insert into ESTADOCOMPRA(EST_COMPRA_NOMBRE) values ('Sin pagos')";
                    string sql2 = "insert into ESTADOCOMPRA(EST_COMPRA_NOMBRE) values ('Pagada parcialmente')";
                    string sql3 = "insert into ESTADOCOMPRA(EST_COMPRA_NOMBRE) values ('Pagada totalmente')";
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

        private void mostrarGrillaUno()
        {
            conexion.Open();
            string sql = "select PROD_REV_DESCR, DET_COMPRA_CANTIDAD, DET_COMPRA_PR_UNIT from DETALLECOMPRA as det join PRODUCTOREVENTA as prodrev on prodrev.PROD_REV_ID = det.PROD_REV_ID ORDER BY PROD_REV_DESCR ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["PROD_REV_DESCR"].ToString(),
                                  registros["DET_COMPRA_CANTIDAD"].ToString(), registros["DET_COMPRA_PR_UNIT"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button5_Click(object sender, EventArgs e)
        {

        }


        int b = 0;
        private void button2_Click(object sender, EventArgs e)
        {
            listArt.Clear();
            if (textBox1.Text == "" || comboBox1.SelectedIndex == -1 || dataGridView1.Rows.Count == 0)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Por favor cargue un número de factura y detalles de compra";
                m.ShowDialog();
            }
            else if (!ExisteNumFactura(textBox1.Text) && b == 0)
            {
                guardarCompra();
                int compraId = identificadorIdCompra();

                try
                {
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {

                        String tipo = "";
                        tipo = (string)row.Cells[1].Value;
                        String Name = (string)row.Cells[0].Value;
                        float Precio = float.Parse((string)row.Cells["PrecioUnitario"].Value);

                        if (tipo == "Materia Prima")
                        {
                            int materiaId = identificadorIdMateriaPrima(Name);
                            conexion.Open();
                            SqlCommand agregar = new SqlCommand("insert into DETALLECOMPRA (COMPRA_ID, DET_COMPRA_CANTIDAD, DET_COMPRA_PR_UNIT, MATERIAPR_ID) values (@compraid, @cantidadcompra,@preciounitario,@materiaid)", conexion);
                            agregar.Parameters.Clear();
                            agregar.Parameters.Add("@compraid", SqlDbType.Int).Value = compraId;
                            agregar.Parameters.AddWithValue("@cantidadcompra", Convert.ToString(row.Cells["Cantidad"].Value));
                            agregar.Parameters.AddWithValue("@preciounitario", SqlDbType.Float).Value = Precio;
                            agregar.Parameters.Add("@materiaid", SqlDbType.Int).Value = materiaId;
                            agregar.ExecuteNonQuery();
                            conexion.Close();
                        }
                        else if (tipo == "Producto de Reventa")
                        {
                            int prodrevId = identificadorIdProductoReventa(Name);
                            conexion.Open();
                            SqlCommand agregar = new SqlCommand("insert into DETALLECOMPRA (COMPRA_ID, DET_COMPRA_CANTIDAD, DET_COMPRA_PR_UNIT, PROD_REV_ID) values (@compraid, @cantidadcompra,@preciounitario,@prodrevid)", conexion);
                            agregar.Parameters.Clear();
                            agregar.Parameters.Add("@compraid", SqlDbType.Int).Value = compraId;
                            agregar.Parameters.AddWithValue("@cantidadcompra", Convert.ToString(row.Cells["Cantidad"].Value));
                            agregar.Parameters.AddWithValue("@preciounitario", SqlDbType.Float).Value = Precio;
                            agregar.Parameters.Add("@prodrevid", SqlDbType.Int).Value = prodrevId;
                            agregar.ExecuteNonQuery();
                            conexion.Close();
                        }
                    }
                }
                finally
                {

                }
                dataGridView1.Rows.Clear();
                label4.Text = "Total: $";
                textBox1.Text = "";
                comboBox1.SelectedIndex = 0;
                dateTimePicker1.Value = DateTime.Now;
                numericUpDown1.Value = 0;
                textBox3.Text = "";

                mostrarTrasGrabar();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya existe una factura con ese número";
                m.ShowDialog();
            }

        }

        private void mostrarTrasGrabar()
        {
            pictureBox8.Visible = true;
            button7.Visible = true;
            groupBox1.Visible = true;
        }

        private void actualizarCompra()
        {
            conexion.Open();
            string sql = "update COMPRA set PROVEE_ID=@proveeid, COMPRA_FECHA=@fechacompra, EST_COMPRA_ID=@estadocompraid where COMPRA_NUM_FACT=@numfact";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
            comando.Parameters.Add("@numfact", SqlDbType.VarChar).Value = textBox1.Text;
            comando.Parameters.Add("@fechacompra", SqlDbType.Date).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@estadocompraid", SqlDbType.Int).Value = "1";
            int cant = comando.ExecuteNonQuery();
            conexion.Close();
        }

        private void guardarCompra()
        {
            conexion.Open();
            string sql = "insert into COMPRA (PROVEE_ID, COMPRA_NUM_FACT, COMPRA_FECHA , EST_COMPRA_ID) values (@proveeid,@numfact,@fechacompra,@estadocompraid)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@proveeid", SqlDbType.Int).Value = comboBox1.SelectedValue.ToString();
            comando.Parameters.Add("@numfact", SqlDbType.VarChar).Value = textBox1.Text;
            comando.Parameters.Add("@fechacompra", SqlDbType.Date).Value = dateTimePicker1.Value;
            comando.Parameters.Add("@estadocompraid", SqlDbType.Int).Value = "1";
            comando.ExecuteNonQuery();
            conexion.Close();
            Aviso m = new Aviso();
            m.label1.Text = "La compra fue registrada";
            m.ShowDialog();
        }

        public int identificadorIdCompra()
        {
            int idCompra;
            conexion.Open();
            string sql = "select COMPRA_ID from COMPRA where compra_num_fact=@numfact";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@numfact", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idCom = registro["COMPRA_ID"].ToString();
            idCompra = int.Parse(idCom);
            conexion.Close();
            return idCompra;
        }

        public int identificadorIdMateriaPrima(String Name)
        {
            int idMateria;
            String nombre = "";
            nombre = Name;
            conexion.Open();
            string sql = "select MATERIAPR_ID from MATERIAPRIMA where MATERIAPR_DESCR=@materiaprdesc";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@materiaprdesc", SqlDbType.VarChar).Value = nombre;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idCom = registro["MATERIAPR_ID"].ToString();
            idMateria = int.Parse(idCom);
            conexion.Close();
            return idMateria;
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

        public void guardarDetalleDeCompraUno()
        {
            float subtotalfila = 0;
            float preciounitariofila = 0;
            int cantidadfila = 0;
            preciounitariofila = float.Parse(textBox3.Text);
            cantidadfila = (int)numericUpDown1.Value;
            subtotalfila = preciounitariofila * cantidadfila;
            dataGridView1.Rows.Add(comboBox4.Text, comboBox5.Text, comboBox2.Text, comboBox3.Text, numericUpDown1.Value, textBox3.Text, subtotalfila);
        }

        public void calcularTotal()
        {
            Decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[6].Value);
            }
            label4.Text = "Total: $" + suma.ToString();
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

        private void cargarComboBox2()
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
                comboBox2.DisplayMember = "RUBRO_NOMBRE";
                comboBox2.ValueMember = "RUBRO_ID";
                comboBox2.DataSource = tabla1;
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
                comboBox2.DisplayMember = "RUBRO_NOMBRE";
                comboBox2.ValueMember = "RUBRO_ID";
                comboBox2.DataSource = tabla1;
            }
        }

        private void cargarComboBox3()
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

        private void cargarComboBox4()
        {
            if (comboBox5.Text == "Materia Prima" && comboBox2.SelectedIndex!=-1 && comboBox3.SelectedIndex != -1)
            {
                conexion.Open();
                string sql = "select MATERIAPR_ID, MATERIAPR_DESCR from MATERIAPRIMA where RUBRO_ID=@rubroid and MARCA_ID=@marcaid ORDER BY MATERIAPR_DESCR ASC";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@rubroid", SqlDbType.Int).Value = comboBox2.SelectedValue;
                comando.Parameters.Add("@marcaid", SqlDbType.Int).Value = comboBox3.SelectedValue;
                SqlDataAdapter adaptador1 = new SqlDataAdapter();
                adaptador1.SelectCommand = comando;
                DataTable tabla1 = new DataTable();
                adaptador1.Fill(tabla1);
                conexion.Close();
                comboBox4.DataSource = tabla1;
                comboBox4.DisplayMember = "MATERIAPR_DESCR";
                comboBox4.ValueMember = "MATERIAPR_ID";
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

       
        private void CargarComboBox4(object sender, EventArgs e) //evento disparado por los otros ComboBox
        {
            cargarComboBox2();
            cargarComboBox3();
        }
         
        private bool ExisteNumFactura(string numerofactura)
        {
            conexion.Open();
            string sql = "select COMPRA_NUM_FACT from COMPRA where COMPRA_NUM_FACT=@numerofact";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@numerofact", SqlDbType.VarChar).Value = numerofactura;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        List<string> listArt = new List<string>();
        private void button6_Click(object sender, EventArgs e)
        {
            int cantFilas = dataGridView1.Rows.Count;
            if (cantFilas == 0)
            {
                if(textBox3.Text == ""  && numericUpDown1.Value == 0 && comboBox5.SelectedIndex == -1 && comboBox2.SelectedIndex == -1 && comboBox3.SelectedIndex == -1 && comboBox4.SelectedIndex == -1)
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
                else if (textBox3.Text == "" || textBox3.Text == "0")
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Debe cargar algún precio";
                    m.ShowDialog();
                }
                else
                {
                    guardarDetalleDeCompraUno();
                    calcularTotal();
                    listArt.Add(comboBox4.Text);

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
                    verificar = dataGridView1.Rows[i].Cells["Nombre"].Value.ToString()+ dataGridView1.Rows[i].Cells["Marca"].Value.ToString();
                    if (artMarcaConcatenado == verificar)
                    {
                        presente = true;
                    }
                }
                if (presente==true)
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "El artículo seleccionado ya fue agregado, no puede repetirse";
                    m.ShowDialog();
                }
                else 
                {
                    guardarDetalleDeCompraUno();
                    calcularTotal();
                    listArt.Add(comboBox4.Text);

                    ocultarTrasAgregar();
                }
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (b == 1)
            {
                calcularSubTotal();
                calcularTotal();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            listArt.Clear();
            DialogResult dialogResult = MessageBox.Show("¿Está seguro que desea limpiar toda la lista?", "Eliminar todos las filas", MessageBoxButtons.YesNo);
            if (dialogResult == DialogResult.Yes)
            {
                dataGridView1.Rows.Clear();
                calcularTotal();
            }
            else if (dialogResult == DialogResult.No)
            {
                return;
            }
        }

        private void button8_Click(object sender, EventArgs e)
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
                    string cellValue = Convert.ToString(selectedRow.Cells["Nombre"].Value);
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

        private void button3_Click(object sender, EventArgs e)
        {

        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button7_Click(object sender, EventArgs e)
        {
            if (!ExisteNumFactura(textBox1.Text))
            {
                Aviso m = new Aviso();
                m.label1.Text = "No existe ese número de factura";
                m.ShowDialog();
            }
            else
            {
                ocultarTrasConsultar();

                selectCompra();
                selectDetalleDeFactura();
                calcularTotal();
                saldoPendienteDePago();
                button4.Enabled = true;
                button2.Enabled = false;
                foreach (var col in dataGridView1.Columns.Cast<DataGridViewColumn>())
                {
                    col.ReadOnly = true;
                    dataGridView1.Columns["Cantidad"].ReadOnly = false;
                    dataGridView1.Columns["PrecioUnitario"].ReadOnly = false;
                }
                b = 1;
                numericUpDown1.Enabled = false;
                textBox1.Enabled = false;
                textBox3.Enabled = false;
                comboBox2.Enabled = false;
                comboBox3.Enabled = false;
                comboBox4.Enabled = false;
                comboBox5.Enabled = false;
                button6.Enabled = false;
                button8.Enabled = false;
                button9.Enabled = false;
            }
        }

        private void ocultarTrasConsultar()
        {
            label5.Visible = false;
            label9.Visible = false;
            label10.Visible = false;
            label12.Visible = false;
            label7.Visible = false;
            label8.Visible = false;
            comboBox5.Visible = false;
            comboBox2.Visible = false;
            comboBox3.Visible = false;
            comboBox4.Visible = false;
            numericUpDown1.Visible = false;
            textBox3.Visible = false;
            button6.Visible = false;
            pictureBox6.Visible = false;
            pictureBox7.Visible = false;
            button8.Visible = false;
            button9.Visible = false;
            pictureBox1.Visible = false;
            button2.Visible = false;
        }

        private void ocultarTrasAgregar()
        {
            pictureBox8.Visible = false;
            button7.Visible = false;
            groupBox1.Visible = false;
        }

            private void selectDetalleDeFactura()
        {
                 
                string PR = "Producto de Reventa";
                string MP = "Materia Prima";
                conexion.Open();
                string sql = "select SUM(DET_COMPRA_CANTIDAD*DET_COMPRA_PR_UNIT) as total , MATERIAPR_DESCR,DET_COMPRA_CANTIDAD, RUBRO_NOMBRE, MARCA_NOMBRE, det.MATERIAPR_ID, PROD_REV_ID, DET_COMPRA_PR_UNIT  from DETALLECOMPRA as det join COMPRA as com on com.COMPRA_ID = det.COMPRA_ID join MATERIAPRIMA as mat on det.MATERIAPR_ID = mat.MATERIAPR_ID join RUBRO as rub on rub.RUBRO_ID = mat.RUBRO_ID join MARCA as marc on marc.MARCA_ID = mat.MARCA_ID WHERE COMPRA_NUM_FACT = @numerofact GROUP BY  MATERIAPR_DESCR,DET_COMPRA_CANTIDAD, RUBRO_NOMBRE, MARCA_NOMBRE, det.MATERIAPR_ID, PROD_REV_ID, DET_COMPRA_PR_UNIT UNION ALL select SUM(DET_COMPRA_CANTIDAD * DET_COMPRA_PR_UNIT) as total, PROD_REV_DESCR,DET_COMPRA_CANTIDAD, RUBRO_NOMBRE, MARCA_NOMBRE, MATERIAPR_ID, det.PROD_REV_ID, DET_COMPRA_PR_UNIT from DETALLECOMPRA as det join COMPRA as com on com.COMPRA_ID = det.COMPRA_ID join PRODUCTOREVENTA as prod on det.PROD_REV_ID = prod.PROD_REV_ID join RUBRO as rub on rub.RUBRO_ID = prod.RUBRO_ID join MARCA as marc on marc.MARCA_ID = prod.MARCA_ID WHERE COMPRA_NUM_FACT = @numerofact GROUP BY  PROD_REV_DESCR,DET_COMPRA_CANTIDAD, RUBRO_NOMBRE, MARCA_NOMBRE, MATERIAPR_ID, det.PROD_REV_ID, DET_COMPRA_PR_UNIT";
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@numerofact", SqlDbType.VarChar).Value = textBox1.Text;
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                    decimal MateriaPr = 0;
                    decimal ProdRev = 0;
                    Decimal.TryParse(registros["MATERIAPR_ID"].ToString(), out MateriaPr);
                    Decimal.TryParse(registros["PROD_REV_ID"].ToString(), out ProdRev);

                    if (MateriaPr!=0)
                    {
                        dataGridView1.Rows.Add(registros["MATERIAPR_DESCR"].ToString(), MP,
                        registros["RUBRO_NOMBRE"].ToString(), registros["MARCA_NOMBRE"].ToString(),
                        registros["DET_COMPRA_CANTIDAD"].ToString(), registros["DET_COMPRA_PR_UNIT"].ToString(), registros["total"].ToString());
                    }
                    else
                    if (ProdRev!=0) 
                    {

                        dataGridView1.Rows.Add(registros["MATERIAPR_DESCR"].ToString(), PR,
                        registros["RUBRO_NOMBRE"].ToString(), registros["MARCA_NOMBRE"].ToString(),
                        registros["DET_COMPRA_CANTIDAD"].ToString(), registros["DET_COMPRA_PR_UNIT"].ToString(), registros["total"].ToString());
                    }

                }
                registros.Close();
                conexion.Close();
        }

        private void saldoPendienteDePago()
        {
            int compraId = identificadorIdCompra();
            conexion.Open();
            string sql = "SELECT T1.totalcompra-T2.pagos as saldoapagar FROM (SELECT det.COMPRA_ID, SUM(DET_COMPRA_CANTIDAD*DET_COMPRA_PR_UNIT) as 'totalcompra'  FROM DETALLECOMPRA as det, COMPRA as comp WHERE det.COMPRA_ID=comp.COMPRA_ID and comp.COMPRA_ID=@compraid group by det.COMPRA_ID) T1 LEFT JOIN (SELECT compra.COMPRA_ID, coalesce(sum(PAGOCOMPRA.PAGO_COMPRA_MONTO), 0) as 'pagos'  FROM COMPRA left JOIN PAGOCOMPRA ON COMPRA.COMPRA_ID = PAGOCOMPRA.COMPRA_ID group by COMPRA.COMPRA_ID) T2 on (T1.COMPRA_ID=T2.COMPRA_ID)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@compraid", SqlDbType.Int).Value = compraId;
            SqlDataReader registros = comando.ExecuteReader();
            while (registros.Read())
            {
                label14.Text = registros["saldoapagar"].ToString();
            }
            registros.Close();
            conexion.Close();
        }


        private void selectCompra()
        {
            conexion.Open();
            string sql = "select PROVEE_ID,COMPRA_FECHA, EST_COMPRA_NOMBRE from COMPRA as comp, ESTADOCOMPRA as estcomp WHERE COMPRA_NUM_FACT=@numerofact and comp.EST_COMPRA_ID=estcomp.EST_COMPRA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@numerofact", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                comboBox1.SelectedValue = registros["PROVEE_ID"].ToString();
                dateTimePicker1.Value = Convert.ToDateTime(registros["COMPRA_FECHA"]);
                label6.Text = registros["EST_COMPRA_NOMBRE"].ToString();
            }
            registros.Close();
            conexion.Close();
        }

        private void button3_Click_1(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1"))
            {
                if (textBox1.Text == "")
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Debe cargar algún número de factura";
                    m.ShowDialog();
                }
                else if (ExistePago(textBox1.Text))
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No se puede eliminar la factura porque se ingresó un pago";
                    m.ShowDialog();
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("¿Confirma la eliminación?", "Eliminar Factura", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        eliminarDetalleFactura();
                        conexion.Open();
                        string sql = "delete from COMPRA where COMPRA_NUM_FACT=@numfact"; //and reg_clave=@clave and reg_repetirclave=@repetirclave
                        SqlCommand comando = new SqlCommand(sql, conexion);
                        comando.Parameters.Add("@numfact", SqlDbType.VarChar).Value = textBox1.Text;
                        int cant = comando.ExecuteNonQuery();
                        conexion.Close();
                        if (cant == 1)
                        {
                            textBox1.Text = "";
                            numericUpDown1.Enabled = true;
                            textBox1.Enabled = true;
                            textBox3.Enabled = true;
                            comboBox2.Enabled = true;
                            comboBox3.Enabled = true;
                            comboBox4.Enabled = true;
                            comboBox5.Enabled = true;
                            button6.Enabled = true;
                            button8.Enabled = true;
                            button9.Enabled = true;
                            button2.Enabled = true;
                            button4.Enabled = false;
                            dataGridView1.Rows.Clear();
                            label4.Text = "Total: $";
                            textBox1.Text = "";
                            comboBox1.SelectedIndex = 0;
                            dateTimePicker1.Value = DateTime.Now;
                            numericUpDown1.Value = 0;
                            textBox3.Text = "";
                            Aviso m = new Aviso();
                            m.label1.Text = "Se ha eliminado la factura";
                            m.ShowDialog();
                            dataGridView1.Rows.Clear();
                            mostrarTrasModifElim();
                        }
                        else
                        {
                            Aviso m = new Aviso();
                            m.label1.Text = "No existe una factura con ese número";
                            m.ShowDialog();
                        }
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Solo el Administrador puede eliminar una factura";
                m.ShowDialog();
            }
        }

        private void eliminarDetalleFactura()
        {
            if (ExisteNumFactura(textBox1.Text))
            {
                int compraId = identificadorIdCompra();
                conexion.Open();
                string sql = "delete from DETALLECOMPRA where COMPRA_ID=@compraid"; //and reg_clave=@clave and reg_repetirclave=@repetirclave
                SqlCommand comando = new SqlCommand(sql, conexion);
                comando.Parameters.Add("@compraid", SqlDbType.VarChar).Value = compraId;
                int cant = comando.ExecuteNonQuery();
                conexion.Close();
            }
        }


        public void calcularSubTotal()
        {
            Decimal suma = 0;
            Decimal cantidadSub, preciounit;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                cantidadSub = Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value);
                preciounit = Convert.ToDecimal(dataGridView1.Rows[i].Cells[5].Value);
                suma = preciounit * cantidadSub;

                dataGridView1.Rows[i].Cells[6].Value = suma.ToString();
            }
        }


        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (b == 1)
            {
                calcularSubTotal();
            }
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (b == 1)
            {
                calcularSubTotal();
                calcularTotal();
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (ExistePago(textBox1.Text))
            {
                Aviso m = new Aviso();
                m.label1.Text = "No se puede modificar la factura porque se ingresó un pago";
                m.ShowDialog();
            }
            else if (b == 1)
            {
                actualizarCompra();
                int compraId = identificadorIdCompra();
                try
                {
                    bool bandera = false;
                    foreach (DataGridViewRow row in dataGridView1.Rows)
                    {

                        String tipo = "";
                        tipo = (string)row.Cells[1].Value;
                        String Name = (string)row.Cells[0].Value;
                        for (int i = 0; i < dataGridView1.Rows.Count; ++i)
                        {
                            if (Convert.ToString(dataGridView1.Rows[i].Cells["Cantidad"].Value) == "0" || Convert.ToString(dataGridView1.Rows[i].Cells["Cantidad"].Value).Length == 0)
                            {
                                bandera = true;
                            }
                        }
                        if (bandera == false && tipo == "Materia Prima")
                        {
                            int materiaId = identificadorIdMateriaPrima(Name);
                            conexion.Open();
                            SqlCommand agregar = new SqlCommand("update DETALLECOMPRA set DET_COMPRA_CANTIDAD=@cantidadcompra, DET_COMPRA_PR_UNIT=@preciounitario where COMPRA_ID=@compraid and MATERIAPR_ID=@materiaid", conexion);
                            agregar.Parameters.Clear();
                            agregar.Parameters.AddWithValue("@cantidadcompra", Convert.ToString(row.Cells["Cantidad"].Value));
                            agregar.Parameters.AddWithValue("@preciounitario", Convert.ToString(row.Cells["PrecioUnitario"].Value));
                            agregar.Parameters.Add("@compraid", SqlDbType.Int).Value = compraId;
                            agregar.Parameters.Add("@materiaid", SqlDbType.Int).Value = materiaId;
                            agregar.ExecuteNonQuery();
                            conexion.Close();
                            quitarFiltrosModificar();
                            mostrarTrasModifElim();
                        }
                        else if (bandera == false && tipo == "Producto de Reventa")
                        {
                            int prodrevId = identificadorIdProductoReventa(Name);
                            conexion.Open();
                            SqlCommand agregar = new SqlCommand("update DETALLECOMPRA set DET_COMPRA_CANTIDAD=@cantidadcompra, DET_COMPRA_PR_UNIT=@preciounitario where COMPRA_ID=@compraid and PROD_REV_ID=@prodrevid", conexion);
                            agregar.Parameters.Clear();
                            agregar.Parameters.AddWithValue("@cantidadcompra", Convert.ToString(row.Cells["Cantidad"].Value));
                            agregar.Parameters.AddWithValue("@preciounitario", Convert.ToString(row.Cells["PrecioUnitario"].Value));
                            agregar.Parameters.Add("@compraid", SqlDbType.Int).Value = compraId;
                            agregar.Parameters.Add("@prodrevid", SqlDbType.Int).Value = prodrevId;
                            agregar.ExecuteNonQuery();
                            conexion.Close();
                            quitarFiltrosModificar();
                            mostrarTrasModifElim();
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

        private void mostrarTrasModifElim()
        {
            label5.Visible = true;
            label9.Visible = true;
            label10.Visible = true;
            label12.Visible = true;
            label7.Visible = true;
            label8.Visible = true;
            comboBox5.Visible = true;
            comboBox2.Visible = true;
            comboBox3.Visible = true;
            comboBox4.Visible = true;
            numericUpDown1.Visible = true;
            textBox3.Visible = true;
            button6.Visible = true;
            pictureBox6.Visible = true;
            pictureBox7.Visible = true;
            button8.Visible = true;
            button9.Visible = true;
            pictureBox1.Visible = true;
            button2.Visible = true;
        }

        private void quitarFiltrosModificar()
        {
            Aviso m = new Aviso();
            m.label1.Text = "Se ha modificado la compra";
            m.ShowDialog();
            numericUpDown1.Enabled = true;
            textBox1.Enabled = true;
            textBox3.Enabled = true;
            comboBox2.Enabled = true;
            comboBox3.Enabled = true;
            comboBox4.Enabled = true;
            comboBox5.Enabled = true;
            button6.Enabled = true;
            button8.Enabled = true;
            button9.Enabled = true;
            button2.Enabled = true;
            button4.Enabled = false;
            dataGridView1.Rows.Clear();
            label4.Text = "Total: $";
            textBox1.Text = "";
            comboBox1.SelectedIndex = 0;
            dateTimePicker1.Value = DateTime.Now;
            numericUpDown1.Value = 0;
            textBox3.Text = "";
        }
        private bool ExistePago(string numerofactura)
        {
            conexion.Open();
            string sql = "select COMPRA_NUM_FACT from COMPRA as comp, PAGOCOMPRA as pag where COMPRA_NUM_FACT=@numerofact and comp.COMPRA_ID=pag.COMPRA_ID";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@numerofact", SqlDbType.VarChar).Value = numerofactura;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void comboBox4_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (char.IsNumber(e.KeyChar) || e.KeyChar == ',' || e.KeyChar == Convert.ToChar(Keys.Back))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        private void comboBox3_SelectedValueChanged(object sender, EventArgs e)
        {
            cargarComboBox4();
        }

        private void comboBox2_SelectedValueChanged(object sender, EventArgs e)
        {
            cargarComboBox4();
        }

        private void dataGridView1_Click(object sender, EventArgs e)
        {
            if (b == 1)
            {
                calcularSubTotal();
                calcularTotal();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (b == 1)
            {
                calcularSubTotal();
                calcularTotal();
            }
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }
    }
}
