using System.Diagnostics;
using System.Windows.Forms;

namespace cc.isr.VI.WinControls
{
    public partial class ConnectorView
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(ConnectorView));
            _layout = new TableLayoutPanel();
            _connectGroupBox = new GroupBox();
            _resourceSelectorConnector = new cc.isr.WinControls.SelectorOpener();
            _resourceInfoLabel = new Label();
            _identityTextBox = new TextBox();
            _layout.SuspendLayout();
            _connectGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // _layout
            // 
            _layout.ColumnCount = 3;
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f));
            _layout.ColumnStyles.Add(new ColumnStyle());
            _layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50.0f));
            _layout.Controls.Add(_connectGroupBox, 1, 1);
            _layout.Dock = DockStyle.Fill;
            _layout.Location = new System.Drawing.Point(0, 0);
            _layout.Name = "_Layout";
            _layout.RowCount = 3;
            _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50.0f));
            _layout.RowStyles.Add(new RowStyle());
            _layout.RowStyles.Add(new RowStyle(SizeType.Percent, 50.0f));
            _layout.Size = new System.Drawing.Size(548, 419);
            _layout.TabIndex = 1;
            // 
            // _connectGroupBox
            // 
            _connectGroupBox.Controls.Add(_resourceSelectorConnector);
            _connectGroupBox.Controls.Add(_resourceInfoLabel);
            _connectGroupBox.Controls.Add(_identityTextBox);
            _connectGroupBox.Location = new System.Drawing.Point(42, 106);
            _connectGroupBox.Name = "_ConnectGroupBox";
            _connectGroupBox.Size = new System.Drawing.Size(464, 207);
            _connectGroupBox.TabIndex = 2;
            _connectGroupBox.TabStop = false;
            _connectGroupBox.Text = "CONNECT";
            // 
            // _resourceSelectorConnector
            // 
            _resourceSelectorConnector.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            _resourceSelectorConnector.BackColor = System.Drawing.Color.Transparent;
            _resourceSelectorConnector.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _resourceSelectorConnector.Location = new System.Drawing.Point(24, 21);
            _resourceSelectorConnector.Margin = new Padding(0);
            _resourceSelectorConnector.Name = "_ResourceSelectorConnector";
            _resourceSelectorConnector.Size = new System.Drawing.Size(419, 29);
            _resourceSelectorConnector.TabIndex = 5;
            // 
            // _resourceInfoLabel
            // 
            _resourceInfoLabel.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _resourceInfoLabel.Location = new System.Drawing.Point(28, 89);
            _resourceInfoLabel.Name = "_ResourceInfoLabel";
            _resourceInfoLabel.Size = new System.Drawing.Size(408, 102);
            _resourceInfoLabel.TabIndex = 4;
            _resourceInfoLabel.Text = resources.GetString("_ResourceInfoLabel.Text");
            // 
            // _identityTextBox
            // 
            _identityTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            _identityTextBox.Location = new System.Drawing.Point(28, 61);
            _identityTextBox.Name = "_IdentityTextBox";
            _identityTextBox.ReadOnly = true;
            _identityTextBox.Size = new System.Drawing.Size(408, 25);
            _identityTextBox.TabIndex = 3;
            // 
            // ConnectorView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_layout);
            Name = "ConnectorView";
            Size = new System.Drawing.Size(548, 419);
            _layout.ResumeLayout(false);
            _connectGroupBox.ResumeLayout(false);
            _connectGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        private TableLayoutPanel _layout;
        private GroupBox _connectGroupBox;
        private cc.isr.WinControls.SelectorOpener _resourceSelectorConnector;
        private Label _resourceInfoLabel;
        private TextBox _identityTextBox;
    }
}
