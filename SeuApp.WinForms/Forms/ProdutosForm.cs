using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using SeuApp.Core.Entities;
using SeuApp.Data;                // <- DbSync.Gate
using SeuApp.Data.Repositories;

namespace SeuApp.WinForms.Forms;

public partial class ProdutosForm : Form
{
    // REMOVIDO: lock local — agora usamos DbSync.Gate (global)
    // private readonly SemaphoreSlim _dbLock = new(1, 1);

    private readonly IGenericRepository<Produto> _repo;
    private int? _editId = null;

    private bool _isEditing = false;

    private void SetEditing(bool on)
    {
        _isEditing = on;

        // Campos de edição
        Descrição.ReadOnly = !on;
        Valor.ReadOnly = !on;
        Observação.ReadOnly = !on;
        Quantidade.Enabled = on;

        // Botões de ação
        Salvar.Enabled = on;

        // Ações que não fazem sentido enquanto edita
        Novo.Enabled = !on;
        Editar.Enabled = !on;
        Excluir.Enabled = !on;
        Voltar.Enabled = !on;
    }

    private void EnableAllIdle()
    {
        // Estado "parado" (sem editar)
        SetEditing(false);
        // Garantir tudo habilitado fora de edição
        Novo.Enabled = true;
        Editar.Enabled = true;
        Excluir.Enabled = true;
        Voltar.Enabled = true;
        Salvar.Enabled = false; // só habilita quando entrar em edição
    }

    private void BeginEditMode()
    {
        SetEditing(true);
        Descrição.Focus();
    }

    public ProdutosForm(IGenericRepository<Produto> repo)
    {
        _repo = repo;
        InitializeComponent();

        // Carrega e estiliza ao abrir
        Load += async (_, __) =>
        {
            UiTheme.Apply(this, dark: false);

            // Estilos
            UiTheme.StyleGrid(dataGridView1);
            UiTheme.StyleButtonPrimary(Novo);     // azul
            UiTheme.StyleButtonSuccess(Salvar);   // verde
            UiTheme.StyleButtonDanger(Excluir);   // vermelho
            UiTheme.StyleButtonPrimary(Editar);   // azul
            UiTheme.StyleButtonOutline(Voltar);   // contorno

            UiTheme.StyleLabel(LblDescricao);
            UiTheme.StyleLabel(LblValor);
            UiTheme.StyleLabel(LblQtd);
            UiTheme.StyleLabel(LblObs);
            UiTheme.StyleTextBox(Descrição);
            UiTheme.StyleMasked(Valor);
            UiTheme.StyleTextBox(Observação);

            // NumericUpDown
            Quantidade.Minimum = 0;
            Quantidade.Maximum = 1_000_000;
            Quantidade.ThousandsSeparator = true;
            Quantidade.TextAlign = HorizontalAlignment.Right;

            // Garantias visuais da grid
            dataGridView1.ColumnHeadersVisible = true;
            dataGridView1.ColumnHeadersHeight = 36;
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            await CarregarGrid();

            // “desliga” o destaque de seleção (fica estático)
            var normalBg = dataGridView1.DefaultCellStyle.BackColor;
            var normalFg = dataGridView1.DefaultCellStyle.ForeColor;
            dataGridView1.DefaultCellStyle.SelectionBackColor = normalBg;
            dataGridView1.DefaultCellStyle.SelectionForeColor = normalFg;
            dataGridView1.RowsDefaultCellStyle.SelectionBackColor = normalBg;
            dataGridView1.RowsDefaultCellStyle.SelectionForeColor = normalFg;
            dataGridView1.AlternatingRowsDefaultCellStyle.SelectionBackColor =
                dataGridView1.AlternatingRowsDefaultCellStyle.BackColor;
            dataGridView1.AlternatingRowsDefaultCellStyle.SelectionForeColor = normalFg;

            // Desliga a inserção de dados até que um botão seja clicado
            SetEditMode(false);
        };

        // Atalhos
        KeyPreview = true;
        KeyDown += ProdutosForm_KeyDown;
    }

