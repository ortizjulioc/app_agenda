using System;
using System.Drawing;
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

        private readonly Panel _cardPanel;
        private readonly IconPictureBox _iconFavorite;
        private readonly Label _lblName;
        private readonly Label _lblPhone;
        private readonly IconButton _btnEdit;
        private readonly IconButton _btnDelete;

        public ContactCard(Contact contact, Action<int> onFavoriteToggle, Action<int> onEdit, Action<int> onDelete)
        {
            _contact = contact;
            _onFavoriteToggle = onFavoriteToggle;
            _onEdit = onEdit;
            _onDelete = onDelete;

            Size = new Size(600, 54);
            Margin = new Padding(8, 4, 8, 4);

            _cardPanel = new Panel
            {
                Size = new Size(584, 54),
                Location = new Point(0, 0),
                BackColor = ColorTranslator.FromHtml("#8DB3E2"),
                Padding = new Padding(12, 0, 12, 0)
            };

            _iconFavorite = new IconPictureBox
            {
                Size = new Size(24, 24),
                Location = new Point(10, 15),
                IconChar = contact.IsFavorite ? IconChar.Heart : IconChar.Heart,
                ForeColor = contact.IsFavorite ? Color.Red : Color.White,
                Cursor = Cursors.Hand
            };
            _iconFavorite.MouseClick += (s, e) => _onFavoriteToggle(_contact.Id);

            _lblName = new Label
            {
                Text = contact.Name,
                Location = new Point(46, 8),
                Size = new Size(200, 20),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                AutoSize = false,
                AutoEllipsis = true
            };

            _lblPhone = new Label
            {
                Text = contact.PhoneNumber,
                Location = new Point(46, 28),
                Size = new Size(150, 18),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 9),
                AutoSize = false,
                AutoEllipsis = true
            };

            // Configuración del botón Editar (Lápiz)
            _btnEdit = new IconButton
            {
                Size = new Size(32, 32),
                Location = new Point(510, 11),
                IconChar = IconChar.Pencil,
                IconColor = Color.White,
                IconSize = 20,           
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            _btnEdit.MouseClick += (s, e) => _onEdit(_contact.Id);

            // Configuración del botón Eliminar (Zafacón)
            _btnDelete = new IconButton
            {
                Size = new Size(32, 32),
                Location = new Point(548, 11),
                IconChar = IconChar.Trash,
                IconColor = Color.FromArgb(255, 82, 82),  
                IconSize = 20,          
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            _btnDelete.MouseClick += (s, e) => _onDelete(_contact.Id);

            _cardPanel.Controls.AddRange(new Control[] { _iconFavorite, _lblName, _lblPhone, _btnEdit, _btnDelete });
            Controls.Add(_cardPanel);
        }

        public void UpdateFavorite(bool isFavorite)
        {
            _contact.IsFavorite = isFavorite;
            _iconFavorite.ForeColor = isFavorite ? Color.Red : Color.White;
        }
    }
}