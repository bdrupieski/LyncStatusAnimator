using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LyncStatusAnimator
{
    public sealed partial class LyncStatusAnimatorForm : Form
    {
        private readonly BackgroundWorker _backgroundWorker;
        private readonly LyncAnimator _lyncAnimator;

        public LyncStatusAnimatorForm()
        {
            InitializeComponent();
            MinimumSize = Size;
            MaximumSize = Size;

            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.DoWork += BackgroundWorker_DoWork;
            _backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            _backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            Resize += LyncStatusAnimatorForm_Resize;
            notifyIcon.DoubleClick += NotifyIcon_DoubleClick;
            buttonStop.Click += ButtonStop_Click;
            buttonGo.Click += ButtonGo_Click;

            _lyncAnimator = new LyncAnimator(ShouldCancel, ReportNewStatus);
            labelNewStatus.Text = string.Empty;
        }

        private void LyncStatusAnimatorForm_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(500);
                ShowInTaskbar = false;
            }
            else if (WindowState == FormWindowState.Normal)
            {
                notifyIcon.Visible = false;
                ShowInTaskbar = true;
            }
        }

        private void NotifyIcon_DoubleClick(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Normal;
        }

        private void ButtonGo_Click(object sender, EventArgs e)
        {
            _backgroundWorker.RunWorkerAsync();
            buttonGo.Enabled = false;
            buttonStop.Enabled = true;
        }

        private void ButtonStop_Click(object sender, EventArgs e)
        {
            _backgroundWorker.CancelAsync();
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            _lyncAnimator.Animate();
        }

        private void ReportNewStatus(string newStatus)
        {
            _backgroundWorker.ReportProgress(0, newStatus);
        }

        private bool ShouldCancel()
        {
            return _backgroundWorker.CancellationPending;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            var newStatus = (string)e.UserState;
            labelNewStatus.Text = newStatus;
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            buttonGo.Enabled = true;
            buttonStop.Enabled = false;
        }
    }
}