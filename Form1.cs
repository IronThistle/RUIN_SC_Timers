using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace RUIN_SC_Timers.App
{
    public partial class Form1 : Form
    {
        private TableLayoutPanel mainGrid;
        private TrackBar sldOpacity;
        private CheckBox chkOnTop;
        private Panel headerPanel;
        private List<TimerController> controllers = new List<TimerController>();
        private Button btnS, btnM, btnD;

        public Form1()
        {
            // This calls the version in Form1.Designer.cs
            InitializeComponent();

            this.BackColor = Color.FromArgb(20, 20, 20);
            this.Text = "RUIN SC Timers";
            this.MinimumSize = new Size(380, 280);

            SetupAppLayout();

            this.Shown += (s, e) => SyncLayout();
            this.Resize += (s, e) => SyncLayout();
        }

        private void SetupAppLayout()
        {
            headerPanel = new Panel { Dock = DockStyle.Top, Height = 45, BackColor = Color.FromArgb(45, 45, 45) };
            int cTop = 8; int cHeight = 28;

            chkOnTop = new CheckBox { Text = "ON TOP", ForeColor = Color.White, Left = 10, Top = cTop, Width = 65, Height = cHeight, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 7f) };
            chkOnTop.CheckedChanged += (s, e) => this.TopMost = chkOnTop.Checked;

            sldOpacity = new TrackBar { Minimum = 20, Maximum = 100, Value = 100, Left = 75, Top = cTop - 2, Width = 60, Height = cHeight, TickStyle = TickStyle.None };
            sldOpacity.Scroll += (s, e) => this.Opacity = sldOpacity.Value / 100.0;

            btnS = CreateHeaderBtn("SMALL", 145, cTop, cHeight);
            btnM = CreateHeaderBtn("MEDIUM", 235, cTop, cHeight);
            btnD = CreateHeaderBtn("DEFAULT", 325, cTop, cHeight);

            btnS.Click += (s, e) => { this.WindowState = FormWindowState.Normal; this.Size = new Size(400, 310); };
            btnM.Click += (s, e) => { this.WindowState = FormWindowState.Normal; this.Size = new Size(700, 500); };
            btnD.Click += (s, e) => { this.WindowState = FormWindowState.Normal; this.Size = new Size(1100, 800); };

            headerPanel.Controls.AddRange(new Control[] { chkOnTop, sldOpacity, btnS, btnM, btnD });
            this.Controls.Add(headerPanel);

            mainGrid = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 3, RowCount = 2, BackColor = Color.Transparent };
            for (int i = 0; i < 3; i++) mainGrid.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            for (int i = 0; i < 2; i++) mainGrid.RowStyles.Add(new RowStyle(SizeType.Percent, 50f));

            this.Controls.Add(mainGrid);
            mainGrid.BringToFront();

            AddSection("THE VAULT", Color.Purple, true);
            AddSection("TABLET 5", Color.LimeGreen, false);
            AddSection("TABLET 6", Color.LimeGreen, false);
            AddSection("THE LAST RESORT", Color.Yellow, false);
            AddSection("THE WASTE LAND", Color.DeepSkyBlue, false);
            AddSection("THE CRYPT", Color.LimeGreen, false);
        }

        private void SyncLayout()
        {
            if (btnS == null) return;
            bool isCompact = this.Width < 550;
            btnS.Text = isCompact ? "S" : "SMALL"; btnM.Text = isCompact ? "M" : "MEDIUM"; btnD.Text = isCompact ? "D" : "DEFAULT";
            btnS.Width = isCompact ? 35 : 85; btnM.Width = isCompact ? 35 : 85; btnD.Width = isCompact ? 35 : 85;
            btnS.Left = 145; btnM.Left = btnS.Right + 5; btnD.Left = btnM.Right + 5;
            foreach (var c in controllers) c.UpdateLayout();
        }

        private Button CreateHeaderBtn(string txt, int x, int y, int h) => new Button { Text = txt, Left = x, Top = y, Height = h, Width = 85, BackColor = Color.FromArgb(70, 70, 70), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Font = new Font("Segoe UI", 7f, FontStyle.Bold) };

        private void AddSection(string title, Color titleColor, bool isVault)
        {
            var controller = new TimerController(title, titleColor, isVault);
            controllers.Add(controller);
            mainGrid.Controls.Add(controller.ContainerPanel);
        }
    }
}