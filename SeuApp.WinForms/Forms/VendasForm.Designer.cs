using System.Drawing;
using System.Windows.Forms;

namespace SeuApp.WinForms.Forms
{
    partial class VendasForm
    {
        private Label LblNome;
        private TextBox NomeCliente;
        private Label LblCpf;
        private MaskedTextBox CPFCliente;
        private Label Total;
        private DataGridView dataGridView1;
        private DataGridViewComboBoxColumn ProdutoId;
        private DataGridViewTextBoxColumn Quantidade;
        private DataGridViewTextBoxColumn ValorUnit;
        private DataGridViewTextBoxColumn TotalItem;
        private Button AdicionarItem;
        private Button RemoverItem;
        private Button SalvarVenda;
        private Button Voltar;

        private void InitializeComponent()
        {
            LblNome = new Label();
            NomeCliente = new TextBox();
            LblCpf = new Label();
            CPFCliente = new MaskedTextBox();
            Total = new Label();
            dataGridView1 = new DataGridView();
            ProdutoId = new DataGridViewComboBoxColumn();
            Quantidade = new DataGridViewTextBoxColumn();
            ValorUnit = new DataGridViewTextBoxColumn();
            TotalItem = new DataGridViewTextBoxColumn();
            AdicionarItem = new Button();
            RemoverItem = new Button();
            SalvarVenda = new Button();
            Voltar = new Button();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            SuspendLayout();
            // 
            // LblNome
            // 
            LblNome.AutoSize = true;
            LblNome.Location = new Point(9, 3);
            LblNome.Name = "LblNome";
            LblNome.Size = new Size(95, 15);
            LblNome.TabIndex = 0;
            LblNome.Text = "Nome do cliente";
            LblNome.Click += LblNome_Click;
            // 
            // NomeCliente
            // 
            NomeCliente.Location = new Point(24, 36);
            NomeCliente.Name = "NomeCliente";
            NomeCliente.PlaceholderText = "Ex.: Maria da Silva";
            NomeCliente.Size = new Size(320, 23);
            NomeCliente.TabIndex = 1;
            // 
            // LblCpf
            // 
            LblCpf.AutoSize = true;
            LblCpf.Location = new Point(341, 3);
            LblCpf.Name = "LblCpf";
            LblCpf.Size = new Size(28, 15);
            LblCpf.TabIndex = 2;
            LblCpf.Text = "CPF";
            LblCpf.Click += LblCpf_Click;
            // 
            // CPFCliente
            // 
            CPFCliente.Location = new Point(355, 36);
            CPFCliente.Name = "CPFCliente";
            CPFCliente.Size = new Size(130, 23);
            CPFCliente.TabIndex = 3;
            // 
            // Total
            // 
            Total.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            Total.AutoSize = true;
            Total.Font = new Font("Segoe UI Semibold", 11F, FontStyle.Bold);
            Total.ForeColor = Color.FromArgb(59, 130, 246);
            Total.Location = new Point(700, 39);
            Total.Name = "Total";
            Total.Size = new Size(58, 20);
            Total.TabIndex = 4;
            Total.Text = "R$ 0,00";
            // 
            // dataGridView1
            // 
            dataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Columns.AddRange(new DataGridViewColumn[] { ProdutoId, Quantidade, ValorUnit, TotalItem });
            dataGridView1.Location = new Point(24, 75);
            dataGridView1.MultiSelect = false;
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowHeadersVisible = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.Size = new Size(752, 290);
            dataGridView1.TabIndex = 5;
            // 
            // ProdutoId
            // 
            ProdutoId.HeaderText = "Produto";
            ProdutoId.Name = "ProdutoId";
            // 
            // Quantidade
            // 
            Quantidade.HeaderText = "Qtd";
            Quantidade.Name = "Quantidade";
            // 
            // ValorUnit
            // 
            ValorUnit.HeaderText = "Valor (R$)";
            ValorUnit.Name = "ValorUnit";
            ValorUnit.ReadOnly = true;
            // 
            // TotalItem
            // 
            TotalItem.HeaderText = "Total";
            TotalItem.Name = "TotalItem";
            TotalItem.ReadOnly = true;
            // 
            // AdicionarItem
            // 
            AdicionarItem.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            AdicionarItem.Location = new Point(24, 385);
            AdicionarItem.Name = "AdicionarItem";
            AdicionarItem.Size = new Size(130, 34);
            AdicionarItem.TabIndex = 6;
            AdicionarItem.Text = "Adicionar (Ins)";
            AdicionarItem.Click += AdicionarItem_Click;
            // 
            // RemoverItem
            // 
            RemoverItem.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            RemoverItem.Location = new Point(164, 385);
            RemoverItem.Name = "RemoverItem";
            RemoverItem.Size = new Size(130, 34);
            RemoverItem.TabIndex = 7;
            RemoverItem.Text = "Remover (Del)";
            RemoverItem.Click += RemoverItem_Click;
            // 
            // SalvarVenda
            // 
            SalvarVenda.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SalvarVenda.Location = new Point(429, 385);
            SalvarVenda.Name = "SalvarVenda";
            SalvarVenda.Size = new Size(110, 34);
            SalvarVenda.TabIndex = 9;
            SalvarVenda.Text = "Salvar (Ctrl+S)";
            SalvarVenda.Click += SalvarVenda_Click;
            // 
            // Voltar
            // 
            Voltar.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            Voltar.Location = new Point(642, 385);
            Voltar.Name = "Voltar";
            Voltar.Size = new Size(110, 34);
            Voltar.TabIndex = 10;
            Voltar.Text = "Voltar";
            Voltar.Click += BtnVoltar_Click;
            // 
            // VendasForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 440);
            Controls.Add(LblNome);
            Controls.Add(NomeCliente);
            Controls.Add(LblCpf);
            Controls.Add(CPFCliente);
            Controls.Add(Total);
            Controls.Add(dataGridView1);
            Controls.Add(AdicionarItem);
            Controls.Add(RemoverItem);
            Controls.Add(SalvarVenda);
            Controls.Add(Voltar);
            Name = "VendasForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Vendas";
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }
    }
}
