using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;

namespace app_agenda
{
    partial class MainForm
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
            components = new Container();
            pnlSidebar = new Panel();
            flpCategories = new FlowLayoutPanel();
            pnlLogo = new Panel();
            iconLogo = new IconPictureBox();
            lblLogo = new Label();
            pnlHeader = new Panel();
            lblWelcome = new Label();
            btnLogout = new IconButton();
            pnlHeaderSeparator = new Panel();
            pnlContent = new Panel();
            flpDisplay = new FlowLayoutPanel();
            pnlSearch = new Panel();
            btnAddContact = new IconButton();
            txtSearch = new TextBox();
            lblSearch = new Label();
            toolTip1 = new ToolTip(components);
            pnlSidebar.SuspendLayout();
            pnlLogo.SuspendLayout();
            ((ISupportInitialize)iconLogo).BeginInit();
            pnlHeader.SuspendLayout();
            pnlContent.SuspendLayout();
            pnlSearch.SuspendLayout();
            SuspendLayout();
            // 
            // pnlSidebar
            // 
            pnlSidebar.BackColor = Color.FromArgb(36, 158, 160);
            pnlSidebar.Controls.Add(flpCategories);
            pnlSidebar.Controls.Add(pnlLogo);
            pnlSidebar.Dock = DockStyle.Left;
            pnlSidebar.Location = new Point(0, 0);
            pnlSidebar.Name = "pnlSidebar";
            pnlSidebar.Size = new Size(250, 650);
            pnlSidebar.TabIndex = 2;
            // 
            // flpCategories
            // 
            flpCategories.AutoScroll = true;
            flpCategories.BackColor = Color.Transparent;
            flpCategories.Dock = DockStyle.Fill;
            flpCategories.FlowDirection = FlowDirection.TopDown;
            flpCategories.Location = new Point(0, 100);
            flpCategories.Name = "flpCategories";
            flpCategories.Size = new Size(250, 550);
            flpCategories.TabIndex = 0;
            flpCategories.WrapContents = false;
            // 
            // pnlLogo
            // 
            pnlLogo.BackColor = Color.Transparent;
            pnlLogo.Controls.Add(iconLogo);
            pnlLogo.Controls.Add(lblLogo);
            pnlLogo.Dock = DockStyle.Top;
            pnlLogo.Location = new Point(0, 0);
            pnlLogo.Name = "pnlLogo";
            pnlLogo.Size = new Size(250, 100);
            pnlLogo.TabIndex = 1;
            // 
            // iconLogo
            // 
            iconLogo.BackColor = Color.Transparent;
            iconLogo.IconChar = IconChar.ContactBook;
            iconLogo.IconColor = Color.White;
            iconLogo.IconFont = IconFont.Auto;
            iconLogo.IconSize = 45;
            iconLogo.Location = new Point(35, 37);
            iconLogo.Name = "iconLogo";
            iconLogo.Size = new Size(45, 45);
            iconLogo.TabIndex = 0;
            iconLogo.TabStop = false;
            // 
            // lblLogo
            // 
            lblLogo.AutoSize = true;
            lblLogo.BackColor = Color.Transparent;
            lblLogo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            lblLogo.ForeColor = Color.White;
            lblLogo.Location = new Point(75, 32);
            lblLogo.Name = "lblLogo";
            lblLogo.Size = new Size(185, 45);
            lblLogo.TabIndex = 1;
            lblLogo.Text = "Mi Agenda";
            // 
            // pnlHeader
            // 
            pnlHeader.BackColor = Color.White;
            pnlHeader.Controls.Add(lblWelcome);
            pnlHeader.Controls.Add(btnLogout);
            pnlHeader.Controls.Add(pnlHeaderSeparator);
            pnlHeader.Dock = DockStyle.Top;
            pnlHeader.Location = new Point(250, 0);
            pnlHeader.Name = "pnlHeader";
            pnlHeader.Size = new Size(777, 60);
            pnlHeader.TabIndex = 1;
            // 
            // lblWelcome
            // 
            lblWelcome.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            lblWelcome.ForeColor = Color.FromArgb(36, 158, 160);
            lblWelcome.Location = new Point(20, 18);
            lblWelcome.Name = "lblWelcome";
            lblWelcome.Size = new Size(300, 24);
            lblWelcome.TabIndex = 0;
            // 
            // btnLogout
            // 
            btnLogout.BackColor = Color.Transparent;
            btnLogout.Cursor = Cursors.Hand;
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.FlatAppearance.MouseDownBackColor = Color.FromArgb(30, 36, 158, 160);
            btnLogout.FlatAppearance.MouseOverBackColor = Color.FromArgb(20, 36, 158, 160);
            btnLogout.FlatStyle = FlatStyle.Flat;
            btnLogout.IconChar = IconChar.SignOutAlt;
            btnLogout.IconColor = Color.FromArgb(36, 158, 160);
            btnLogout.IconFont = IconFont.Auto;
            btnLogout.Location = new Point(600, 10);
            btnLogout.Name = "btnLogout";
            btnLogout.Size = new Size(130, 40);
            btnLogout.TabIndex = 1;
            btnLogout.TextImageRelation = TextImageRelation.ImageBeforeText;
            toolTip1.SetToolTip(btnLogout, "Cerrar");
            btnLogout.UseVisualStyleBackColor = false;
            btnLogout.Click += BtnLogout_Click;
            // 
            // pnlHeaderSeparator
            // 
            pnlHeaderSeparator.BackColor = Color.FromArgb(224, 224, 224);
            pnlHeaderSeparator.Location = new Point(0, 59);
            pnlHeaderSeparator.Name = "pnlHeaderSeparator";
            pnlHeaderSeparator.Size = new Size(750, 1);
            pnlHeaderSeparator.TabIndex = 2;
            // 
            // pnlContent
            // 
            pnlContent.BackColor = Color.FromArgb(240, 242, 245);
            pnlContent.Controls.Add(flpDisplay);
            pnlContent.Controls.Add(pnlSearch);
            pnlContent.Dock = DockStyle.Fill;
            pnlContent.Location = new Point(250, 60);
            pnlContent.Name = "pnlContent";
            pnlContent.Size = new Size(777, 590);
            pnlContent.TabIndex = 0;
            // 
            // flpDisplay
            // 
            flpDisplay.AutoScroll = true;
            flpDisplay.BackColor = Color.FromArgb(240, 242, 245);
            flpDisplay.Dock = DockStyle.Fill;
            flpDisplay.FlowDirection = FlowDirection.TopDown;
            flpDisplay.Location = new Point(0, 63);
            flpDisplay.Name = "flpDisplay";
            flpDisplay.Size = new Size(777, 527);
            flpDisplay.TabIndex = 0;
            flpDisplay.WrapContents = false;
            // 
            // pnlSearch
            // 
            pnlSearch.BackColor = Color.White;
            pnlSearch.Controls.Add(btnAddContact);
            pnlSearch.Controls.Add(txtSearch);
            pnlSearch.Controls.Add(lblSearch);
            pnlSearch.Dock = DockStyle.Top;
            pnlSearch.Location = new Point(0, 0);
            pnlSearch.Name = "pnlSearch";
            pnlSearch.Size = new Size(777, 63);
            pnlSearch.TabIndex = 1;
            // 
            // btnAddContact
            // 
            btnAddContact.BackColor = Color.FromArgb(36, 158, 160);
            btnAddContact.Cursor = Cursors.Hand;
            btnAddContact.FlatAppearance.BorderSize = 0;
            btnAddContact.FlatStyle = FlatStyle.Flat;
            btnAddContact.IconChar = IconChar.UserPlus;
            btnAddContact.IconColor = Color.White;
            btnAddContact.IconFont = IconFont.Auto;
            btnAddContact.IconSize = 24;
            btnAddContact.Location = new Point(307, 14);
            btnAddContact.Name = "btnAddContact";
            btnAddContact.Size = new Size(130, 40);
            btnAddContact.TabIndex = 0;
            btnAddContact.Text = "Agregar";
            btnAddContact.TextImageRelation = TextImageRelation.ImageBeforeText;
            btnAddContact.UseVisualStyleBackColor = false;
            btnAddContact.Click += BtnAddContact_Click;
            // 
            // txtSearch
            // 
            txtSearch.BorderStyle = BorderStyle.FixedSingle;
            txtSearch.Font = new Font("Segoe UI", 10F);
            txtSearch.Location = new Point(101, 17);
            txtSearch.Name = "txtSearch";
            txtSearch.Size = new Size(200, 34);
            txtSearch.TabIndex = 1;
            txtSearch.TextChanged += TxtSearch_TextChanged;
            // 
            // lblSearch
            // 
            lblSearch.Font = new Font("Segoe UI", 10F);
            lblSearch.ForeColor = Color.Gray;
            lblSearch.Location = new Point(3, 17);
            lblSearch.Name = "lblSearch";
            lblSearch.Size = new Size(82, 32);
            lblSearch.TabIndex = 2;
            lblSearch.Text = "Buscar:";
            // 
            // MainForm
            // 
            ClientSize = new Size(1027, 650);
            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Controls.Add(pnlSidebar);
            MinimumSize = new Size(800, 500);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Agenda Telefónica";
            Load += MainForm_Load;
            pnlSidebar.ResumeLayout(false);
            pnlLogo.ResumeLayout(false);
            pnlLogo.PerformLayout();
            ((ISupportInitialize)iconLogo).EndInit();
            pnlHeader.ResumeLayout(false);
            pnlContent.ResumeLayout(false);
            pnlSearch.ResumeLayout(false);
            pnlSearch.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private IconButton CreateSidebarButton(string text, IconChar icon, Action onClick)
        {
            var btn = new IconButton
            {
                Text = text,
                Size = new Size(250, 50),
                IconChar = icon,
                IconColor = Color.White,
                IconSize = 24,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleLeft,
                ImageAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(25, 0, 0, 0),
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 11, FontStyle.Regular)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.FlatAppearance.MouseDownBackColor = ColorTranslator.FromHtml("#1E857E");
            btn.FlatAppearance.MouseOverBackColor = ColorTranslator.FromHtml("#2bb6b8");

            btn.Click += (s, e) => { onClick(); SetActiveButton(btn); };
            return btn;
        }

        // Controles estáticos del formulario
        private Panel pnlSidebar;
        private Panel pnlLogo;
        private IconPictureBox iconLogo;
        private Label lblLogo;
        private FlowLayoutPanel flpCategories;
        private Panel pnlHeader;
        private Label lblWelcome;
        private IconButton btnLogout;
        private Panel pnlHeaderSeparator;
        private Panel pnlContent;
        private Panel pnlSearch;
        private Label lblSearch;
        private TextBox txtSearch;
        private IconButton btnAddContact;
        private FlowLayoutPanel flpDisplay;
        private ToolTip toolTip1;

        // Controles dinámicos (creados en LoadCategories)
        private IconButton? btnTodos;
        private IconButton? btnFavoritos;
        private IconButton? btnTodo;
    }
}
