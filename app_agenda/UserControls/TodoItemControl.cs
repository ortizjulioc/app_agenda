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

        public TodoItemControl(TodoItem todo, Action<int> onToggle, Action<int> onDelete)
        {
            _todo = todo;
            _onToggle = onToggle;
            _onDelete = onDelete;

            Size = new Size(560, 48);
            Margin = new Padding(8, 4, 8, 4);

            _cardPanel = new Panel
            {
                Size = new Size(544, 48),
                Location = new Point(0, 0),
                BackColor = Color.White,
                Dock = DockStyle.None
            };

            using (var g = _cardPanel.CreateGraphics())
            using (var pen = new Pen(ColorTranslator.FromHtml("#249EA0"), 2))
            {
                g.DrawLine(pen, 0, 47, 544, 47);
            }

            _chkCompleted = new CheckBox
            {
                Location = new Point(12, 12),
                Size = new Size(24, 24),
                Checked = todo.IsCompleted,
                Cursor = Cursors.Hand,
                BackColor = Color.Transparent
            };
            _chkCompleted.CheckedChanged += (s, e) => _onToggle(_todo.Id);

            _lblTitle = new Label
            {
                Text = todo.Title,
                Location = new Point(48, 14),
                Size = new Size(420, 22),
                ForeColor = todo.IsCompleted ? Color.Gray : Color.Black,
                Font = new Font("Segoe UI", 10),
                AutoEllipsis = true
            };
            if (todo.IsCompleted)
                _lblTitle.Font = new Font(_lblTitle.Font, FontStyle.Strikeout);

            _btnDelete = new IconButton
            {
                Size = new Size(32, 32),
                Location = new Point(488, 8),
                IconChar = IconChar.Trash,
                IconColor = ColorTranslator.FromHtml("#249EA0"),
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
            _lblTitle.ForeColor = isCompleted ? Color.Gray : Color.Black;
            _lblTitle.Font = new Font(_lblTitle.Font, isCompleted ? FontStyle.Strikeout : FontStyle.Regular);
        }
    }
}