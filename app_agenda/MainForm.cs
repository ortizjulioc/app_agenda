using System;
using System.Linq; // Necesario para el .Any()
using System.Windows.Forms;
using global::app_agenda.Data;
using global::app_agenda.Data.Models; // <--- Esta es la línea que te faltaba

namespace app_agenda
{
    public partial class MainForm : Form
    {
        private int _loggedUserId;

        public MainForm(int userId)
        {
            InitializeComponent();
            _loggedUserId = userId; // Guardamos el ID para usarlo después
        }
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            try
            {
                using (var db = new AgendaContext())
                {
                    // 1. Verificamos conexión
                    if (db.Database.CanConnect())
                    {
                        // 2. Si conectó, verificamos si hay usuarios
                        if (!db.Users.Any())
                        {
                            var primerUsuario = new User
                            {
                                Username = "admin",
                                PasswordHash = "12345"
                            };

                            db.Users.Add(primerUsuario);
                            db.SaveChanges();
                            MessageBox.Show("¡Conexión Exitosa y usuario 'admin' creado!", "Éxito");
                        }
                        else
                        {
                            MessageBox.Show("¡Conexión Exitosa!", "Éxito");
                        }
                    }
                    else
                    {
                        MessageBox.Show("No se pudo conectar. Verifica tu servidor en AgendaContext.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error de conexión: " + ex.Message);
            }
        }
    }
}