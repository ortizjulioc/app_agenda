using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using app_agenda.UI.Services;

namespace app_agenda.UI
{
    public partial class RegisterForm : Form
    {
        private readonly AuthService _authService;
        private bool dragging = false;
        private Point dragOffset;

        public RegisterForm()
        {
            _authService = new AuthService();
            InitializeComponent();
            this.MouseDown += RegisterForm_MouseDown;
            this.MouseMove += RegisterForm_MouseMove;
            this.MouseUp += RegisterForm_MouseUp;
        }

        private void btnClose_MouseClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }

        private void btnRegister_MouseClick(object sender, MouseEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtUser.Text))
            {
                MessageBox.Show("Por favor, ingrese el usuario.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUser.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtPass.Text))
            {
                MessageBox.Show("Por favor, ingrese la contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPass.Focus();
                return;
            }

            if (string.IsNullOrWhiteSpace(txtConfirmPass.Text))
            {
                MessageBox.Show("Por favor, confirme la contraseña.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPass.Focus();
                return;
            }

            if (txtPass.Text != txtConfirmPass.Text)
            {
                MessageBox.Show("Las contraseñas no coinciden.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtConfirmPass.Focus();
                return;
            }

            if (_authService.Register(txtUser.Text, txtPass.Text))
            {
                MessageBox.Show("Usuario registrado exitosamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                MessageBox.Show("El usuario ya existe. Por favor, elija otro nombre de usuario.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtUser.Focus();
            }
        }

        private void linkLogin_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Close();
        }

        private void RegisterForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragOffset = new Point(e.X, e.Y);
            }
        }

        private void RegisterForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point newLocation = new Point(
                    this.Left + e.X - dragOffset.X,
                    this.Top + e.Y - dragOffset.Y
                );
                this.Location = newLocation;
            }
        }

        private void RegisterForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void RegisterForm_Load(object sender, EventArgs e)
        {

        }

        private void panelWhite_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}