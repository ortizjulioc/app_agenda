using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using app_agenda.UI.Services; // <--- Importante: Donde está tu servicio

namespace app_agenda.UI
{
    public partial class LoginForm : Form
    {
        // 1. Campos para el arrastre y el servicio
        private bool dragging = false;
        private Point dragOffset;
        private readonly AuthService _authService;

        public LoginForm()
        {
            InitializeComponent();
            _authService = new AuthService(); // 2. Inicializamos el servicio

            // Eventos para mover el formulario
            this.MouseDown += LoginForm_MouseDown;
            this.MouseMove += LoginForm_MouseMove;
            this.MouseUp += LoginForm_MouseUp;
        }

        // 3. Evento del botón cerrar (mejorado)
        private void btnClose_MouseClick(object sender, MouseEventArgs e)
        {
            Application.Exit(); // Cierra toda la aplicación
        }

        // 4. Lógica Real de Login (Movida a btnLogin_Click que es el estándar)
        private void btnLogin_Click(object sender, EventArgs e)
        {
            string user = txtUser.Text.Trim();
            string pass = txtPass.Text.Trim();

            // Validaciones básicas
            if (string.IsNullOrWhiteSpace(user) || string.IsNullOrWhiteSpace(pass))
            {
                MessageBox.Show("Por favor, complete todos los campos.", "Validación", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // LLAMADA AL SERVICIO (Consulta real a la BD)
                var usuarioEncontrado = _authService.Login(user, pass);

                if (usuarioEncontrado != null)
                {
                    // Login exitoso
                    this.Hide();
                    MainForm main = new MainForm(usuarioEncontrado.Id); // Pasamos el ID real
                    main.Show();
                }
                else
                {
                    // Datos incorrectos
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

        // Puedes borrar el btnLogin_MouseClick sobrante si no lo usas en el designer
    }
}