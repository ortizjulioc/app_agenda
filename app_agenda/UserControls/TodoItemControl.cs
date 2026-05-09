using System;
using System.Drawing;
using System.Windows.Forms;
using FontAwesome.Sharp;
using app_agenda.Data.Models;

namespace app_agenda.UI.UserControls
{
    public class TodoItemControl : UserControl
    {
        private readonly TodoItem _todo;
        private readonly Action<int> _onToggle;
        private readonly Action<int> _onDelete;

        private readonly Panel _cardPanel;
        private readonly CheckBox _chkCompleted;
        private readonly Label _lblTitle;
        private readonly IconButton _btnDelete;

        private static readonly Color TealAccent = ColorTranslator.FromHtml("#249EA0");
        private static readonly Color TextColor = Color.FromArgb(40, 40, 60);

        public TodoItemControl(TodoItem todo, int width, Action<int> onToggle, Action<int> onDelete)
        {
            _todo = todo;
            _onToggle = onToggle;
            _onDelete = onDelete;

            int innerWidth = Math.Max(280, width - 8);
            Size = new Size(width, 56);
            Margin = new Padding(4, 2, 4, 2);

            _cardPanel = new Panel
            {
                Size = new Size(innerWidth, 56),
                Location = new Point(0, 0),
                BackColor = Color.White
            };

            // Línea inferior teal — usando Paint para que persista entre redibujados
            _cardPanel.Paint += (s, e) =>
            {
                using var pen = new Pen(TealAccent, 2);
                e.Graphics.DrawLine(pen, 0, _cardPanel.Height - 2, _cardPanel.Width, _cardPanel.Height - 2);
            };

            _chkCompleted = new CheckBox
            {
                Location = new Point(14, 18),
                Size = new Size(22, 22),
                Checked = todo.IsCompleted,
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            _chkCompleted.CheckedChanged += (s, e) => _onToggle(_todo.Id);

            _lblTitle = new Label
            {
                Text = todo.Title,
                Location = new Point(46, 12),
                Size = new Size(innerWidth - 46 - 56, 32),
                ForeColor = todo.IsCompleted ? Color.Gray : TextColor,
                Font = new Font("Verdana", 12, todo.IsCompleted ? FontStyle.Strikeout : FontStyle.Regular),
                AutoEllipsis = true,
                TextAlign = ContentAlignment.MiddleLeft,
                BackColor = Color.Transparent
            };

            _btnDelete = new IconButton
            {
                Size = new Size(40, 40),
                Location = new Point(innerWidth - 48, 8),
                IconChar = IconChar.Trash,
                IconColor = TealAccent,
                IconSize = 26,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            _btnDelete.MouseClick += (s, e) => _onDelete(_todo.Id);

            _cardPanel.Controls.AddRange(new Control[] { _chkCompleted, _lblTitle, _btnDelete });
            Controls.Add(_cardPanel);
        }

        public void UpdateCompleted(bool isCompleted)
        {
            _todo.IsCompleted = isCompleted;
            _lblTitle.ForeColor = isCompleted ? Color.Gray : TextColor;
            _lblTitle.Font = new Font(_lblTitle.Font.FontFamily, _lblTitle.Font.Size,
                isCompleted ? FontStyle.Strikeout : FontStyle.Regular);
        }
    }
}
