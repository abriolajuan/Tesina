
namespace GestionDeUsuarios
{
    partial class GlobalProvee
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
            this.GobalProveeBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.DataSetGlobalProvee = new GestionDeUsuarios.DataSetGlobalProvee();
            this.reportViewer1 = new Microsoft.Reporting.WinForms.ReportViewer();
            this.GobalProveeTableAdapter = new GestionDeUsuarios.DataSetGlobalProveeTableAdapters.GobalProveeTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.GobalProveeBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetGlobalProvee)).BeginInit();
            this.SuspendLayout();
            // 
            // GobalProveeBindingSource
            // 
            this.GobalProveeBindingSource.DataMember = "GobalProvee";
            this.GobalProveeBindingSource.DataSource = this.DataSetGlobalProvee;
            // 
            // DataSetGlobalProvee
            // 
            this.DataSetGlobalProvee.DataSetName = "DataSetGlobalProvee";
            this.DataSetGlobalProvee.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.GobalProveeBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "GestionDeUsuarios.Rep.GlobalProvee.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(883, 875);
            this.reportViewer1.TabIndex = 0;
            // 
            // GobalProveeTableAdapter
            // 
            this.GobalProveeTableAdapter.ClearBeforeFill = true;
            // 
            // GlobalProvee
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 875);
            this.Controls.Add(this.reportViewer1);
            this.Name = "GlobalProvee";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte global de proveedores";
            this.Load += new System.EventHandler(this.GlobalProvee_Load);
            ((System.ComponentModel.ISupportInitialize)(this.GobalProveeBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetGlobalProvee)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource GobalProveeBindingSource;
        private DataSetGlobalProvee DataSetGlobalProvee;
        private DataSetGlobalProveeTableAdapters.GobalProveeTableAdapter GobalProveeTableAdapter;
    }
}