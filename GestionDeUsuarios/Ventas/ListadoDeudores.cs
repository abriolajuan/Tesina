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
    public partial class ListadoDeudores : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        public ListadoDeudores()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ListadoDeudores_Load(object sender, EventArgs e)
        {
            mostrarGrilla();
            calcularSaldoTotal();
        }

        private void mostrarGrilla()
        {
            conexion.Open();
            string sql = "SELECT T1.nombre_completo, T1.CLIENTE_TEL, T1.totalvendido-T3.DESCUENTO AS 'TOTALVENDIDO',t2.cobros, T3.SALDO FROM (SELECT(CLIENTE_NOMBRE + ' ' + CLIENTE_APELLIDO) as nombre_completo, CLIENTE_TEL, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'totalvendido' FROM CLIENTE cl, DETALLEVENTA det, VENTA ven WHERE det.VENTA_ID = ven.VENTA_ID AND ven.CLIENTE_ID = cl.CLIENTE_ID GROUP BY cl.CLIENTE_NOMBRE, CLIENTE_APELLIDO, CLIENTE_TEL) T1 LEFT JOIN (SELECT CLIENTE_TEL, coalesce(sum(COBRO_VENTA_MONTO), 0) as cobros FROM VENTA ven left JOIN COBROVENTA cob ON ven.VENTA_ID = cob.VENTA_ID left JOIN CLIENTE cl ON ven.CLIENTE_ID = cl.CLIENTE_ID group by cl.CLIENTE_TEL) T2 ON(T1.CLIENTE_TEL = T2.CLIENTE_TEL) LEFT JOIN (SELECT TA1.CLIENTE_TEL, TA1.TOTALVENDIDO-TA2.COBROS-TA2.DESCUENTO AS SALDO, TA2.DESCUENTO FROM (SELECT CLIENTE_TEL, SUM(DET_VENTA_CANT*(DET_VENTA_PR_UNIT-ISNULL(det.DET_VENTA_DESC_UNIT,0))) as 'totalvendido' FROM CLIENTE cl, DETALLEVENTA det, VENTA ven WHERE det.VENTA_ID = ven.VENTA_ID AND ven.CLIENTE_ID = cl.CLIENTE_ID GROUP BY cl.CLIENTE_TEL) TA1 LEFT JOIN (SELECT CLIENTE_TEL, coalesce(sum(COBRO_VENTA_MONTO), 0) as cobros,  SUM(VENTA_DTO) AS 'DESCUENTO' FROM VENTA ven left JOIN COBROVENTA cob ON ven.VENTA_ID = cob.VENTA_ID left JOIN CLIENTE cl ON ven.CLIENTE_ID = cl.CLIENTE_ID group by cl.CLIENTE_TEL) TA2 ON(TA1.CLIENTE_TEL = TA2.CLIENTE_TEL))T3 ON(T3.CLIENTE_TEL = T2.CLIENTE_TEL) WHERE SALDO> 0";
            SqlCommand comando = new SqlCommand(sql, conexion);
            SqlDataReader registros = comando.ExecuteReader();
            dataGridView1.Rows.Clear();
            while (registros.Read())
            {
                dataGridView1.Rows.Add(registros["NOMBRE_COMPLETO"],
                                  registros["CLIENTE_TEL"].ToString(),
                                  registros["TOTALVENDIDO"].ToString(),
                                  registros["COBROS"].ToString(),
                                  registros["SALDO"].ToString());
            }
            registros.Close();
            conexion.Close();
        }

        public void calcularSaldoTotal()
        {
            decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[4].Value);
            }
            label1.Text = "Suma de todos los Saldos($): " + suma.ToString();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ListClieDeudores form = new ListClieDeudores();
            form.ShowDialog();
        }
    }
}
