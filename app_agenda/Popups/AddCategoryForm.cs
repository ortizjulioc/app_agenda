using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace app_agenda.UI.Popups
{
    public class AddCategoryForm : Form
    {
        public string CategoryName { get; private set; } = string.Empty;
        public string IconCode { get; private set; } = string.Empty;

        private TextBox _txtName;
        private FlowLayoutPanel _flpIcons;
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

            SetupForm();
        }

        private void SetupForm()
        {
            Text = "Nueva Categoría";
            Size = new Size(320, 280);
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
            StartPosition = FormStartPosition.CenterParent;
            ShadowForm();

            var lblTitle = new Label
            {
                Text = "Nueva Categoría",
                Location = new Point(20, 20),
                Size = new Size(280, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#249EA0")
            };

            var lblName = new Label
            {
                Text = "Nombre:",
                Location = new Point(20, 65),
                Size = new Size(280, 20),
                Font = new Font("Segoe UI", 10)
            };

            _txtName = new TextBox
            {
                Location = new Point(20, 87),
                Size = new Size(280, 28),
                Font = new Font("Segoe UI", 11),
                BorderStyle = BorderStyle.FixedSingle
            };

            var lblIcon = new Label
            {
                Text = "Icono:",
                Location = new Point(20, 125),
                Size = new Size(280, 20),
                Font = new Font("Segoe UI", 10)
            };

            _flpIcons = new FlowLayoutPanel
            {
                Location = new Point(20, 147),
                Size = new Size(280, 60),
                FlowDirection = FlowDirection.LeftToRight,
                BackColor = ColorTranslator.FromHtml("#F0F2F5")
            };

            foreach (var icon in _availableIcons)
            {
                var btn = CreateIconButton(icon.Key, icon.Value);
                _flpIcons.Controls.Add(btn);
            }

            _selectedIcon = new IconPictureBox
            {
                Location = new Point(265, 147),
                Size = new Size(30, 30),
                IconChar = IconChar.Check,
                IconColor = ColorTranslator.FromHtml("#249EA0")
            };
            _flpIcons.Controls.Add(_selectedIcon);

            var btnSave = new Button
            {
                Text = "Guardar",
                Location = new Point(20, 218),
                Size = new Size(130, 36),
                BackColor = ColorTranslator.FromHtml("#249EA0"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11)
            };
            btnSave.Click += BtnSave_Click;

            var btnCancel = new Button
            {
                Text = "Cancelar",
                Location = new Point(170, 218),
                Size = new Size(130, 36),
                BackColor = Color.White,
                ForeColor = ColorTranslator.FromHtml("#249EA0"),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11)
            };
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[] { lblTitle, lblName, _txtName, lblIcon, _flpIcons, btnSave, btnCancel });

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

        private void ShadowForm()
        {
            var shadow = new Panel
            {
                Dock = DockStyle.None,
                Size = new Size(320, 280),
                Location = new Point(3, 3),
                BackColor = Color.FromArgb(80, 0, 0, 0)
            };
        }
    }
}