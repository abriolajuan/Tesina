using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GestionDeUsuarios
{
    public partial class CuentaGlobal : Form
    {
        private SqlConnection conexion = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["conexionbd"].ConnectionString);
        public CuentaGlobal()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Close();
        }

        public void calcularTotal()
        {
            Decimal suma = 0;
            for (int i = 0; i < dataGridView1.Rows.Count; ++i)
            {
                suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
            }

            if (suma > 0)
            {
                label1.ForeColor = Color.FromArgb(255, 0, 0);
                label1.Text = "Suma de todos los Saldos: $" + suma.ToString();
            }
            else if (suma <= 0)
            {
                label1.ForeColor = Color.FromArgb(0, 128, 0);
                label1.Text = "Suma de todos los Saldos: $" + suma.ToString();
            }

            /* Decimal suma = 0;
             for (int i = 0; i < dataGridView1.Rows.Count; ++i)
             {
                 suma += Convert.ToDecimal(dataGridView1.Rows[i].Cells[3].Value);
             }
             label1.Text = "Suma de todos los saldos: $" + suma.ToString(); */
        }

        private void CuentaGlobal_Load(object sender, EventArgs e)
        {
                
                conexion.Open();
                string sql = "SELECT T1.PROVEE_NOMBRE, T1.totalcomprado,t2.pagos, T1.totalcomprado-T2.pagos as saldo FROM(SELECT PROVEE_NOMBRE, SUM(DET_COMPRA_CANTIDAD*DET_COMPRA_PR_UNIT) as 'totalcomprado' FROM PROVEEDOR pro , DETALLECOMPRA det, COMPRA com WHERE det.COMPRA_ID=com.COMPRA_ID AND com.PROVEE_ID=pro.PROVEE_ID GROUP BY pro.PROVEE_NOMBRE) T1 LEFT JOIN(SELECT PROVEEDOR.PROVEE_NOMBRE, coalesce(sum(PAGOCOMPRA.PAGO_COMPRA_MONTO), 0) as pagos FROM COMPRA left JOIN PAGOCOMPRA ON COMPRA.COMPRA_ID = PAGOCOMPRA.COMPRA_ID left JOIN PROVEEDOR ON COMPRA.PROVEE_ID = PROVEEDOR.PROVEE_ID group by PROVEEDOR.PROVEE_NOMBRE) T2 ON (T1.PROVEE_NOMBRE=T2.PROVEE_NOMBRE)";
                SqlCommand comando = new SqlCommand(sql, conexion);
                SqlDataReader registros = comando.ExecuteReader();
                dataGridView1.Rows.Clear();
                while (registros.Read())
                {
                dataGridView1.Rows.Add(registros["PROVEE_NOMBRE"].ToString(),
                                      registros["totalcomprado"].ToString(),
                                      registros["pagos"].ToString(),
                                      registros["saldo"].ToString());
                }

                
                registros.Close();
                conexion.Close();
                calcularTotal();
        }

        private void button2_Click(object sender, EventArgs e) // BOTÓN BORRADO DESCARGAR LISTADO PDF
        {
            /* if (dataGridView1.Rows.Count > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "Archivo.pdf";
                bool fileError = false;
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("No fue posible guardar los datos en el disco" + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfTable = new PdfPTable(dataGridView1.Columns.Count);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_CENTER;

                            foreach (DataGridViewColumn column in dataGridView1.Columns)
                            {
                                PdfPCell cell = new PdfPCell(new Phrase(column.HeaderText));
                                cell.HorizontalAlignment = 1; //0=Left, 1=Centre, 2=Right
                                cell.VerticalAlignment = 1;
                                cell.BackgroundColor = new iTextSharp.text.BaseColor(255, 102, 0);
                                pdfTable.AddCell(cell);
                            }

                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                foreach (DataGridViewCell cell in row.Cells)
                                {
                                    pdfTable.AddCell(cell.Value?.ToString());
                                }
                            }

                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                                PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Datos exportados exitosamente", "Archivo PDF");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error: " + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("No hay registros para exportar", "Info");
            }*/

        }

        private void button3_Click(object sender, EventArgs e)
        {
            GlobalProvee form = new GlobalProvee();
            form.ShowDialog();
        }
            
    }
}
