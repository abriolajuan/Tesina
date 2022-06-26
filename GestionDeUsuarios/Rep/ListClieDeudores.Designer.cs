
namespace GestionDeUsuarios
{
    partial class ListClieDeudores
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
            this.DataSetListClieDeudores = new GestionDeUsuarios.DataSetListClieDeudores();
            this.ListClieDeudoresBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ListClieDeudoresTableAdapter = new GestionDeUsuarios.DataSetListClieDeudoresTableAdapters.ListClieDeudoresTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetListClieDeudores)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListClieDeudoresBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DSClieDeudores";
            reportDataSource1.Value = this.ListClieDeudoresBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "GestionDeUsuarios.Rep.ListClieDeudores.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(883, 875);
            this.reportViewer1.TabIndex = 0;
            // 
            // DataSetListClieDeudores
            // 
            this.DataSetListClieDeudores.DataSetName = "DataSetListClieDeudores";
            this.DataSetListClieDeudores.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ListClieDeudoresBindingSource
            // 
            this.ListClieDeudoresBindingSource.DataMember = "ListClieDeudores";
            this.ListClieDeudoresBindingSource.DataSource = this.DataSetListClieDeudores;
            // 
            // ListClieDeudoresTableAdapter
            // 
            this.ListClieDeudoresTableAdapter.ClearBeforeFill = true;
            // 
            // ListClieDeudores
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 875);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ListClieDeudores";
            this.Text = "Reporte global de clientes deudores";
            this.Load += new System.EventHandler(this.ListClieDeudores_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataSetListClieDeudores)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListClieDeudoresBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource ListClieDeudoresBindingSource;
        private DataSetListClieDeudores DataSetListClieDeudores;
        private DataSetListClieDeudoresTableAdapters.ListClieDeudoresTableAdapter ListClieDeudoresTableAdapter;
    }
}