using System;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls
{
    public partial class MeasurementViewBase
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
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(MeasurementViewBase));
            _ttmToolStripContainer = new System.Windows.Forms.ToolStripContainer();
            _ttmMeasureControlsToolStrip = new System.Windows.Forms.ToolStrip();
            _measureToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            _abortSequenceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _abortSequenceToolStripMenuItem.Click += new EventHandler(AbortSequenceToolStripMenuItem_Click);
            _measureAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _measureAllToolStripMenuItem.Click += new EventHandler(MeasureAllToolStripMenuItem_Click);
            _finalResistanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _finalResistanceToolStripMenuItem.Click += new EventHandler(FinalResistanceToolStripMenuItem_Click);
            _thermalTransientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _thermalTransientToolStripMenuItem.Click += new EventHandler(ThermalTransientToolStripMenuItem_Click);
            _initialResistanceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _initialResistanceToolStripMenuItem.Click += new EventHandler(InitialResistanceToolStripMenuItem_Click);
            _clearToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _clearToolStripMenuItem.Click += new EventHandler(ClearToolStripMenuItem_Click);
            _ttmToolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            _toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            _triggerToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            _waitForTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _waitForTriggerToolStripMenuItem.CheckedChanged += new EventHandler(WaitForTriggerToolStripMenuItem_CheckChanged);
            _assertTriggerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _assertTriggerToolStripMenuItem.Click += new EventHandler(AssertTriggerToolStripMenuItem_Click);
            _abortToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _abortToolStripMenuItem.Click += new EventHandler(AbortToolStripMenuItem_Click);
            _triggerActionToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            _toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            _traceToolStripDropDownButton = new System.Windows.Forms.ToolStripDropDownButton();
            _modelTraceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _modelTraceToolStripMenuItem.Click += new EventHandler(ModelTraceToolStripMenuItem_Click);
            _saveTraceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _saveTraceToolStripMenuItem.Click += new EventHandler(SaveTraceToolStripMenuItem_Click);
            _clearTraceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _clearTraceToolStripMenuItem.Click += new EventHandler(ClearTraceToolStripMenuItem_Click);
            _readTraceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _readTraceToolStripMenuItem.Click += new EventHandler(ReadTraceToolStripMenuItem_Click);
            _logTraceLevelComboBox = new cc.isr.WinControls.ToolStripComboBox();
            _logTraceLevelComboBox.SelectedIndexChanged += new EventHandler(LogTraceLevelComboBox_SelectedIndexChanged);
            _displayTraceLevelComboBox = new cc.isr.WinControls.ToolStripComboBox();
            _displayTraceLevelComboBox.SelectedIndexChanged += new EventHandler(DisplayTraceLevelComboBox_SelectedIndexChanged);
            _chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            _meterTimer = new System.Windows.Forms.Timer(components);
            _traceDataGridView = new System.Windows.Forms.DataGridView();
            _splitContainer = new System.Windows.Forms.SplitContainer();
            _ttmToolStripContainer.BottomToolStripPanel.SuspendLayout();
            _ttmToolStripContainer.ContentPanel.SuspendLayout();
            _ttmToolStripContainer.SuspendLayout();
            _ttmMeasureControlsToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_chart).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_traceDataGridView).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_splitContainer).BeginInit();
            _splitContainer.Panel1.SuspendLayout();
            _splitContainer.Panel2.SuspendLayout();
            _splitContainer.SuspendLayout();
            SuspendLayout();
            //
            // _ttmToolStripContainer
            //
            //
            // _ttmToolStripContainer.BottomToolStripPanel
            //
            _ttmToolStripContainer.BottomToolStripPanel.Controls.Add(_ttmMeasureControlsToolStrip);
            //
            // _ttmToolStripContainer.ContentPanel
            //
            _ttmToolStripContainer.ContentPanel.Controls.Add(_splitContainer);
            _ttmToolStripContainer.ContentPanel.Size = new System.Drawing.Size(523, 427);
            _ttmToolStripContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            _ttmToolStripContainer.Location = new System.Drawing.Point(0, 0);
            _ttmToolStripContainer.Name = "_TtmToolStripContainer";
            _ttmToolStripContainer.Size = new System.Drawing.Size(523, 477);
            _ttmToolStripContainer.TabIndex = 7;
            _ttmToolStripContainer.Text = "ToolStripContainer1";
            //
            // _ttmMeasureControlsToolStrip
            //
            _ttmMeasureControlsToolStrip.Dock = System.Windows.Forms.DockStyle.None;
            _ttmMeasureControlsToolStrip.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _ttmMeasureControlsToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _measureToolStripDropDownButton, _ttmToolStripProgressBar, _toolStripSeparator1, _triggerToolStripDropDownButton, _triggerActionToolStripLabel, _toolStripSeparator2, _traceToolStripDropDownButton });
            _ttmMeasureControlsToolStrip.Location = new System.Drawing.Point(3, 0);
            _ttmMeasureControlsToolStrip.Name = "_TtmMeasureControlsToolStrip";
            _ttmMeasureControlsToolStrip.Size = new System.Drawing.Size(389, 25);
            _ttmMeasureControlsToolStrip.TabIndex = 0;
            _ttmMeasureControlsToolStrip.Text = "TTM Measure Controls";
            //
            // _measureToolStripDropDownButton
            //
            _measureToolStripDropDownButton.AutoToolTip = false;
            _measureToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            _measureToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _abortSequenceToolStripMenuItem, _measureAllToolStripMenuItem, _finalResistanceToolStripMenuItem, _thermalTransientToolStripMenuItem, _initialResistanceToolStripMenuItem, _clearToolStripMenuItem, _logTraceLevelComboBox, _displayTraceLevelComboBox });
            _measureToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _measureToolStripDropDownButton.Name = "_MeasureToolStripDropDownButton";
            _measureToolStripDropDownButton.Size = new System.Drawing.Size(78, 22);
            _measureToolStripDropDownButton.Text = "MEASURE:";
            _measureToolStripDropDownButton.ToolTipText = "Measure thermal transient elements";
            //
            // _abortSequenceToolStripMenuItem
            //
            _abortSequenceToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "media-playback-stop-2" );
            _abortSequenceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _abortSequenceToolStripMenuItem.Name = "_abortSequenceToolStripMenuItem";
            _abortSequenceToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            _abortSequenceToolStripMenuItem.Text = "ABORT";
            _abortSequenceToolStripMenuItem.ToolTipText = "Abort measurement sequence";
            //
            // _measureAllToolStripMenuItem
            //
            _measureAllToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "arrow-right-double" );
            _measureAllToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _measureAllToolStripMenuItem.Name = "_MeasureAllToolStripMenuItem";
            _measureAllToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            _measureAllToolStripMenuItem.Text = "ALL";
            _measureAllToolStripMenuItem.ToolTipText = "Measure initial resistance, thermal transient, and final resistance";
            //
            // _finalResistanceToolStripMenuItem
            //
            _finalResistanceToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "arrow-right-2" );
            _finalResistanceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _finalResistanceToolStripMenuItem.Name = "_FinalResistanceToolStripMenuItem";
            _finalResistanceToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            _finalResistanceToolStripMenuItem.Text = "FINAL RESISTANCE";
            _finalResistanceToolStripMenuItem.ToolTipText = "Measure Final MeasuredValue";
            //
            // _thermalTransientToolStripMenuItem
            //
            _thermalTransientToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "arrow-right-2" );
            _thermalTransientToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _thermalTransientToolStripMenuItem.Name = "_ThermalTransientToolStripMenuItem";
            _thermalTransientToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            _thermalTransientToolStripMenuItem.Text = "THERMAL TRANSIENT";
            _thermalTransientToolStripMenuItem.ToolTipText = "Measure Thermal Transient";
            //
            // _initialResistanceToolStripMenuItem
            //
            _initialResistanceToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "arrow-right-2" );
            _initialResistanceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _initialResistanceToolStripMenuItem.Name = "_InitialResistanceToolStripMenuItem";
            _initialResistanceToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            _initialResistanceToolStripMenuItem.Text = "INITIAL RESISTANCE";
            _initialResistanceToolStripMenuItem.ToolTipText = "Measure Initial MeasuredValue";
            //
            // _clearToolStripMenuItem
            //
            _clearToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "edit-clear-2" );
            _clearToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _clearToolStripMenuItem.Name = "_ClearToolStripMenuItem";
            _clearToolStripMenuItem.Size = new System.Drawing.Size(223, 38);
            _clearToolStripMenuItem.Text = "CLEAR MEASUREMENTS";
            _clearToolStripMenuItem.ToolTipText = "Clear measurements on next Initial MeasuredValue measurement";
            //
            // _ttmToolStripProgressBar
            //
            _ttmToolStripProgressBar.AutoSize = false;
            _ttmToolStripProgressBar.Name = "_TtmToolStripProgressBar";
            _ttmToolStripProgressBar.Size = new System.Drawing.Size(100, 15);
            _ttmToolStripProgressBar.ToolTipText = "Measurement sequence progress";
            //
            // _toolStripSeparator1
            //
            _toolStripSeparator1.Name = "_ToolStripSeparator1";
            _toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            //
            // _triggerToolStripDropDownButton
            //
            _triggerToolStripDropDownButton.AutoToolTip = false;
            _triggerToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            _triggerToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _waitForTriggerToolStripMenuItem, _assertTriggerToolStripMenuItem, _abortToolStripMenuItem });
            _triggerToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _triggerToolStripDropDownButton.Name = "_TriggerToolStripDropDownButton";
            _triggerToolStripDropDownButton.Size = new System.Drawing.Size(71, 22);
            _triggerToolStripDropDownButton.Text = "TRIGGER";
            _triggerToolStripDropDownButton.ToolTipText = "Trigger Options";
            //
            // _waitForTriggerToolStripMenuItem
            //
            _waitForTriggerToolStripMenuItem.CheckOnClick = true;
            _waitForTriggerToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "view-refresh-7" );
            _waitForTriggerToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _waitForTriggerToolStripMenuItem.Name = "_WaitForTriggerToolStripMenuItem";
            _waitForTriggerToolStripMenuItem.Size = new System.Drawing.Size(179, 38);
            _waitForTriggerToolStripMenuItem.Text = "Wait for Trigger";
            _waitForTriggerToolStripMenuItem.ToolTipText = "Check to enable waiting for trigger";
            //
            // _assertTriggerToolStripMenuItem
            //
            _assertTriggerToolStripMenuItem.Enabled = false;
            _assertTriggerToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "edit-add" );
            _assertTriggerToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _assertTriggerToolStripMenuItem.Name = "_assertTriggerToolStripMenuItem";
            _assertTriggerToolStripMenuItem.Size = new System.Drawing.Size(179, 38);
            _assertTriggerToolStripMenuItem.Text = "Assert Trigger";
            _assertTriggerToolStripMenuItem.ToolTipText = "Assert instrument trigger";
            //
            // _abortToolStripMenuItem
            //
            _abortToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "process-stop" );
            _abortToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _abortToolStripMenuItem.Name = "_abortToolStripMenuItem";
            _abortToolStripMenuItem.Size = new System.Drawing.Size(179, 38);
            _abortToolStripMenuItem.Text = "ABORT";
            _abortToolStripMenuItem.ToolTipText = "Forces abort";
            //
            // _triggerActionToolStripLabel
            //
            _triggerActionToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9.0f, System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, 0);
            _triggerActionToolStripLabel.Name = "_TriggerActionToolStripLabel";
            _triggerActionToolStripLabel.Size = new System.Drawing.Size(52, 22);
            _triggerActionToolStripLabel.Text = "Inactive";
            //
            // _toolStripSeparator2
            //
            _toolStripSeparator2.Name = "_ToolStripSeparator2";
            _toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            //
            // _traceToolStripDropDownButton
            //
            _traceToolStripDropDownButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            _traceToolStripDropDownButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _modelTraceToolStripMenuItem, _saveTraceToolStripMenuItem, _clearTraceToolStripMenuItem, _readTraceToolStripMenuItem });
            _traceToolStripDropDownButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _traceToolStripDropDownButton.Name = "_TraceToolStripDropDownButton";
            _traceToolStripDropDownButton.Size = new System.Drawing.Size(62, 22);
            _traceToolStripDropDownButton.Text = "TRACE: ";
            //
            // _modelTraceToolStripMenuItem
            //
            _modelTraceToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "games-solve" );
            _modelTraceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _modelTraceToolStripMenuItem.Name = "_ModelTraceToolStripMenuItem";
            _modelTraceToolStripMenuItem.Size = new System.Drawing.Size(148, 38);
            _modelTraceToolStripMenuItem.Text = "CURVE FIT";
            _modelTraceToolStripMenuItem.ToolTipText = "Fix a model to the trace";
            //
            // _saveTraceToolStripMenuItem
            //
            _saveTraceToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "document-export" );
            _saveTraceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _saveTraceToolStripMenuItem.Name = "_SaveTraceToolStripMenuItem";
            _saveTraceToolStripMenuItem.Size = new System.Drawing.Size(148, 38);
            _saveTraceToolStripMenuItem.Text = "SAVE";
            _saveTraceToolStripMenuItem.ToolTipText = "Save trace values to file";
            //
            // _clearTraceToolStripMenuItem
            //
            _clearTraceToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "edit-clear-2" );
            _clearTraceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _clearTraceToolStripMenuItem.Name = "_ClearTraceToolStripMenuItem";
            _clearTraceToolStripMenuItem.Size = new System.Drawing.Size(148, 38);
            _clearTraceToolStripMenuItem.Text = "CLEAR";
            _clearTraceToolStripMenuItem.ToolTipText = "Clear trace chart and list";
            //
            // _readTraceToolStripMenuItem
            //
            _readTraceToolStripMenuItem.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( "view-object-histogram-logarithmic" );
            _readTraceToolStripMenuItem.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            _readTraceToolStripMenuItem.Name = "_ReadTraceToolStripMenuItem";
            _readTraceToolStripMenuItem.Size = new System.Drawing.Size(148, 38);
            _readTraceToolStripMenuItem.Text = "READ";
            //
            // _chart
            //
            _chart.BorderlineColor = System.Drawing.Color.Aqua;
            _chart.BorderlineDashStyle = System.Windows.Forms.DataVisualization.Charting.ChartDashStyle.Solid;
            _chart.Dock = System.Windows.Forms.DockStyle.Fill;
            _chart.Location = new System.Drawing.Point(0, 0);
            _chart.Name = "_Chart";
            _chart.Size = new System.Drawing.Size(330, 427);
            _chart.TabIndex = 0;
            _chart.Text = "Chart1";
            //
            // _traceDataGridView
            //
            _traceDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _traceDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            _traceDataGridView.Location = new System.Drawing.Point(0, 0);
            _traceDataGridView.Name = "_TraceDataGridView";
            _traceDataGridView.Size = new System.Drawing.Size(189, 427);
            _traceDataGridView.TabIndex = 1;
            //
            // _splitContainer
            //
            _splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            _splitContainer.Location = new System.Drawing.Point(0, 0);
            _splitContainer.Name = "_SplitContainer";
            //
            // _splitContainer.Panel1
            //
            _splitContainer.Panel1.Controls.Add(_chart);
            //
            // _splitContainer.Panel2
            //
            _splitContainer.Panel2.Controls.Add(_traceDataGridView);
            _splitContainer.Size = new System.Drawing.Size(523, 427);
            _splitContainer.SplitterDistance = 330;
            _splitContainer.TabIndex = 2;
            //
            // _logTraceLevelComboBox
            //
            _logTraceLevelComboBox.Name = "_LogTraceLevelComboBox";
            _logTraceLevelComboBox.Size = new System.Drawing.Size(100, 22);
            _logTraceLevelComboBox.Text = "Verbose";
            _logTraceLevelComboBox.ToolTipText = "Log Trace Level";
            //
            // _displayTraceLevelComboBox
            //
            _displayTraceLevelComboBox.Name = "_DisplayTraceLevelComboBox";
            _displayTraceLevelComboBox.Size = new System.Drawing.Size(100, 22);
            _displayTraceLevelComboBox.Text = "Warning";
            _displayTraceLevelComboBox.ToolTipText = "Display trace level";
            //
            // MeasurementPanelBase
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_ttmToolStripContainer);
            Name = "MeasurementPanelBase";
            Size = new System.Drawing.Size(523, 477);
            _ttmToolStripContainer.BottomToolStripPanel.ResumeLayout(false);
            _ttmToolStripContainer.BottomToolStripPanel.PerformLayout();
            _ttmToolStripContainer.ContentPanel.ResumeLayout(false);
            _ttmToolStripContainer.ResumeLayout(false);
            _ttmToolStripContainer.PerformLayout();
            _ttmMeasureControlsToolStrip.ResumeLayout(false);
            _ttmMeasureControlsToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_chart).EndInit();
            ((System.ComponentModel.ISupportInitialize)_traceDataGridView).EndInit();
            _splitContainer.Panel1.ResumeLayout(false);
            _splitContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)_splitContainer).EndInit();
            _splitContainer.ResumeLayout(false);
            ResumeLayout(false);
        }

        private System.Windows.Forms.ToolStripContainer _ttmToolStripContainer;
        private System.Windows.Forms.ToolStrip _ttmMeasureControlsToolStrip;
        private System.Windows.Forms.ToolStripProgressBar _ttmToolStripProgressBar;
        private System.Windows.Forms.DataVisualization.Charting.Chart _chart;
        private System.Windows.Forms.Timer _meterTimer;
        private System.Windows.Forms.ToolStripDropDownButton _measureToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem _abortSequenceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _measureAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _finalResistanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _thermalTransientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _initialResistanceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _clearToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator1;
        private System.Windows.Forms.ToolStripDropDownButton _triggerToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem _waitForTriggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _assertTriggerToolStripMenuItem;
        private System.Windows.Forms.ToolStripLabel _triggerActionToolStripLabel;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator2;
        private System.Windows.Forms.ToolStripDropDownButton _traceToolStripDropDownButton;
        private System.Windows.Forms.ToolStripMenuItem _modelTraceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _saveTraceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _clearTraceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _readTraceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _abortToolStripMenuItem;
        private System.Windows.Forms.DataGridView _traceDataGridView;
        private System.Windows.Forms.SplitContainer _splitContainer;
        private cc.isr.WinControls.ToolStripComboBox _logTraceLevelComboBox;
        private cc.isr.WinControls.ToolStripComboBox _displayTraceLevelComboBox;
    }
}
