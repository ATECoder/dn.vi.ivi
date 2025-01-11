using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.SubsystemsWinControls
{
    public partial class TraceBufferView
    {
        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.  
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(TraceBufferView));
            _limitToolStripPanel = new ToolStripPanel();
            _subsystemToolStrip = new ToolStrip();
            _subsystemSplitButton = new ToolStripSplitButton();
            _applySettingsMenuItem = new ToolStripMenuItem();
            _applySettingsMenuItem.Click += new EventHandler(ApplySettingsMenuItem_Click);
            _readSettingsMenuItem = new ToolStripMenuItem();
            _readSettingsMenuItem.Click += new EventHandler(ReadSettingsMenuItem_Click);
            _feedSourceComboLabel = new ToolStripLabel();
            _feedSourceComboBox = new ToolStripComboBox();
            _elementGroupToggleButton = new ToolStripButton();
            _elementGroupToggleButton.CheckStateChanged += new EventHandler(ElementGroupToggleButton_CheckStateChanged);
            _bufferControlComboBoxLabel = new ToolStripLabel();
            _feedControlComboBox = new ToolStripComboBox();
            _bufferConfigurationToolStrip = new ToolStrip();
            _bufferCondigureLabel = new ToolStripLabel();
            _bufferNameLabel = new ToolStripLabel();
            _sizeNumericLabel = new ToolStripLabel();
            _sizeNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _preTriggerCountNumericLabel = new ToolStripLabel();
            _preTriggerCountNumeric = new cc.isr.WinControls.ToolStripNumericUpDown();
            _clearBufferButton = new ToolStripButton();
            _clearBufferButton.Click += new EventHandler(ClearBufferButton_Click);
            _freeCountLabelLabel = new ToolStripLabel();
            _freeCountLabel = new ToolStripLabel();
            _bufferFormatToolStrip = new ToolStrip();
            _formatLabel = new ToolStripLabel();
            _timestampFormatToggleButton = new ToolStripButton();
            _timestampFormatToggleButton.CheckStateChanged += new EventHandler(TimestampFormatToggleButton_CheckStateChanged);
            _infoTextBox = new TextBox();
            _limitToolStripPanel.SuspendLayout();
            _subsystemToolStrip.SuspendLayout();
            _bufferConfigurationToolStrip.SuspendLayout();
            _bufferFormatToolStrip.SuspendLayout();
            SuspendLayout();
            // 
            // _limitToolStripPanel
            // 
            _limitToolStripPanel.BackColor = System.Drawing.Color.Transparent;
            _limitToolStripPanel.Controls.Add(_bufferConfigurationToolStrip);
            _limitToolStripPanel.Controls.Add(_subsystemToolStrip);
            _limitToolStripPanel.Controls.Add(_bufferFormatToolStrip);
            _limitToolStripPanel.Dock = DockStyle.Top;
            _limitToolStripPanel.Location = new System.Drawing.Point(1, 1);
            _limitToolStripPanel.Name = "_LimitToolStripPanel";
            _limitToolStripPanel.Orientation = Orientation.Horizontal;
            _limitToolStripPanel.RowMargin = new Padding(0);
            _limitToolStripPanel.Size = new System.Drawing.Size(440, 78);
            // 
            // _subsystemToolStrip
            // 
            _subsystemToolStrip.BackColor = System.Drawing.Color.Transparent;
            _subsystemToolStrip.GripMargin = new Padding(0);
            _subsystemToolStrip.Items.AddRange(new ToolStripItem[] { _subsystemSplitButton, _feedSourceComboLabel, _feedSourceComboBox, _elementGroupToggleButton, _bufferControlComboBoxLabel, _feedControlComboBox });
            _subsystemToolStrip.Location = new System.Drawing.Point(0, 0);
            _subsystemToolStrip.Name = "_SubsystemToolStrip";
            _subsystemToolStrip.Size = new System.Drawing.Size(440, 25);
            _subsystemToolStrip.Stretch = true;
            _subsystemToolStrip.TabIndex = 0;
            // 
            // _subsystemSplitButton
            // 
            _subsystemSplitButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _subsystemSplitButton.DoubleClickEnabled = true;
            _subsystemSplitButton.DropDownItems.AddRange(new ToolStripItem[] { _applySettingsMenuItem, _readSettingsMenuItem });
            _subsystemSplitButton.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _subsystemSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _subsystemSplitButton.Name = "_SubsystemSplitButton";
            _subsystemSplitButton.Size = new System.Drawing.Size(58, 22);
            _subsystemSplitButton.Text = "Trace";
            _subsystemSplitButton.ToolTipText = "Trace or buffer label";
            // 
            // _applySettingsMenuItem
            // 
            _applySettingsMenuItem.Name = "_applySettingsMenuItem";
            _applySettingsMenuItem.Size = new System.Drawing.Size(150, 22);
            _applySettingsMenuItem.Text = "Apply Settings";
            _applySettingsMenuItem.ToolTipText = "Applies settings onto the instrument";
            // 
            // _readSettingsMenuItem
            // 
            _readSettingsMenuItem.Name = "_ReadSettingsMenuItem";
            _readSettingsMenuItem.Size = new System.Drawing.Size(150, 22);
            _readSettingsMenuItem.Text = "Read Settings";
            _readSettingsMenuItem.ToolTipText = "Reads settings from the instrument";
            // 
            // _feedSourceComboLabel
            // 
            _feedSourceComboLabel.Name = "_FeedSourceComboLabel";
            _feedSourceComboLabel.Size = new System.Drawing.Size(34, 22);
            _feedSourceComboLabel.Text = "Data:";
            // 
            // _feedSourceComboBox
            // 
            _feedSourceComboBox.Name = "_FeedSourceComboBox";
            _feedSourceComboBox.Size = new System.Drawing.Size(80, 25);
            _feedSourceComboBox.Text = "Sense";
            _feedSourceComboBox.ToolTipText = "Selects the source of buffer data";
            // 
            // _elementGroupToggleButton
            // 
            _elementGroupToggleButton.Checked = true;
            _elementGroupToggleButton.CheckOnClick = true;
            _elementGroupToggleButton.CheckState = CheckState.Indeterminate;
            _elementGroupToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _elementGroupToggleButton.Image = (System.Drawing.Image)resources.GetObject("_ElementGroupToggleButton.Image");
            _elementGroupToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _elementGroupToggleButton.Name = "_ElementGroupToggleButton";
            _elementGroupToggleButton.Size = new System.Drawing.Size(67, 22);
            _elementGroupToggleButton.Text = "Elements ?";
            _elementGroupToggleButton.ToolTipText = "Toggle element group between Full and Compact";
            // 
            // _bufferControlComboBoxLabel
            // 
            _bufferControlComboBoxLabel.Name = "_BufferControlComboBoxLabel";
            _bufferControlComboBoxLabel.Size = new System.Drawing.Size(50, 22);
            _bufferControlComboBoxLabel.Text = "Control:";
            // 
            // _feedControlComboBox
            // 
            _feedControlComboBox.Name = "_FeedControlComboBox";
            _feedControlComboBox.Size = new System.Drawing.Size(121, 25);
            _feedControlComboBox.Text = "Never";
            _feedControlComboBox.ToolTipText = "Selects the feed control";
            // 
            // _bufferConfigurationToolStrip
            // 
            _bufferConfigurationToolStrip.BackColor = System.Drawing.Color.Transparent;
            _bufferConfigurationToolStrip.GripMargin = new Padding(0);
            _bufferConfigurationToolStrip.Items.AddRange(new ToolStripItem[] { _bufferCondigureLabel, _bufferNameLabel, _sizeNumericLabel, _sizeNumeric, _preTriggerCountNumericLabel, _preTriggerCountNumeric, _clearBufferButton, _freeCountLabelLabel, _freeCountLabel });
            _bufferConfigurationToolStrip.Location = new System.Drawing.Point(0, 25);
            _bufferConfigurationToolStrip.Name = "_BufferConfigurationToolStrip";
            _bufferConfigurationToolStrip.Size = new System.Drawing.Size(440, 28);
            _bufferConfigurationToolStrip.Stretch = true;
            _bufferConfigurationToolStrip.TabIndex = 1;
            // 
            // _bufferCondigureLabel
            // 
            _bufferCondigureLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _bufferCondigureLabel.Margin = new Padding(10, 1, 0, 2);
            _bufferCondigureLabel.Name = "_BufferCondigureLabel";
            _bufferCondigureLabel.Size = new System.Drawing.Size(39, 25);
            _bufferCondigureLabel.Text = "Buffer";
            // 
            // _bufferNameLabel
            // 
            _bufferNameLabel.Name = "_BufferNameLabel";
            _bufferNameLabel.Size = new System.Drawing.Size(34, 25);
            _bufferNameLabel.Text = "Trace";
            _bufferNameLabel.ToolTipText = "Buffer name";
            // 
            // _sizeNumericLabel
            // 
            _sizeNumericLabel.Name = "_SizeNumericLabel";
            _sizeNumericLabel.Size = new System.Drawing.Size(30, 25);
            _sizeNumericLabel.Text = "Size:";
            // 
            // _sizeNumeric
            // 
            _sizeNumeric.Name = "_SizeNumeric";
            _sizeNumeric.Size = new System.Drawing.Size(41, 25);
            _sizeNumeric.Text = "3";
            _sizeNumeric.ToolTipText = "Buffer size";
            _sizeNumeric.Value = new decimal(new int[] { 3, 0, 0, 0 });
            // 
            // _preTriggerCountNumericLabel
            // 
            _preTriggerCountNumericLabel.Margin = new Padding(3, 1, 0, 2);
            _preTriggerCountNumericLabel.Name = "_PreTriggerCountNumericLabel";
            _preTriggerCountNumericLabel.Size = new System.Drawing.Size(68, 25);
            _preTriggerCountNumericLabel.Text = "Pre-Trigger:";
            // 
            // _preTriggerCountNumeric
            // 
            _preTriggerCountNumeric.Name = "_PreTriggerCountNumeric";
            _preTriggerCountNumeric.Size = new System.Drawing.Size(41, 25);
            _preTriggerCountNumeric.Text = "0";
            _preTriggerCountNumeric.ToolTipText = "Number of Pre-Trigger  readings";
            _preTriggerCountNumeric.Value = new decimal(new int[] { 0, 0, 0, 0 });
            // 
            // _clearBufferButton
            // 
            _clearBufferButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _clearBufferButton.Image = (System.Drawing.Image)resources.GetObject("_ClearBufferButton.Image");
            _clearBufferButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _clearBufferButton.Name = "_ClearBufferButton";
            _clearBufferButton.Size = new System.Drawing.Size(38, 25);
            _clearBufferButton.Text = "Clear";
            _clearBufferButton.ToolTipText = "Clears the buffer";
            // 
            // _freeCountLabelLabel
            // 
            _freeCountLabelLabel.Name = "_FreeCountLabelLabel";
            _freeCountLabelLabel.Size = new System.Drawing.Size(32, 25);
            _freeCountLabelLabel.Text = "Free:";
            // 
            // _freeCountLabel
            // 
            _freeCountLabel.Name = "_FreeCountLabel";
            _freeCountLabel.Size = new System.Drawing.Size(13, 25);
            _freeCountLabel.Text = "0";
            // 
            // _bufferFormatToolStrip
            // 
            _bufferFormatToolStrip.BackColor = System.Drawing.Color.Transparent;
            _bufferFormatToolStrip.GripMargin = new Padding(0);
            _bufferFormatToolStrip.Items.AddRange(new ToolStripItem[] { _formatLabel, _timestampFormatToggleButton });
            _bufferFormatToolStrip.Location = new System.Drawing.Point(0, 53);
            _bufferFormatToolStrip.Name = "_BufferFormatToolStrip";
            _bufferFormatToolStrip.Size = new System.Drawing.Size(440, 25);
            _bufferFormatToolStrip.Stretch = true;
            _bufferFormatToolStrip.TabIndex = 2;
            // 
            // _formatLabel
            // 
            _formatLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _formatLabel.Margin = new Padding(6, 1, 2, 2);
            _formatLabel.Name = "_FormatLabel";
            _formatLabel.Size = new System.Drawing.Size(44, 22);
            _formatLabel.Text = "Format";
            // 
            // _timestampFormatToggleButton
            // 
            _timestampFormatToggleButton.Checked = true;
            _timestampFormatToggleButton.CheckOnClick = true;
            _timestampFormatToggleButton.CheckState = CheckState.Indeterminate;
            _timestampFormatToggleButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            _timestampFormatToggleButton.Image = (System.Drawing.Image)resources.GetObject("_TimestampFormatToggleButton.Image");
            _timestampFormatToggleButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _timestampFormatToggleButton.Name = "_TimestampFormatToggleButton";
            _timestampFormatToggleButton.Size = new System.Drawing.Size(131, 22);
            _timestampFormatToggleButton.Text = "Timestamp: Absolute ?";
            _timestampFormatToggleButton.ToolTipText = "Toggles timestamp format style";
            // 
            // _infoTextBox
            // 
            _infoTextBox.Dock = DockStyle.Fill;
            _infoTextBox.Location = new System.Drawing.Point(1, 79);
            _infoTextBox.Margin = new Padding(3, 4, 3, 4);
            _infoTextBox.Multiline = true;
            _infoTextBox.Name = "_InfoTextBox";
            _infoTextBox.ReadOnly = true;
            _infoTextBox.ScrollBars = ScrollBars.Both;
            _infoTextBox.Size = new System.Drawing.Size(440, 167);
            _infoTextBox.TabIndex = 0;
            _infoTextBox.Text = "<info>";
            // 
            // TraceBufferView
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(_infoTextBox);
            Controls.Add(_limitToolStripPanel);
            Name = "TraceBufferView";
            Padding = new Padding(1);
            Size = new System.Drawing.Size(442, 247);
            _limitToolStripPanel.ResumeLayout(false);
            _limitToolStripPanel.PerformLayout();
            _subsystemToolStrip.ResumeLayout(false);
            _subsystemToolStrip.PerformLayout();
            _bufferConfigurationToolStrip.ResumeLayout(false);
            _bufferConfigurationToolStrip.PerformLayout();
            _bufferFormatToolStrip.ResumeLayout(false);
            _bufferFormatToolStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private ToolStripPanel _limitToolStripPanel;
        private ToolStrip _bufferFormatToolStrip;
        private ToolStripLabel _formatLabel;
        private ToolStrip _bufferConfigurationToolStrip;
        private ToolStripLabel _bufferCondigureLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _sizeNumeric;
        private ToolStripLabel _preTriggerCountNumericLabel;
        private cc.isr.WinControls.ToolStripNumericUpDown _preTriggerCountNumeric;
        private ToolStrip _subsystemToolStrip;
        private ToolStripLabel _sizeNumericLabel;
        private ToolStripLabel _feedSourceComboLabel;
        private ToolStripComboBox _feedSourceComboBox;
        private ToolStripLabel _bufferNameLabel;
        private ToolStripButton _clearBufferButton;
        private ToolStripLabel _freeCountLabelLabel;
        private ToolStripLabel _freeCountLabel;
        private ToolStripButton _elementGroupToggleButton;
        private ToolStripLabel _bufferControlComboBoxLabel;
        private ToolStripComboBox _feedControlComboBox;
        private ToolStripButton _timestampFormatToggleButton;
        private ToolStripSplitButton _subsystemSplitButton;
        private ToolStripMenuItem _applySettingsMenuItem;
        private ToolStripMenuItem _readSettingsMenuItem;
        private TextBox _infoTextBox;
    }
}
