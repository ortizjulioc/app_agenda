using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using app_agenda.Data.Models;

namespace app_agenda.UI.Popups
{
    public partial class AddCategoryForm : Form
    {
        public string CategoryName { get; private set; } = string.Empty;
        public string IconCode { get; private set; } = string.Empty;

        private string _selectedIconCode = "Folder";
        private readonly Dictionary<string, Panel> _iconPanels = new();
        private readonly List<KeyValuePair<string, IconChar>> _availableIcons;
        private readonly bool _isEdit;

        private static readonly Color SelectedBg = ColorTranslator.FromHtml("#249EA0");

        public AddCategoryForm(Category? existing = null)
        {
            _isEdit = existing != null;
            _availableIcons = new List<KeyValuePair<string, IconChar>>
            {
                new("Folder", IconChar.Folder),
                new("FolderOpen", IconChar.FolderOpen),
                new("User", IconChar.User),
                new("Users", IconChar.Users),
                new("Briefcase", IconChar.Briefcase),
                new("House", IconChar.House),
                new("Building", IconChar.Building),
                new("Phone", IconChar.Phone),
                new("Envelope", IconChar.Envelope),
                new("Heart", IconChar.Heart),
                new("Star", IconChar.Star),
                new("Camera", IconChar.Camera),
                new("Music", IconChar.Music),
                new("Coffee", IconChar.Coffee),
                new("Utensils", IconChar.Utensils),
                new("Plane", IconChar.Plane),
                new("Car", IconChar.Car),
                new("Bicycle", IconChar.Bicycle),
                new("Dumbbell", IconChar.Dumbbell),
                new("Book", IconChar.Book),
                new("GraduationCap", IconChar.GraduationCap),
                new("Globe", IconChar.Globe),
                new("Calendar", IconChar.Calendar),
                new("Bell", IconChar.Bell),
                new("Gift", IconChar.Gift),
                new("Tag", IconChar.Tag),
                new("Trophy", IconChar.Trophy),
                new("Dog", IconChar.Dog),
                new("Cat", IconChar.Cat),
                new("Lightbulb", IconChar.Lightbulb),
                new("CreditCard", IconChar.CreditCard),
                new("Hospital", IconChar.Hospital),
            };

            InitializeComponent();

            foreach (var icon in _availableIcons)
            {
                var p = CreateIconButton(icon.Key, icon.Value);
                _iconPanels[icon.Key] = p;
                _flpIcons.Controls.Add(p);
            }

            _txtName.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter) BtnSave_Click(s, e);
            };

            if (_isEdit && existing != null)
            {
                lblTitle.Text = "Editar Categoría";
                Text = lblTitle.Text;
                _txtName.Text = existing.Name;
                _selectedIconCode = existing.IconCode;
            }

            HighlightIcon(_selectedIconCode);
        }

        private Panel CreateIconButton(string code, IconChar iconChar)
        {
            var panel = new Panel
            {
                Size = new Size(34, 34),
                Tag = code,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Margin = new Padding(2)
            };

            var iconBox = new IconPictureBox
            {
                Size = new Size(24, 24),
                Location = new Point(5, 5),
                IconChar = iconChar,
                IconColor = Color.Black,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };

            panel.Click += (s, e) => HighlightIcon(code);
            iconBox.MouseClick += (s, e) => HighlightIcon(code);

            panel.Controls.Add(iconBox);
            return panel;
        }

        private void HighlightIcon(string code)
        {
            _selectedIconCode = code;
            foreach (var kvp in _iconPanels)
            {
                bool selected = kvp.Key == code;
                kvp.Value.BackColor = selected ? SelectedBg : Color.Transparent;
                if (kvp.Value.Controls.Count > 0 && kvp.Value.Controls[0] is IconPictureBox box)
                    box.IconColor = selected ? Color.White : Color.Black;
            }
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
