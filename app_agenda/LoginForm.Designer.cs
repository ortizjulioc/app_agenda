namespace app_agenda.UI
{
    partial class LoginForm
    {
        private System.ComponentModel.IContainer components = null;

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
            btnClose = new FontAwesome.Sharp.IconPictureBox();
            panelWhite = new Panel();
            pictureUser = new FontAwesome.Sharp.IconPictureBox();
            txtUser = new TextBox();
            panelLine1 = new Panel();
            txtPass = new TextBox();
            panelLine2 = new Panel();
            btnLogin = new FontAwesome.Sharp.IconButton();
            linkRegister = new LinkLabel();
            ((System.ComponentModel.ISupportInitialize)btnClose).BeginInit();
            panelWhite.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureUser).BeginInit();
            SuspendLayout();
            // 
            // btnClose
            // 
            btnClose.BackColor = Color.Transparent;
            btnClose.Cursor = Cursors.Hand;
            btnClose.ForeColor = Color.FromArgb(36, 158, 160);
            btnClose.IconChar = FontAwesome.Sharp.IconChar.Close;
            btnClose.IconColor = Color.FromArgb(36, 158, 160);
            btnClose.IconFont = FontAwesome.Sharp.IconFont.Auto;
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
            panelWhite.Controls.Add(btnLogin);
            panelWhite.Controls.Add(linkRegister);
            panelWhite.Location = new Point(40, 40);
            panelWhite.Name = "panelWhite";
            panelWhite.Padding = new Padding(30, 60, 30, 30);
            panelWhite.Size = new Size(320, 420);
            panelWhite.TabIndex = 1;
            // 
            // pictureUser
            // 
            pictureUser.BackColor = Color.Transparent;
            pictureUser.ForeColor = Color.FromArgb(36, 158, 160);
            pictureUser.IconChar = FontAwesome.Sharp.IconChar.UserCircle;
            pictureUser.IconColor = Color.FromArgb(36, 158, 160);
            pictureUser.IconFont = FontAwesome.Sharp.IconFont.Auto;
            pictureUser.IconSize = 80;
            pictureUser.Location = new Point(120, 70);
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
            txtUser.Location = new Point(30, 175);
            txtUser.Name = "txtUser";
            txtUser.PlaceholderText = "Usuario";
            txtUser.Size = new Size(260, 20);
            txtUser.TabIndex = 1;
            // 
            // panelLine1
            // 
            panelLine1.BackColor = Color.FromArgb(36, 158, 160);
            panelLine1.Location = new Point(30, 202);
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
            txtPass.Location = new Point(30, 245);
            txtPass.Name = "txtPass";
            txtPass.PlaceholderText = "Contraseña";
            txtPass.Size = new Size(260, 20);
            txtPass.TabIndex = 3;
            txtPass.UseSystemPasswordChar = true;
            txtPass.TextChanged += txtPass_TextChanged;
            txtPass.KeyDown += txtPass_KeyDown;
            // 
            // panelLine2
            // 
            panelLine2.BackColor = Color.FromArgb(36, 158, 160);
            panelLine2.Location = new Point(30, 272);
            panelLine2.Name = "panelLine2";
            panelLine2.Size = new Size(260, 1);
            panelLine2.TabIndex = 4;
            // 
            // btnLogin
            // 
            btnLogin.BackColor = Color.FromArgb(36, 158, 160);
            btnLogin.Cursor = Cursors.Hand;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.Font = new Font("Segoe UI", 11F, FontStyle.Bold);
            btnLogin.ForeColor = Color.White;
            btnLogin.IconChar = FontAwesome.Sharp.IconChar.RightToBracket;
            btnLogin.IconColor = Color.White;
            btnLogin.IconFont = FontAwesome.Sharp.IconFont.Auto;
            btnLogin.Location = new Point(27, 333);
            btnLogin.Name = "btnLogin";
            btnLogin.Size = new Size(260, 45);
            btnLogin.TabIndex = 5;
            btnLogin.Text = "Ingresar";
            btnLogin.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnLogin.UseVisualStyleBackColor = false;
            btnLogin.Click += btnLogin_Click;
            // 
            // linkRegister
            // 
            linkRegister.ActiveLinkColor = Color.FromArgb(20, 120, 120);
            linkRegister.Font = new Font("Segoe UI", 10F);
            linkRegister.LinkColor = Color.FromArgb(36, 158, 160);
            linkRegister.Location = new Point(27, 385);
            linkRegister.Name = "linkRegister";
            linkRegister.Size = new Size(260, 20);
            linkRegister.TabIndex = 6;
            linkRegister.TabStop = true;
            linkRegister.Text = "Crear cuenta nueva";
            linkRegister.TextAlign = ContentAlignment.MiddleCenter;
            linkRegister.LinkClicked += linkRegister_LinkClicked;
            // 
            // LoginForm
            // 
            BackColor = Color.FromArgb(240, 242, 245);
            ClientSize = new Size(463, 500);
            Controls.Add(btnClose);
            Controls.Add(panelWhite);
            FormBorderStyle = FormBorderStyle.None;
            Name = "LoginForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Login";
            Load += LoginForm_Load;
            ((System.ComponentModel.ISupportInitialize)btnClose).EndInit();
            panelWhite.ResumeLayout(false);
            panelWhite.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureUser).EndInit();
            ResumeLayout(false);
        }

        private FontAwesome.Sharp.IconPictureBox btnClose;
        private System.Windows.Forms.Panel panelWhite;
        private FontAwesome.Sharp.IconPictureBox pictureUser;
        private System.Windows.Forms.TextBox txtUser;
        private System.Windows.Forms.Panel panelLine1;
        private System.Windows.Forms.TextBox txtPass;
        private System.Windows.Forms.Panel panelLine2;
        private FontAwesome.Sharp.IconButton btnLogin;
        private System.Windows.Forms.LinkLabel linkRegister;
    }
}