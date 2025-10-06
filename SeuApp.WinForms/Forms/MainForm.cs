using Microsoft.Extensions.DependencyInjection;
using ReaLTaiizor.Controls;

namespace SeuApp.WinForms.Forms
{
    public partial class MainForm : Form
    {
        private readonly IServiceProvider _provider;
        private ProdutosForm? _produtosForm;
        private VendasForm? _vendasForm;

        public MainForm(IServiceProvider provider)
        {
            InitializeComponent();
            _provider = provider;

            // Tema (true = dark, false = light)
            UiTheme.Apply(this, dark: false);

            // Deixa botões com cara moderna
            UiTheme.StyleButton(BtnProdutos);
            UiTheme.StyleButton(BtnVendas);
        }

        private void BtnProdutos_Click(object sender, EventArgs e)
        {
            if (_produtosForm == null || _produtosForm.IsDisposed)
            {
                _produtosForm = _provider.GetRequiredService<ProdutosForm>();
                _produtosForm.FormClosed += (_, __) => _produtosForm = null;
                _produtosForm.Show(this);
            }
            else
            {
                _produtosForm.BringToFront();
                _produtosForm.Focus();
            }
        }

        private void BtnVendas_Click(object sender, EventArgs e)
        {
            if (_vendasForm == null || _vendasForm.IsDisposed)
            {
                _vendasForm = _provider.GetRequiredService<VendasForm>();
                _vendasForm.FormClosed += (_, __) => _vendasForm = null;
                _vendasForm.Show(this);
            }
            else
            {
                _vendasForm.BringToFront();
                _vendasForm.Focus();
            }
        }
    }
}