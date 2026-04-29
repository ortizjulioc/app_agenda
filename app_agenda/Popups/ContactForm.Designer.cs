using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace app_agenda.UI.Popups
{
    partial class ContactForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            lblTitle = new Label();
            lblName = new Label();
            _txtName = new TextBox();
            lblPhone = new Label();
            _txtPhone = new TextBox();
            lblCategory = new Label();
            _cmbCategory = new ComboBox();
            _chkFavorite = new CheckBox();
            btnSave = new Button();
            btnCancel = new Button();
            SuspendLayout();
            // 
            // lblTitle
            // 
            lblTitle.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblTitle.ForeColor = Color.FromArgb(36, 158, 160);
            lblTitle.Location = new Point(20, 20);
            lblTitle.Name = "lblTitle";
            lblTitle.Size = new Size(320, 30);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Nuevo Contacto";
            // 
            // lblName
            // 
            lblName.Font = new Font("Segoe UI", 10F);
            lblName.Location = new Point(20, 65);
            lblName.Name = "lblName";
            lblName.Size = new Size(320, 33);
            lblName.TabIndex = 1;
            lblName.Text = "Nombre:";
            // 
            // _txtName
            // 
            _txtName.BorderStyle = BorderStyle.FixedSingle;
            _txtName.Font = new Font("Segoe UI", 11F);
            _txtName.Location = new Point(20, 101);
            _txtName.Name = "_txtName";
            _txtName.Size = new Size(320, 37);
            _txtName.TabIndex = 2;
            // 
            // lblPhone
            // 
            lblPhone.Font = new Font("Segoe UI", 10F);
            lblPhone.Location = new Point(20, 141);
            lblPhone.Name = "lblPhone";
            lblPhone.Size = new Size(320, 33);
            lblPhone.TabIndex = 3;
            lblPhone.Text = "Teléfono:";
            // 
            // _txtPhone
            // 
            _txtPhone.BorderStyle = BorderStyle.FixedSingle;
            _txtPhone.Font = new Font("Segoe UI", 11F);
            _txtPhone.Location = new Point(20, 172);
            _txtPhone.Name = "_txtPhone";
            _txtPhone.Size = new Size(320, 37);
            _txtPhone.TabIndex = 4;
            // 
            // lblCategory
            // 
            lblCategory.Font = new Font("Segoe UI", 10F);
            lblCategory.Location = new Point(20, 215);
            lblCategory.Name = "lblCategory";
            lblCategory.Size = new Size(320, 34);
            lblCategory.TabIndex = 5;
            lblCategory.Text = "Categoría:";
            // 
            // _cmbCategory
            // 
            _cmbCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            _cmbCategory.Font = new Font("Segoe UI", 10F);
            _cmbCategory.Location = new Point(20, 253);
            _cmbCategory.Name = "_cmbCategory";
            _cmbCategory.Size = new Size(320, 36);
            _cmbCategory.TabIndex = 6;
            // 
            // _chkFavorite
            // 
            _chkFavorite.Font = new Font("Segoe UI", 10F);
            _chkFavorite.Location = new Point(20, 306);
            _chkFavorite.Name = "_chkFavorite";
            _chkFavorite.Size = new Size(277, 47);
            _chkFavorite.TabIndex = 7;
            _chkFavorite.Text = "Marcar como favorito";
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(36, 158, 160);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 11F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(20, 359);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(150, 36);
            btnSave.TabIndex = 8;
            btnSave.Text = "Guardar";
            btnSave.UseVisualStyleBackColor = false;
            btnSave.Click += BtnSave_Click;
            // 
            // btnCancel
            // 
            btnCancel.BackColor = Color.White;
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.FlatStyle = FlatStyle.Flat;
            btnCancel.Font = new Font("Segoe UI", 11F);
            btnCancel.ForeColor = Color.FromArgb(36, 158, 160);
            btnCancel.Location = new Point(190, 359);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(150, 36);
            btnCancel.TabIndex = 9;
            btnCancel.Text = "Cancelar";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += BtnCancel_Click;
            // 
            // ContactForm
            // 
            BackColor = Color.White;
            ClientSize = new Size(360, 481);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(_chkFavorite);
            Controls.Add(_cmbCategory);
            Controls.Add(lblCategory);
            Controls.Add(_txtPhone);
            Controls.Add(lblPhone);
            Controls.Add(_txtName);
            Controls.Add(lblName);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.None;
            Name = "ContactForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nuevo Contacto";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblName;
        private TextBox _txtName;
        private Label lblPhone;
        private TextBox _txtPhone;
        private Label lblCategory;
        private ComboBox _cmbCategory;
        private CheckBox _chkFavorite;
        private Button btnSave;
        private Button btnCancel;
    }
}
