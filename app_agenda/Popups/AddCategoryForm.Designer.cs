using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace app_agenda.UI.Popups
{
    partial class AddCategoryForm
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
            lblIcon = new Label();
            _flpIcons = new FlowLayoutPanel();
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
            lblTitle.Size = new Size(280, 45);
            lblTitle.TabIndex = 0;
            lblTitle.Text = "Nueva Categoría";
            // 
            // lblName
            // 
            lblName.Font = new Font("Segoe UI", 10F);
            lblName.Location = new Point(20, 65);
            lblName.Name = "lblName";
            lblName.Size = new Size(280, 34);
            lblName.TabIndex = 1;
            lblName.Text = "Nombre:";
            // 
            // _txtName
            // 
            _txtName.BorderStyle = BorderStyle.FixedSingle;
            _txtName.Font = new Font("Segoe UI", 11F);
            _txtName.Location = new Point(20, 95);
            _txtName.Name = "_txtName";
            _txtName.Size = new Size(280, 37);
            _txtName.TabIndex = 2;
            // 
            // lblIcon
            // 
            lblIcon.Font = new Font("Segoe UI", 10F);
            lblIcon.Location = new Point(20, 125);
            lblIcon.Name = "lblIcon";
            lblIcon.Size = new Size(280, 36);
            lblIcon.TabIndex = 3;
            lblIcon.Text = "Icono:";
            // 
            // _flpIcons
            // 
            _flpIcons.BackColor = Color.FromArgb(240, 242, 245);
            _flpIcons.Location = new Point(20, 164);
            _flpIcons.Name = "_flpIcons";
            _flpIcons.Size = new Size(280, 60);
            _flpIcons.TabIndex = 4;
            // 
            // btnSave
            // 
            btnSave.BackColor = Color.FromArgb(36, 158, 160);
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.FlatStyle = FlatStyle.Flat;
            btnSave.Font = new Font("Segoe UI", 11F);
            btnSave.ForeColor = Color.White;
            btnSave.Location = new Point(20, 231);
            btnSave.Name = "btnSave";
            btnSave.Size = new Size(130, 36);
            btnSave.TabIndex = 5;
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
            btnCancel.Location = new Point(170, 231);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new Size(130, 36);
            btnCancel.TabIndex = 6;
            btnCancel.Text = "Cancelar";
            btnCancel.UseVisualStyleBackColor = false;
            btnCancel.Click += BtnCancel_Click;
            // 
            // AddCategoryForm
            // 
            BackColor = Color.White;
            ClientSize = new Size(400, 386);
            Controls.Add(btnCancel);
            Controls.Add(btnSave);
            Controls.Add(_flpIcons);
            Controls.Add(lblIcon);
            Controls.Add(_txtName);
            Controls.Add(lblName);
            Controls.Add(lblTitle);
            FormBorderStyle = FormBorderStyle.None;
            Name = "AddCategoryForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Nueva Categoría";
            Load += AddCategoryForm_Load;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label lblTitle;
        private Label lblName;
        private TextBox _txtName;
        private Label lblIcon;
        private FlowLayoutPanel _flpIcons;
        private Button btnSave;
        private Button btnCancel;
    }
}
