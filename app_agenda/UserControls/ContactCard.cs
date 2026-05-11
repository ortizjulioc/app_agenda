using System;
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

        private static readonly Color DarkAccent = Color.FromArgb(45, 52, 71);
        private static readonly Color CardBlue = ColorTranslator.FromHtml("#8DB3E2");
        private static readonly Color CardHover = ColorTranslator.FromHtml("#7A9FCF");
        private static readonly Color PageBg = ColorTranslator.FromHtml("#F0F2F5");

        public ContactCard(Contact contact, Action<int> onFavoriteToggle, Action<int> onEdit, Action<int> onDelete)
        {
            _contact = contact;
            _onFavoriteToggle = onFavoriteToggle;
            _onEdit = onEdit;
            _onDelete = onDelete;

            this.Size = new Size(620, 70);
            this.Margin = new Padding(10, 5, 10, 5);
            this.BackColor = PageBg;
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, false);

            // 1. Botón de Favorito 
            _iconFavorite = new IconPictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(5, 15),
                IconChar = IconChar.Heart,
                IconSize = 32,
                ForeColor = contact.IsFavorite ? Color.Red : Color.FromArgb(150, 150, 150),
                BackColor = PageBg, 
                Cursor = Cursors.Hand
            };
            _iconFavorite.Click += (s, e) => _onFavoriteToggle(_contact.Id);

            // 2. Panel Central
            _cardPanel = new RoundedPanel
            {
                Size = new Size(480, 50),
                Location = new Point(50, 10),
                BackColor = CardBlue,
                CornerRadius = 25
            };

            // 3. Etiquetas de texto
            _lblName = new Label
            {
                Text = contact.Name.Replace("\r", "").Replace("\n", ""),
                Location = new Point(20, 10), 
                Size = new Size(180, 30),    
                ForeColor = Color.White,
                Font = new Font("Segoe UI Semibold", 13),
                AutoSize = false,
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = CardBlue,
                UseCompatibleTextRendering = true
            };

            _lblPhone = new Label
            {
                Text = contact.PhoneNumber,
                Location = new Point(210, 10), 
                Size = new Size(250, 30),     
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14f),
                AutoSize = false,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = CardBlue,
                UseCompatibleTextRendering = true
            };

            // 4. Botones Laterales
            _btnEdit = CreateActionButton(IconChar.PencilAlt, DarkAccent, 540);
            _btnEdit.Click += (s, e) => _onEdit(_contact.Id);

            _btnDelete = CreateActionButton(IconChar.TrashAlt, Color.FromArgb(230, 70, 70), 580);
            _btnDelete.Click += (s, e) => _onDelete(_contact.Id);

            _cardPanel.Controls.Add(_lblPhone);
            _cardPanel.Controls.Add(_lblName);

            this.Controls.Add(_iconFavorite);
            this.Controls.Add(_cardPanel);
            this.Controls.Add(_btnEdit);
            this.Controls.Add(_btnDelete);

            _cardPanel.MouseEnter += (s, e) => UpdateColors(CardHover);
            _cardPanel.MouseLeave += (s, e) => UpdateColors(CardBlue);

            _lblName.MouseEnter += (s, e) => UpdateColors(CardHover);
            _lblPhone.MouseEnter += (s, e) => UpdateColors(CardHover);
        }

        private void UpdateColors(Color color)
        {
            _cardPanel.BackColor = color;
            _lblName.BackColor = color;
            _lblPhone.BackColor = color;
        }

        private IconButton CreateActionButton(IconChar icon, Color color, int xPos)
        {
            return new IconButton
            {
                Size = new Size(35, 35),
                Location = new Point(xPos, 18),
                IconChar = icon,
                IconColor = color,
                IconSize = 24,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0, MouseOverBackColor = Color.FromArgb(225, 225, 230) },
                Cursor = Cursors.Hand,
                BackColor = PageBg 
            };
        }

        public void UpdateFavorite(bool isFavorite)
        {
            _contact.IsFavorite = isFavorite;
            _iconFavorite.ForeColor = isFavorite ? Color.Red : Color.FromArgb(150, 150, 150);
        }

        private class RoundedPanel : Panel
        {
            [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
            public int CornerRadius { get; set; } = 25;

            public RoundedPanel()
            {
                this.DoubleBuffered = true;
            }

            protected override void OnPaint(PaintEventArgs e)
            {
                base.OnPaint(e);
                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                using var path = GetRoundRectPath(ClientRectangle, CornerRadius);
                this.Region = new Region(path);
            }

            private GraphicsPath GetRoundRectPath(Rectangle rect, int radius)
            {
                GraphicsPath path = new GraphicsPath();
                float d = radius * 2F;
                if (d <= 0) d = 1;
                path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                path.CloseFigure();
                return path;
            }
        }
    }
}