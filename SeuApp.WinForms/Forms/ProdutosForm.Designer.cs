using System.Drawing;
using System.Windows.Forms;

namespace SeuApp.WinForms.Forms
{
    partial class ProdutosForm
    {
        private System.ComponentModel.IContainer components = null;

        private DataGridView dataGridView1;
        private TextBox Descrição;
        private MaskedTextBox Valor;
        private NumericUpDown Quantidade;
        private TextBox Observação;

        private Button Novo;
        private Button Salvar;
        private Button Excluir;
        private Button Editar;
        private Button Voltar;

        private Label LblDescricao;
        private Label LblValor;
        private Label LblQtd;
        private Label LblObs;

        private Panel Footer; // painel inferior para evitar sobreposição

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            dataGridView1 = new DataGridView();
            Footer = new Panel();
            LblDescricao = new Label();
            Descrição = new TextBox();
            LblValor = new Label();
            Valor = new MaskedTextBox();
            LblQtd = new Label();
            Quantidade = new NumericUpDown();
            LblObs = new Label();
            Observação = new TextBox();
            Novo = new Button();
            Salvar = new Button();
            Excluir = new Button();
            Editar = new Button();
            Voltar = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            Footer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)Quantidade).BeginInit();
            SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.AllowUserToDeleteRows = false;
            dataGridView1.AllowUserToResizeRows = false;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeight = 36;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.Location = new Point(0, 0);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.ReadOnly = true;
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(800, 350);
            dataGridView1.TabIndex = 0;
            // 
            // Footer
            // 
            Footer.Controls.Add(LblDescricao);
            Footer.Controls.Add(Descrição);
            Footer.Controls.Add(LblValor);
            Footer.Controls.Add(Valor);
            Footer.Controls.Add(LblQtd);
            Footer.Controls.Add(Quantidade);
            Footer.Controls.Add(LblObs);
            Footer.Controls.Add(Observação);
            Footer.Controls.Add(Novo);
            Footer.Controls.Add(Salvar);
            Footer.Controls.Add(Excluir);
            Footer.Controls.Add(Editar);
            Footer.Controls.Add(Voltar);
            Footer.Dock = DockStyle.Bottom;
            Footer.Location = new Point(0, 350);
            Footer.Name = "Footer";
            Footer.Padding = new Padding(12, 10, 12, 10);
            Footer.Size = new Size(800, 110);
            Footer.TabIndex = 1;
            // 
            // LblDescricao
            // 
            LblDescricao.AutoSize = true;
            LblDescricao.Location = new Point(12, 10);
            LblDescricao.Name = "LblDescricao";
            LblDescricao.Size = new Size(58, 15);
            LblDescricao.TabIndex = 0;
            LblDescricao.Text = "Descrição";
            // 
            // Descrição
            // 
            Descrição.Location = new Point(12, 28);
            Descrição.Name = "Descrição";
            Descrição.PlaceholderText = "Ex.: Caneta Azul";
            Descrição.Size = new Size(300, 23);
            Descrição.TabIndex = 1;
            // 
            // LblValor
            // 
            LblValor.AutoSize = true;
            LblValor.Location = new Point(324, 10);
            LblValor.Name = "LblValor";
            LblValor.Size = new Size(33, 15);
            LblValor.TabIndex = 2;
            LblValor.Text = "Valor";
            // 
            // Valor
            // 
            Valor.Location = new Point(324, 28);
            Valor.Name = "Valor";
            Valor.PromptChar = ' ';
            Valor.Size = new Size(120, 23);
            Valor.TabIndex = 3;
            Valor.TextMaskFormat = MaskFormat.ExcludePromptAndLiterals;
            // 
            // LblQtd
            // 
            LblQtd.AutoSize = true;
            LblQtd.Location = new Point(452, 10);
            LblQtd.Name = "LblQtd";
            LblQtd.Size = new Size(69, 15);
            LblQtd.TabIndex = 4;
            LblQtd.Text = "Quantidade";
            // 
            // Quantidade
            // 
            Quantidade.Location = new Point(452, 28);
            Quantidade.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
            Quantidade.Name = "Quantidade";
            Quantidade.Size = new Size(70, 23);
            Quantidade.TabIndex = 5;
            Quantidade.TextAlign = HorizontalAlignment.Right;
            Quantidade.ThousandsSeparator = true;
            // 
            // LblObs
            // 
            LblObs.AutoSize = true;
            LblObs.Location = new Point(536, 10);
            LblObs.Name = "LblObs";
            LblObs.Size = new Size(69, 15);
            LblObs.TabIndex = 6;
            LblObs.Text = "Observação";
            // 
            // Observação
            // 
            Observação.Location = new Point(536, 28);
            Observação.Name = "Observação";
            Observação.PlaceholderText = "Opcional";
            Observação.Size = new Size(233, 23);
            Observação.TabIndex = 7;
            // 
            // Novo
            // 
            Novo.Location = new Point(12, 66);
            Novo.Name = "Novo";
            Novo.Size = new Size(90, 30);
            Novo.TabIndex = 8;
            Novo.Text = "Novo (F2)";
            Novo.UseVisualStyleBackColor = true;
            Novo.Click += BtnNovo_Click;
            // 
            // Salvar
            // 
            Salvar.Location = new Point(108, 66);
            Salvar.Name = "Salvar";
            Salvar.Size = new Size(110, 30);
            Salvar.TabIndex = 9;
            Salvar.Text = "Salvar (Ctrl+S)";
            Salvar.UseVisualStyleBackColor = true;
            Salvar.Click += BtnSalvar_Click;
            // 
            // Excluir
            // 
            Excluir.Location = new Point(224, 66);
            Excluir.Name = "Excluir";
            Excluir.Size = new Size(100, 30);
            Excluir.TabIndex = 10;
            Excluir.Text = "Excluir (Del)";
            Excluir.UseVisualStyleBackColor = true;
            Excluir.Click += BtnExcluir_Click;
            // 
            // Editar
            // 
            Editar.Location = new Point(330, 66);
            Editar.Name = "Editar";
            Editar.Size = new Size(100, 30);
            Editar.TabIndex = 11;
            Editar.Text = "Editar (F5)";
            Editar.UseVisualStyleBackColor = true;
            Editar.Click += BtnAtualizar_Click;
            // 
            // Voltar
            // 
            Voltar.Location = new Point(669, 66);
            Voltar.Name = "Voltar";
            Voltar.Size = new Size(100, 30);
            Voltar.TabIndex = 12;
            Voltar.Text = "Voltar";
            Voltar.UseVisualStyleBackColor = true;
            Voltar.Click += BtnVoltar_Click;
            // 
            // ProdutosForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 460);
            Controls.Add(dataGridView1);
            Controls.Add(Footer);
            Name = "ProdutosForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Produtos";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            Footer.ResumeLayout(false);
            Footer.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)Quantidade).EndInit();
            ResumeLayout(false);
        }
    }
}
