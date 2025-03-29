using System;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls
{
    public partial class MeterView
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MeterView));
            _splitContainer = new System.Windows.Forms.SplitContainer();
            _navigatorTreeView = new System.Windows.Forms.TreeView();
            _navigatorTreeView.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(NavigatorTreeView_BeforeSelect);
            _navigatorTreeView.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(NavigatorTreeView_afterSelect);
            _tabs = new System.Windows.Forms.TabControl();
            _tabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(Tabs_DrawItem);
            _connectTabPage = new System.Windows.Forms.TabPage();
            _connectTabLayout = new System.Windows.Forms.TableLayoutPanel();
            _connectGroupBox = new System.Windows.Forms.GroupBox();
            _resourceSelectorConnector = new cc.isr.WinControls.SelectorOpener();
            _resourceInfoLabel = new System.Windows.Forms.Label();
            _identityTextBox = new System.Windows.Forms.TextBox();
            _ttmConfigTabPage = new System.Windows.Forms.TabPage();
            _mainLayout = new System.Windows.Forms.TableLayoutPanel();
            _configurationLayout = new System.Windows.Forms.TableLayoutPanel();
            _ttmConfigurationPanel = new ConfigurationView();
            _ttmConfigurationPanel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(TTMConfigurationPanel_PropertyChanged);
            _ttmTabPage = new System.Windows.Forms.TabPage();
            _ttmLayout = new System.Windows.Forms.TableLayoutPanel();
            _measurementPanel = new MeasurementView();
            _shuntTabPage = new System.Windows.Forms.TabPage();
            _shuntLayout = new System.Windows.Forms.TableLayoutPanel();
            _shuntDisplayLayout = new System.Windows.Forms.TableLayoutPanel();
            _shuntResistanceTextBox = new System.Windows.Forms.TextBox();
            _shuntResistanceTextBoxLabel = new System.Windows.Forms.Label();
            _shuntGroupBoxLayout = new System.Windows.Forms.TableLayoutPanel();
            _measureShuntResistanceButton = new System.Windows.Forms.Button();
            _measureShuntResistanceButton.Click += new EventHandler(MeasureShuntResistanceButton_Click);
            _shuntConfigureGroupBoxLayout = new System.Windows.Forms.TableLayoutPanel();
            _shuntResistanceConfigurationGroupBox = new System.Windows.Forms.GroupBox();
            _restoreShuntResistanceDefaultsButton = new System.Windows.Forms.Button();
            _restoreShuntResistanceDefaultsButton.Click += new EventHandler(RestoreShuntResistanceDefaultsButton_Click);
            _applyNewShuntResistanceConfigurationButton = new System.Windows.Forms.Button();
            _applyNewShuntResistanceConfigurationButton.Click += new EventHandler(ApplyNewShuntResistanceConfigurationButton_Click);
            _applyShuntResistanceConfigurationButton = new System.Windows.Forms.Button();
            _applyShuntResistanceConfigurationButton.Click += new EventHandler(ApplyShuntResistanceConfigurationButton_Click);
            _shuntResistanceCurrentRangeNumeric = new System.Windows.Forms.NumericUpDown();
            _shuntResistanceCurrentRangeNumericLabel = new System.Windows.Forms.Label();
            _shuntResistanceLowLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _shuntResistanceLowLimitNumericLabel = new System.Windows.Forms.Label();
            _shuntResistanceHighLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _shuntResistanceHighLimitNumericLabel = new System.Windows.Forms.Label();
            _shuntResistanceVoltageLimitNumeric = new System.Windows.Forms.NumericUpDown();
            _shuntResistanceVoltageLimitNumericLabel = new System.Windows.Forms.Label();
            _shuntResistanceCurrentLevelNumeric = new System.Windows.Forms.NumericUpDown();
            _shuntResistanceCurrentLevelNumericLabel = new System.Windows.Forms.Label();
            _partsTabPage = new System.Windows.Forms.TabPage();
            _partsLayout = new System.Windows.Forms.TableLayoutPanel();
            _partsPanel = new PartsView();
            _partsPanel.PropertyChanged += new System.ComponentModel.PropertyChangedEventHandler(PartsPanel_PropertyChanged);
            _messagesTabPage = new System.Windows.Forms.TabPage();
            _traceMessagesBox = new cc.isr.Logging.TraceLog.WinForms.MessagesBox();
            _partHeader = new PartHeader();
            _thermalTransientHeader = new ThermalTransientHeader();
            _statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            _statusStrip = new System.Windows.Forms.StatusStrip();
            _measurementsHeader = new MeasurementsHeader();
            _meterTimer = new System.Windows.Forms.Timer(components);
            ((System.ComponentModel.ISupportInitialize)_splitContainer).BeginInit();
            _splitContainer.Panel1.SuspendLayout();
            _splitContainer.SuspendLayout();
            _tabs.SuspendLayout();
            _connectTabPage.SuspendLayout();
            _connectTabLayout.SuspendLayout();
            _connectGroupBox.SuspendLayout();
            _ttmConfigTabPage.SuspendLayout();
            _mainLayout.SuspendLayout();
            _configurationLayout.SuspendLayout();
            _ttmTabPage.SuspendLayout();
            _ttmLayout.SuspendLayout();
            _shuntTabPage.SuspendLayout();
            _shuntLayout.SuspendLayout();
            _shuntDisplayLayout.SuspendLayout();
            _shuntGroupBoxLayout.SuspendLayout();
            _shuntConfigureGroupBoxLayout.SuspendLayout();
            _shuntResistanceConfigurationGroupBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceCurrentRangeNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceLowLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceHighLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceVoltageLimitNumeric).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceCurrentLevelNumeric).BeginInit();
            _partsTabPage.SuspendLayout();
            _partsLayout.SuspendLayout();
            _messagesTabPage.SuspendLayout();
            _statusStrip.SuspendLayout();
            SuspendLayout();
            //
            // _splitContainer
            //
            _splitContainer.Location = new System.Drawing.Point(12, 413);
            _splitContainer.Name = "_SplitContainer";

            //
            // _splitContainer.Panel1
            //
            _splitContainer.Panel1.Controls.Add(_navigatorTreeView);
            _splitContainer.Size = new System.Drawing.Size(264, 231);
            _splitContainer.SplitterDistance = 133;
            _splitContainer.TabIndex = 12;
            //
            // _navigatorTreeView
            //
            _navigatorTreeView.Dock = System.Windows.Forms.DockStyle.Fill;
            _navigatorTreeView.Enabled = false;
            _navigatorTreeView.Location = new System.Drawing.Point(0, 0);
            _navigatorTreeView.Name = "_NavigatorTreeView";
            _navigatorTreeView.Size = new System.Drawing.Size(133, 231);
            _navigatorTreeView.TabIndex = 3;
            //
            // _tabs
            //
            _tabs.Controls.Add(_connectTabPage);
            _tabs.Controls.Add(_ttmConfigTabPage);
            _tabs.Controls.Add(_ttmTabPage);
            _tabs.Controls.Add(_shuntTabPage);
            _tabs.Controls.Add(_partsTabPage);
            _tabs.Controls.Add(_messagesTabPage);
            _tabs.Location = new System.Drawing.Point(56, 205);
            _tabs.Multiline = true;
            _tabs.Name = "_Tabs";
            _tabs.Padding = new System.Drawing.Point(12, 3);
            _tabs.SelectedIndex = 0;
            _tabs.Size = new System.Drawing.Size(676, 537);
            _tabs.TabIndex = 8;
            //
            // _connectTabPage
            //
            _connectTabPage.Controls.Add(_connectTabLayout);
            _connectTabPage.Location = new System.Drawing.Point(4, 26);
            _connectTabPage.Name = "_ConnectTabPage";
            _connectTabPage.Size = new System.Drawing.Size(668, 507);
            _connectTabPage.TabIndex = 2;
            _connectTabPage.Text = "CONNECT";
            _connectTabPage.UseVisualStyleBackColor = true;
            //
            // _connectTabLayout
            //
            _connectTabLayout.ColumnCount = 3;
            _connectTabLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _connectTabLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _connectTabLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _connectTabLayout.Controls.Add(_connectGroupBox, 1, 1);
            _connectTabLayout.Location = new System.Drawing.Point(12, 12);
            _connectTabLayout.Name = "_ConnectTabLayout";
            _connectTabLayout.RowCount = 3;
            _connectTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _connectTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _connectTabLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _connectTabLayout.Size = new System.Drawing.Size(644, 403);
            _connectTabLayout.TabIndex = 0;
            //
            // _connectGroupBox
            //
            _connectGroupBox.Controls.Add(_resourceSelectorConnector);
            _connectGroupBox.Controls.Add(_resourceInfoLabel);
            _connectGroupBox.Controls.Add(_identityTextBox);
            _connectGroupBox.Location = new System.Drawing.Point(5, 100);
            _connectGroupBox.Name = "_ConnectGroupBox";
            _connectGroupBox.Size = new System.Drawing.Size(634, 203);
            _connectGroupBox.TabIndex = 2;
            _connectGroupBox.TabStop = false;
            _connectGroupBox.Text = "CONNECT";
            //
            // _resourceSelectorConnector
            //
            _resourceSelectorConnector.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _resourceSelectorConnector.BackColor = System.Drawing.Color.Transparent;
            _resourceSelectorConnector.Clearable = true;
            _resourceSelectorConnector.Openable = true;
            _resourceSelectorConnector.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _resourceSelectorConnector.Location = new System.Drawing.Point(28, 21);
            _resourceSelectorConnector.Margin = new System.Windows.Forms.Padding(0);
            _resourceSelectorConnector.Name = "_ResourceSelectorConnector";
            _resourceSelectorConnector.Searchable = true;
            _resourceSelectorConnector.Size = new System.Drawing.Size(578, 29);
            _resourceSelectorConnector.TabIndex = 5;
            ToolTip.SetToolTip(_resourceSelectorConnector, "Find resources and connect");
            //
            // _resourceInfoLabel
            //
            _resourceInfoLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _resourceInfoLabel.Location = new System.Drawing.Point(28, 89);
            _resourceInfoLabel.Name = "_ResourceInfoLabel";
            _resourceInfoLabel.Size = new System.Drawing.Size(578, 102);
            _resourceInfoLabel.TabIndex = 4;
            _resourceInfoLabel.Text = resources.GetString("_ResourceInfoLabel.Text");
            //
            // _identityTextBox
            //
            _identityTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _identityTextBox.Location = new System.Drawing.Point(28, 61);
            _identityTextBox.Name = "_IdentityTextBox";
            _identityTextBox.ReadOnly = true;
            _identityTextBox.Size = new System.Drawing.Size(578, 25);
            _identityTextBox.TabIndex = 3;
            ToolTip.SetToolTip(_identityTextBox, "Displays the meter identity information");
            //
            // _ttmConfigTabPage
            //
            _ttmConfigTabPage.Controls.Add(_mainLayout);
            _ttmConfigTabPage.Location = new System.Drawing.Point(4, 26);
            _ttmConfigTabPage.Name = "_TtmConfigTabPage";
            _ttmConfigTabPage.Size = new System.Drawing.Size(668, 507);
            _ttmConfigTabPage.TabIndex = 0;
            _ttmConfigTabPage.Text = "TTM CONFIG.";
            _ttmConfigTabPage.UseVisualStyleBackColor = true;
            //
            // _mainLayout
            //
            _mainLayout.ColumnCount = 3;
            _mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _mainLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _mainLayout.Controls.Add(_configurationLayout, 1, 1);
            _mainLayout.Location = new System.Drawing.Point(13, 12);
            _mainLayout.Name = "_MainLayout";
            _mainLayout.RowCount = 3;
            _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _mainLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _mainLayout.Size = new System.Drawing.Size(655, 453);
            _mainLayout.TabIndex = 0;
            //
            // _configurationLayout
            //
            _configurationLayout.ColumnCount = 3;
            _configurationLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _configurationLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _configurationLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _configurationLayout.Controls.Add(_ttmConfigurationPanel, 1, 0);
            _configurationLayout.Location = new System.Drawing.Point(-13, 55);
            _configurationLayout.Name = "_ConfigurationLayout";
            _configurationLayout.RowCount = 1;
            _configurationLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0f));
            _configurationLayout.Size = new System.Drawing.Size(681, 343);
            _configurationLayout.TabIndex = 2;
            //
            // _ttmConfigurationPanel
            //
            _ttmConfigurationPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _ttmConfigurationPanel.BackColor = System.Drawing.Color.Transparent;
            _ttmConfigurationPanel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _ttmConfigurationPanel.IsNewConfigurationSettingAvailable = false;
            _ttmConfigurationPanel.Location = new System.Drawing.Point(68, 4);
            _ttmConfigurationPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            _ttmConfigurationPanel.Name = "_TTMConfigurationPanel";
            _ttmConfigurationPanel.Size = new System.Drawing.Size(545, 335);
            _ttmConfigurationPanel.TabIndex = 0;
            //
            // _ttmTabPage
            //
            _ttmTabPage.Controls.Add(_ttmLayout);
            _ttmTabPage.Location = new System.Drawing.Point(4, 26);
            _ttmTabPage.Name = "_TtmTabPage";
            _ttmTabPage.Size = new System.Drawing.Size(668, 507);
            _ttmTabPage.TabIndex = 4;
            _ttmTabPage.Text = "TTM";
            _ttmTabPage.UseVisualStyleBackColor = true;
            //
            // _ttmLayout
            //
            _ttmLayout.ColumnCount = 3;
            _ttmLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _ttmLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0f));
            _ttmLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _ttmLayout.Controls.Add(_measurementPanel, 1, 1);
            _ttmLayout.Location = new System.Drawing.Point(16, 16);
            _ttmLayout.Name = "_TtmLayout";
            _ttmLayout.RowCount = 3;
            _ttmLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _ttmLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0f));
            _ttmLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _ttmLayout.Size = new System.Drawing.Size(589, 413);
            _ttmLayout.TabIndex = 0;
            //
            // _measurementPanel
            //
            _measurementPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _measurementPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _measurementPanel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _measurementPanel.Location = new System.Drawing.Point(23, 23);
            _measurementPanel.Name = "_MeasurementPanel";
            _measurementPanel.Size = new System.Drawing.Size(543, 367);
            _measurementPanel.TabIndex = 0;
            //
            // _shuntTabPage
            //
            _shuntTabPage.Controls.Add(_shuntLayout);
            _shuntTabPage.Location = new System.Drawing.Point(4, 26);
            _shuntTabPage.Name = "_ShuntTabPage";
            _shuntTabPage.Size = new System.Drawing.Size(668, 507);
            _shuntTabPage.TabIndex = 3;
            _shuntTabPage.Text = "SHUNT";
            _shuntTabPage.UseVisualStyleBackColor = true;
            //
            // _shuntLayout
            //
            _shuntLayout.ColumnCount = 3;
            _shuntLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _shuntLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntLayout.Controls.Add(_shuntDisplayLayout, 1, 1);
            _shuntLayout.Controls.Add(_shuntGroupBoxLayout, 1, 3);
            _shuntLayout.Controls.Add(_shuntConfigureGroupBoxLayout, 1, 2);
            _shuntLayout.Location = new System.Drawing.Point(5, 5);
            _shuntLayout.Name = "_ShuntLayout";
            _shuntLayout.RowCount = 5;
            _shuntLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntLayout.Size = new System.Drawing.Size(466, 492);
            _shuntLayout.TabIndex = 0;
            //
            // _shuntDisplayLayout
            //
            _shuntDisplayLayout.BackColor = System.Drawing.Color.Black;
            _shuntDisplayLayout.ColumnCount = 3;
            _shuntDisplayLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65.0f));
            _shuntDisplayLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _shuntDisplayLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 65.0f));
            _shuntDisplayLayout.Controls.Add(_shuntResistanceTextBox, 1, 2);
            _shuntDisplayLayout.Controls.Add(_shuntResistanceTextBoxLabel, 1, 1);
            _shuntDisplayLayout.Dock = System.Windows.Forms.DockStyle.Left;
            _shuntDisplayLayout.Location = new System.Drawing.Point(64, 11);
            _shuntDisplayLayout.Name = "_ShuntDisplayLayout";
            _shuntDisplayLayout.RowCount = 4;
            _shuntDisplayLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0f));
            _shuntDisplayLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntDisplayLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntDisplayLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 5.0f));
            _shuntDisplayLayout.Size = new System.Drawing.Size(337, 75);
            _shuntDisplayLayout.TabIndex = 0;
            //
            // _shuntResistanceTextBox
            //
            _shuntResistanceTextBox.BackColor = System.Drawing.Color.Black;
            _shuntResistanceTextBox.Dock = System.Windows.Forms.DockStyle.Top;
            _shuntResistanceTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _shuntResistanceTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _shuntResistanceTextBox.Location = new System.Drawing.Point(68, 25);
            _shuntResistanceTextBox.Name = "_ShuntResistanceTextBox";
            _shuntResistanceTextBox.ReadOnly = true;
            _shuntResistanceTextBox.Size = new System.Drawing.Size(201, 39);
            _shuntResistanceTextBox.TabIndex = 7;
            _shuntResistanceTextBox.Text = "0.000";
            _shuntResistanceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_shuntResistanceTextBox, "Measured Shunt MeasuredValue");
            //
            // _shuntResistanceTextBoxLabel
            //
            _shuntResistanceTextBoxLabel.AutoSize = true;
            _shuntResistanceTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _shuntResistanceTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _shuntResistanceTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _shuntResistanceTextBoxLabel.Location = new System.Drawing.Point(68, 5);
            _shuntResistanceTextBoxLabel.Name = "_ShuntResistanceTextBoxLabel";
            _shuntResistanceTextBoxLabel.Size = new System.Drawing.Size(201, 17);
            _shuntResistanceTextBoxLabel.TabIndex = 6;
            _shuntResistanceTextBoxLabel.Text = "SHUNT RESISTANCE [Ω]";
            _shuntResistanceTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //
            // _shuntGroupBoxLayout
            //
            _shuntGroupBoxLayout.ColumnCount = 3;
            _shuntGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _shuntGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntGroupBoxLayout.Controls.Add(_measureShuntResistanceButton, 1, 1);
            _shuntGroupBoxLayout.Location = new System.Drawing.Point(64, 417);
            _shuntGroupBoxLayout.Name = "_ShuntGroupBoxLayout";
            _shuntGroupBoxLayout.RowCount = 3;
            _shuntGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0f));
            _shuntGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 10.0f));
            _shuntGroupBoxLayout.Size = new System.Drawing.Size(337, 63);
            _shuntGroupBoxLayout.TabIndex = 2;
            //
            // _measureShuntResistanceButton
            //
            _measureShuntResistanceButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _measureShuntResistanceButton.Location = new System.Drawing.Point(53, 13);
            _measureShuntResistanceButton.Name = "_MeasureShuntResistanceButton";
            _measureShuntResistanceButton.Size = new System.Drawing.Size(230, 36);
            _measureShuntResistanceButton.TabIndex = 3;
            _measureShuntResistanceButton.Text = "READ SHUNT RESISTANCE";
            _measureShuntResistanceButton.UseVisualStyleBackColor = true;
            //
            // _shuntConfigureGroupBoxLayout
            //
            _shuntConfigureGroupBoxLayout.ColumnCount = 3;
            _shuntConfigureGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntConfigureGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _shuntConfigureGroupBoxLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntConfigureGroupBoxLayout.Controls.Add(_shuntResistanceConfigurationGroupBox, 1, 1);
            _shuntConfigureGroupBoxLayout.Location = new System.Drawing.Point(64, 92);
            _shuntConfigureGroupBoxLayout.Name = "_ShuntConfigureGroupBoxLayout";
            _shuntConfigureGroupBoxLayout.RowCount = 3;
            _shuntConfigureGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntConfigureGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _shuntConfigureGroupBoxLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _shuntConfigureGroupBoxLayout.Size = new System.Drawing.Size(337, 319);
            _shuntConfigureGroupBoxLayout.TabIndex = 3;
            //
            // _shuntResistanceConfigurationGroupBox
            //
            _shuntResistanceConfigurationGroupBox.Controls.Add(_restoreShuntResistanceDefaultsButton);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_applyNewShuntResistanceConfigurationButton);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_applyShuntResistanceConfigurationButton);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceCurrentRangeNumeric);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceCurrentRangeNumericLabel);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceLowLimitNumeric);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceLowLimitNumericLabel);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceHighLimitNumeric);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceHighLimitNumericLabel);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceVoltageLimitNumeric);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceVoltageLimitNumericLabel);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceCurrentLevelNumeric);
            _shuntResistanceConfigurationGroupBox.Controls.Add(_shuntResistanceCurrentLevelNumericLabel);
            _shuntResistanceConfigurationGroupBox.Dock = System.Windows.Forms.DockStyle.Top;
            _shuntResistanceConfigurationGroupBox.Location = new System.Drawing.Point(39, 12);
            _shuntResistanceConfigurationGroupBox.Name = "_ShuntResistanceConfigurationGroupBox";
            _shuntResistanceConfigurationGroupBox.Size = new System.Drawing.Size(258, 294);
            _shuntResistanceConfigurationGroupBox.TabIndex = 2;
            _shuntResistanceConfigurationGroupBox.TabStop = false;
            _shuntResistanceConfigurationGroupBox.Text = "SHUNT RESISTANCE CONFIG.";
            //
            // _restoreShuntResistanceDefaultsButton
            //
            _restoreShuntResistanceDefaultsButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _restoreShuntResistanceDefaultsButton.Location = new System.Drawing.Point(14, 252);
            _restoreShuntResistanceDefaultsButton.Name = "_RestoreShuntResistanceDefaultsButton";
            _restoreShuntResistanceDefaultsButton.Size = new System.Drawing.Size(234, 30);
            _restoreShuntResistanceDefaultsButton.TabIndex = 12;
            _restoreShuntResistanceDefaultsButton.Text = "RESTORE DEFAULTS";
            _restoreShuntResistanceDefaultsButton.UseVisualStyleBackColor = true;
            //
            // _applyNewShuntResistanceConfigurationButton
            //
            _applyNewShuntResistanceConfigurationButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _applyNewShuntResistanceConfigurationButton.Location = new System.Drawing.Point(133, 210);
            _applyNewShuntResistanceConfigurationButton.Name = "_applyNewShuntResistanceConfigurationButton";
            _applyNewShuntResistanceConfigurationButton.Size = new System.Drawing.Size(115, 30);
            _applyNewShuntResistanceConfigurationButton.TabIndex = 11;
            _applyNewShuntResistanceConfigurationButton.Text = "APPLY CHANGES";
            _applyNewShuntResistanceConfigurationButton.UseVisualStyleBackColor = true;
            //
            // _applyShuntResistanceConfigurationButton
            //
            _applyShuntResistanceConfigurationButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _applyShuntResistanceConfigurationButton.Location = new System.Drawing.Point(11, 209);
            _applyShuntResistanceConfigurationButton.Name = "_applyShuntResistanceConfigurationButton";
            _applyShuntResistanceConfigurationButton.Size = new System.Drawing.Size(115, 30);
            _applyShuntResistanceConfigurationButton.TabIndex = 10;
            _applyShuntResistanceConfigurationButton.Text = "APPLY ALL";
            ToolTip.SetToolTip(_applyShuntResistanceConfigurationButton, "Clears last measurement and applies shunt resistance configuration");
            _applyShuntResistanceConfigurationButton.UseVisualStyleBackColor = true;
            //
            // _shuntResistanceCurrentRangeNumeric
            //
            _shuntResistanceCurrentRangeNumeric.DecimalPlaces = 3;
            _shuntResistanceCurrentRangeNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _shuntResistanceCurrentRangeNumeric.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            _shuntResistanceCurrentRangeNumeric.Location = new System.Drawing.Point(161, 62);
            _shuntResistanceCurrentRangeNumeric.Maximum = new decimal(new int[] { 1, 0, 0, 65536 });
            _shuntResistanceCurrentRangeNumeric.Name = "_ShuntResistanceCurrentRangeNumeric";
            _shuntResistanceCurrentRangeNumeric.Size = new System.Drawing.Size(75, 25);
            _shuntResistanceCurrentRangeNumeric.TabIndex = 3;
            _shuntResistanceCurrentRangeNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_shuntResistanceCurrentRangeNumeric, "Current range in Amperes");
            _shuntResistanceCurrentRangeNumeric.Value = new decimal(new int[] { 1, 0, 0, 131072 });
            //
            // _shuntResistanceCurrentRangeNumericLabel
            //
            _shuntResistanceCurrentRangeNumericLabel.AutoSize = true;
            _shuntResistanceCurrentRangeNumericLabel.Location = new System.Drawing.Point(25, 66);
            _shuntResistanceCurrentRangeNumericLabel.Name = "_ShuntResistanceCurrentRangeNumericLabel";
            _shuntResistanceCurrentRangeNumericLabel.Size = new System.Drawing.Size(134, 17);
            _shuntResistanceCurrentRangeNumericLabel.TabIndex = 2;
            _shuntResistanceCurrentRangeNumericLabel.Text = "CURRENT RANGE [A]:";
            _shuntResistanceCurrentRangeNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // _shuntResistanceLowLimitNumeric
            //
            _shuntResistanceLowLimitNumeric.DecimalPlaces = 1;
            _shuntResistanceLowLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _shuntResistanceLowLimitNumeric.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            _shuntResistanceLowLimitNumeric.Location = new System.Drawing.Point(161, 172);
            _shuntResistanceLowLimitNumeric.Maximum = new decimal(new int[] { 2000, 0, 0, 0 });
            _shuntResistanceLowLimitNumeric.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            _shuntResistanceLowLimitNumeric.Name = "_ShuntResistanceLowLimitNumeric";
            _shuntResistanceLowLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _shuntResistanceLowLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _shuntResistanceLowLimitNumeric.TabIndex = 9;
            _shuntResistanceLowLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_shuntResistanceLowLimitNumeric, "Low limit of passed shunt resistance");
            _shuntResistanceLowLimitNumeric.Value = new decimal(new int[] { 1150, 0, 0, 0 });
            //
            // _shuntResistanceLowLimitNumericLabel
            //
            _shuntResistanceLowLimitNumericLabel.AutoSize = true;
            _shuntResistanceLowLimitNumericLabel.Location = new System.Drawing.Point(63, 176);
            _shuntResistanceLowLimitNumericLabel.Name = "_ShuntResistanceLowLimitNumericLabel";
            _shuntResistanceLowLimitNumericLabel.Size = new System.Drawing.Size(96, 17);
            _shuntResistanceLowLimitNumericLabel.TabIndex = 8;
            _shuntResistanceLowLimitNumericLabel.Text = "LO&W LIMIT [Ω]:";
            _shuntResistanceLowLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // _shuntResistanceHighLimitNumeric
            //
            _shuntResistanceHighLimitNumeric.DecimalPlaces = 1;
            _shuntResistanceHighLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _shuntResistanceHighLimitNumeric.Increment = new decimal(new int[] { 10, 0, 0, 0 });
            _shuntResistanceHighLimitNumeric.Location = new System.Drawing.Point(161, 135);
            _shuntResistanceHighLimitNumeric.Maximum = new decimal(new int[] { 3000, 0, 0, 0 });
            _shuntResistanceHighLimitNumeric.Minimum = new decimal(new int[] { 10, 0, 0, 0 });
            _shuntResistanceHighLimitNumeric.Name = "_ShuntResistanceHighLimitNumeric";
            _shuntResistanceHighLimitNumeric.RightToLeft = System.Windows.Forms.RightToLeft.No;
            _shuntResistanceHighLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _shuntResistanceHighLimitNumeric.TabIndex = 7;
            _shuntResistanceHighLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_shuntResistanceHighLimitNumeric, "High limit of passed shunt resistance.");
            _shuntResistanceHighLimitNumeric.Value = new decimal(new int[] { 1250, 0, 0, 0 });
            //
            // _shuntResistanceHighLimitNumericLabel
            //
            _shuntResistanceHighLimitNumericLabel.AutoSize = true;
            _shuntResistanceHighLimitNumericLabel.Location = new System.Drawing.Point(61, 139);
            _shuntResistanceHighLimitNumericLabel.Name = "_ShuntResistanceHighLimitNumericLabel";
            _shuntResistanceHighLimitNumericLabel.Size = new System.Drawing.Size(98, 17);
            _shuntResistanceHighLimitNumericLabel.TabIndex = 6;
            _shuntResistanceHighLimitNumericLabel.Text = "&HIGH LIMIT [Ω]:";
            _shuntResistanceHighLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // _shuntResistanceVoltageLimitNumeric
            //
            _shuntResistanceVoltageLimitNumeric.DecimalPlaces = 2;
            _shuntResistanceVoltageLimitNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _shuntResistanceVoltageLimitNumeric.Increment = new decimal(new int[] { 1, 0, 0, 65536 });
            _shuntResistanceVoltageLimitNumeric.Location = new System.Drawing.Point(161, 98);
            _shuntResistanceVoltageLimitNumeric.Maximum = new decimal(new int[] { 40, 0, 0, 0 });
            _shuntResistanceVoltageLimitNumeric.Name = "_ShuntResistanceVoltageLimitNumeric";
            _shuntResistanceVoltageLimitNumeric.Size = new System.Drawing.Size(75, 25);
            _shuntResistanceVoltageLimitNumeric.TabIndex = 5;
            _shuntResistanceVoltageLimitNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_shuntResistanceVoltageLimitNumeric, "Voltage Limit in Volts");
            _shuntResistanceVoltageLimitNumeric.Value = new decimal(new int[] { 10, 0, 0, 0 });
            //
            // _shuntResistanceVoltageLimitNumericLabel
            //
            _shuntResistanceVoltageLimitNumericLabel.AutoSize = true;
            _shuntResistanceVoltageLimitNumericLabel.Location = new System.Drawing.Point(38, 102);
            _shuntResistanceVoltageLimitNumericLabel.Name = "_ShuntResistanceVoltageLimitNumericLabel";
            _shuntResistanceVoltageLimitNumericLabel.Size = new System.Drawing.Size(119, 17);
            _shuntResistanceVoltageLimitNumericLabel.TabIndex = 4;
            _shuntResistanceVoltageLimitNumericLabel.Text = "&VOLTAGE LIMIT [V]:";
            _shuntResistanceVoltageLimitNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // _shuntResistanceCurrentLevelNumeric
            //
            _shuntResistanceCurrentLevelNumeric.DecimalPlaces = 4;
            _shuntResistanceCurrentLevelNumeric.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _shuntResistanceCurrentLevelNumeric.Increment = new decimal(new int[] { 1, 0, 0, 196608 });
            _shuntResistanceCurrentLevelNumeric.Location = new System.Drawing.Point(161, 27);
            _shuntResistanceCurrentLevelNumeric.Maximum = new decimal(new int[] { 100, 0, 0, 262144 });
            _shuntResistanceCurrentLevelNumeric.Name = "_ShuntResistanceCurrentLevelNumeric";
            _shuntResistanceCurrentLevelNumeric.Size = new System.Drawing.Size(75, 25);
            _shuntResistanceCurrentLevelNumeric.TabIndex = 1;
            _shuntResistanceCurrentLevelNumeric.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_shuntResistanceCurrentLevelNumeric, "Current Level in Amperes");
            _shuntResistanceCurrentLevelNumeric.Value = new decimal(new int[] { 1, 0, 0, 196608 });
            //
            // _shuntResistanceCurrentLevelNumericLabel
            //
            _shuntResistanceCurrentLevelNumericLabel.AutoSize = true;
            _shuntResistanceCurrentLevelNumericLabel.Location = new System.Drawing.Point(33, 31);
            _shuntResistanceCurrentLevelNumericLabel.Name = "_ShuntResistanceCurrentLevelNumericLabel";
            _shuntResistanceCurrentLevelNumericLabel.Size = new System.Drawing.Size(126, 17);
            _shuntResistanceCurrentLevelNumericLabel.TabIndex = 0;
            _shuntResistanceCurrentLevelNumericLabel.Text = "C&URRENT LEVEL [A]:";
            _shuntResistanceCurrentLevelNumericLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            //
            // _partsTabPage
            //
            _partsTabPage.Controls.Add(_partsLayout);
            _partsTabPage.Location = new System.Drawing.Point(4, 26);
            _partsTabPage.Name = "_PartsTabPage";
            _partsTabPage.Size = new System.Drawing.Size(668, 507);
            _partsTabPage.TabIndex = 5;
            _partsTabPage.Text = "PARTS";
            _partsTabPage.UseVisualStyleBackColor = true;
            //
            // _partsLayout
            //
            _partsLayout.ColumnCount = 3;
            _partsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _partsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0f));
            _partsLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _partsLayout.Controls.Add(_partsPanel, 1, 1);
            _partsLayout.Location = new System.Drawing.Point(14, 7);
            _partsLayout.Name = "_PartsLayout";
            _partsLayout.RowCount = 3;
            _partsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _partsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0f));
            _partsLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _partsLayout.Size = new System.Drawing.Size(573, 489);
            _partsLayout.TabIndex = 0;
            //
            // _partsPanel
            //
            _partsPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _partsPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            _partsPanel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _partsPanel.Location = new System.Drawing.Point(23, 24);
            _partsPanel.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            _partsPanel.Name = "_PartsPanel";
            _partsPanel.Size = new System.Drawing.Size(527, 441);
            _partsPanel.TabIndex = 0;
            //
            // _messagesTabPage
            //
            _messagesTabPage.Controls.Add(_traceMessagesBox);
            _messagesTabPage.Location = new System.Drawing.Point(4, 26);
            _messagesTabPage.Name = "_MessagesTabPage";
            _messagesTabPage.Size = new System.Drawing.Size(668, 507);
            _messagesTabPage.TabIndex = 1;
            _messagesTabPage.Text = "Log";
            _messagesTabPage.UseVisualStyleBackColor = true;
            //
            // _traceMessagesBox
            //
            _traceMessagesBox.BackColor = System.Drawing.SystemColors.Info;
            _traceMessagesBox.CausesValidation = false;
            _traceMessagesBox.Font = new System.Drawing.Font("Consolas", 8.0f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _traceMessagesBox.Location = new System.Drawing.Point(0, 0);
            _traceMessagesBox.Multiline = true;
            _traceMessagesBox.Name = "_TraceMessagesBox";
            _traceMessagesBox.ReadOnly = true;
            _traceMessagesBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            _traceMessagesBox.Size = new System.Drawing.Size(601, 471);
            _traceMessagesBox.TabIndex = 0;
            //
            // _partHeader
            //
            _partHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _partHeader.BackColor = System.Drawing.SystemColors.Desktop;
            _partHeader.Dock = System.Windows.Forms.DockStyle.Top;
            _partHeader.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _partHeader.Location = new System.Drawing.Point(0, 119);
            _partHeader.Margin = new System.Windows.Forms.Padding(0);
            _partHeader.Name = "_PartHeader";
            _partHeader.Size = new System.Drawing.Size(804, 26);
            _partHeader.TabIndex = 11;
            //
            // _thermalTransientHeader
            //
            _thermalTransientHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _thermalTransientHeader.BackColor = System.Drawing.Color.Black;
            _thermalTransientHeader.Dock = System.Windows.Forms.DockStyle.Top;
            _thermalTransientHeader.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _thermalTransientHeader.Location = new System.Drawing.Point(0, 55);
            _thermalTransientHeader.Name = "_ThermalTransientHeader";
            _thermalTransientHeader.Size = new System.Drawing.Size(804, 64);
            _thermalTransientHeader.TabIndex = 10;
            //
            // _statusLabel
            //
            _statusLabel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold);
            _statusLabel.Name = "_StatusLabel";
            _statusLabel.Overflow = System.Windows.Forms.ToolStripItemOverflow.Never;
            _statusLabel.Size = new System.Drawing.Size(787, 17);
            _statusLabel.Spring = true;
            _statusLabel.Text = "Loading...";
            _statusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            //
            // _statusStrip
            //
            _statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _statusLabel });
            _statusStrip.Location = new System.Drawing.Point(0, 790);
            _statusStrip.Name = "_StatusStrip";
            _statusStrip.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            _statusStrip.Size = new System.Drawing.Size(804, 22);
            _statusStrip.TabIndex = 7;
            _statusStrip.Text = "StatusStrip1";
            //
            // _measurementsHeader
            //
            _measurementsHeader.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            _measurementsHeader.BackColor = System.Drawing.Color.Black;
            _measurementsHeader.Dock = System.Windows.Forms.DockStyle.Top;
            _measurementsHeader.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _measurementsHeader.Location = new System.Drawing.Point(0, 0);
            _measurementsHeader.Name = "_MeasurementsHeader";
            _measurementsHeader.Size = new System.Drawing.Size(804, 55);
            _measurementsHeader.TabIndex = 9;
            //
            // MeterView
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_splitContainer);
            Controls.Add(_tabs);
            Controls.Add(_partHeader);
            Controls.Add(_thermalTransientHeader);
            Controls.Add(_statusStrip);
            Controls.Add(_measurementsHeader);
            Name = "MeterView";
            Size = new System.Drawing.Size(804, 812);
            ToolTip.IsBalloon = true;
            _splitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_splitContainer).EndInit();
            _splitContainer.ResumeLayout(false);
            _tabs.ResumeLayout(false);
            _connectTabPage.ResumeLayout(false);
            _connectTabLayout.ResumeLayout(false);
            _connectGroupBox.ResumeLayout(false);
            _connectGroupBox.PerformLayout();
            _ttmConfigTabPage.ResumeLayout(false);
            _mainLayout.ResumeLayout(false);
            _configurationLayout.ResumeLayout(false);
            _ttmTabPage.ResumeLayout(false);
            _ttmLayout.ResumeLayout(false);
            _shuntTabPage.ResumeLayout(false);
            _shuntLayout.ResumeLayout(false);
            _shuntDisplayLayout.ResumeLayout(false);
            _shuntDisplayLayout.PerformLayout();
            _shuntGroupBoxLayout.ResumeLayout(false);
            _shuntConfigureGroupBoxLayout.ResumeLayout(false);
            _shuntResistanceConfigurationGroupBox.ResumeLayout(false);
            _shuntResistanceConfigurationGroupBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceCurrentRangeNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceLowLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceHighLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceVoltageLimitNumeric).EndInit();
            ((System.ComponentModel.ISupportInitialize)_shuntResistanceCurrentLevelNumeric).EndInit();
            _partsTabPage.ResumeLayout(false);
            _partsLayout.ResumeLayout(false);
            _messagesTabPage.ResumeLayout(false);
            _messagesTabPage.PerformLayout();
            _statusStrip.ResumeLayout(false);
            _statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.SplitContainer _splitContainer;
        private System.Windows.Forms.TreeView _navigatorTreeView;
        private System.Windows.Forms.TabControl _tabs;
        private System.Windows.Forms.TabPage _connectTabPage;
        private System.Windows.Forms.TableLayoutPanel _connectTabLayout;
        private System.Windows.Forms.GroupBox _connectGroupBox;
        private cc.isr.WinControls.SelectorOpener _resourceSelectorConnector;
        private System.Windows.Forms.Label _resourceInfoLabel;
        private System.Windows.Forms.TextBox _identityTextBox;
        private System.Windows.Forms.TabPage _ttmConfigTabPage;
        private System.Windows.Forms.TableLayoutPanel _mainLayout;
        private System.Windows.Forms.TableLayoutPanel _configurationLayout;
        private ConfigurationView _ttmConfigurationPanel;
        private System.Windows.Forms.TabPage _ttmTabPage;
        private System.Windows.Forms.TableLayoutPanel _ttmLayout;
        private MeasurementView _measurementPanel;
        private System.Windows.Forms.TabPage _shuntTabPage;
        private System.Windows.Forms.TableLayoutPanel _shuntLayout;
        private System.Windows.Forms.TableLayoutPanel _shuntDisplayLayout;
        private System.Windows.Forms.TextBox _shuntResistanceTextBox;
        private System.Windows.Forms.Label _shuntResistanceTextBoxLabel;
        private System.Windows.Forms.TableLayoutPanel _shuntGroupBoxLayout;
        private System.Windows.Forms.Button _measureShuntResistanceButton;
        private System.Windows.Forms.TableLayoutPanel _shuntConfigureGroupBoxLayout;
        private System.Windows.Forms.GroupBox _shuntResistanceConfigurationGroupBox;
        private System.Windows.Forms.Button _restoreShuntResistanceDefaultsButton;
        private System.Windows.Forms.Button _applyNewShuntResistanceConfigurationButton;
        private System.Windows.Forms.Button _applyShuntResistanceConfigurationButton;
        private System.Windows.Forms.NumericUpDown _shuntResistanceCurrentRangeNumeric;
        private System.Windows.Forms.Label _shuntResistanceCurrentRangeNumericLabel;
        private System.Windows.Forms.NumericUpDown _shuntResistanceLowLimitNumeric;
        private System.Windows.Forms.Label _shuntResistanceLowLimitNumericLabel;
        private System.Windows.Forms.NumericUpDown _shuntResistanceHighLimitNumeric;
        private System.Windows.Forms.Label _shuntResistanceHighLimitNumericLabel;
        private System.Windows.Forms.NumericUpDown _shuntResistanceVoltageLimitNumeric;
        private System.Windows.Forms.Label _shuntResistanceVoltageLimitNumericLabel;
        private System.Windows.Forms.NumericUpDown _shuntResistanceCurrentLevelNumeric;
        private System.Windows.Forms.Label _shuntResistanceCurrentLevelNumericLabel;
        private System.Windows.Forms.TabPage _partsTabPage;
        private System.Windows.Forms.TableLayoutPanel _partsLayout;
        private PartsView _partsPanel;
        private System.Windows.Forms.TabPage _messagesTabPage;
        private cc.isr.Logging.TraceLog.WinForms.MessagesBox _traceMessagesBox;
        private PartHeader _partHeader;
        private ThermalTransientHeader _thermalTransientHeader;
        private System.Windows.Forms.StatusStrip _statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel _statusLabel;
        private MeasurementsHeader _measurementsHeader;
        private System.Windows.Forms.Timer _meterTimer;
    }
}
