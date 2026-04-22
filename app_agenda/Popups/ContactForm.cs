using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using app_agenda.Data.Models;
using app_agenda.UI.Services;
using FontAwesome.Sharp;

namespace app_agenda.UI.Popups
{
    public class ContactForm : Form
    {
        public Contact Contact { get; private set; } = null!;

        private readonly bool _isEdit;
        private readonly int _userId;
        private readonly List<Category> _categories;

        private TextBox _txtName;
        private TextBox _txtPhone;
        private ComboBox _cmbCategory;
        private CheckBox _chkFavorite;

        public ContactForm(int userId, Contact? contact = null)
        {
            _isEdit = contact != null;
            _userId = userId;
            _categories = new CategoryService().GetCategoriesByUser(userId);

            SetupForm();

            if (_isEdit && contact != null)
            {
                _txtName.Text = contact.Name;
                _txtPhone.Text = contact.PhoneNumber;
                _chkFavorite.Checked = contact.IsFavorite;
                SelectCategory(contact.CategoryId);
            }
        }

        private void SetupForm()
        {
            Text = _isEdit ? "Editar Contacto" : "Nuevo Contacto";
            Size = new Size(360, 320);
            FormBorderStyle = FormBorderStyle.None;
            BackColor = Color.White;
            StartPosition = FormStartPosition.CenterParent;

            var lblTitle = new Label
            {
                Text = Text,
                Location = new Point(20, 20),
                Size = new Size(320, 30),
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = ColorTranslator.FromHtml("#249EA0")
            };

            var lblName = new Label { Text = "Nombre:", Location = new Point(20, 65), Size = new Size(320, 20), Font = new Font("Segoe UI", 10) };
            _txtName = new TextBox { Location = new Point(20, 87), Size = new Size(320, 28), Font = new Font("Segoe UI", 11), BorderStyle = BorderStyle.FixedSingle };

            var lblPhone = new Label { Text = "Teléfono:", Location = new Point(20, 125), Size = new Size(320, 20), Font = new Font("Segoe UI", 10) };
            _txtPhone = new TextBox { Location = new Point(20, 147), Size = new Size(320, 28), Font = new Font("Segoe UI", 11), BorderStyle = BorderStyle.FixedSingle };

            var lblCategory = new Label { Text = "Categoría:", Location = new Point(20, 185), Size = new Size(320, 20), Font = new Font("Segoe UI", 10) };
            _cmbCategory = new ComboBox { Location = new Point(20, 207), Size = new Size(320, 28), DropDownStyle = ComboBoxStyle.DropDownList };
            foreach (var cat in _categories)
                _cmbCategory.Items.Add(new ComboBoxItem(cat.Id, cat.Name));

            _chkFavorite = new CheckBox
            {
                Text = "Marcar como favorito",
                Location = new Point(20, 242),
                Size = new Size(200, 20),
                Font = new Font("Segoe UI", 10)
            };

            var btnSave = new Button
            {
                Text = "Guardar",
                Location = new Point(20, 270),
                Size = new Size(150, 36),
                BackColor = ColorTranslator.FromHtml("#249EA0"),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11)
            };
            btnSave.Click += BtnSave_Click;

            var btnCancel = new Button
            {
                Text = "Cancelar",
                Location = new Point(190, 270),
                Size = new Size(150, 36),
                BackColor = Color.White,
                ForeColor = ColorTranslator.FromHtml("#249EA0"),
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 11)
            };
            btnCancel.Click += (s, e) => DialogResult = DialogResult.Cancel;

            Controls.AddRange(new Control[] { lblTitle, lblName, _txtName, lblPhone, _txtPhone, lblCategory, _cmbCategory, _chkFavorite, btnSave, btnCancel });
        }

        private void SelectCategory(int categoryId)
        {
            for (int i = 0; i < _cmbCategory.Items.Count; i++)
            {
                if (_cmbCategory.Items[i] is ComboBoxItem item && item.Id == categoryId)
                {
                    _cmbCategory.SelectedIndex = i;
                    break;
                }
            }
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(_txtName.Text) || string.IsNullOrWhiteSpace(_txtPhone.Text))
            {
                MessageBox.Show("Complete todos los campos obligatorios.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (_cmbCategory.SelectedItem is not ComboBoxItem selectedCat)
            {
                MessageBox.Show("Seleccione una categoría.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            Contact = new Contact
            {
                Id = _isEdit ? Contact.Id : 0,
                Name = _txtName.Text.Trim(),
                PhoneNumber = _txtPhone.Text.Trim(),
                CategoryId = selectedCat.Id,
                UserId = _userId,
                IsFavorite = _chkFavorite.Checked
            };

            DialogResult = DialogResult.OK;
        }

        private class ComboBoxItem
        {
            public int Id { get; }
            public string Name { get; }
            public ComboBoxItem(int id, string name) { Id = id; Name = name; }
            public override string ToString() => Name;
        }
    }
}