using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Microsoft.EntityFrameworkCore;
using SeuApp.Core.Entities;
using SeuApp.Data;
using SeuApp.Data.Repositories;

namespace SeuApp.WinForms.Forms;

public partial class VendasForm : Form
{
    private readonly AppDbContext _ctx;
    private readonly IGenericRepository<Venda> _repoVendas;
    private readonly IGenericRepository<Produto> _repoProdutos;

    // Serializa o acesso ao mesmo DbContext usado pelos repositórios
    private readonly SemaphoreSlim _dbLock = new(1, 1);

    private List<Produto> _produtos = new();

    public VendasForm(
        AppDbContext ctx,
        IGenericRepository<Venda> repoVendas,
        IGenericRepository<Produto> repoProdutos)
    {
        InitializeComponent();
        _ctx = ctx;
        _repoVendas = repoVendas;
        _repoProdutos = repoProdutos;

        // Tema + estilos ao carregar
        Load += async (_, __) =>
        {
            // Tema geral
            UiTheme.Apply(this, dark: false);

            // Cabeçalho estático e grid moderno
            UiTheme.StyleGrid(dataGridView1, dark: false);
            dataGridView1.EnableHeadersVisualStyles = false;
            // evita "azul" no cabeçalho quando a célula está em edição
            var headerBack = dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;
            var headerFore = dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = headerBack;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionForeColor = headerFore;

            // Estilos de botões conforme solicitado
            UiTheme.StyleButtonPrimary(AdicionarItem);
            UiTheme.StyleButtonDanger(RemoverItem);
            UiTheme.StyleButtonSuccess(SalvarVenda);
            UiTheme.StyleButtonOutline(Voltar);

            // Inputs
            UiTheme.StyleLabel(LblNome);
            UiTheme.StyleLabel(LblCpf);
            UiTheme.StyleTextBox(NomeCliente);
            UiTheme.StyleMasked(CPFCliente);

            await InitGrid();
        };

        // Atalhos
        KeyPreview = true;
        KeyDown += VendasForm_KeyDown;

        // Trata erros de binding/combobox (evita exception visual)
        dataGridView1.DataError += (_, __) => { /* ignora visualmente */ };
    }

    private void BtnVoltar_Click(object? sender, EventArgs e)
    {
        // Apenas fecha; MainForm já está aberto
        Close();
    }

