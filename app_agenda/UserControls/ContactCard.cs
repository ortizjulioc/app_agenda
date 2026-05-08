using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using FontAwesome.Sharp;
using app_agenda.Data.Models;

namespace app_agenda.UI.UserControls
{
    public class ContactCard : UserControl
    {
        private readonly Contact _contact;
        private readonly Action<int> _onFavoriteToggle;
        private readonly Action<int> _onEdit;
        private readonly Action<int> _onDelete;

        private readonly RoundedPanel _cardPanel;
        private readonly IconPictureBox _iconFavorite;
        private readonly Label _lblName;
        private readonly Label _lblPhone;
        private readonly IconButton _btnEdit;
        private readonly IconButton _btnDelete;

        // Color azul oscuro usado para el teléfono y el ícono de editar
        private static readonly Color DarkAccent = Color.FromArgb(40, 40, 80);
        // Color del fondo de la página (igual que flpDisplay) — para el área transparente alrededor del card
        private static readonly Color PageBg = ColorTranslator.FromHtml("#F0F2F5");

        public ContactCard(Contact contact, Action<int> onFavoriteToggle, Action<int> onEdit, Action<int> onDelete)
        {
            _contact = contact;
            _onFavoriteToggle = onFavoriteToggle;
            _onEdit = onEdit;
            _onDelete = onDelete;

            Size = new Size(600, 64);
            Margin = new Padding(8, 4, 8, 4);
            BackColor = PageBg;

            // Corazón a la izquierda, FUERA del panel redondeado
            _iconFavorite = new IconPictureBox
            {
                Size = new Size(28, 28),
                Location = new Point(8, 18),
                IconChar = IconChar.Heart,
                IconSize = 26,
                ForeColor = contact.IsFavorite ? Color.Red : Color.FromArgb(120, 120, 120),
                BackColor = PageBg,
                Cursor = Cursors.Hand
            };
            _iconFavorite.MouseClick += (s, e) => _onFavoriteToggle(_contact.Id);

            // Panel redondeado azul claro (estilo "pill")
            _cardPanel = new RoundedPanel
            {
                Size = new Size(460, 48),
                Location = new Point(44, 8),
                BackColor = ColorTranslator.FromHtml("#8DB3E2"),
                CornerRadius = 24
            };

            // Nombre — grande, blanco, dentro del panel
            _lblName = new Label
            {
                Text = contact.Name,
                Location = new Point(22, 6),
                Size = new Size(190, 36),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                AutoSize = false,
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Teléfono — al lado del nombre, en azul oscuro
            _lblPhone = new Label
            {
                Text = contact.PhoneNumber,
                Location = new Point(220, 10),
                Size = new Size(225, 28),
                ForeColor = DarkAccent,
                Font = new Font("Segoe UI", 11),
                AutoSize = false,
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            // Botón Editar (lápiz) — FUERA del panel, color oscuro
            _btnEdit = new IconButton
            {
                Size = new Size(32, 32),
                Location = new Point(514, 16),
                IconChar = IconChar.Pencil,
                IconColor = DarkAccent,
                IconSize = 22,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                BackColor = PageBg
            };
            _btnEdit.MouseClick += (s, e) => _onEdit(_contact.Id);

            // Botón Eliminar (zafacón) — FUERA del panel, rojo
            _btnDelete = new IconButton
            {
                Size = new Size(32, 32),
                Location = new Point(552, 16),
                IconChar = IconChar.Trash,
                IconColor = Color.FromArgb(255, 82, 82),
                IconSize = 22,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                BackColor = PageBg
            };
            _btnDelete.MouseClick += (s, e) => _onDelete(_contact.Id);

            // Labels van adentro del panel redondeado
            _cardPanel.Controls.AddRange(new Control[] { _lblName, _lblPhone });

            // Heart, panel y botones van directo en el UserControl
            Controls.AddRange(new Control[] { _iconFavorite, _cardPanel, _btnEdit, _btnDelete });
        }

        public void UpdateFavorite(bool isFavorite)
        {
            _contact.IsFavorite = isFavorite;
            _iconFavorite.ForeColor = isFavorite ? Color.Red : Color.FromArgb(120, 120, 120);
        }

        /// <summary>
        /// Panel con esquinas redondeadas (usa Region para recortar el contorno).
        /// </summary>
        private class RoundedPanel : Panel
        {
            [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
            public int CornerRadius { get; set; } = 24;

            public RoundedPanel()
            {
                SetStyle(ControlStyles.ResizeRedraw | ControlStyles.OptimizedDoubleBuffer, true);
            }

            protected override void OnSizeChanged(EventArgs e)
            {
                base.OnSizeChanged(e);
                ApplyRoundedRegion();
            }

            protected override void OnHandleCreated(EventArgs e)
            {
                base.OnHandleCreated(e);
                ApplyRoundedRegion();
            }

            private void ApplyRoundedRegion()
            {
                if (Width <= 0 || Height <= 0) return;
                int d = Math.Min(CornerRadius * 2, Math.Min(Width, Height));
                using var path = new GraphicsPath();
                path.AddArc(0, 0, d, d, 180, 90);
                path.AddArc(Width - d - 1, 0, d, d, 270, 90);
                path.AddArc(Width - d - 1, Height - d - 1, d, d, 0, 90);
                path.AddArc(0, Height - d - 1, d, d, 90, 90);
                path.CloseFigure();
                Region = new Region(path);
            }
        }
    }
}
