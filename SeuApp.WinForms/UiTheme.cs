using System;
using System.Drawing;
using System.Windows.Forms;

namespace SeuApp.WinForms.Forms
{
    public static class UiTheme
    {
        // Paleta base (claro/escuro)
        private static readonly Color LightBack = Color.FromArgb(246, 248, 250);
        private static readonly Color LightCard = Color.White;
        private static readonly Color LightText = Color.FromArgb(30, 41, 59);
        private static readonly Color LightSubt = Color.FromArgb(100, 116, 139);

        private static readonly Color DarkBack = Color.FromArgb(18, 22, 28);
        private static readonly Color DarkCard = Color.FromArgb(28, 33, 40);
        private static readonly Color DarkText = Color.FromArgb(229, 231, 235);
        private static readonly Color DarkSubt = Color.FromArgb(148, 163, 184);

        // Cores por intenção
        private static readonly Color Primary = Color.FromArgb(59, 130, 246);   // azul
        private static readonly Color PrimaryAlt = Color.FromArgb(37, 99, 235);

        private static readonly Color Success = Color.FromArgb(16, 185, 129);   // verde
        private static readonly Color SuccessAlt = Color.FromArgb(5, 150, 105);

        private static readonly Color Danger = Color.FromArgb(239, 68, 68);    // vermelho
        private static readonly Color DangerAlt = Color.FromArgb(220, 38, 38);

        public static void Apply(Form form, bool dark = false)
        {
            var bg = dark ? DarkBack : LightBack;
            var fg = dark ? DarkText : LightText;
            var card = dark ? DarkCard : LightCard;

            form.BackColor = bg;
            form.ForeColor = fg;
            form.Font = new Font("Segoe UI", 10F, FontStyle.Regular, GraphicsUnit.Point);
            form.Padding = new Padding(12);

            foreach (Control c in form.Controls)
                PaintControl(c, dark, card, fg);
        }

        private static void PaintControl(Control c, bool dark, Color card, Color fg)
        {
            switch (c)
            {
                case Panel p:
                    p.BackColor = card; break;
                case GroupBox g:
                    g.BackColor = card; g.ForeColor = fg; g.Padding = new Padding(10); break;
                case Label lb:
                    StyleLabel(lb); break;
                case Button b:
                    break;
                case TextBox tb:
                    StyleTextBox(tb); break;
                case MaskedTextBox mtb:
                    StyleMasked(mtb); break;
                case DataGridView grid:
                    StyleGrid(grid, dark); break;
            }

            foreach (Control child in c.Controls)
                PaintControl(child, dark, card, fg);
        }

        public static void StyleLabel(Label l)
        {
            l.AutoSize = true;
            l.Margin = new Padding(0, 6, 0, 4);
            l.ForeColor = LightSubt;
        }

        public static void StyleTextBox(TextBox t)
        {
            t.BorderStyle = BorderStyle.FixedSingle;
            t.BackColor = Color.White;
            t.ForeColor = LightText;
            t.Margin = new Padding(0, 0, 8, 0);
        }

        public static void StyleMasked(MaskedTextBox t)
        {
            t.BorderStyle = BorderStyle.FixedSingle;
            t.BackColor = Color.White;
            t.ForeColor = LightText;
            t.Margin = new Padding(0, 0, 8, 0);
        }

        // ===== Botões =====

        // Mantido por compatibilidade: "StyleButton" = botão primário (azul)
        public static void StyleButton(Button b) => StyleButtonPrimary(b);

        public static void StyleButtonPrimary(Button b) => StyleSolid(b, Primary, PrimaryAlt);
        public static void StyleButtonSuccess(Button b) => StyleSolid(b, Success, SuccessAlt);
        public static void StyleButtonDanger(Button b) => StyleSolid(b, Danger, DangerAlt);

        public static void StyleButtonOutline(Button b, Color? color = null)
        {
            var c = color ?? Primary;
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 1;
            b.FlatAppearance.BorderColor = c;
            b.BackColor = Color.Transparent;
            b.ForeColor = c;
            b.Cursor = Cursors.Hand;
            b.Height = Math.Max(b.Height, 34);
            b.Padding = new Padding(10, 4, 10, 4);
            b.MouseEnter += (_, __) => b.BackColor = Color.FromArgb(245, 249, 255);
            b.MouseLeave += (_, __) => b.BackColor = Color.Transparent;
        }

        private static void StyleSolid(Button b, Color baseC, Color hoverC)
        {
            b.FlatStyle = FlatStyle.Flat;
            b.FlatAppearance.BorderSize = 0;
            b.BackColor = baseC;
            b.ForeColor = Color.White;
            b.Cursor = Cursors.Hand;
            b.Height = Math.Max(b.Height, 34);
            b.Padding = new Padding(10, 4, 10, 4);
            b.MouseEnter += (_, __) => b.BackColor = hoverC;
            b.MouseLeave += (_, __) => b.BackColor = baseC;
        }

        public static void StyleGrid(DataGridView g, bool dark = false)
        {
            g.EnableHeadersVisualStyles = false;
            g.BackgroundColor = dark ? DarkCard : LightCard;
            g.BorderStyle = BorderStyle.None;
            g.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            g.ColumnHeadersDefaultCellStyle.BackColor = dark ? DarkCard : Color.FromArgb(244, 246, 248);
            g.ColumnHeadersDefaultCellStyle.ForeColor = dark ? DarkText : LightText;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI Semibold", 10F);
            g.ColumnHeadersDefaultCellStyle.Padding = new Padding(6, 8, 6, 8);

            g.DefaultCellStyle.BackColor = dark ? DarkCard : Color.White;
            g.DefaultCellStyle.ForeColor = dark ? DarkText : LightText;
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(219, 234, 254);
            g.DefaultCellStyle.SelectionForeColor = LightText;
            g.RowTemplate.Height = 30;
            g.GridColor = Color.FromArgb(229, 231, 235);

            g.AlternatingRowsDefaultCellStyle.BackColor = dark
                ? Color.FromArgb(32, 38, 46)
                : Color.FromArgb(250, 250, 250);
        }
    }
}
