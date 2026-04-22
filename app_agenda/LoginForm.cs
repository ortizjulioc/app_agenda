using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using app_agenda.UI.Services;

namespace app_agenda.UI
{
    public partial class LoginForm : Form
    {
        private bool dragging = false;
        private Point dragOffset;
        private readonly AuthService _authService;
        private readonly CategoryService _categoryService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService();
            _categoryService = new CategoryService();

            // --- ESTA ES LA LÍNEA MÁGICA ---
            // Hace que al presionar Enter en cualquier lugar, se ejecute btnLogin_Click
            this.AcceptButton = this.btnLogin;

            this.MouseDown += LoginForm_MouseDown;
            this.MouseMove += LoginForm_MouseMove;
            this.MouseUp += LoginForm_MouseUp;

            this.panelWhite.MouseDown += LoginForm_MouseDown;
            this.panelWhite.MouseMove += LoginForm_MouseMove;
            this.panelWhite.MouseUp += LoginForm_MouseUp;
        }

        private void btnClose_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text.Trim();

            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var usuarioEncontrado = _authService.Login(user, pass);

                if (usuarioEncontrado != null)
                {
                    _categoryService.EnsureDefaultCategories(usuarioEncontrado.Id);

                    this.Hide();
                    MainForm main = new MainForm(usuarioEncontrado.Id, usuarioEncontrado.Username);
                    main.Show();
                }
                else
                {
                    MessageBox.Show("Usuario o contraseña incorrectos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    txtPass.Clear();
                    txtPass.Focus();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al conectar: " + ex.Message);
            }
        }

        private void linkRegister_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.Hide();
            using var registerForm = new RegisterForm();
            registerForm.ShowDialog();
            this.Show();
        }

        #region Arrastre del Formulario
        private void LoginForm_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                dragging = true;
                dragOffset = new Point(e.X, e.Y);
            }
        }

        private void LoginForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                this.Location = new Point(
                    this.Left + e.X - dragOffset.X,
                    this.Top + e.Y - dragOffset.Y
                );
            }
        }

        private void LoginForm_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }
        #endregion

        private void LoginForm_Load(object sender, EventArgs e)
        {
            txtUser.Focus();
        }

        private void txtPass_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtPass_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                // Esto evita que Windows haga el sonido de error
                e.SuppressKeyPress = true;

                // El AcceptButton se encargará de hacer el clic por nosotros
            }
        }
    }
}