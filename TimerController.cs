using System;
using System.Drawing;
using System.Windows.Forms;

namespace RUIN_SC_Timers.App
{
    public class TimerController
    {
        public Panel ContainerPanel { get; } = new Panel();
        private Label lblTitle = new Label();
        private Label lblTime = new Label();
        private Button btnStart = new Button();
        private TableLayoutPanel btnRow = new TableLayoutPanel();
        private Button btnMinus, btnReset, btnPlus;

        // Explicitly naming the Timer type to avoid ambiguity
        private System.Windows.Forms.Timer coreTimer = new System.Windows.Forms.Timer();

        private int secondsRemaining;
        private bool isRunning = false;
        private bool isVaultMode;
        private int vaultState = 0;

        public TimerController(string title, Color titleColor, bool isVault)
        {
            this.isVaultMode = isVault; this.secondsRemaining = isVault ? 0 : 1800;
            ContainerPanel.Dock = DockStyle.Fill; ContainerPanel.Padding = new Padding(2); ContainerPanel.BorderStyle = BorderStyle.FixedSingle;

            lblTitle.Text = title; lblTitle.ForeColor = titleColor; lblTitle.Dock = DockStyle.Top; lblTitle.TextAlign = ContentAlignment.MiddleCenter; lblTitle.AutoSize = false;
            lblTime.Text = isVault ? "00:00" : "30:00"; lblTime.ForeColor = Color.Red; lblTime.Dock = DockStyle.Fill; lblTime.TextAlign = ContentAlignment.MiddleCenter; lblTime.AutoSize = false; lblTime.UseCompatibleTextRendering = true;

            btnStart.Text = isVaultMode ? "INITIATE" : "START"; btnStart.BackColor = isVaultMode ? Color.RoyalBlue : Color.Green; btnStart.ForeColor = Color.White; btnStart.Dock = DockStyle.Bottom; btnStart.FlatStyle = FlatStyle.Flat;
            btnStart.Click += (s, e) => { if (isVaultMode) HandleVault(); else ToggleTimer(); };

            btnRow.Dock = DockStyle.Bottom; btnRow.ColumnCount = 3;
            btnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f)); btnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f)); btnRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25f));

            btnMinus = CreateSmallBtn("-30", Color.FromArgb(60, 60, 60)); btnReset = CreateSmallBtn("RESET", Color.Firebrick); btnPlus = CreateSmallBtn("+30", Color.FromArgb(60, 60, 60));
            btnMinus.Click += (s, e) => Adjust(-30); btnReset.Click += (s, e) => Reset(); btnPlus.Click += (s, e) => Adjust(30);

            btnRow.Controls.Add(btnMinus, 0, 0); btnRow.Controls.Add(btnReset, 1, 0); btnRow.Controls.Add(btnPlus, 2, 0);
            ContainerPanel.Controls.Add(lblTime); ContainerPanel.Controls.Add(lblTitle); ContainerPanel.Controls.Add(btnStart); ContainerPanel.Controls.Add(btnRow);

            coreTimer.Interval = 1000; coreTimer.Tick += (s, e) => { if (secondsRemaining > 0) secondsRemaining--; else EndTimer(); UpdateDisplay(); };
        }

        public void UpdateLayout()
        {
            int h = ContainerPanel.Height; int w = ContainerPanel.Width; if (h <= 10 || w <= 10) return;
            bool showAdjusters = h > 165; btnRow.Visible = showAdjusters; btnRow.Height = showAdjusters ? Math.Max(22, h / 9) : 0;
            btnStart.Height = Math.Max(26, h / 7); btnStart.Font = new Font("Segoe UI", Math.Max(7f, h / 28f), FontStyle.Bold);
            btnReset.Text = (w < 95) ? "R" : "RESET"; btnMinus.Text = (w < 95) ? "-" : "-30"; btnPlus.Text = (w < 95) ? "+" : "+30";
            lblTitle.Height = Math.Max(15, h / 12); float titleFontSize = Math.Max(6f, Math.Min(h / 22f, w / 16f)); lblTitle.Font = new Font("Segoe UI", titleFontSize, FontStyle.Bold);
            float fontSizeH = h / 4.8f; float fontSizeW = w / 5.2f; float finalSize = Math.Min(fontSizeH, fontSizeW);
            lblTime.Font = new Font("Consolas", Math.Max(8, finalSize), FontStyle.Bold);
        }

        private Button CreateSmallBtn(string t, Color bg) => new Button { Text = t, BackColor = bg, ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Dock = DockStyle.Fill, Margin = new Padding(1), Font = new Font("Segoe UI", 6.5f, FontStyle.Bold) };
        private void ToggleTimer() { isRunning = !isRunning; coreTimer.Enabled = isRunning; btnStart.Text = isRunning ? "STOP" : "START"; btnStart.BackColor = isRunning ? Color.Red : Color.Green; }
        private void HandleVault() { if (vaultState == 0) { vaultState = 1; secondsRemaining = 120; coreTimer.Start(); btnStart.Text = "OPEN"; btnStart.BackColor = Color.Green; lblTime.ForeColor = Color.LimeGreen; } }
        private void EndTimer() { if (isVaultMode) { if (vaultState == 1) { vaultState = 2; secondsRemaining = 1800; btnStart.Text = "CLOSED"; btnStart.BackColor = Color.DarkRed; lblTime.ForeColor = Color.Red; } else { vaultState = 1; secondsRemaining = 120; btnStart.Text = "OPEN"; btnStart.BackColor = Color.Green; lblTime.ForeColor = Color.LimeGreen; } } else { coreTimer.Stop(); isRunning = false; secondsRemaining = 1800; btnStart.Text = "START"; btnStart.BackColor = Color.Green; lblTime.ForeColor = Color.Red; } }
        private void Adjust(int val) { secondsRemaining = Math.Max(0, secondsRemaining + val); UpdateDisplay(); }
        private void Reset() { coreTimer.Stop(); isRunning = false; vaultState = 0; secondsRemaining = isVaultMode ? 0 : 1800; btnStart.Text = isVaultMode ? "INITIATE" : "START"; btnStart.BackColor = isVaultMode ? Color.RoyalBlue : Color.Green; lblTime.ForeColor = Color.Red; UpdateDisplay(); }
        private void UpdateDisplay() { TimeSpan t = TimeSpan.FromSeconds(secondsRemaining); lblTime.Text = string.Format("{0:D2}:{1:D2}", (int)t.TotalMinutes, t.Seconds); }
    }
}