    private void VendasForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.Insert) BtnAdicionarItem_Click(this, EventArgs.Empty);
        else if (e.KeyCode == Keys.Delete) BtnRemoverItem_Click(this, EventArgs.Empty);
        else if (e.Control && e.KeyCode == Keys.S) BtnSalvarVenda_Click(this, EventArgs.Empty);
    }

    private async Task InitGrid()
    {
        await _dbLock.WaitAsync();
        try
        {
            _produtos = (await _repoProdutos.GetAllAsync()).ToList();
        }
        finally
        {
            _dbLock.Release();
        }

        // Grid base
        dataGridView1.ReadOnly = false;                        // vamos controlar por coluna
        dataGridView1.AllowUserToAddRows = true;
        dataGridView1.AllowUserToDeleteRows = true;
        dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
        dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;

        // Remove o retângulo pontilhado e mantém aparência neutra
        dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
        dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;
        dataGridView1.RowsDefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
        dataGridView1.RowsDefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;

        // Impede que o "foco" desenhe o retângulo pontilhado
        dataGridView1.CellPainting += (s, e) =>
        {
            if ((e.State & DataGridViewElementStates.Selected) != 0)
            {
                var parts = e.PaintParts & ~DataGridViewPaintParts.Focus; // remove o "focus cue"
                e.Paint(e.ClipBounds, parts);
                e.Handled = true;
            }
        };
        dataGridView1.MultiSelect = false;
        dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        dataGridView1.RowHeadersVisible = false;

        // Coluna de produto (combobox) — veio do Designer
        var colProd = (DataGridViewComboBoxColumn)dataGridView1.Columns["ProdutoId"];
        colProd.DataSource = _produtos;
        colProd.DisplayMember = nameof(Produto.Descricao);
        colProd.ValueMember = nameof(Produto.Id);
        colProd.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;

        // Somente leitura controlado: apenas Produto e Quantidade editáveis
        foreach (DataGridViewColumn c in dataGridView1.Columns)
            c.ReadOnly = true; // bloqueia tudo

        dataGridView1.Columns["ProdutoId"].ReadOnly = false;   // libera Produto
        dataGridView1.Columns["Quantidade"].ReadOnly = false;  // libera Qtd
        dataGridView1.Columns["ValorUnit"].ReadOnly = true;    // travado
        dataGridView1.Columns["TotalItem"].ReadOnly = true;    // travado

        // Alinhamentos e formatos
        dataGridView1.Columns["Quantidade"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridView1.Columns["ValorUnit"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        dataGridView1.Columns["TotalItem"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        dataGridView1.Columns["ValorUnit"].DefaultCellStyle.Format = "N2";
        dataGridView1.Columns["TotalItem"].DefaultCellStyle.Format = "C2";

        // Evita duplicar handlers
        dataGridView1.CurrentCellDirtyStateChanged -= DataGridView1_CurrentCellDirtyStateChanged;
        dataGridView1.CellValueChanged -= DataGridView1_CellValueChanged;
        dataGridView1.CellEndEdit -= DataGridView1_CellEndEdit;
        dataGridView1.RowsAdded -= DataGridView1_RowsChanged;
        dataGridView1.RowsRemoved -= DataGridView1_RowsChanged;
        dataGridView1.EditingControlShowing -= DataGridView1_EditingControlShowing;

        dataGridView1.CurrentCellDirtyStateChanged += DataGridView1_CurrentCellDirtyStateChanged;
        dataGridView1.CellValueChanged += DataGridView1_CellValueChanged;
        dataGridView1.CellEndEdit += DataGridView1_CellEndEdit;
        dataGridView1.RowsAdded += DataGridView1_RowsChanged;
        dataGridView1.RowsRemoved += DataGridView1_RowsChanged;
        dataGridView1.EditingControlShowing += DataGridView1_EditingControlShowing;

        // Entrada
        CPFCliente.Mask = "000.000.000-00";
        Total.Text = 0m.ToString("C");
    }

    private void DataGridView1_CurrentCellDirtyStateChanged(object? sender, EventArgs e)
    {
        // Garante CellValueChanged ao trocar o combo
        if (dataGridView1.IsCurrentCellDirty)
            dataGridView1.CommitEdit(DataGridViewDataErrorContexts.Commit);
    }

    private void DataGridView1_EditingControlShowing(object? sender, DataGridViewEditingControlShowingEventArgs e)
    {
        // Recalcula ao sair do editor
        e.Control.Leave -= Editor_Leave;
        e.Control.Leave += Editor_Leave;
    }

    private void Editor_Leave(object? sender, EventArgs e) => Recalcular();
    private void DataGridView1_CellEndEdit(object? sender, DataGridViewCellEventArgs e) => Recalcular();
    private void DataGridView1_RowsChanged(object? sender, EventArgs e) => Recalcular();

    private void DataGridView1_CellValueChanged(object? sender, DataGridViewCellEventArgs e)
    {
        if (e.RowIndex < 0) return;
        var row = dataGridView1.Rows[e.RowIndex];
        if (row.IsNewRow) return;

        // Ao escolher Produto → carrega Valor do produto
        if (dataGridView1.Columns[e.ColumnIndex].Name == "ProdutoId")
        {
            var val = row.Cells["ProdutoId"].Value;
            if (val != null && int.TryParse(Convert.ToString(val), out var prodId))
            {
                var prod = _produtos.FirstOrDefault(p => p.Id == prodId);
                if (prod != null)
                {
                    row.Cells["ValorUnit"].Value = prod.Valor;

                    // Se quantidade não definida, usa 1
                    if (!TryGetDecimal(row.Cells["Quantidade"].Value, out var qtd) || qtd <= 0)
                        row.Cells["Quantidade"].Value = 1m;
                }
            }
        }

        // Se quantidade alterada, valida mínimo 1
        if (dataGridView1.Columns[e.ColumnIndex].Name == "Quantidade")
        {
            if (!TryGetDecimal(row.Cells["Quantidade"].Value, out var qtd) || qtd <= 0)
                row.Cells["Quantidade"].Value = 1m;
        }

        Recalcular();
    }

    private void Recalcular()
    {
        decimal total = 0m;

        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            if (row.IsNewRow) continue;

            TryGetDecimal(row.Cells["Quantidade"].Value, out var qtd);
            TryGetDecimal(row.Cells["ValorUnit"].Value, out var vlr);

            var tot = qtd * vlr;
            row.Cells["TotalItem"].Value = tot;
            total += tot;
        }

        Total.Text = total.ToString("C");
    }

    private static bool TryGetDecimal(object? value, out decimal result)
    {
        var s = Convert.ToString(value) ?? "";
        var br = new CultureInfo("pt-BR");
        var en = CultureInfo.InvariantCulture;
        return (decimal.TryParse(s, NumberStyles.Any, br, out result)
             || decimal.TryParse(s, NumberStyles.Any, en, out result));
    }

    private static decimal TryGetDecimal(object? value) => TryGetDecimal(value, out var d) ? d : 0m;

    private async void BtnSalvarVenda_Click(object? sender, EventArgs e)
    {
        if (string.IsNullOrWhiteSpace(NomeCliente.Text))
        {
            MessageBox.Show("Nome do cliente é obrigatório.");
            return;
        }

        var validIds = _produtos.Select(p => p.Id).ToHashSet();
        var linhasInvalidas = new List<int>();

        for (int i = 0; i < dataGridView1.Rows.Count; i++)
        {
            var row = dataGridView1.Rows[i];
            if (row.IsNewRow) continue;

            var raw = row.Cells["ProdutoId"].Value;
            if (raw == null || !int.TryParse(Convert.ToString(raw), out var pid) || pid <= 0 || !validIds.Contains(pid))
                linhasInvalidas.Add(i + 1); // 1-based p/ usuário
        }

        if (linhasInvalidas.Count > 0)
        {
            MessageBox.Show(
                "Há itens com Produto inválido nas linhas: " + string.Join(", ", linhasInvalidas) +
                ". Selecione um produto válido antes de salvar.",
                "Itens inválidos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var idsValidosDb = (await _ctx.Produtos
    .AsNoTracking()
    .Select(p => p.Id)
    .ToListAsync())
    .ToHashSet();


        var venda = new Venda
        {
            NomeCliente = NomeCliente.Text.Trim(),
            Cpf = string.IsNullOrWhiteSpace(CPFCliente.Text) ? null : CPFCliente.Text,
            Total = 0m
        };

        foreach (DataGridViewRow row in dataGridView1.Rows)
        {
            if (row.IsNewRow) continue;

            // ProdutoId precisa existir e ser int válido presente no banco
            var rawId = row.Cells["ProdutoId"].Value;
            if (rawId == null || !int.TryParse(Convert.ToString(rawId), out var produtoId)) continue;
            if (!idsValidosDb.Contains(produtoId)) continue;

            // Quantidade e Valor
            var qtd = TryGetDecimal(row.Cells["Quantidade"].Value);
            var vlr = TryGetDecimal(row.Cells["ValorUnit"].Value);

            if (qtd <= 0 || vlr <= 0) continue;

            // Busca descrição só para exibir/armazenar (opcional)
            var prodDesc = _produtos.FirstOrDefault(p => p.Id == produtoId)?.Descricao ?? "";

            venda.Itens.Add(new VendaItem
            {
                ProdutoId = produtoId,
                Descricao = prodDesc,
                Quantidade = qtd,
                ValorUnit = vlr
            });
        }

        venda.Itens = venda.Itens
    .Where(i => idsValidosDb.Contains(i.ProdutoId))
    .ToList();

        if (venda.Itens.Count == 0)
        {
            MessageBox.Show("Inclua ao menos um item válido (produto e quantidade).");
            return;
        }

        if (venda.Itens.Count == 0)
        {
            MessageBox.Show("Inclua ao menos um item válido.");
            return;
        }

        venda.Total = venda.Itens.Sum(i => i.Quantidade * i.ValorUnit);

        await _dbLock.WaitAsync();
        try
        {
            await _repoVendas.AddAsync(venda);
            await _repoVendas.SaveChangesAsync();
        }
        finally
        {
            _dbLock.Release();
        }

        MessageBox.Show("Venda salva!");
        dataGridView1.Rows.Clear();
        NomeCliente.Clear();
        CPFCliente.Clear();
        Total.Text = 0m.ToString("C");
    }

    private void BtnRemoverItem_Click(object? sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow != null && !dataGridView1.CurrentRow.IsNewRow)
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
    }

    private void BtnAdicionarItem_Click(object? sender, EventArgs e)
    {
        var idx = dataGridView1.Rows.Add();
        var row = dataGridView1.Rows[idx];
        row.Cells["Quantidade"].Value = 1m;
        row.Cells["ValorUnit"].Value = 0m; // será preenchido ao escolher o produto
        dataGridView1.CurrentCell = row.Cells["ProdutoId"];
        dataGridView1.BeginEdit(true);
    }

    private void BtnEditar_Click(object? sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow is null || dataGridView1.CurrentRow.IsNewRow)
        {
            MessageBox.Show("Selecione um item para editar.");
            return;
        }

        var row = dataGridView1.CurrentRow;
        // Foca na primeira coluna editável disponível
        DataGridViewCell cell = row.Cells["ProdutoId"];
        if (cell.ReadOnly) cell = row.Cells["Quantidade"];
        dataGridView1.CurrentCell = cell;
        dataGridView1.BeginEdit(true);
    }

    // Mapeamento dos botões do Designer (mantidos)
    private void AdicionarItem_Click(object? sender, EventArgs e) => BtnAdicionarItem_Click(sender, e);
    private void RemoverItem_Click(object? sender, EventArgs e) => BtnRemoverItem_Click(sender, e);
    private void ExcluirItem_Click(object? sender, EventArgs e) => BtnRemoverItem_Click(sender, e); // Está invisível no Designer
    private void SalvarVenda_Click(object? sender, EventArgs e) => BtnSalvarVenda_Click(sender, e);

    private void LblCpf_Click(object sender, EventArgs e)
    {

    }

    private void LblNome_Click(object sender, EventArgs e)
    {

    }
}
