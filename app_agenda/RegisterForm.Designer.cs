using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
using FontAwesome.Sharp;

namespace app_agenda.UI
{
    partial class RegisterForm
    {
        private IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            btnClose = new IconPictureBox();
            panelWhite = new Panel();
            pictureUser = new IconPictureBox();
            txtUser = new TextBox();
            panelLine1 = new Panel();
            txtPass = new TextBox();
            panelLine2 = new Panel();
            txtConfirmPass = new TextBox();
            panelLine3 = new Panel();
            btnRegister = new IconButton();
            linkLogin = new LinkLabel();
            ((ISupportInitialize)btnClose).BeginInit();
            panelWhite.SuspendLayout();
            ((ISupportInitialize)pictureUser).BeginInit();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Transparent;
            btnClose.Cursor = Cursors.Hand;
            btnClose.ForeColor = Color.FromArgb(36, 158, 160);
            btnClose.IconChar = IconChar.Close;
            btnClose.IconColor = Color.FromArgb(36, 158, 160);
            btnClose.IconFont = IconFont.Auto;
            btnClose.IconSize = 30;
            btnClose.Location = new Point(365, 10);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(30, 30);
            btnClose.TabIndex = 0;
            btnClose.TabStop = false;
            btnClose.MouseClick += btnClose_MouseClick;
            // 
            // panelWhite
            // 
            panelWhite.BackColor = Color.White;
            panelWhite.Controls.Add(pictureUser);
            panelWhite.Controls.Add(txtUser);
            panelWhite.Controls.Add(panelLine1);
            panelWhite.Controls.Add(txtPass);
            panelWhite.Controls.Add(panelLine2);
            panelWhite.Controls.Add(txtConfirmPass);
            panelWhite.Controls.Add(panelLine3);
            panelWhite.Controls.Add(btnRegister);
            panelWhite.Controls.Add(linkLogin);
            panelWhite.Location = new Point(40, 40);
            panelWhite.Name = "panelWhite";
            panelWhite.Padding = new Padding(30, 60, 30, 30);
            panelWhite.Size = new Size(320, 520);
            panelWhite.TabIndex = 1;
            panelWhite.Paint += panelWhite_Paint;
            // 
            // pictureUser
            // 
            pictureUser.BackColor = Color.Transparent;
            pictureUser.ForeColor = Color.FromArgb(36, 158, 160);
            pictureUser.IconChar = IconChar.UserPlus;
            pictureUser.IconColor = Color.FromArgb(36, 158, 160);
            pictureUser.IconFont = IconFont.Auto;
            pictureUser.IconSize = 80;
            pictureUser.Location = new Point(120, 50);
            pictureUser.Name = "pictureUser";
            pictureUser.Size = new Size(80, 80);
            pictureUser.TabIndex = 0;
            pictureUser.TabStop = false;
            // 
            // txtUser
            // 
            txtUser.BackColor = Color.White;
            txtUser.BorderStyle = BorderStyle.None;
            txtUser.Font = new Font("Segoe UI", 11F);
            txtUser.ForeColor = Color.FromArgb(64, 64, 64);
            txtUser.Location = new Point(30, 155);
            txtUser.Name = "txtUser";
            txtUser.PlaceholderText = "Usuario";
            txtUser.Size = new Size(260, 30);
            txtUser.TabIndex = 1;
            // 
            // panelLine1
            // 
            panelLine1.BackColor = Color.FromArgb(36, 158, 160);
            panelLine1.Location = new Point(30, 182);
            panelLine1.Name = "panelLine1";
            panelLine1.Size = new Size(260, 1);
            panelLine1.TabIndex = 2;
            // 
            // txtPass
            // 
            txtPass.BackColor = Color.White;
            txtPass.BorderStyle = BorderStyle.None;
            txtPass.Font = new Font("Segoe UI", 11F);
            txtPass.ForeColor = Color.FromArgb(64, 64, 64);
            txtPass.Location = new Point(30, 225);
            txtPass.Name = "txtPass";
            txtPass.PlaceholderText = "Contraseña";
            txtPass.Size = new Size(260, 30);
            txtPass.TabIndex = 3;
            txtPass.UseSystemPasswordChar = true;
            // 
            // panelLine2
            // 
            panelLine2.BackColor = Color.FromArgb(36, 158, 160);
            panelLine2.Location = new Point(30, 252);
            panelLine2.Name = "panelLine2";
            panelLine2.Size = new Size(260, 1);
            panelLine2.TabIndex = 4;
            // 
            // txtConfirmPass
            // 
            txtConfirmPass.BackColor = Color.White;
            txtConfirmPass.BorderStyle = BorderStyle.None;
            txtConfirmPass.Font = new Font("Segoe UI", 11F);
            txtConfirmPass.ForeColor = Color.FromArgb(64, 64, 64);
            txtConfirmPass.Location = new Point(30, 295);
            txtConfirmPass.Name = "txtConfirmPass";
            txtConfirmPass.PlaceholderText = "Confirmar Contraseña";
            txtConfirmPass.Size = new Size(260, 30);
            txtConfirmPass.TabIndex = 5;
            txtConfirmPass.UseSystemPasswordChar = true;
            // 
            // panelLine3
            // 
            panelLine3.BackColor = Color.FromArgb(36, 158, 160);
            panelLine3.Location = new Point(30, 322);
            panelLine3.Name = "panelLine3";
            panelLine3.Size = new Size(260, 1);
            panelLine3.TabIndex = 6;
            // 
            // btnRegister
            // 
            btnRegister.BackColor = Color.FromArgb(36, 158, 160);
            btnRegister.Cursor = Cursors.Hand;
            btnRegister.FlatAppearance.BorderSize = 0;
            btnRegister.FlatStyle = FlatStyle.Flat;
            btnRegister.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnRegister.ForeColor = Color.White;
            btnRegister.IconChar = IconChar.Save;
            btnRegister.IconColor = Color.White;
            btnRegister.IconFont = IconFont.Auto;
            btnRegister.Location = new Point(30, 360);
            btnRegister.Name = "btnRegister";
            btnRegister.Size = new Size(260, 45);
            btnRegister.TabIndex = 7;
            btnRegister.Text = "Registrarse";
            btnRegister.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnRegister.UseVisualStyleBackColor = false;
            btnRegister.MouseClick += btnRegister_MouseClick;
            // 
            // linkLogin
            // 
            linkLogin.ActiveLinkColor = Color.FromArgb(20, 120, 120);
            linkLogin.Font = new Font("Segoe UI", 10F);
            linkLogin.LinkColor = Color.FromArgb(36, 158, 160);
            linkLogin.Location = new Point(30, 420);
            linkLogin.Name = "linkLogin";
            linkLogin.Size = new Size(260, 39);
            linkLogin.TabIndex = 8;
            linkLogin.TabStop = true;
            linkLogin.Text = "¿Ya tienes cuenta? Inicia sesión";
            linkLogin.TextAlign = ContentAlignment.MiddleCenter;
            linkLogin.LinkClicked += linkLogin_LinkClicked;
            // 
            // RegisterForm
            // 
            BackColor = Color.FromArgb(240, 242, 245);
            ClientSize = new Size(400, 600);
            Controls.Add(btnClose);
            Controls.Add(panelWhite);
            FormBorderStyle = FormBorderStyle.None;
            Name = "RegisterForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Registro";
            Load += RegisterForm_Load;
            ((ISupportInitialize)btnClose).EndInit();
            panelWhite.ResumeLayout(false);
            panelWhite.PerformLayout();
            ((ISupportInitialize)pictureUser).EndInit();
            ResumeLayout(false);
        }

        private IconPictureBox btnClose;
        private Panel panelWhite;
        private IconPictureBox pictureUser;
        private TextBox txtUser;
        private Panel panelLine1;
        private TextBox txtPass;
        private Panel panelLine2;
        private TextBox txtConfirmPass;
        private Panel panelLine3;
        private IconButton btnRegister;
        private LinkLabel linkLogin;
    }
}