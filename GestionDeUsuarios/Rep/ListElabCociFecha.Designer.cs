
namespace GestionDeUsuarios
{
    partial class ListElabCociFecha
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
            this.DataSetListElabCociFecha = new GestionDeUsuarios.DataSetListElabCociFecha();
            this.ListElabCociFechaBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ListElabCociFechaTableAdapter = new GestionDeUsuarios.DataSetListElabCociFechaTableAdapters.ListElabCociFechaTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetListElabCociFecha)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListElabCociFechaBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.ListElabCociFechaBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "GestionDeUsuarios.Rep.ListElabCociFecha.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(883, 875);
            this.reportViewer1.TabIndex = 0;
            // 
            // DataSetListElabCociFecha
            // 
            this.DataSetListElabCociFecha.DataSetName = "DataSetListElabCociFecha";
            this.DataSetListElabCociFecha.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ListElabCociFechaBindingSource
            // 
            this.ListElabCociFechaBindingSource.DataMember = "ListElabCociFecha";
            this.ListElabCociFechaBindingSource.DataSource = this.DataSetListElabCociFecha;
            // 
            // ListElabCociFechaTableAdapter
            // 
            this.ListElabCociFechaTableAdapter.ClearBeforeFill = true;
            // 
            // ListElabCociFecha
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 875);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ListElabCociFecha";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte de elaboración";
            this.Load += new System.EventHandler(this.ListElabCociFecha_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataSetListElabCociFecha)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListElabCociFechaBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource ListElabCociFechaBindingSource;
        private DataSetListElabCociFecha DataSetListElabCociFecha;
        private DataSetListElabCociFechaTableAdapters.ListElabCociFechaTableAdapter ListElabCociFechaTableAdapter;
    }
}