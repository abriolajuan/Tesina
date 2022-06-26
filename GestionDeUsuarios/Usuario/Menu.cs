using GestionDeUsuarios.Ventas;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionDeUsuarios
{
    //1 Administrador
    //2 General
    //3 Vendedor
    //4 Cocinero
    //5 Compras
    public partial class Menu : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);

        string tipoActivo;
        string dniActivo;
        string nombreActivo2;
        string apellidoActivo2;
        string IdUsu;
        string Usuario;

        [DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
        private static extern IntPtr CreateRoundRectRgn
(
  int nLeftRect, // x-coordinate of upper-left corner
  int nTopRect, // y-coordinate of upper-left corner
  int nRightRect, // x-coordinate of lower-right corner
  int nBottomRect, // y-coordinate of lower-right corner
  int nWidthEllipse, // height of ellipse
  int nHeightEllipse // width of ellipse
);



        public Menu()
        {
            InitializeComponent();
            personalizarDiseño();
            this.FormBorderStyle = FormBorderStyle.None;
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }   

        private void PantallaAdministrador_Load(object sender, EventArgs e)
        {            
            button7.Text = "          Usuario activo: \n          " + Usuario;
            grabarIngresoSesion();            
        }

        public void fijarUsuario(string usuarioActivo)
        {
            Usuario = usuarioActivo;
        }

        public void fijarTipo(string tipo)
        {
            tipoActivo = tipo;
        }

        public void fijarDni(string dni)
        {
            dniActivo = dni;
        }

        public void fijarNombre(string nombreActivo)
        {
            nombreActivo2 = nombreActivo;
        }

        public void fijarApellido(string apellidoActivo)
        {
            apellidoActivo2 = apellidoActivo;
        }

        public void fijarIdUsu (string UsuIdActivo)
        {
            IdUsu = UsuIdActivo;
        }

        private void administrarUsuariosToolStripMenuItem1_Click(object sender, EventArgs e)
        {            
            if (tipoActivo.Equals("1"))
            {
                AbrirpanelHijo(new AdministrarUsuarios());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void cerrarSesiónToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Close();
        }


        private void cambiarMiContraseñaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RecupClave m = new RecupClave();
            m.ShowDialog();
        }

        private void acercaDeSandSysToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "Software 'SandSys'.\n" +
                "Versión: Beta (demo).\n" +
                "Año: 2021.\n\n" +
                "© Todos los derechos reservados.";
            m.ShowDialog();
        }

        private void soporteTécnicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "Para consultas generales, por favor contacte a:\n" +
                "soportesandsys@gmail.com\n\n" +
                "Para soporte técnico, por favor contacte a:\n" +
                "+54 9 3512966008 / +54 9 3512536792 / +54 9 3516361160";
            m.ShowDialog();
        }

        private void comprasToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }

        private void insumosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                Materias m = new Materias();
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void proveedoresToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                Proveedores m = new Proveedores();
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void pedidosAProveedoresToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                Compras m = new Compras(tipoActivo);
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void reportesEstadísticosToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                ReportesCompras m = new ReportesCompras();
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void elaboraciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("4"))
            {
                Elaboracion m = new Elaboracion(tipoActivo,nombreActivo2,apellidoActivo2);
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void reportesEstadísticosToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("4"))
            {
                ReportesElab m = new ReportesElab(tipoActivo, nombreActivo2, apellidoActivo2);
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        int aclientes = 0;
        private void productosALaVentaToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("3"))
            {
                Clientes m = new Clientes(tipoActivo,IdUsu,aclientes);
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void clientesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            validarUnoDosTres();
        }
        public void validarUnoDosTres() 
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("3"))
            {
                AbrirpanelHijo(new Clientes(tipoActivo, IdUsu,aclientes));
                ocultarSubMenu();

                //Clientes m = new Clientes();
                //m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }
        string numeroCelu = "";
        private void pedidosDeClientesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("3"))
            {
                Venta m = new Venta(tipoActivo, IdUsu, numeroCelu);
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void reportesEstadísticosToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("3"))
            {
                ReportesVentas m = new ReportesVentas();
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void personalizarDiseño()
        {
            panelVentas.Visible = false;
            panelElaboracion.Visible = false;
            panelCompras.Visible = false;
            panelSoporte.Visible = false;
            panelAyuda.Visible = false;
            panelUsuario.Visible = false;
        }

        private void ocultarSubMenu()
        {
            if (panelVentas.Visible == true)
                panelVentas.Visible = false;
            if (panelElaboracion.Visible == true)
                panelElaboracion.Visible = false;
            if (panelCompras.Visible == true)
                panelCompras.Visible = false;
            if (panelSoporte.Visible == true)
                panelSoporte.Visible = false;
            if (panelAyuda.Visible == true)
                panelAyuda.Visible = false;
            if (panelUsuario.Visible == true)
                panelUsuario.Visible = false;
        }

        private void mostrarSubMenu(Panel subMenu)
        {
            if (subMenu.Visible == false)
            {
                ocultarSubMenu();
                subMenu.Visible = true;
            }
            else
            {
                subMenu.Visible = false;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            mostrarSubMenu(panelVentas);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("3"))
            {
                AbrirpanelHijo(new MenuCobro());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            validarUnoDosTres();
        }
        private void button4_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("3"))
            {
                AbrirpanelHijo (new Venta(tipoActivo, IdUsu, numeroCelu));
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
         if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("3"))
            {
                AbrirpanelHijo(new ReportesVentas());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            mostrarSubMenu(panelElaboracion);
        }

        private void button10_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("4"))
            {
                AbrirpanelHijo(new Elaboracion(tipoActivo,nombreActivo2,apellidoActivo2));
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button9_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("4"))
            {
                AbrirpanelHijo(new Producción.ProdElab());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            mostrarSubMenu(panelCompras);
        }

        private void button17_Click_1(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                AbrirpanelHijo(new MenuReportesCompras());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                AbrirpanelHijo(new Materias());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                AbrirpanelHijo(new Compras(tipoActivo));
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                AbrirpanelHijo(new Proveedores());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                AbrirpanelHijo(new ProdReventaMain());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
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
            panelHijo.Controls.Add(formularioHijo);
            panelHijo.Tag = formularioHijo;
            formularioHijo.BringToFront(); // ESTO POR SI PONEMOS UN LOGO DE FONDO ADELANTE VA EL FORMULARIO
            formularioHijo.Show();
        }


        private void button16_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new RecupClave());
            ocultarSubMenu();
        }

        private void button7_Click_1(object sender, EventArgs e)
        {
            mostrarSubMenu(panelUsuario);
        }


        private void button8_Click_1(object sender, EventArgs e)
        {

           /* AvisoYesNo m = new AvisoYesNo();
            m.label1.Text = "¿Está seguro que desea cerrar sesión?";
            m.ShowDialog();
           */
             DialogResult dialogResult = MessageBox.Show("¿Está seguro que desea cerrar sesión?", "Cerrar sesión", MessageBoxButtons.YesNo);
             if (dialogResult == DialogResult.Yes)
             {
                 grabarSalidaSesion();
                 Close();
             }
             else if (dialogResult == DialogResult.No)
             {
                 return;
             }
        }

        private void button16_Click_1(object sender, EventArgs e)
        {
            AbrirpanelHijo(new RecupClave());
            ocultarSubMenu();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void rubrosToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1"))
            {
                AbrirpanelHijo(new Rubros());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void entidadesCrediticiasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1"))
            {  
                AbrirpanelHijo(new EntidadesCred());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }            
        }

        private void marcasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1"))
            {
                AbrirpanelHijo(new Marcas());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }            
        }

        private void mediosDeTransacciónToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1"))
            {
                AbrirpanelHijo(new MediosDeTrans());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }            
        }

        private void ubicacionesGeográficasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1"))
            {
                AbrirpanelHijo(new UbicGeogr());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void consultarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1"))
            {
                AbrirpanelHijo(new Sesiones());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }


        public int identificarIdDeDni()
        {
            int idUser;

            conexion.Open();
            string sql = "select USUARIO_ID from USUARIO where USUARIO_DNI=@usuariodni";
            SqlCommand comando = new SqlCommand(sql, conexion);
            comando.Parameters.Add("@usuariodni", SqlDbType.VarChar).Value = dniActivo;
            SqlDataReader registro = comando.ExecuteReader();
            registro.Read();
            string idUsu = registro["USUARIO_ID"].ToString();
            idUser = int.Parse(idUsu);
            conexion.Close();
            return idUser;
        }

        private void grabarIngresoSesion()
        { 
            DateTime fecha = DateTime.Now;

            int valorId = identificarIdDeDni();

            conexion.Open();
            string sql2 = "insert into SESION (USUARIO_ID, SESION_FECHA, SESION_EVENTO) values (@usuarioid,@sesionfecha,@sesionevento)";
            SqlCommand comando2 = new SqlCommand(sql2, conexion);
            comando2.Parameters.Add("@usuarioid", SqlDbType.Int).Value = valorId;
            comando2.Parameters.Add("@sesionfecha", SqlDbType.DateTime).Value = fecha;
            comando2.Parameters.Add("@sesionevento", SqlDbType.VarChar).Value = "Ingreso";
            comando2.ExecuteNonQuery();
            conexion.Close();
        }

        public void grabarSalidaSesion()
        {
            DateTime fecha = DateTime.Now;

            int valorId = identificarIdDeDni();

            conexion.Open();
            string sql2 = "insert into SESION (USUARIO_ID, SESION_FECHA, SESION_EVENTO) values (@usuarioid,@sesionfecha,@sesionevento)";
            SqlCommand comando2 = new SqlCommand(sql2, conexion);
            comando2.Parameters.Add("@usuarioid", SqlDbType.Int).Value = valorId;
            comando2.Parameters.Add("@sesionfecha", SqlDbType.DateTime).Value = fecha;
            comando2.Parameters.Add("@sesionevento", SqlDbType.VarChar).Value = "Salida";
            comando2.ExecuteNonQuery();
            conexion.Close();
        }

        private void productosDeReventaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                ProdReventaMain m = new ProdReventaMain();
                m.ShowDialog();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void panelSideMenu_Paint(object sender, PaintEventArgs e)
        {

        }

        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void button27_Click(object sender, EventArgs e)
        {
            mostrarSubMenu(panelSoporte);
        }

        private void button20_Click(object sender, EventArgs e)
        {
            mostrarSubMenu(panelAyuda);
        }

        private void button18_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1"))
            {
                AbrirpanelHijo(new AdministrarUsuarios());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1"))
            {
                AbrirpanelHijo(new Sesiones());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "Para consultas generales, por favor contacte a:\n" +
                "soportesandsys@gmail.com\n\n" +
                "Para soporte técnico, por favor contacte a:\n" +
                "+54 9 3512966008 / +54 9 3512536792 / +54 9 3516361160";
            m.ShowDialog();
        }

        private void button22_Click(object sender, EventArgs e)
        {
            Aviso m = new Aviso();
            m.label1.Text = "SandSys\n" +
                "2021\n\n" +
                "© Todos los derechos reservados";
            m.ShowDialog();
        }

        private void button28_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1"))
            {
                AbrirpanelHijo(new AdministrarCategorias());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button23_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("5"))
            {
                AbrirpanelHijo(new MenuPagos());
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Normal)
            {
                WindowState = FormWindowState.Minimized;
            }
            else if (WindowState == FormWindowState.Minimized)
            {
                WindowState = FormWindowState.Normal;
            }
        }


        private void button24_Paint(object sender, PaintEventArgs e)
        {
            Region = System.Drawing.Region.FromHrgn(CreateRoundRectRgn(0, 0, Width, Height, 20, 20));
        }

        private void button25_Click(object sender, EventArgs e)
        {
            AbrirpanelHijo(new Producción.ProdElab());
            ocultarSubMenu();
        }

        private void button26_Click(object sender, EventArgs e)
        {
            if (tipoActivo.Equals("1") || tipoActivo.Equals("2") || tipoActivo.Equals("4"))
            {
                AbrirpanelHijo(new ReportesElab(tipoActivo, nombreActivo2, apellidoActivo2));
                ocultarSubMenu();
            }
            else
            {
                Aviso m = new Aviso();
                m.label1.Text = "Usted no posee acceso a esta sección.\nPor favor contacte al Administrador.";
                m.ShowDialog();
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // AbrirpanelHijo(new Menu());
        }
    }
    
}
