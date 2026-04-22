using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using FontAwesome.Sharp;
using app_agenda.Data.Models;
using app_agenda.UI.Services;
using app_agenda.UI.UserControls;
using app_agenda.UI.Popups;

namespace app_agenda
{
    public partial class MainForm : Form
    {
        private readonly int _currentUserId;
        private readonly string _userName;
        private readonly ContactService _contactService;
        private readonly CategoryService _categoryService;
        private readonly TodoService _todoService;

        private IconButton? _activeButton;
        private int? _currentCategoryId;
        private string _currentView = "contacts";

        private Panel pnlSidebar = null!;
        private Panel pnlHeader = null!;
        private Panel pnlContent = null!;

        private FlowLayoutPanel? flpCategories;
        private TextBox? txtSearch;
        private FlowLayoutPanel? flpDisplay;

        private IconButton? btnTodos;
        private IconButton? btnFavoritos;
        private IconButton? btnTodo;

        public MainForm(int userId, string userName)
        {
            _currentUserId = userId;
            _userName = userName;
            _contactService = new ContactService();
            _categoryService = new CategoryService();
            _todoService = new TodoService();

            InitializeComponent();
            SetupPanels();
            SetupSidebar();
            SetupHeader();
            SetupContent();

            LoadCategories();
            LoadContacts(null);
            LoadUserName();
        }

        private void SetupPanels()
        {
            pnlSidebar = new Panel
            {
                Size = new Size(250, 650),
                BackColor = ColorTranslator.FromHtml("#249EA0"),
                Dock = DockStyle.Left
            };

            pnlHeader = new Panel
            {
                Size = new Size(750, 60),
                Location = new Point(250, 0),
                BackColor = Color.White,
                Dock = DockStyle.Top
            };

            pnlContent = new Panel
            {
                Size = new Size(750, 590),
                Location = new Point(250, 60),
                BackColor = ColorTranslator.FromHtml("#F0F2F5"),
                Dock = DockStyle.Fill
            };

            Controls.Add(pnlContent);
            Controls.Add(pnlHeader);
            Controls.Add(pnlSidebar);
        }

        private void SetupSidebar()
        {
            var logoPanel = new Panel { Size = new Size(250, 80), Dock = DockStyle.Top };
            var iconLogo = new IconPictureBox
            {
                Size = new Size(40, 40),
                Location = new Point(15, 20),
                IconChar = IconChar.AddressBook,
                IconColor = Color.White,
                IconSize = 32
            };
            var lblLogo = new Label
            {
                Text = "Mi Agenda",
                Location = new Point(65, 22),
                Size = new Size(170, 25),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 16, FontStyle.Bold)
            };
            logoPanel.Controls.AddRange(new Control[] { iconLogo, lblLogo });

            flpCategories = new FlowLayoutPanel
            {
                Size = new Size(250, 350),
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false
            };

            btnTodos = CreateSidebarButton("Todos", IconChar.Inbox, () => { _currentCategoryId = null; LoadContacts(null); });
            btnFavoritos = CreateSidebarButton("Favoritos", IconChar.Heart, () => { LoadFavorites(); });
            btnTodo = CreateSidebarButton("To Do", IconChar.ListUl, () => { LoadTodos(); });

            var btnAddCategory = new IconButton
            {
                Text = "+ Nueva Categoría",
                Location = new Point(10, flpCategories!.Bottom + 10),
                Size = new Size(230, 40),
                IconChar = IconChar.Plus,
                IconColor = Color.White,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnAddCategory.Click += BtnAddCategory_Click;

            flpCategories.Controls.Add(btnTodos!);
            flpCategories.Controls.Add(btnFavoritos!);
            foreach (Control ctrl in new Control[] { btnAddCategory })
                flpCategories.Controls.Add(ctrl);
            flpCategories.Controls.Add(btnTodo!);

            pnlSidebar.Controls.Add(flpCategories);
            pnlSidebar.Controls.Add(logoPanel);

            SetActiveButton(btnTodos!);
        }

