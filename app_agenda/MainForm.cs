using System;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
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
        private bool _suppressSearch = false;

        public MainForm(int userId, string userName)
        {
            _currentUserId = userId;
            _userName = userName;
            _contactService = new ContactService();
            _categoryService = new CategoryService();
            _todoService = new TodoService();

            InitializeComponent();

            SetupThinScrollbar();
            LoadCategories();
            LoadContacts(null);
            LoadUserName();
        }

        // ── Scrollbar custom delgado para flpCategories ──────────────────
        [DllImport("user32.dll")]
        private static extern int ShowScrollBar(IntPtr hWnd, int wBar, int bShow);
        private const int SB_HORZ = 0;
        private const int SB_VERT = 1;

        private Panel? _scrollThumb;
        private bool _scrollThumbDragging;
        private int _scrollThumbDragOffset;
        private bool _internalScrollUpdate;

        private void HideNativeScrollbars()
        {
            if (flpCategories?.IsHandleCreated == true)
            {
                ShowScrollBar(flpCategories.Handle, SB_HORZ, 0);
                ShowScrollBar(flpCategories.Handle, SB_VERT, 0);
            }
        }

        private void SetupThinScrollbar()
        {
            // Truco para esconder scrollbars nativos pero dejar AutoScroll activo
            // (el wheel del mouse y la propiedad AutoScrollPosition siguen funcionando)
            flpCategories.HorizontalScroll.Maximum = 0;
            flpCategories.AutoScroll = false;
            flpCategories.VerticalScroll.Visible = false;
            flpCategories.HorizontalScroll.Visible = false;
            flpCategories.AutoScroll = true;

            // Belt-and-suspenders: forzar ocultamiento via Win32 cada vez que el FLP recalcule layout
            flpCategories.HandleCreated += (s, e) => HideNativeScrollbars();
            flpCategories.Layout += (s, e) => HideNativeScrollbars();
            flpCategories.SizeChanged += (s, e) => HideNativeScrollbars();

            _scrollThumb = new Panel
            {
                Width = 4,
                BackColor = Color.FromArgb(120, 220, 222),
                Cursor = Cursors.Hand,
                Visible = false
            };
            pnlSidebar.Controls.Add(_scrollThumb);
            _scrollThumb.BringToFront();

            flpCategories.Scroll += (s, e) => UpdateScrollThumb();
            flpCategories.SizeChanged += (s, e) => UpdateScrollThumb();
            flpCategories.ControlAdded += (s, e) => UpdateScrollThumb();
            flpCategories.ControlRemoved += (s, e) => UpdateScrollThumb();

            _scrollThumb.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left)
                {
                    _scrollThumbDragging = true;
                    _scrollThumbDragOffset = e.Y;
                }
            };
            _scrollThumb.MouseMove += (s, e) =>
            {
                if (!_scrollThumbDragging || _scrollThumb == null) return;
                int trackTop = flpCategories.Top;
                int trackHeight = flpCategories.Height;
                int newTop = _scrollThumb.Top + e.Y - _scrollThumbDragOffset;
                int maxTop = trackTop + trackHeight - _scrollThumb.Height;
                newTop = Math.Max(trackTop, Math.Min(maxTop, newTop));
                _scrollThumb.Top = newTop;

                int contentH = flpCategories.DisplayRectangle.Height;
                int viewH = flpCategories.ClientSize.Height;
                if (contentH <= viewH) return;
                float ratio = (float)(newTop - trackTop) / Math.Max(1, trackHeight - _scrollThumb.Height);
                int scrollPos = (int)(ratio * (contentH - viewH));
                _internalScrollUpdate = true;
                flpCategories.AutoScrollPosition = new Point(0, scrollPos);
                _internalScrollUpdate = false;
            };
            _scrollThumb.MouseUp += (s, e) => _scrollThumbDragging = false;
        }

        private void UpdateScrollThumb()
        {
            if (_scrollThumb == null) return;
            if (_internalScrollUpdate) return;

            int contentH = flpCategories.DisplayRectangle.Height;
            int viewH = flpCategories.ClientSize.Height;

            if (contentH <= viewH)
            {
                _scrollThumb.Visible = false;
                return;
            }

            int trackTop = flpCategories.Top;
            int trackHeight = flpCategories.Height;
            int thumbHeight = Math.Max(30, (int)((float)viewH / contentH * trackHeight));
            int scrollPos = -flpCategories.AutoScrollPosition.Y;
            int maxScroll = contentH - viewH;
            int thumbTop = trackTop + (maxScroll > 0
                ? (int)((float)scrollPos / maxScroll * (trackHeight - thumbHeight))
                : 0);

            _scrollThumb.Size = new Size(4, thumbHeight);
            _scrollThumb.Location = new Point(pnlSidebar.Width - 8, thumbTop);
            _scrollThumb.Visible = true;
            _scrollThumb.BringToFront();
        }

        private void SetActiveButton(IconButton btn)
        {
            if (_activeButton != null)
            {
                _activeButton.BackColor = Color.Transparent;
                var oldIndicator = _activeButton.Controls.OfType<Panel>().FirstOrDefault(p => p.Name == "activeIndicator");
                if (oldIndicator != null)
                    _activeButton.Controls.Remove(oldIndicator);
            }

            _activeButton = btn;
            _activeButton.BackColor = ColorTranslator.FromHtml("#1E857E");

            var indicator = new Panel
            {
                Name = "activeIndicator",
                Size = new Size(5, _activeButton.Height),
                Location = new Point(0, 0),
                BackColor = Color.White
            };
            _activeButton.Controls.Add(indicator);
        }

        private void LoadUserName()
        {
            string stylizedName = char.ToUpper(_userName[0]) + _userName.Substring(1).ToLower();
            lblWelcome.Text = $"Hola, {stylizedName}";
        }

        private void LoadCategories()
        {
            flpCategories.Controls.Clear();

            btnTodos = CreateSidebarButton("Todos", IconChar.Inbox, () => {
                _currentCategoryId = null;
                ClearSearch();
                LoadContacts(null);
            });

            btnFavoritos = CreateSidebarButton("Favoritos", IconChar.Heart, () => {
                ClearSearch();
                LoadFavorites();
            });

            flpCategories!.Controls.Add(btnTodos);
            flpCategories.Controls.Add(btnFavoritos);

            var separator = new Panel
            {
                Size = new Size(190, 1),
                BackColor = Color.FromArgb(50, Color.White),
                Margin = new Padding(20, 10, 20, 10)
            };
            flpCategories.Controls.Add(separator);

            var categories = _categoryService.GetCategoriesByUser(_currentUserId);
            foreach (var cat in categories)
            {
                var capturedCat = cat;
                var icon = ParseIcon(cat.IconCode);
                var btn = CreateSidebarButton(cat.Name, icon, () => {
                    _currentCategoryId = capturedCat.Id;
                    ClearSearch();
                    LoadContacts(capturedCat.Id);
                });
                btn.ContextMenuStrip = BuildCategoryContextMenu(capturedCat);
                flpCategories.Controls.Add(btn);
            }

            var separator2 = new Panel
            {
                Size = new Size(190, 1),
                BackColor = Color.FromArgb(50, Color.White),
                Margin = new Padding(20, 10, 20, 10)
            };
            flpCategories.Controls.Add(separator2);

            var btnAddCategory = CreateSidebarButton("Nueva Categoría", IconChar.PlusCircle, () => BtnAddCategory_Click(null!, null!));
            btnTodo = CreateSidebarButton("To Do List", IconChar.ListCheck, () => LoadTodos());

            flpCategories.Controls.Add(btnAddCategory);
            flpCategories.Controls.Add(btnTodo);

            if (_currentCategoryId == null) SetActiveButton(btnTodos);
        }

        private IconChar ParseIcon(string code)
        {
            return code switch
            {
                "Folder" => IconChar.Folder,
                "FolderOpen" => IconChar.FolderOpen,
                "User" => IconChar.User,
                "Users" => IconChar.Users,
                "Briefcase" => IconChar.Briefcase,
                "Home" => IconChar.House,
                "House" => IconChar.House,
                "Building" => IconChar.Building,
                "Phone" => IconChar.Phone,
                "Envelope" => IconChar.Envelope,
                "Heart" => IconChar.Heart,
                "Star" => IconChar.Star,
                "Camera" => IconChar.Camera,
                "Music" => IconChar.Music,
                "Coffee" => IconChar.Coffee,
                "Utensils" => IconChar.Utensils,
                "Plane" => IconChar.Plane,
                "Car" => IconChar.Car,
                "Bicycle" => IconChar.Bicycle,
                "Dumbbell" => IconChar.Dumbbell,
                "Book" => IconChar.Book,
                "GraduationCap" => IconChar.GraduationCap,
                "Globe" => IconChar.Globe,
                "Calendar" => IconChar.Calendar,
                "Bell" => IconChar.Bell,
                "Gift" => IconChar.Gift,
                "Tag" => IconChar.Tag,
                "Trophy" => IconChar.Trophy,
                "Dog" => IconChar.Dog,
                "Cat" => IconChar.Cat,
                "Lightbulb" => IconChar.Lightbulb,
                "CreditCard" => IconChar.CreditCard,
                "Hospital" => IconChar.Hospital,
                _ => IconChar.Folder
            };
        }

        private void LoadContacts(int? categoryId)
        {
            _currentView = "contacts";
            ShowSearchPanel(true);
            flpDisplay!.Controls.Clear();

            var contacts = categoryId == null
                ? _contactService.GetContacts(_currentUserId)
                : _contactService.GetContacts(_currentUserId, categoryId);

            foreach (var contact in contacts)
                flpDisplay.Controls.Add(CreateContactCard(contact));
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

            // Ancho responsivo: ocupa el área disponible menos espacio para el scrollbar y los márgenes
            int viewportWidth = flpDisplay.ClientSize.Width;
            if (viewportWidth <= 0) viewportWidth = pnlContent.ClientSize.Width;
            if (viewportWidth <= 0) viewportWidth = 600;
            int rowWidth = Math.Max(340, viewportWidth - 30);

            const int btnW = 120;
            const int padL = 16;
            const int padR = 16;
            const int gap = 12;

            var inputPanel = new Panel
            {
                Size = new Size(rowWidth, 60),
                BackColor = Color.White,
                Margin = new Padding(4, 6, 4, 8)
            };

            var txtNewTodo = new TextBox
            {
                Location = new Point(padL, 16),
                Size = new Size(rowWidth - padL - btnW - gap - padR, 28),
                Font = new Font("Verdana", 12),
                BorderStyle = BorderStyle.FixedSingle
            };

            var btnAddTodo = new IconButton
            {
                Text = "Agregar",
                Location = new Point(rowWidth - btnW - padR, 12),
                Size = new Size(btnW, 36),
                IconChar = IconChar.Plus,
                IconColor = Color.White,
                IconSize = 20,
                BackColor = ColorTranslator.FromHtml("#249EA0"),
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                TextImageRelation = TextImageRelation.ImageBeforeText,
                Cursor = Cursors.Hand,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAddTodo.FlatAppearance.BorderSize = 0;

            void AddCurrentTodo()
            {
                if (!string.IsNullOrWhiteSpace(txtNewTodo.Text))
                {
                    _todoService.AddTodo(_currentUserId, txtNewTodo.Text.Trim());
                    txtNewTodo.Clear();
                    LoadTodos();
                }
            }

            btnAddTodo.Click += (s, e) => AddCurrentTodo();
            txtNewTodo.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Enter)
                {
                    AddCurrentTodo();
                    e.SuppressKeyPress = true;
                }
            };

            inputPanel.Controls.AddRange(new Control[] { btnAddTodo, txtNewTodo });
            flpDisplay.Controls.Add(inputPanel);

            var todos = _todoService.GetTodos(_currentUserId);
            foreach (var todo in todos)
                flpDisplay.Controls.Add(CreateTodoItem(todo, rowWidth));

            // Foco automático en el textbox al entrar a To Do List
            BeginInvoke(new Action(() => txtNewTodo.Focus()));
        }

        private void ShowSearchPanel(bool show)
        {
            pnlSearch.Visible = show;
        }

        private UserControl CreateContactCard(Contact contact)
        {
            return new ContactCard(contact,
                id => { _contactService.ToggleFavorite(id); RefreshCurrentView(); },
                id => { var c = _contactService.GetById(id); if (c != null) OpenContactForm(c); },
                id =>
                {
                    if (MessageBox.Show($"¿Eliminar contacto '{contact.Name}'?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        _contactService.DeleteContact(id);
                        RefreshCurrentView();
                    }
                });
        }

        private UserControl CreateTodoItem(TodoItem todo, int width)
        {
            return new TodoItemControl(todo, width,
                id => { _todoService.ToggleTodo(id); LoadTodos(); },
                id =>
                {
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

        private void ClearSearch()
        {
            if (txtSearch == null) return;
            _suppressSearch = true;
            txtSearch.Clear();
            _suppressSearch = false;
        }

        private void TxtSearch_TextChanged(object? sender, EventArgs e)
        {
            if (_suppressSearch) return;

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
                if (_categoryService.CategoryExists(_currentUserId, form.CategoryName))
                {
                    MessageBox.Show(
                        $"Ya existe una categoría con el nombre '{form.CategoryName}'.",
                        "Nombre duplicado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                _categoryService.AddCategory(_currentUserId, form.CategoryName, form.IconCode);
                LoadCategories();
            }
        }

        private ContextMenuStrip BuildCategoryContextMenu(Category cat)
        {
            var ctx = new ContextMenuStrip
            {
                Font = new Font("Segoe UI", 10),
                ShowImageMargin = false
            };

            var editItem = new ToolStripMenuItem("Editar");
            editItem.Click += (s, e) => EditCategory(cat);

            var deleteItem = new ToolStripMenuItem("Eliminar")
            {
                ForeColor = Color.FromArgb(200, 50, 50)
            };
            deleteItem.Click += (s, e) => DeleteCategory(cat);

            ctx.Items.Add(editItem);
            ctx.Items.Add(deleteItem);
            return ctx;
        }

        private void EditCategory(Category cat)
        {
            using var form = new AddCategoryForm(cat);
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (_categoryService.CategoryExists(_currentUserId, form.CategoryName, cat.Id))
                {
                    MessageBox.Show(
                        $"Ya existe una categoría con el nombre '{form.CategoryName}'.",
                        "Nombre duplicado",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    return;
                }
                cat.Name = form.CategoryName;
                cat.IconCode = form.IconCode;
                _categoryService.UpdateCategory(cat);
                LoadCategories();
                RefreshCurrentView();
            }
        }

        private void DeleteCategory(Category cat)
        {
            var contacts = _contactService.GetContacts(_currentUserId, cat.Id);
            string msg = $"¿Eliminar la categoría '{cat.Name}'?";
            if (contacts.Count > 0)
                msg += $"\n\nEsta categoría tiene {contacts.Count} contacto(s) que también serán eliminados.";

            var result = MessageBox.Show(msg, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            foreach (var contact in contacts)
                _contactService.DeleteContact(contact.Id);
            _categoryService.DeleteCategory(cat.Id);

            if (_currentCategoryId == cat.Id)
                _currentCategoryId = null;

            LoadCategories();
            LoadContacts(_currentCategoryId);
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

        private void MainForm_Load(object sender, EventArgs e)
        {
        }
    }
}
