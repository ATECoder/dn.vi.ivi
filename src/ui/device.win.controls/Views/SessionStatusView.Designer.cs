using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls
{
    public partial class SessionStatusView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(SessionStatusView));
            StatusView = new StatusView();
            SessionView = new SessionView();
            _selectorOpener = new cc.isr.WinControls.SelectorOpener();
            SuspendLayout();
            //
            // _statusView
            //
            StatusView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            StatusView.BackColor = System.Drawing.Color.Transparent;
            StatusView.Dock = DockStyle.Bottom;
            StatusView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            StatusView.Location = new System.Drawing.Point(0, 266);
            StatusView.Margin = new Padding(0);
            StatusView.Name = "_StatusView";
            StatusView.Size = new System.Drawing.Size(383, 31);
            StatusView.TabIndex = 0;
            //
            // _sessionView
            //
            SessionView.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            SessionView.Dock = DockStyle.Fill;
            SessionView.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            SessionView.Location = new System.Drawing.Point(0, 0);
            SessionView.Margin = new Padding(3, 4, 3, 4);
            SessionView.Name = "_SessionView";
            SessionView.Size = new System.Drawing.Size(383, 266);
            SessionView.TabIndex = 1;
            SessionView.Termination = @"\n";
            //
            // _selectorOpener
            //
            _selectorOpener.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _selectorOpener.BackColor = System.Drawing.Color.Transparent;
            _selectorOpener.Dock = DockStyle.Bottom;
            _selectorOpener.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _selectorOpener.Location = new System.Drawing.Point(0, 297);
            _selectorOpener.Margin = new Padding(0);
            _selectorOpener.Name = "_SelectorOpener";
            _selectorOpener.Size = new System.Drawing.Size(383, 29);
            _selectorOpener.TabIndex = 2;
            //
            // SessionStatusView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(SessionView);
            Controls.Add(StatusView);
            Controls.Add(_selectorOpener);
            Name = "SessionStatusView";
            Size = new System.Drawing.Size(383, 326);
            ResumeLayout(false);
        }

        private cc.isr.WinControls.SelectorOpener _selectorOpener;
    }
}
