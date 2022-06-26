
namespace GestionDeUsuarios
{
    partial class ListElabEstadoFecha
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            Microsoft.Reporting.WinForms.ReportDataSource reportDataSource1 = new Microsoft.Reporting.WinForms.ReportDataSource();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.DataSetListElabEstadoFecha = new GestionDeUsuarios.DataSetListElabEstadoFecha();
            this.ListElabEstadoFechaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ListElabEstadoFechaTableAdapter = new GestionDeUsuarios.DataSetListElabEstadoFechaTableAdapters.ListElabEstadoFechaTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetListElabEstadoFecha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListElabEstadoFechaBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.ListElabEstadoFechaBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "GestionDeUsuarios.Rep.ListElabEstadoFecha.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(883, 875);
            this.reportViewer1.TabIndex = 0;
            // 
            // DataSetListElabEstadoFecha
            // 
            this.DataSetListElabEstadoFecha.DataSetName = "DataSetListElabEstadoFecha";
            this.DataSetListElabEstadoFecha.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ListElabEstadoFechaBindingSource
            // 
            this.ListElabEstadoFechaBindingSource.DataMember = "ListElabEstadoFecha";
            this.ListElabEstadoFechaBindingSource.DataSource = this.DataSetListElabEstadoFecha;
            // 
            // ListElabEstadoFechaTableAdapter
            // 
            this.ListElabEstadoFechaTableAdapter.ClearBeforeFill = true;
            // 
            // ListElabEstadoFecha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 875);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ListElabEstadoFecha";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte de elaboración";
            this.Load += new System.EventHandler(this.ListElabEstadoFecha_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataSetListElabEstadoFecha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListElabEstadoFechaBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource ListElabEstadoFechaBindingSource;
        private DataSetListElabEstadoFecha DataSetListElabEstadoFecha;
        private DataSetListElabEstadoFechaTableAdapters.ListElabEstadoFechaTableAdapter ListElabEstadoFechaTableAdapter;
    }
}