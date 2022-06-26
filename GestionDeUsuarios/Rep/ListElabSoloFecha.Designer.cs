
namespace GestionDeUsuarios
{
    partial class ListElabSoloFecha
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
            this.ListElabSoloFechaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DataSetListElabSoloFecha = new GestionDeUsuarios.DataSetListElabSoloFecha();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.ListElabSoloFechaTableAdapter = new GestionDeUsuarios.DataSetListElabSoloFechaTableAdapters.ListElabSoloFechaTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.ListElabSoloFechaBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetListElabSoloFecha)).BeginInit();
            this.SuspendLayout();
            // 
            // ListElabSoloFechaBindingSource
            // 
            this.ListElabSoloFechaBindingSource.DataMember = "ListElabSoloFecha";
            this.ListElabSoloFechaBindingSource.DataSource = this.DataSetListElabSoloFecha;
            // 
            // DataSetListElabSoloFecha
            // 
            this.DataSetListElabSoloFecha.DataSetName = "DataSetListElabSoloFecha";
            this.DataSetListElabSoloFecha.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.ListElabSoloFechaBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "GestionDeUsuarios.Rep.ListElabSoloFecha.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(883, 875);
            this.reportViewer1.TabIndex = 0;
            // 
            // ListElabSoloFechaTableAdapter
            // 
            this.ListElabSoloFechaTableAdapter.ClearBeforeFill = true;
            // 
            // ListElabSoloFecha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 875);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ListElabSoloFecha";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte de elaboración";
            this.Load += new System.EventHandler(this.ListElabSoloFecha_Load);
            ((System.ComponentModel.ISupportInitialize)(this.ListElabSoloFechaBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetListElabSoloFecha)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource ListElabSoloFechaBindingSource;
        private DataSetListElabSoloFecha DataSetListElabSoloFecha;
        private DataSetListElabSoloFechaTableAdapters.ListElabSoloFechaTableAdapter ListElabSoloFechaTableAdapter;
    }
}