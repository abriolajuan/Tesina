using GestionDeUsuarios.Ventas;
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
    public partial class Clientes : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        string tipoActivo;
        string IdUsu;
        int bclientes;
        String numeroCelu;
        public Clientes(string tipoActivo2, string usuIdActivo, int aclientes)
        {
            InitializeComponent();
            tipoActivo = tipoActivo2;
            IdUsu = usuIdActivo;
            bclientes = aclientes;

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

        private void Clientes_Load(object sender, EventArgs e)
        {
            cargarComboBox1();
            MostrarGrilla();            
            ocultarModificar();
            label10.Visible = false;
            textBox7.Visible = false;
            comboBox1.SelectedValue = -1;
            comboBox2.SelectedValue = -1;
            comboBox3.SelectedValue = -1;
            checkBox1.Checked = true;
            this.toolTip1.SetToolTip(this.button2, "Para consultar es necesario al menos: \n -Ingresar Teléfono (N°). \n o \n -Ingresar nombre y apellido (*No se habilita el botón modificar hasta consultar por teléfono).");
        }

        private void ocultarModificar()
        {
            button3.Enabled = false;
            button3.Visible = false;
            pictureBox2.Visible = false;

            button4.Visible = true;
            pictureBox1.Visible = true;
        }

        private void mostrarModificar()
        {
            button3.Enabled = true;
            button3.Visible = true;
            pictureBox2.Visible = true;

            button4.Visible = false;
            pictureBox1.Visible = false;
        }

        public Boolean largoAdecuadoTelefono(string telefonoDeseado)
        {
            if (telefonoDeseado.Length == 10)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void button4_Click(object sender, EventArgs e) // GRABAR CLIENTE
        {
            
            if (textBox2.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un teléfono de cliente";
                m.ShowDialog();
            }
            else if (!largoAdecuadoTelefono(textBox2.Text))
            {
                Aviso m = new Aviso();
                m.label1.Text = "El telefono debe contener 10 caracteres";
                m.ShowDialog();
            }
            else if (textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un nombre de cliente";
                m.ShowDialog();
            }
            else if (textBox6.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un apellido de cliente";
                m.ShowDialog();
            }
            else if (textBox4.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un nombre de calle";
                m.ShowDialog();
            }
            else if (textBox5.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un número de calle";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar una provincia";
                m.ShowDialog();
            }
            else if (comboBox2.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar una localidad";
                m.ShowDialog();
            }
            else if (comboBox3.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un barrio";
                m.ShowDialog();
            }
            else if (!ExisteCliente())
            {
                guardarDomicilioCliente();
                guardarCliente();
                comboBox1.SelectedValue = -1;
                comboBox2.SelectedValue = -1;
                comboBox3.SelectedValue = -1;
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Ya existe un cliente con ese teléfono";
                m.ShowDialog();
            }
        }

        private void button2_Click_1(object sender, EventArgs e) // CONSULTAR CLIENTE
        {
            if (textBox2.Text == "" && textBox1.Text == "" && textBox6.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un teléfono o un nombre y apellido para buscar un cliente";
                m.ShowDialog();
            }
            else if (textBox2.Text != "" && textBox1.Text=="" && textBox6.Text=="")
            {
                if (!ExisteCliente())
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No existe un cliente con ese teléfono";
                    m.ShowDialog();
                }
                else
                {
                    buscarPorTelefono();
                    button2.Enabled = false;
                    comboBox1.SelectedValue = -1;
                    comboBox2.SelectedValue = -1;
                    comboBox3.SelectedValue = -1;
                }
            }
            else if (textBox1.Text != "" && textBox6.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un apellido";
                m.ShowDialog();
            }
            else if (textBox1.Text == "" && textBox6.Text != "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Debe cargar un nombre";
                m.ShowDialog();
            }
            else if (textBox2.Text == "" && textBox1.Text != "" && textBox6.Text != "")
            {
                if (!ExisteClienteNombreYApellido())
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No existe un cliente con ese nombre y apellido";
                    m.ShowDialog();
                }
                else
                {
                    buscarPorNombreYApellido();
                }
            }
            else if (textBox1.Text != "" && textBox2.Text != "" && textBox6.Text != "")
            {
                if (!ExisteClienteNombreApellidoYTel())
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "No existe un cliente con ese teléfono, nombre y apellido";
                    m.ShowDialog();
                }
                else
                {
                    buscarPorTelefonoNombreYApellido();
                }
            }
        }


        private void buscarPorNombreYApellido ()
        {
            conexion.Open();
            string sql = "select CLIENTE_ID, CLIENTE_APELLIDO, CLIENTE_NOMBRE,CLIENTE_TEL,CLIENTE_CUIT, PROVINCIA_NOMBRE, LOCALIDAD_NOMBRE,BARRIO_NOMBRE, provinc.PROVINCIA_ID, loc.LOCALIDAD_ID, dom.BARRIO_ID, DOMIC_CALLE_NOMBRE, DOMIC_CALLE_ALTURA from CLIENTE as cl join DOMICILIO as dom on dom.DOMIC_ID = cl.DOMIC_ID join BARRIO as bar on bar.BARRIO_ID = dom.BARRIO_ID join LOCALIDAD as loc on loc.LOCALIDAD_ID = bar.LOCALIDAD_ID join PROVINCIA as provinc on provinc.PROVINCIA_ID= loc.PROVINCIA_ID WHERE CLIENTE_APELLIDO=@APELLIDO AND CLIENTE_NOMBRE=@NOMBRE ORDER BY CLIENTE_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@APELLIDO", SqlDbType.VarChar).Value = textBox6.Text;
            comando.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["CLIENTE_ID"].ToString(),
                    registros["CLIENTE_NOMBRE"].ToString(),
                registros["CLIENTE_APELLIDO"].ToString(),
                registros["CLIENTE_TEL"].ToString(), registros["CLIENTE_CUIT"].ToString(),
                registros["PROVINCIA_NOMBRE"].ToString(), registros["LOCALIDAD_NOMBRE"].ToString(),
                registros["BARRIO_NOMBRE"].ToString(), registros["DOMIC_CALLE_NOMBRE"].ToString(),
                registros["DOMIC_CALLE_ALTURA"].ToString());

            }
            registros.Close();
            conexion.Close();
        }

        private void buscarPorTelefonoNombreYApellido()
        {
            conexion.Open();
            string sql = "select CLIENTE_ID, CLIENTE_APELLIDO, CLIENTE_NOMBRE,CLIENTE_TEL,CLIENTE_CUIT, PROVINCIA_NOMBRE, LOCALIDAD_NOMBRE,BARRIO_NOMBRE, provinc.PROVINCIA_ID, loc.LOCALIDAD_ID, dom.BARRIO_ID, DOMIC_CALLE_NOMBRE, DOMIC_CALLE_ALTURA from CLIENTE as cl join DOMICILIO as dom on dom.DOMIC_ID = cl.DOMIC_ID join BARRIO as bar on bar.BARRIO_ID = dom.BARRIO_ID join LOCALIDAD as loc on loc.LOCALIDAD_ID = bar.LOCALIDAD_ID join PROVINCIA as provinc on provinc.PROVINCIA_ID= loc.PROVINCIA_ID WHERE CLIENTE_APELLIDO=@APELLIDO AND CLIENTE_NOMBRE=@NOMBRE AND CLIENTE_TEL=@TELEFONO ORDER BY CLIENTE_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@APELLIDO", SqlDbType.VarChar).Value = textBox6.Text;
            comando.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = textBox1.Text;
            comando.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = textBox2.Text;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["CLIENTE_ID"].ToString(),
                    registros["CLIENTE_NOMBRE"].ToString(),
                registros["CLIENTE_APELLIDO"].ToString(),
                registros["CLIENTE_TEL"].ToString(), registros["CLIENTE_CUIT"].ToString(),
                registros["PROVINCIA_NOMBRE"].ToString(), registros["LOCALIDAD_NOMBRE"].ToString(),
                registros["BARRIO_NOMBRE"].ToString(), registros["DOMIC_CALLE_NOMBRE"].ToString(),
                registros["DOMIC_CALLE_ALTURA"].ToString());


                textBox7.Text = registros["CLIENTE_ID"].ToString();
                textBox1.Text = registros["CLIENTE_NOMBRE"].ToString();
                textBox6.Text = registros["CLIENTE_APELLIDO"].ToString();
                textBox2.Text = registros["CLIENTE_TEL"].ToString();
                textBox3.Text = registros["CLIENTE_CUIT"].ToString();
                textBox4.Text = registros["DOMIC_CALLE_NOMBRE"].ToString();
                textBox5.Text = registros["DOMIC_CALLE_ALTURA"].ToString();

            }
            mostrarModificar();
            button4.Enabled = false;
            button2.Enabled = false;
            registros.Close();
            conexion.Close();
        }
        private void button3_Click(object sender, EventArgs e) // MODIFICAR CLIENTE
        {
            
            if (textBox2.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para modificar un cliente tiene que estar cargado el teléfono";
                m.ShowDialog();
            }
            else if (!largoAdecuadoTelefono(textBox2.Text))
            {
                Aviso m = new Aviso();
                m.label1.Text = "El telefono debe contener 10 caracteres";
                m.ShowDialog();
            }
            else if (textBox1.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para modificar un cliente tiene que estar cargado el nombre";
                m.ShowDialog();
            }
            else if (textBox6.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para modificar un cliente tiene que estar cargado el apellido";
                m.ShowDialog();
            }
            else if (textBox4.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para modificar un cliente tiene que estar cargado el nombre de la calle";
                m.ShowDialog();
            }
            else if (textBox5.Text == "")
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para modificar un cliente tiene que estar cargado el número de la calle";
                m.ShowDialog();
            }
            else if (comboBox1.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para modificar un cliente debe cargar una provincia";
                m.ShowDialog();
            }
            else if (comboBox2.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para modificar un cliente debe cargar una localidad";
                m.ShowDialog();
            }
            else if (comboBox3.SelectedIndex == -1)
            {
                Aviso m = new Aviso();
                m.label1.Text = "Para modificar un cliente debe cargar un barrio";
                m.ShowDialog();
            }
            else
            {
                if (!ExisteCliente())
                {
                    button4.Enabled = true;
                    ocultarModificar();
                    button2.Enabled = true;
                    modificarDomicilioCliente();
                    modificarCliente();
                    comboBox1.SelectedValue = -1;
                    comboBox2.SelectedValue = -1;
                    comboBox3.SelectedValue = -1;
                }
                else
                {
                    Aviso m = new Aviso();
                    m.label1.Text = "Ya existe un cliente con el teléfono ingresado";
                    m.ShowDialog();
                }
            }
        }

        private void cancelar_Click_1(object sender, EventArgs e)
        {
            Close();
        }


        private bool ExisteCliente()
        {
            conexion.Open();
            string sql = "select CLIENTE_TEL from CLIENTE where CLIENTE_TEL=@TELEFONO and CLIENTE_ID!=@id";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = textBox2.Text;
            comando.Parameters.Add("@id", SqlDbType.VarChar).Value = textBox7.Text;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }
        private bool ExisteClienteNombreYApellido()
        {
            conexion.Open();
            string sql = "select CLIENTE_APELLIDO, CLIENTE_NOMBRE from CLIENTE where CLIENTE_NOMBRE=@NOMBRE AND CLIENTE_APELLIDO=@APELLIDO";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@APELLIDO", SqlDbType.VarChar).Value = textBox6.Text;
            comando.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = textBox1.Text;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }

        private bool ExisteClienteNombreApellidoYTel()
        {
            conexion.Open();
            string sql = "select CLIENTE_APELLIDO, CLIENTE_NOMBRE, CLIENTE_TEL from CLIENTE where CLIENTE_NOMBRE=@NOMBRE AND CLIENTE_APELLIDO=@APELLIDO AND CLIENTE_TEL=@TELEFONO";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@APELLIDO", SqlDbType.VarChar).Value = textBox6.Text;
            comando.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = textBox1.Text;
            comando.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = textBox2.Text;
            SqlDataReader registro = comando.ExecuteReader();
            bool existe = false;
            if (registro.Read())
                existe = true;
            registro.Close();
            conexion.Close();
            return existe;
        }


        private void guardarDomicilioCliente()
        {
            conexion.Open();
            string sql = "insert into DOMICILIO (BARRIO_ID, DOMIC_CALLE_NOMBRE , DOMIC_CALLE_ALTURA) values (@barrioid,@domcalle,@domaltura)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@domcalle", SqlDbType.VarChar).Value = textBox4.Text;
            comando.Parameters.Add("@barrioid", SqlDbType.Int).Value = comboBox3.SelectedValue.ToString();
            comando.Parameters.Add("@domaltura", SqlDbType.Int).Value = textBox5.Text;
            comando.ExecuteNonQuery();
            conexion.Close();
            MostrarGrilla();
        }

        public int identificadorDomicilio()
        {
            int idDomicilio;

            conexion.Open();
            string sql = "select DOMIC_ID from DOMICILIO where domic_calle_nombre=@domicnombre and domic_calle_altura=@domicaltura";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@domicnombre", SqlDbType.VarChar).Value = textBox4.Text;
            comando.Parameters.Add("@domicaltura", SqlDbType.Int).Value = textBox5.Text;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idDomic = registro["DOMIC_ID"].ToString();
            idDomicilio = int.Parse(idDomic);
            conexion.Close();
            return idDomicilio;
        }


        private void guardarCliente()
        {
            int domId = identificadorDomicilio();
            conexion.Open();
            string sql = "insert into CLIENTE (DOMIC_ID, CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_TEL , CLIENTE_CUIT) values (@domid,@clientenom,@clienteape,@clientetel,@clientecuit)";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@domid", SqlDbType.Int).Value = domId;
            comando.Parameters.Add("@clientenom", SqlDbType.VarChar).Value = textBox1.Text;
            comando.Parameters.Add("@clienteape", SqlDbType.VarChar).Value = textBox6.Text;
            comando.Parameters.Add("@clientetel", SqlDbType.VarChar).Value = textBox2.Text;
            comando.Parameters.Add("@clientecuit", SqlDbType.VarChar).Value = textBox3.Text;
            comando.ExecuteNonQuery();
            conexion.Close();
            Aviso m = new Aviso();
            m.label1.Text = "El cliente fue registrado";
            m.ShowDialog();
            if (checkBox1.Checked)
                whatsappGuardar(); //WHATSAPP
            MostrarGrilla();
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        private void whatsappGuardar() //WHATSAPP
        {
            String txtEncabezado = "https://web.whatsapp.com/send?phone=54";
            String txtCelular = textBox2.Text;
            String txtConector = "&text=";
            String txtMensaje = "%C2%A1Hola!%20Hemos%20registrado%20tus%20datos%20y%20celular%20correctamente%20%F0%9F%93%B1%20%C2%A1Muchas%20gracias!%20%F0%9F%98%83%20MTA25%20%F0%9F%A5%AA";
            System.Diagnostics.Process.Start(txtEncabezado + txtCelular + txtConector + txtMensaje);
        }


        private void cargarComboBox1()
        {
            conexion.Open();
            string sql = "select PROVINCIA_ID, PROVINCIA_NOMBRE from PROVINCIA ORDER BY PROVINCIA_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox1.DisplayMember = "PROVINCIA_NOMBRE";
            comboBox1.ValueMember = "PROVINCIA_ID";
            comboBox1.DataSource = tabla1;
        }


        private void cargarComboBox2()
        {
            conexion.Open();
            string sql = "select LOCALIDAD_ID, LOCALIDAD_NOMBRE from LOCALIDAD where PROVINCIA_ID=@provinciaid ORDER BY LOCALIDAD_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@provinciaid", SqlDbType.Int).Value = comboBox1.SelectedValue;
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox2.DisplayMember = "LOCALIDAD_NOMBRE";
            comboBox2.ValueMember = "LOCALIDAD_ID";
            comboBox2.DataSource = tabla1;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedIndex != -1)
            {
                cargarComboBox2();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox2.SelectedIndex != -1)
            {
                cargarComboBox3();
            }
        }

        private void cargarComboBox3()
        {
            conexion.Open();
            string sql = "select BARRIO_ID, BARRIO_NOMBRE from BARRIO where LOCALIDAD_ID=@localidadid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@localidadid", SqlDbType.Int).Value = comboBox2.SelectedValue;
            SqlDataAdapter adaptador1 = new SqlDataAdapter();
            adaptador1.SelectCommand = comando;
            DataTable tabla1 = new DataTable();
            adaptador1.Fill(tabla1);
            conexion.Close();
            comboBox3.DisplayMember = "BARRIO_NOMBRE";
            comboBox3.ValueMember = "BARRIO_ID";
            comboBox3.DataSource = tabla1;
        }

        private void MostrarGrilla()
        {
            conexion.Open();
            string sql = "select CLIENTE_ID, CLIENTE_APELLIDO, CLIENTE_NOMBRE,CLIENTE_TEL,CLIENTE_CUIT, PROVINCIA_NOMBRE, LOCALIDAD_NOMBRE,BARRIO_NOMBRE, DOMIC_CALLE_NOMBRE, DOMIC_CALLE_ALTURA from CLIENTE as cl join DOMICILIO as dom on dom.DOMIC_ID = cl.DOMIC_ID join BARRIO as bar on bar.BARRIO_ID = dom.BARRIO_ID  join LOCALIDAD as loc on loc.LOCALIDAD_ID = bar.LOCALIDAD_ID join PROVINCIA as provinc on provinc.PROVINCIA_ID= loc.PROVINCIA_ID ORDER BY CLIENTE_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["CLIENTE_ID"].ToString(),
                    registros["CLIENTE_NOMBRE"].ToString(),
                 registros["CLIENTE_APELLIDO"].ToString(),
                 registros["CLIENTE_TEL"].ToString(), registros["CLIENTE_CUIT"].ToString(),
                 registros["PROVINCIA_NOMBRE"].ToString(), registros["LOCALIDAD_NOMBRE"].ToString(),
                 registros["BARRIO_NOMBRE"].ToString(), registros["DOMIC_CALLE_NOMBRE"].ToString(),
                 registros["DOMIC_CALLE_ALTURA"].ToString());
            }
            registros.Close();
            conexion.Close();
        }


        string cant;
        private void buscarPorTelefono()
        {
            conexion.Open();
            string sql = "select CLIENTE_ID, CLIENTE_APELLIDO, CLIENTE_NOMBRE,CLIENTE_TEL,CLIENTE_CUIT, PROVINCIA_NOMBRE, LOCALIDAD_NOMBRE,BARRIO_NOMBRE, provinc.PROVINCIA_ID, loc.LOCALIDAD_ID, dom.BARRIO_ID, DOMIC_CALLE_NOMBRE, DOMIC_CALLE_ALTURA from CLIENTE as cl join DOMICILIO as dom on dom.DOMIC_ID = cl.DOMIC_ID join BARRIO as bar on bar.BARRIO_ID = dom.BARRIO_ID join LOCALIDAD as loc on loc.LOCALIDAD_ID = bar.LOCALIDAD_ID join PROVINCIA as provinc on provinc.PROVINCIA_ID= loc.PROVINCIA_ID WHERE CLIENTE_TEL=@TELEFONO ORDER BY CLIENTE_NOMBRE ASC";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = textBox2.Text;
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["CLIENTE_ID"].ToString(),
                    registros["CLIENTE_NOMBRE"].ToString(),
                registros["CLIENTE_APELLIDO"].ToString(),
                registros["CLIENTE_TEL"].ToString(), registros["CLIENTE_CUIT"].ToString(),
                registros["PROVINCIA_NOMBRE"].ToString(), registros["LOCALIDAD_NOMBRE"].ToString(),
                registros["BARRIO_NOMBRE"].ToString(), registros["DOMIC_CALLE_NOMBRE"].ToString(),
                registros["DOMIC_CALLE_ALTURA"].ToString());

                textBox7.Text = registros["CLIENTE_ID"].ToString();
                textBox1.Text = registros["CLIENTE_NOMBRE"].ToString();
                textBox6.Text = registros["CLIENTE_APELLIDO"].ToString();
                textBox2.Text = registros["CLIENTE_TEL"].ToString();
                textBox3.Text = registros["CLIENTE_CUIT"].ToString();
                textBox4.Text = registros["DOMIC_CALLE_NOMBRE"].ToString();
                textBox5.Text = registros["DOMIC_CALLE_ALTURA"].ToString(); 
                //comboBox1.SelectedValue = registros["PROVINCIA_ID"].ToString();
                //comboBox2.SelectedValue = registros["LOCALIDAD_ID"].ToString();
                //comboBox3.SelectedValue = registros["BARRIO_ID"].ToString();
            }
            /*if ((cant == null) || (cant.Equals(String.Empty)))
            {
                Aviso m = new Aviso();
                m.label1.Text = "No existe un cliente con ese teléfono";
                m.ShowDialog();
            }
            else
            {*/
            mostrarModificar();
            button4.Enabled = false;
            button2.Enabled = false;
            //  }
            registros.Close();
            conexion.Close();
        }


        private void modificarDomicilioCliente()
        {
            int domId = identificadorDomicilioModificar();
            conexion.Open();
            string sql = "update DOMICILIO set DOMIC_CALLE_NOMBRE=@domcalle, DOMIC_CALLE_ALTURA=@domaltura, BARRIO_ID=@barrioid  where DOMIC_ID=@domid";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@domcalle", SqlDbType.VarChar).Value = textBox4.Text;
            comando.Parameters.Add("@barrioid", SqlDbType.Int).Value = comboBox3.SelectedValue.ToString();
            comando.Parameters.Add("@domaltura", SqlDbType.Int).Value = textBox5.Text;
            comando.Parameters.Add("@domid", SqlDbType.Int).Value = domId;
            comando.ExecuteNonQuery();
            conexion.Close();
        }

        private void modificarCliente()
        {
            int domId = identificadorDomicilio();
            conexion.Open();
            string sql = "update CLIENTE set CLIENTE_CUIT= @cuit, CLIENTE_NOMBRE=@NOMBRE, CLIENTE_APELLIDO=@APELLIDO, CLIENTE_TEL=@TELEFONO where CLIENTE_ID=@id";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@id", SqlDbType.Int).Value = textBox7.Text;
            comando.Parameters.Add("@cuit", SqlDbType.VarChar).Value = textBox3.Text;
            comando.Parameters.Add("@NOMBRE", SqlDbType.VarChar).Value = textBox1.Text;
            comando.Parameters.Add("@APELLIDO", SqlDbType.VarChar).Value = textBox6.Text;
            comando.Parameters.Add("@TELEFONO", SqlDbType.VarChar).Value = textBox2.Text;
            comando.ExecuteNonQuery();
            conexion.Close();
            Aviso m = new Aviso();
            m.label1.Text = "El cliente se modificó correctamente";
            m.ShowDialog();
            if (checkBox1.Checked)
                whatsappModificar(); //WHATSAPP
            MostrarGrilla();
            button2.Enabled = true;
            textBox2.Enabled = true;
            textBox1.Enabled = true;
            textBox6.Enabled = true;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }

        private void whatsappModificar() //WHATSAPP
        {
            String txtEncabezado = "https://web.whatsapp.com/send?phone=54";
            String txtCelular = textBox2.Text;
            String txtConector = "&text=";
            String txtMensaje = "%C2%A1Hola!%20Hemos%20actualizado%20tus%20datos%20correctamente%20%F0%9F%98%83%20%C2%A1Muchas%20gracias!%20MTA25%20%F0%9F%A5%AA";
            System.Diagnostics.Process.Start(txtEncabezado + txtCelular + txtConector + txtMensaje);
        }

        public int identificadorDomicilioModificar()
        {
            int idDomicilio;
            conexion.Open();
            string sql = "select DOMIC_ID from CLIENTE where CLIENTE_ID=@id";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@id", SqlDbType.Int).Value = textBox7.Text;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idDomic = registro["DOMIC_ID"].ToString();
            idDomicilio = int.Parse(idDomic);
            conexion.Close();
            return idDomicilio;
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox6_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox4_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)) // Si presiona un numero
            {
                e.Handled = true; // No acepta numeros
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void textBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void textBox5_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsLetter(e.KeyChar)) //Al pulsar una letra
            {
                e.Handled = true; //No se acepta letras 
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (bclientes == 0)
            {
                try
                {
                    numeroCelu = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    Venta v = new Venta(tipoActivo, IdUsu, numeroCelu);
                    v.textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    v.textBox3.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); // LE PASO EL ID
                    v.textBox5.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    AbrirpanelHijo((v));
                    v.radioButton2.Checked = true;
                    v.cargarComboBox1Cliente();
                }
                catch
                {

                }
            }
            else if (bclientes == 1)
            {
                try
                {
                    numeroCelu = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    CobroEfectivo ef = new CobroEfectivo(numeroCelu);
                    ef.textBox3.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    ef.textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); // LE PASO EL ID
                    ef.textBox5.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    AbrirpanelHijo((ef));
                }
                catch
                {

                }
            }
            else if (bclientes == 2)
            {
                try
                {
                    numeroCelu = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    CobroTarjetas t = new CobroTarjetas(numeroCelu);
                    t.textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    t.textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); // LE PASO EL ID
                    t.textBox8.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    AbrirpanelHijo((t));
                    t.radioButton3.Checked = true;
                }
                catch
                {

                }
            }
            else if (bclientes == 3)
            {
                try
                {
                    numeroCelu = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    CobroTransferencia t = new CobroTransferencia(numeroCelu);
                    t.textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    t.textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); // LE PASO EL ID
                    t.textBox6.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    AbrirpanelHijo((t));
                    t.radioButton3.Checked = true;
                }
                catch
                {

                }
            }
            else if (bclientes == 4)
            {
                try
                {

                    CobroCheque t = new CobroCheque();
                    t.textBox1.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    t.textBox2.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); // LE PASO EL ID
                    AbrirpanelHijo((t));
                    t.radioButton3.Checked = true;
                }
                catch
                {

                }
            }
            else if (bclientes == 5)
            {
                try
                {
                    numeroCelu = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    CuentaCliente c = new CuentaCliente(numeroCelu);
                    c.textBox3.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    c.textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); // LE PASO EL ID
                    c.textBox5.Text = dataGridView1.CurrentRow.Cells[3].Value.ToString();
                    AbrirpanelHijo((c));
                }
                catch
                {

                }
            }
            else if (bclientes == 6)
            {
                try
                {
                    AnalisisVentas av = new AnalisisVentas();
                    av.textBox3.Text = dataGridView1.CurrentRow.Cells[1].Value.ToString() + " " + dataGridView1.CurrentRow.Cells[2].Value.ToString();
                    av.textBox1.Text = dataGridView1.CurrentRow.Cells[0].Value.ToString(); // LE PASO EL ID
                    AbrirpanelHijo((av));
                }
                catch
                {

                }
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cargarComboBox1();
            MostrarGrilla();
            button4.Enabled = true;
            button2.Enabled = true;
            ocultarModificar();
            label10.Visible = false;
            textBox7.Visible = false;
            comboBox1.SelectedValue = -1;
            comboBox2.SelectedValue = -1;
            comboBox3.SelectedValue = -1;
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            textBox4.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
        }
    }
}