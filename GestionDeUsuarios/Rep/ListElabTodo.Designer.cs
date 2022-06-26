
namespace GestionDeUsuarios
{
    partial class ListElabTodo
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
            this.DataSetListElabTodo = new GestionDeUsuarios.DataSetListElabTodo();
            this.ListElabTodoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.ListElabTodoTableAdapter = new GestionDeUsuarios.DataSetListElabTodoTableAdapters.ListElabTodoTableAdapter();
            ((System.ComponentModel.ISupportInitialize)(this.DataSetListElabTodo)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListElabTodoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // reportViewer1
            // 
            this.reportViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
            reportDataSource1.Name = "DataSet1";
            reportDataSource1.Value = this.ListElabTodoBindingSource;
            this.reportViewer1.LocalReport.DataSources.Add(reportDataSource1);
            this.reportViewer1.LocalReport.ReportEmbeddedResource = "GestionDeUsuarios.Rep.ListElabTodo.rdlc";
            this.reportViewer1.Location = new System.Drawing.Point(0, 0);
            this.reportViewer1.Name = "reportViewer1";
            this.reportViewer1.ServerReport.BearerToken = null;
            this.reportViewer1.Size = new System.Drawing.Size(883, 875);
            this.reportViewer1.TabIndex = 0;
            // 
            // DataSetListElabTodo
            // 
            this.DataSetListElabTodo.DataSetName = "DataSetListElabTodo";
            this.DataSetListElabTodo.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // ListElabTodoBindingSource
            // 
            this.ListElabTodoBindingSource.DataMember = "ListElabTodo";
            this.ListElabTodoBindingSource.DataSource = this.DataSetListElabTodo;
            // 
            // ListElabTodoTableAdapter
            // 
            this.ListElabTodoTableAdapter.ClearBeforeFill = true;
            // 
            // ListElabTodo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(883, 875);
            this.Controls.Add(this.reportViewer1);
            this.Name = "ListElabTodo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Reporte de elaboración";
            this.Load += new System.EventHandler(this.ListElabTodo_Load);
            ((System.ComponentModel.ISupportInitialize)(this.DataSetListElabTodo)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ListElabTodoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Microsoft.Reporting.WinForms.ReportViewer reportViewer1;
        private System.Windows.Forms.BindingSource ListElabTodoBindingSource;
        private DataSetListElabTodo DataSetListElabTodo;
        private DataSetListElabTodoTableAdapters.ListElabTodoTableAdapter ListElabTodoTableAdapter;
    }
}