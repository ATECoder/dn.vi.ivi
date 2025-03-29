using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls
{
    public partial class PartHeader
    {
        // NOTE: The following procedure is required by the Windows Form Designer
        // It can be modified using the Windows Form Designer.
        // Do not modify it using the code editor.
        [DebuggerStepThrough()]
        private void InitializeComponent()
        {
            _tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            _partNumberTextBoxLabel = new System.Windows.Forms.Label();
            _partNumberTextBox = new System.Windows.Forms.TextBox();
            _partSerialNumberTextBoxLabel = new System.Windows.Forms.Label();
            _partSerialNumberTextBox = new System.Windows.Forms.TextBox();
            _tableLayoutPanel.SuspendLayout();
            SuspendLayout();
            //
            // _tableLayoutPanel
            //
            _tableLayoutPanel.BackColor = System.Drawing.Color.Transparent;
            _tableLayoutPanel.ColumnCount = 5;
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0f));
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _tableLayoutPanel.Controls.Add(_partNumberTextBoxLabel, 0, 1);
            _tableLayoutPanel.Controls.Add(_partNumberTextBox, 1, 1);
            _tableLayoutPanel.Controls.Add(_partSerialNumberTextBoxLabel, 3, 1);
            _tableLayoutPanel.Controls.Add(_partSerialNumberTextBox, 4, 1);
            _tableLayoutPanel.Dock = System.Windows.Forms.DockStyle.Top;
            _tableLayoutPanel.Location = new System.Drawing.Point(0, 0);
            _tableLayoutPanel.Name = "_TableLayoutPanel";
            _tableLayoutPanel.RowCount = 3;
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0f));
            _tableLayoutPanel.Size = new System.Drawing.Size(670, 21);
            _tableLayoutPanel.TabIndex = 0;
            //
            // _partNumberTextBoxLabel
            //
            _partNumberTextBoxLabel.AutoSize = true;
            _partNumberTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _partNumberTextBoxLabel.Location = new System.Drawing.Point(3, 2);
            _partNumberTextBoxLabel.Margin = new System.Windows.Forms.Padding(3);
            _partNumberTextBoxLabel.Name = "_PartNumberTextBoxLabel";
            _partNumberTextBoxLabel.Size = new System.Drawing.Size(98, 17);
            _partNumberTextBoxLabel.TabIndex = 1;
            _partNumberTextBoxLabel.Text = "PART NUMBER:";
            //
            // _partNumberTextBox
            //
            _partNumberTextBox.BackColor = System.Drawing.SystemColors.Desktop;
            _partNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _partNumberTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _partNumberTextBox.ForeColor = System.Drawing.Color.LawnGreen;
            _partNumberTextBox.Location = new System.Drawing.Point(107, 2);
            _partNumberTextBox.Name = "_PartNumberTextBox";
            _partNumberTextBox.Size = new System.Drawing.Size(164, 18);
            _partNumberTextBox.TabIndex = 2;
            _partNumberTextBox.Text = "10";
            //
            // _partSerialNumberTextBoxLabel
            //
            _partSerialNumberTextBoxLabel.AutoSize = true;
            _partSerialNumberTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _partSerialNumberTextBoxLabel.Location = new System.Drawing.Point(541, 2);
            _partSerialNumberTextBoxLabel.Margin = new System.Windows.Forms.Padding(3);
            _partSerialNumberTextBoxLabel.Name = "_PartSerialNumberTextBoxLabel";
            _partSerialNumberTextBoxLabel.Size = new System.Drawing.Size(33, 17);
            _partSerialNumberTextBoxLabel.TabIndex = 3;
            _partSerialNumberTextBoxLabel.Text = "S/N:";
            //
            // _partSerialNumberTextBox
            //
            _partSerialNumberTextBox.BackColor = System.Drawing.SystemColors.Desktop;
            _partSerialNumberTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _partSerialNumberTextBox.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _partSerialNumberTextBox.ForeColor = System.Drawing.Color.LawnGreen;
            _partSerialNumberTextBox.Location = new System.Drawing.Point(580, 2);
            _partSerialNumberTextBox.Name = "_PartSerialNumberTextBox";
            _partSerialNumberTextBox.Size = new System.Drawing.Size(87, 18);
            _partSerialNumberTextBox.TabIndex = 4;
            _partSerialNumberTextBox.Text = "10";
            //
            // PartHeader
            //
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.Black;
            Controls.Add(_tableLayoutPanel);
            Name = "PartHeader";
            Size = new System.Drawing.Size(670, 21);
            _tableLayoutPanel.ResumeLayout(false);
            _tableLayoutPanel.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel _tableLayoutPanel;
        private System.Windows.Forms.Label _partNumberTextBoxLabel;
        private System.Windows.Forms.TextBox _partNumberTextBox;
        private System.Windows.Forms.Label _partSerialNumberTextBoxLabel;
        private System.Windows.Forms.TextBox _partSerialNumberTextBox;
    }
}
