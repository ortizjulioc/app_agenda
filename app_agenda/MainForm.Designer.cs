namespace app_agenda
{
    partial class MainForm
    {
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            SuspendLayout();
            // 
            // MainForm
            // 
            ClientSize = new Size(1000, 650);
            MinimumSize = new Size(800, 500);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Agenda Telefónica";
            Load += MainForm_Load;
            ResumeLayout(false);
        }

        #endregion
    }
}