    private void ProdutosForm_KeyDown(object? sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.F2) BtnNovo_Click(this, System.EventArgs.Empty);
        else if (e.KeyCode == Keys.F5) BtnAtualizar_Click(this, System.EventArgs.Empty);
        else if (e.Control && e.KeyCode == Keys.S) BtnSalvar_Click(this, System.EventArgs.Empty);
        else if (e.KeyCode == Keys.Delete) BtnExcluir_Click(this, System.EventArgs.Empty);
    }

    private void BtnVoltar_Click(object? sender, System.EventArgs e) => Close();

    // --------- DATA ----------
    private async Task CarregarGrid()
    {
        await DbSync.Gate.WaitAsync();  // << trocado (era _dbLock)
        try
        {
            var lista = await _repo.GetAllAsync();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            var cId = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Produto.Id),
                HeaderText = "Id",
                Name = "Id",
                Visible = false
            };

            var cDes = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Produto.Descricao),
                HeaderText = "Descrição",
                Name = "Descricao",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 42
            };

            var cVal = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Produto.Valor),
                HeaderText = "Valor",
                Name = "Valor",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 18
            };
            cVal.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            cVal.DefaultCellStyle.Format = "C2";

            var cQtd = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Produto.Quantidade),
                HeaderText = "Quantidade",
                Name = "Quantidade",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 12
            };
            cQtd.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            cQtd.DefaultCellStyle.Format = "N0";

            var cObs = new DataGridViewTextBoxColumn
            {
                DataPropertyName = nameof(Produto.Observacao),
                HeaderText = "Observação",
                Name = "Observacao",
                AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                FillWeight = 28
            };

            dataGridView1.Columns.AddRange(cId, cDes, cVal, cQtd, cObs);

            // Impede o destaque azul ao clicar nas células
            dataGridView1.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
            dataGridView1.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;
            dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = dataGridView1.DefaultCellStyle.BackColor;
            dataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = dataGridView1.DefaultCellStyle.ForeColor;
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.ColumnHeadersDefaultCellStyle.SelectionBackColor = dataGridView1.ColumnHeadersDefaultCellStyle.BackColor;

            // Estilo de seleção de linhas
            dataGridView1.EnableHeadersVisualStyles = false;
            dataGridView1.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 224, 224); // click (cinza leve)
            dataGridView1.DefaultCellStyle.SelectionForeColor = Color.Black;
            dataGridView1.RowTemplate.DefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 224, 224);
            dataGridView1.RowTemplate.DefaultCellStyle.SelectionForeColor = Color.Black;

            // Cor de fundo padrão
            dataGridView1.DefaultCellStyle.BackColor = Color.White;
            dataGridView1.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245); // leve listrado

            // Remover azul padrão
            dataGridView1.RowsDefaultCellStyle.SelectionBackColor = Color.FromArgb(224, 224, 224);
            dataGridView1.RowsDefaultCellStyle.SelectionForeColor = Color.Black;

            // Remove o contorno pontilhado de foco (focus rectangle)
            dataGridView1.RowPrePaint += (s, e) =>
            {
                e.PaintParts &= ~DataGridViewPaintParts.Focus;
            };

            dataGridView1.CellPainting += (s, e) =>
            {
                if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
                {
                    var parts = e.PaintParts & ~DataGridViewPaintParts.Focus;
                    e.Paint(e.ClipBounds, parts);
                    e.Handled = true;
                }
            };

            // Hover
            dataGridView1.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            };
            dataGridView1.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = (e.RowIndex % 2 == 0)
                        ? Color.White
                        : Color.FromArgb(245, 245, 245);
            };

            dataGridView1.DataSource = lista
                .Select(p => new { p.Id, p.Descricao, p.Valor, p.Quantidade, p.Observacao })
                .ToList();

            dataGridView1.ClearSelection();
            EnableAllIdle();
        }
        finally
        {
            DbSync.Gate.Release();        // << trocado (era _dbLock)
        }
    }

    private static bool TryParseMoeda(string? texto, out decimal valor)
    {
        var br = new CultureInfo("pt-BR");
        var en = CultureInfo.InvariantCulture;
        var t = (texto ?? "").Trim();
        return decimal.TryParse(t, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, br, out valor)
            || decimal.TryParse(t, NumberStyles.Number | NumberStyles.AllowCurrencySymbol, en, out valor);
    }

    private void Limpar()
    {
        Descrição.Text = string.Empty;
        Valor.Text = string.Empty;
        Observação.Text = string.Empty;
        Quantidade.Value = 0;
        _editId = null;
        Salvar.Text = "Salvar (Ctrl+S)";
        SetEditMode(false);
    }

    // Habilita/Desabilita os campos de edição e o botão Salvar
    private void SetEditMode(bool on)
    {
        Descrição.ReadOnly = !on;
        Valor.ReadOnly = !on;            // MaskedTextBox suporta ReadOnly
        Quantidade.Enabled = on;         // melhor que ReadOnly para NumericUpDown
        Observação.ReadOnly = !on;

        Salvar.Enabled = on;

        // feedback visual leve (opcional)
        Descrição.BackColor = on ? Color.White : Color.FromArgb(245, 245, 245);
        Valor.BackColor = on ? Color.White : Color.FromArgb(245, 245, 245);
        Observação.BackColor = on ? Color.White : Color.FromArgb(245, 245, 245);
    }

    // --------- AÇÕES ----------
    private void BtnNovo_Click(object? sender, EventArgs e)
    {
        Limpar();
        BeginEditMode();
        dataGridView1.ClearSelection();
    }

    private async void BtnSalvar_Click(object? sender, System.EventArgs e)
    {
        if (!_isEditing) return;

        Salvar.Enabled = false;

        try
        {
            if (string.IsNullOrWhiteSpace(Descrição.Text))
            {
                MessageBox.Show("Descrição é obrigatória."); return;
            }
            if (!TryParseMoeda(Valor.Text, out var valor) || valor < 0)
            {
                MessageBox.Show("Valor inválido."); return;
            }

            var qtd = (int)Quantidade.Value;

            if (_editId is int id) // edição
            {
                var entidade = await _repo.GetByIdAsync(id);
                if (entidade is null)
                {
                    MessageBox.Show("Registro não encontrado para edição.");
                    return;
                }

                entidade.Descricao = Descrição.Text.Trim();
                entidade.Valor = valor;
                entidade.Quantidade = qtd;
                entidade.Observacao = string.IsNullOrWhiteSpace(Observação.Text) ? null : Observação.Text.Trim();

                await _repo.UpdateAsync(entidade);
                await _repo.SaveChangesAsync();

                MessageBox.Show("Produto atualizado!");
            }
            else // inserção
            {
                var novo = new Produto
                {
                    Descricao = Descrição.Text.Trim(),
                    Valor = valor,
                    Quantidade = qtd,
                    Observacao = string.IsNullOrWhiteSpace(Observação.Text) ? null : Observação.Text.Trim()
                };

                await _repo.AddAsync(novo);
                await _repo.SaveChangesAsync();
                MessageBox.Show("Produto salvo!");
            }

            // Notifica Vendas (FORA de lock)
            AppEvents.RaiseProdutosAlterados();

            await CarregarGrid();
            Limpar();
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Erro ao salvar: {ex.Message}");
        }
        finally
        {
            EnableAllIdle();
        }
    }

    private async void BtnExcluir_Click(object? sender, EventArgs e)
    {
        if (!Excluir.Enabled) return;

        Excluir.Enabled = Novo.Enabled = Salvar.Enabled = Editar.Enabled = false;

        if (dataGridView1.CurrentRow == null)
        {
            MessageBox.Show("Selecione um produto.");
            Excluir.Enabled = Novo.Enabled = Salvar.Enabled = Editar.Enabled = true;
            return;
        }

        if (MessageBox.Show("Confirmar exclusão?", "Excluir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
        {
            Excluir.Enabled = Novo.Enabled = Salvar.Enabled = Editar.Enabled = true;
            return;
        }

        var id = Convert.ToInt32(dataGridView1.CurrentRow.Cells["Id"].Value);

        // Exclusão COM lock global
        await DbSync.Gate.WaitAsync();   // << trocado (era _dbLock)
        try
        {
            var entidade = await _repo.GetByIdAsync(id);
            if (entidade != null)
            {
                await _repo.DeleteAsync(entidade);
                await _repo.SaveChangesAsync();
            }
        }
        finally
        {
            DbSync.Gate.Release();       // << trocado (era _dbLock)
        }

        // Notifica Vendas (FORA do lock)
        AppEvents.RaiseProdutosAlterados();

        // Atualiza UI
        await CarregarGrid();
        Limpar();
        EnableAllIdle();
    }

    private void BtnAtualizar_Click(object? sender, EventArgs e)
    {
        if (dataGridView1.CurrentRow == null)
        {
            MessageBox.Show("Selecione um produto na lista.");
            return;
        }

        var cellId = dataGridView1.CurrentRow.Cells["Id"]?.Value;
        if (cellId == null || !int.TryParse(Convert.ToString(cellId), out var id))
        {
            MessageBox.Show("Não foi possível identificar o registro selecionado.");
            return;
        }

        _editId = id;
        Descrição.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["Descricao"]?.Value) ?? string.Empty;
        Valor.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["Valor"]?.Value) ?? string.Empty;
        Observação.Text = Convert.ToString(dataGridView1.CurrentRow.Cells["Observacao"]?.Value) ?? string.Empty;

        var qtdCell = dataGridView1.CurrentRow.Cells["Quantidade"]?.Value;
        Quantidade.Value = (qtdCell != null && int.TryParse(Convert.ToString(qtdCell), out var q)) ? q : 0;

        Salvar.Text = "Atualizar (Ctrl+S)";

        BeginEditMode();
        Descrição.Focus();
    }

    // compat Designer
    private void TextBox1_TextChanged(object? sender, System.EventArgs e) { }
}