        private IconButton CreateSidebarButton(string text, IconChar icon, Action onClick)
        {
            var btn = new IconButton
            {
                Text = text,
                Size = new Size(230, 44),
                IconChar = icon,
                IconColor = Color.White,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btn.MouseClick += (s, e) => { onClick(); SetActiveButton(btn); };
            return btn;
        }

        private void SetActiveButton(IconButton btn)
        {
            if (_activeButton != null)
                _activeButton.BackColor = Color.Transparent;

            _activeButton = btn;
            _activeButton!.BackColor = ColorTranslator.FromHtml("#1E857E");

            var indicator = new Panel
            {
                Size = new Size(4, 44),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            _activeButton.Controls.Add(indicator);
        }

        private void SetupHeader()
        {
            var lblWelcome = new Label
            {
                Name = "lblWelcome",
                Location = new Point(20, 18),
                Size = new Size(300, 24),
                ForeColor = ColorTranslator.FromHtml("#249EA0"),
                Font = new Font("Segoe UI", 14, FontStyle.Bold)
            };

            var btnLogout = new IconButton
            {
                Text = "Cerrar Sesión",
                Location = new Point(600, 10),
                Size = new Size(130, 40),
                IconChar = IconChar.RightFromBracket,
                IconColor = ColorTranslator.FromHtml("#249EA0"),
                FlatStyle = FlatStyle.Flat,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                BackColor = Color.Transparent,
                Cursor = Cursors.Hand
            };
            btnLogout.Click += BtnLogout_Click;

            var separator = new Panel
            {
                Location = new Point(0, 59),
                Size = new Size(750, 1),
                BackColor = ColorTranslator.FromHtml("#E0E0E0")
            };

            pnlHeader.Controls.AddRange(new Control[] { separator, btnLogout, lblWelcome });
        }

        private void SetupContent()
        {
            var searchPanel = new Panel
            {
                Size = new Size(750, 60),
                BackColor = Color.White,
                Dock = DockStyle.Top
            };

            var lblSearch = new Label
            {
                Text = "Buscar:",
                Location = new Point(20, 20),
                Size = new Size(60, 20),
                Font = new Font("Segoe UI", 10),
                ForeColor = Color.Gray
            };

            txtSearch = new TextBox
            {
                Location = new Point(80, 17),
                Size = new Size(200, 26),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            var btnAddContact = new IconButton
            {
                Text = "Agregar Contacto",
                Location = new Point(300, 12),
                Size = new Size(150, 36),
                IconChar = IconChar.UserPlus,
                IconColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#249EA0"),
                FlatStyle = FlatStyle.Flat,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand
            };
            btnAddContact.Click += BtnAddContact_Click;

            searchPanel.Controls.AddRange(new Control[] { btnAddContact, txtSearch!, lblSearch });

            flpDisplay = new FlowLayoutPanel
            {
                Size = new Size(750, 530),
                Location = new Point(0, 60),
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true,
                WrapContents = false,
                BackColor = ColorTranslator.FromHtml("#F0F2F5")
            };

            pnlContent.Controls.Add(flpDisplay!);
            pnlContent.Controls.Add(searchPanel);
        }

        private void LoadUserName()
        {
            var lbl = pnlHeader.Controls.OfType<Label>().FirstOrDefault(l => l.Name == "lblWelcome");
            if (lbl != null)
                lbl.Text = $"Hola, {_userName}";
        }

        private void LoadCategories()
        {
            var categories = _categoryService.GetCategoriesByUser(_currentUserId);
            foreach (var cat in categories)
            {
                var icon = ParseIcon(cat.IconCode);
                var btn = CreateSidebarButton(cat.Name, icon, () => {
                    _currentCategoryId = cat.Id;
                    LoadContacts(cat.Id);
                });
                flpCategories!.Controls.Add(btn);
            }
        }

        private IconChar ParseIcon(string code)
        {
            return code switch
            {
                "User" => IconChar.User,
                "Briefcase" => IconChar.Briefcase,
                "Users" => IconChar.Users,
                "Phone" => IconChar.Phone,
                "Heart" => IconChar.Heart,
                "Star" => IconChar.Star,
                "Folder" => IconChar.Folder,
                "Home" => IconChar.House,
                _ => IconChar.Folder
            };
        }

        private void LoadContacts(int? categoryId)
        {
            _currentView = "contacts";
            ShowSearchPanel(true);

            flpDisplay!.Controls.Clear();

            if (categoryId == null)
            {
                var contacts = _contactService.GetContacts(_currentUserId);
                foreach (var contact in contacts)
                    flpDisplay.Controls.Add(CreateContactCard(contact));
            }
            else
            {
                var contacts = _contactService.GetContacts(_currentUserId, categoryId);
                foreach (var contact in contacts)
                    flpDisplay.Controls.Add(CreateContactCard(contact));
            }
        }

        private void LoadFavorites()
        {
            _currentView = "contacts";
            ShowSearchPanel(true);

            flpDisplay!.Controls.Clear();
            var contacts = _contactService.GetFavorites(_currentUserId);
            foreach (var contact in contacts)
                flpDisplay.Controls.Add(CreateContactCard(contact));
        }

        private void LoadTodos()
        {
            _currentView = "todos";
            ShowSearchPanel(false);

            flpDisplay!.Controls.Clear();

            var inputPanel = new Panel { Size = new Size(750, 50), BackColor = Color.White };
            var txtNewTodo = new TextBox
            {
                Location = new Point(20, 10),
                Size = new Size(500, 26),
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle
            };

            var btnAddTodo = new IconButton
            {
                Text = "Agregar",
                Location = new Point(530, 8),
                Size = new Size(100, 34),
                IconChar = IconChar.Plus,
                IconColor = Color.White,
                BackColor = ColorTranslator.FromHtml("#249EA0"),
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                FlatAppearance = { BorderSize = 0 }
            };
            btnAddTodo.Click += (s, e) => {
                if (!string.IsNullOrWhiteSpace(txtNewTodo.Text))
                {
                    _todoService.AddTodo(_currentUserId, txtNewTodo.Text.Trim());
                    txtNewTodo.Clear();
                    LoadTodos();
                }
            };

            inputPanel.Controls.AddRange(new Control[] { btnAddTodo, txtNewTodo });
            flpDisplay.Controls.Add(inputPanel);

            var todos = _todoService.GetTodos(_currentUserId);
            foreach (var todo in todos)
                flpDisplay.Controls.Add(CreateTodoItem(todo));
        }

        private void ShowSearchPanel(bool show)
        {
            if (pnlContent.Controls.Count > 0 && pnlContent.Controls[pnlContent.Controls.Count - 1] is Panel searchPanel)
            {
                searchPanel.Visible = show;
            }
        }

        private UserControl CreateContactCard(Contact contact)
        {
            return new ContactCard(contact,
                id => { _contactService.ToggleFavorite(id); RefreshCurrentView(); },
                id => { var c = _contactService.GetById(id); if (c != null) OpenContactForm(c); },
                id => {
                    if (MessageBox.Show($"¿Eliminar contacto '{contact.Name}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _contactService.DeleteContact(id);
                        RefreshCurrentView();
                    }
                });
        }

        private UserControl CreateTodoItem(TodoItem todo)
        {
            return new TodoItemControl(todo,
                id => { _todoService.ToggleTodo(id); LoadTodos(); },
                id => {
                    if (MessageBox.Show("¿Eliminar esta tarea?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _todoService.DeleteTodo(id);
                        LoadTodos();
                    }
                });
        }

        private void RefreshCurrentView()
        {
            if (_currentView == "todos")
                LoadTodos();
            else if (_currentCategoryId == null)
                LoadContacts(null);
            else if (_currentCategoryId == -1)
                LoadFavorites();
            else
                LoadContacts(_currentCategoryId);
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            var term = txtSearch?.Text ?? "";
            flpDisplay!.Controls.Clear();

            if (_currentView != "todos" && !string.IsNullOrWhiteSpace(term))
            {
                var contacts = _contactService.SearchContacts(_currentUserId, term, _currentCategoryId);
                foreach (var contact in contacts)
                    flpDisplay.Controls.Add(CreateContactCard(contact));
            }
            else
            {
                RefreshCurrentView();
            }
        }

        private void BtnAddCategory_Click(object sender, EventArgs e)
        {
            using var form = new AddCategoryForm();
            if (form.ShowDialog() == DialogResult.OK)
            {
                _categoryService.AddCategory(_currentUserId, form.CategoryName, form.IconCode);
                LoadCategories();
            }
        }

        private void BtnAddContact_Click(object sender, EventArgs e)
        {
            OpenContactForm(null);
        }

        private void OpenContactForm(Contact? contact)
        {
            using var form = new ContactForm(_currentUserId, contact);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (contact == null)
                    _contactService.AddContact(form.Contact);
                else
                    _contactService.UpdateContact(form.Contact);

                RefreshCurrentView();
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}