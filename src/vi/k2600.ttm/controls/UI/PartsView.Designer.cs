using System;
using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls
{
    public partial class PartsView
    {
        /// <summary>   UserControl overrides dispose to clean up the component list. </summary>
        /// <remarks>   David, 2021-09-07. </remarks>
        /// <param name="disposing">    true to release both managed and unmanaged resources; false to
        ///                             release only unmanaged resources. </param>
        [DebuggerNonUserCode()]
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing)
                {
                    ReleaseResources();
                    components?.Dispose();                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }

        // Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;

        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            var resources = new System.ComponentModel.ComponentResourceManager(typeof(PartsView));
            _partsListToolStrip = new System.Windows.Forms.ToolStrip();
            _addPartToolStripButton = new System.Windows.Forms.ToolStripButton();
            _addPartToolStripButton.Click += new EventHandler(AddPartToolStripButton_Click);
            _clearPartListToolStripButton = new System.Windows.Forms.ToolStripButton();
            _clearPartListToolStripButton.Click += new EventHandler(ClearPartListToolStripButton_Click);
            _savePartsToolStripButton = new System.Windows.Forms.ToolStripButton();
            _savePartsToolStripButton.Click += new EventHandler(SavePartsToolStripButton_Click);
            _partNumberToolStrip = new System.Windows.Forms.ToolStrip();
            _operatorLabel = new System.Windows.Forms.ToolStripLabel();
            _operatorToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            _lotToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            _lotToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            _partNumberToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            _partNumberToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            _toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            _partToolStrip = new System.Windows.Forms.ToolStrip();
            _serialNumberToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            _serialNumberToolStripTextBox = new System.Windows.Forms.ToolStripTextBox();
            _toolStripSplitButton = new System.Windows.Forms.ToolStripSplitButton();
            _autoAddToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _clearMeasurementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            _toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            _outcomeToolStripLabel = new System.Windows.Forms.ToolStripLabel();
            _passFailToolStripButton = new System.Windows.Forms.ToolStripButton();
            _partsDataGridView = new System.Windows.Forms.DataGridView();
            _partsDataGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(PartsDataGridView_DataError);
            _partsListToolStrip.SuspendLayout();
            _partNumberToolStrip.SuspendLayout();
            _partToolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_partsDataGridView).BeginInit();
            SuspendLayout();
            //
            // _partsListToolStrip
            //
            _partsListToolStrip.Dock = System.Windows.Forms.DockStyle.Bottom;
            _partsListToolStrip.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _partsListToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _addPartToolStripButton, _clearPartListToolStripButton, _savePartsToolStripButton });
            _partsListToolStrip.Location = new System.Drawing.Point(0, 404);
            _partsListToolStrip.Name = "_PartsListToolStrip";
            _partsListToolStrip.Size = new System.Drawing.Size(584, 25);
            _partsListToolStrip.TabIndex = 2;
            _partsListToolStrip.Text = "ToolStrip1";
            //
            // _addPartToolStripButton
            //
            _addPartToolStripButton.AutoToolTip = false;
            _addPartToolStripButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _addPartToolStripButton.Image = (System.Drawing.Image)resources.GetObject("_addPartToolStripButton.Image");
            _addPartToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _addPartToolStripButton.Name = "_addPartToolStripButton";
            _addPartToolStripButton.Size = new System.Drawing.Size(93, 22);
            _addPartToolStripButton.Text = "ADD PART";
            _addPartToolStripButton.ToolTipText = "Add part to the list";
            //
            // _clearPartListToolStripButton
            //
            _clearPartListToolStripButton.AutoToolTip = false;
            _clearPartListToolStripButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _clearPartListToolStripButton.Image = (System.Drawing.Image)resources.GetObject("_ClearPartListToolStripButton.Image");
            _clearPartListToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _clearPartListToolStripButton.Name = "_ClearPartListToolStripButton";
            _clearPartListToolStripButton.Size = new System.Drawing.Size(140, 22);
            _clearPartListToolStripButton.Text = "CLEAR PARTS LIST";
            _clearPartListToolStripButton.ToolTipText = "Clear Parts List";
            //
            // _savePartsToolStripButton
            //
            _savePartsToolStripButton.AutoToolTip = false;
            _savePartsToolStripButton.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _savePartsToolStripButton.Image = (System.Drawing.Image)resources.GetObject("_SavePartsToolStripButton.Image");
            _savePartsToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _savePartsToolStripButton.Name = "_SavePartsToolStripButton";
            _savePartsToolStripButton.Size = new System.Drawing.Size(102, 22);
            _savePartsToolStripButton.Text = "SAVE PARTS";
            _savePartsToolStripButton.ToolTipText = "Save parts to file";
            //
            // _partNumberToolStrip
            //
            _partNumberToolStrip.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _partNumberToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _operatorLabel, _operatorToolStripTextBox, _lotToolStripLabel, _lotToolStripTextBox, _partNumberToolStripLabel, _partNumberToolStripTextBox, _toolStripSeparator1 });
            _partNumberToolStrip.Location = new System.Drawing.Point(0, 0);
            _partNumberToolStrip.Name = "_PartNumberToolStrip";
            _partNumberToolStrip.Size = new System.Drawing.Size(584, 25);
            _partNumberToolStrip.TabIndex = 3;
            _partNumberToolStrip.Text = "ToolStrip1";
            //
            // _operatorLabel
            //
            _operatorLabel.Name = "_OperatorLabel";
            _operatorLabel.Size = new System.Drawing.Size(65, 22);
            _operatorLabel.Text = "Operator:";
            //
            // _operatorToolStripTextBox
            //
            _operatorToolStripTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _operatorToolStripTextBox.Name = "_OperatorToolStripTextBox";
            _operatorToolStripTextBox.Size = new System.Drawing.Size(132, 25);
            _operatorToolStripTextBox.ToolTipText = "Operator Id";
            //
            // _lotToolStripLabel
            //
            _lotToolStripLabel.Name = "_LotToolStripLabel";
            _lotToolStripLabel.Size = new System.Drawing.Size(29, 22);
            _lotToolStripLabel.Text = "Lot:";
            //
            // _lotToolStripTextBox
            //
            _lotToolStripTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _lotToolStripTextBox.Name = "_LotToolStripTextBox";
            _lotToolStripTextBox.Size = new System.Drawing.Size(132, 25);
            _lotToolStripTextBox.ToolTipText = "Lot number";
            //
            // _partNumberToolStripLabel
            //
            _partNumberToolStripLabel.Name = "_PartNumberToolStripLabel";
            _partNumberToolStripLabel.Size = new System.Drawing.Size(34, 22);
            _partNumberToolStripLabel.Text = "Part:";
            //
            // _partNumberToolStripTextBox
            //
            _partNumberToolStripTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _partNumberToolStripTextBox.Name = "_PartNumberToolStripTextBox";
            _partNumberToolStripTextBox.Size = new System.Drawing.Size(132, 25);
            _partNumberToolStripTextBox.ToolTipText = "Part number";
            //
            // _toolStripSeparator1
            //
            _toolStripSeparator1.Name = "_ToolStripSeparator1";
            _toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            //
            // _partToolStrip
            //
            _partToolStrip.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            _partToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { _serialNumberToolStripLabel, _serialNumberToolStripTextBox, _toolStripSplitButton, _toolStripSeparator2, _outcomeToolStripLabel, _passFailToolStripButton });
            _partToolStrip.Location = new System.Drawing.Point(0, 25);
            _partToolStrip.Name = "_PartToolStrip";
            _partToolStrip.Size = new System.Drawing.Size(584, 25);
            _partToolStrip.TabIndex = 4;
            _partToolStrip.Text = "ToolStrip1";
            //
            // _serialNumberToolStripLabel
            //
            _serialNumberToolStripLabel.Name = "_SerialNumberToolStripLabel";
            _serialNumberToolStripLabel.Size = new System.Drawing.Size(33, 22);
            _serialNumberToolStripLabel.Text = "S/N:";
            //
            // _serialNumberToolStripTextBox
            //
            _serialNumberToolStripTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _serialNumberToolStripTextBox.Name = "_SerialNumberToolStripTextBox";
            _serialNumberToolStripTextBox.Size = new System.Drawing.Size(132, 25);
            _serialNumberToolStripTextBox.ToolTipText = "Serial Number";
            //
            // _toolStripSplitButton
            //
            _toolStripSplitButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { _autoAddToolStripMenuItem, _clearMeasurementsToolStripMenuItem });
            _toolStripSplitButton.Image = (System.Drawing.Image)resources.GetObject("_ToolStripSplitButton.Image");
            _toolStripSplitButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _toolStripSplitButton.Name = "_ToolStripSplitButton";
            _toolStripSplitButton.Size = new System.Drawing.Size(86, 22);
            _toolStripSplitButton.Text = "Options";
            //
            // _autoAddToolStripMenuItem
            //
            _autoAddToolStripMenuItem.CheckOnClick = true;
            _autoAddToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _autoAddToolStripMenuItem.Name = "_autoAddToolStripMenuItem";
            _autoAddToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            _autoAddToolStripMenuItem.Text = "AUTO ADD";
            _autoAddToolStripMenuItem.ToolTipText = "Automatically add parts when measurements complete.";
            //
            // _clearMeasurementsToolStripMenuItem
            //
            _clearMeasurementsToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _clearMeasurementsToolStripMenuItem.Name = "_ClearMeasurementsToolStripMenuItem";
            _clearMeasurementsToolStripMenuItem.Size = new System.Drawing.Size(222, 22);
            _clearMeasurementsToolStripMenuItem.Text = "CLEAR MEASUREMENTS";
            _clearMeasurementsToolStripMenuItem.ToolTipText = "Clears all measurements on the current part";
            //
            // _toolStripSeparator2
            //
            _toolStripSeparator2.Name = "_ToolStripSeparator2";
            _toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            //
            // _outcomeToolStripLabel
            //
            _outcomeToolStripLabel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _outcomeToolStripLabel.Name = "_OutcomeToolStripLabel";
            _outcomeToolStripLabel.Size = new System.Drawing.Size(80, 22);
            _outcomeToolStripLabel.Text = "<outcome>";
            //
            // _passFailToolStripButton
            //
            _passFailToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            // _passFailToolStripButton.Image = new System.Drawing.Bitmap(  new System.IO.MemoryStream( Properties.Resources.Good ) );
            _passFailToolStripButton.Image = ( System.Drawing.Bitmap ) Properties.Resources.ResourceManager.GetObject( "Good" );
            _passFailToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            _passFailToolStripButton.Name = "_PassFailToolStripButton";
            _passFailToolStripButton.Size = new System.Drawing.Size(23, 22);
            _passFailToolStripButton.Visible = false;
            //
            // _partsDataGridView
            //
            _partsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            _partsDataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            _partsDataGridView.Location = new System.Drawing.Point(0, 50);
            _partsDataGridView.Name = "_PartsDataGridView";
            _partsDataGridView.Size = new System.Drawing.Size(584, 354);
            _partsDataGridView.TabIndex = 5;
            //
            // PartsPanel
            //
            AutoScaleDimensions = new System.Drawing.SizeF(8.0f, 17.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(_partsDataGridView);
            Controls.Add(_partToolStrip);
            Controls.Add(_partNumberToolStrip);
            Controls.Add(_partsListToolStrip);
            Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            Name = "PartsPanel";
            Size = new System.Drawing.Size(584, 429);
            _partsListToolStrip.ResumeLayout(false);
            _partsListToolStrip.PerformLayout();
            _partNumberToolStrip.ResumeLayout(false);
            _partNumberToolStrip.PerformLayout();
            _partToolStrip.ResumeLayout(false);
            _partToolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_partsDataGridView).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        private System.Windows.Forms.ToolStrip _partsListToolStrip;
        private System.Windows.Forms.ToolStripButton _addPartToolStripButton;
        private System.Windows.Forms.ToolStripButton _clearPartListToolStripButton;
        private System.Windows.Forms.ToolStripButton _savePartsToolStripButton;
        private System.Windows.Forms.ToolStrip _partNumberToolStrip;
        private System.Windows.Forms.ToolStripLabel _operatorLabel;
        private System.Windows.Forms.ToolStripTextBox _operatorToolStripTextBox;
        private System.Windows.Forms.ToolStripLabel _lotToolStripLabel;
        private System.Windows.Forms.ToolStripTextBox _lotToolStripTextBox;
        private System.Windows.Forms.ToolStripLabel _partNumberToolStripLabel;
        private System.Windows.Forms.ToolStripTextBox _partNumberToolStripTextBox;
        private System.Windows.Forms.ToolStrip _partToolStrip;
        private System.Windows.Forms.ToolStripLabel _serialNumberToolStripLabel;
        private System.Windows.Forms.ToolStripTextBox _serialNumberToolStripTextBox;
        private System.Windows.Forms.ToolStripSplitButton _toolStripSplitButton;
        private System.Windows.Forms.ToolStripMenuItem _autoAddToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem _clearMeasurementsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator _toolStripSeparator2;
        private System.Windows.Forms.ToolStripLabel _outcomeToolStripLabel;
        private System.Windows.Forms.ToolStripButton _passFailToolStripButton;
        private System.Windows.Forms.DataGridView _partsDataGridView;
    }
}
