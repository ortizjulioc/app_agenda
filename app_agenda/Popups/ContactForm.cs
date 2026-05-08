using System;
using System.Collections.Generic;
using System.Windows.Forms;
using app_agenda.Data.Models;
using app_agenda.UI.Services;

namespace app_agenda.UI.Popups
{
    public partial class ContactForm : Form
    {
        public Contact Contact { get; private set; } = null!;

        private readonly bool _isEdit;
        private readonly int _userId;
        private readonly Contact? _original;
        private readonly List<Category> _categories;

        public ContactForm(int userId, Contact? contact = null)
        {
            _isEdit = contact != null;
            _userId = userId;
            _original = contact;
            _categories = new CategoryService().GetCategoriesByUser(userId);

            InitializeComponent();

            lblTitle.Text = _isEdit ? "Editar Contacto" : "Nuevo Contacto";
            Text = lblTitle.Text;

            foreach (var cat in _categories)
                _cmbCategory.Items.Add(new ComboBoxItem(cat.Id, cat.Name));

            if (_isEdit && contact != null)
            {
                _txtName.Text = contact.Name;
                _txtPhone.Text = contact.PhoneNumber;
                _chkFavorite.Checked = contact.IsFavorite;
                SelectCategory(contact.CategoryId);
            }
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

        private void BtnCancel_Click(object? sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
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
                Id = _isEdit && _original != null ? _original.Id : 0,
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
