namespace SeuApp.WinForms.Forms
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private Label LblTitulo;
        private Button BtnProdutos;
        private Button BtnVendas;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            LblTitulo = new Label();
            BtnProdutos = new Button();
            BtnVendas = new Button();
            SuspendLayout();
            // 
            // LblTitulo
            // 
            LblTitulo.AutoSize = true;
            LblTitulo.Font = new Font("Segoe UI", 12F, FontStyle.Bold);
            LblTitulo.Location = new Point(76, 15);
            LblTitulo.Name = "LblTitulo";
            LblTitulo.Size = new Size(166, 21);
            LblTitulo.TabIndex = 0;
            LblTitulo.Text = "Selecione o módulo:";
            // 
            // BtnProdutos
            // 
            BtnProdutos.Location = new Point(40, 70);
            BtnProdutos.Name = "BtnProdutos";
            BtnProdutos.Size = new Size(100, 40);
            BtnProdutos.TabIndex = 1;
            BtnProdutos.Text = "Produtos";
            BtnProdutos.UseVisualStyleBackColor = true;
            BtnProdutos.Click += BtnProdutos_Click;
            // 
            // BtnVendas
            // 
            BtnVendas.Location = new Point(180, 70);
            BtnVendas.Name = "BtnVendas";
            BtnVendas.Size = new Size(100, 40);
            BtnVendas.TabIndex = 2;
            BtnVendas.Text = "Vendas";
            BtnVendas.UseVisualStyleBackColor = true;
            BtnVendas.Click += BtnVendas_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(330, 140);
            Controls.Add(BtnVendas);
            Controls.Add(BtnProdutos);
            Controls.Add(LblTitulo);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "SeuApp - Início";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
    }
}
