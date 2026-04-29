using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace app_agenda.UI.Popups
{
    public partial class AddCategoryForm : Form
    {
        public string CategoryName { get; private set; } = string.Empty;
        public string IconCode { get; private set; } = string.Empty;

        private IconPictureBox _selectedIcon;
        private string _selectedIconCode = "Folder";
        private readonly List<KeyValuePair<string, IconChar>> _availableIcons;

        public AddCategoryForm()
        {
            _availableIcons = new List<KeyValuePair<string, IconChar>>
            {
                new("Folder", IconChar.Folder),
                new("User", IconChar.User),
                new("Briefcase", IconChar.Briefcase),
                new("Users", IconChar.Users),
                new("Phone", IconChar.Phone),
                new("Heart", IconChar.Heart),
                new("Star", IconChar.Star),
                new("Home", IconChar.House)
            };

            InitializeComponent();

            foreach (var icon in _availableIcons)
                _flpIcons.Controls.Add(CreateIconButton(icon.Key, icon.Value));

            _selectedIcon = new IconPictureBox
            {
                Size = new Size(30, 30),
                IconChar = IconChar.Check,
                IconColor = ColorTranslator.FromHtml("#249EA0")
            };
            _flpIcons.Controls.Add(_selectedIcon);

            _txtName.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) BtnSave_Click(s, e);
            };
        }

        private Panel CreateIconButton(string code, IconChar iconChar)
        {
            var panel = new Panel { Size = new Size(32, 32), Tag = code };

            var iconBox = new IconPictureBox
            {
                Size = new Size(28, 28),
                IconChar = iconChar,
                IconColor = Color.Black,
                Cursor = Cursors.Hand
            };
            iconBox.MouseClick += (s, e) =>
            {
                if (s is IconPictureBox box)
                {
                    _selectedIconCode = code;
                    _selectedIcon.IconChar = box.IconChar;
                }
            };

            panel.Controls.Add(iconBox);
            return panel;
        }

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtName.Text))
            {
                MessageBox.Show("Ingrese un nombre para la categoría.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            CategoryName = _txtName.Text.Trim();
            IconCode = _selectedIconCode;
            DialogResult = DialogResult.OK;
        }

        private void AddCategoryForm_Load(object sender, EventArgs e)
        {

        }
    }
}
