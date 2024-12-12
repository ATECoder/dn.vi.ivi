using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls
{
    public partial class MeasurementsHeader
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
            _outcomeLayout = new System.Windows.Forms.TableLayoutPanel();
            _thermalTransientVoltageTextBox = new System.Windows.Forms.TextBox();
            _thermalTransientVoltageTextBoxLabel = new System.Windows.Forms.Label();
            _finalResistanceTextBox = new System.Windows.Forms.TextBox();
            _initialResistanceTextBox = new System.Windows.Forms.TextBox();
            _finalResistanceTextBoxLabel = new System.Windows.Forms.Label();
            _initialResistanceTextBoxLabel = new System.Windows.Forms.Label();
            _outcomePictureBox = new System.Windows.Forms.PictureBox();
            _alertsPictureBox = new System.Windows.Forms.PictureBox();
            _outcomeTextBox = new System.Windows.Forms.TextBox();
            _outcomeTextBoxLabel = new System.Windows.Forms.Label();
            _outcomeLayout.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)_outcomePictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)_alertsPictureBox).BeginInit();
            SuspendLayout();
            // 
            // _outcomeLayout
            // 
            _outcomeLayout.BackColor = System.Drawing.Color.Transparent;
            _outcomeLayout.ColumnCount = 6;
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0f));
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0f));
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0f));
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25.0f));
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _outcomeLayout.Controls.Add(_thermalTransientVoltageTextBox, 2, 2);
            _outcomeLayout.Controls.Add(_thermalTransientVoltageTextBoxLabel, 2, 1);
            _outcomeLayout.Controls.Add(_finalResistanceTextBox, 3, 2);
            _outcomeLayout.Controls.Add(_initialResistanceTextBox, 1, 2);
            _outcomeLayout.Controls.Add(_finalResistanceTextBoxLabel, 3, 1);
            _outcomeLayout.Controls.Add(_initialResistanceTextBoxLabel, 1, 1);
            _outcomeLayout.Controls.Add(_outcomePictureBox, 5, 2);
            _outcomeLayout.Controls.Add(_alertsPictureBox, 0, 2);
            _outcomeLayout.Controls.Add(_outcomeTextBox, 4, 2);
            _outcomeLayout.Controls.Add(_outcomeTextBoxLabel, 4, 1);
            _outcomeLayout.Dock = System.Windows.Forms.DockStyle.Top;
            _outcomeLayout.Location = new System.Drawing.Point(0, 0);
            _outcomeLayout.Name = "_OutcomeLayout";
            _outcomeLayout.RowCount = 3;
            _outcomeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0f));
            _outcomeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _outcomeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _outcomeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _outcomeLayout.Size = new System.Drawing.Size(968, 59);
            _outcomeLayout.TabIndex = 1;
            // 
            // _thermalTransientVoltageTextBox
            // 
            _thermalTransientVoltageTextBox.BackColor = System.Drawing.Color.Black;
            _thermalTransientVoltageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _thermalTransientVoltageTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _thermalTransientVoltageTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _thermalTransientVoltageTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _thermalTransientVoltageTextBox.Location = new System.Drawing.Point(261, 24);
            _thermalTransientVoltageTextBox.Name = "_ThermalTransientVoltageTextBox";
            _thermalTransientVoltageTextBox.ReadOnly = true;
            _thermalTransientVoltageTextBox.Size = new System.Drawing.Size(219, 32);
            _thermalTransientVoltageTextBox.TabIndex = 3;
            _thermalTransientVoltageTextBox.Text = "0.000";
            _thermalTransientVoltageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _thermalTransientVoltageTextBoxLabel
            // 
            _thermalTransientVoltageTextBoxLabel.AutoSize = true;
            _thermalTransientVoltageTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _thermalTransientVoltageTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _thermalTransientVoltageTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _thermalTransientVoltageTextBoxLabel.Location = new System.Drawing.Point(261, 4);
            _thermalTransientVoltageTextBoxLabel.Name = "_ThermalTransientVoltageTextBoxLabel";
            _thermalTransientVoltageTextBoxLabel.Size = new System.Drawing.Size(219, 17);
            _thermalTransientVoltageTextBoxLabel.TabIndex = 2;
            _thermalTransientVoltageTextBoxLabel.Text = "THERMAL TRANSIENT [V]";
            _thermalTransientVoltageTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // _finalResistanceTextBox
            // 
            _finalResistanceTextBox.BackColor = System.Drawing.Color.Black;
            _finalResistanceTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _finalResistanceTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _finalResistanceTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _finalResistanceTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _finalResistanceTextBox.Location = new System.Drawing.Point(486, 24);
            _finalResistanceTextBox.Name = "_FinalResistanceTextBox";
            _finalResistanceTextBox.ReadOnly = true;
            _finalResistanceTextBox.Size = new System.Drawing.Size(219, 32);
            _finalResistanceTextBox.TabIndex = 5;
            _finalResistanceTextBox.Text = "0.000";
            _finalResistanceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _initialResistanceTextBox
            // 
            _initialResistanceTextBox.BackColor = System.Drawing.Color.Black;
            _initialResistanceTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _initialResistanceTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _initialResistanceTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _initialResistanceTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _initialResistanceTextBox.Location = new System.Drawing.Point(36, 24);
            _initialResistanceTextBox.Name = "_InitialResistanceTextBox";
            _initialResistanceTextBox.ReadOnly = true;
            _initialResistanceTextBox.Size = new System.Drawing.Size(219, 32);
            _initialResistanceTextBox.TabIndex = 1;
            _initialResistanceTextBox.Text = "0.000";
            _initialResistanceTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _finalResistanceTextBoxLabel
            // 
            _finalResistanceTextBoxLabel.AutoSize = true;
            _finalResistanceTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _finalResistanceTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _finalResistanceTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _finalResistanceTextBoxLabel.Location = new System.Drawing.Point(486, 4);
            _finalResistanceTextBoxLabel.Name = "_FinalResistanceTextBoxLabel";
            _finalResistanceTextBoxLabel.Size = new System.Drawing.Size(219, 17);
            _finalResistanceTextBoxLabel.TabIndex = 4;
            _finalResistanceTextBoxLabel.Text = "FINAL RESISTANCE [Ω]";
            _finalResistanceTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // _initialResistanceTextBoxLabel
            // 
            _initialResistanceTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _initialResistanceTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _initialResistanceTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _initialResistanceTextBoxLabel.Location = new System.Drawing.Point(36, 4);
            _initialResistanceTextBoxLabel.Name = "_InitialResistanceTextBoxLabel";
            _initialResistanceTextBoxLabel.Size = new System.Drawing.Size(219, 17);
            _initialResistanceTextBoxLabel.TabIndex = 0;
            _initialResistanceTextBoxLabel.Text = "INITIAL RESISTANCE [Ω]";
            _initialResistanceTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // _outcomePictureBox
            // 
            _outcomePictureBox.BackColor = System.Drawing.Color.Black;
            _outcomePictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            _outcomePictureBox.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( nameof( Ttm.Controls.Properties.Resources.Good ));
            _outcomePictureBox.Location = new System.Drawing.Point(936, 24);
            _outcomePictureBox.Name = "_OutcomePictureBox";
            _outcomePictureBox.Size = new System.Drawing.Size(29, 28);
            _outcomePictureBox.TabIndex = 6;
            _outcomePictureBox.TabStop = false;
            // 
            // _alertsPictureBox
            // 
            _alertsPictureBox.BackColor = System.Drawing.Color.Black;
            _alertsPictureBox.Dock = System.Windows.Forms.DockStyle.Top;
            _alertsPictureBox.Image = ( System.Drawing.Bitmap ) Ttm.Controls.Properties.Resources.ResourceManager.GetObject( nameof( Ttm.Controls.Properties.Resources.Bad ));
            _alertsPictureBox.Location = new System.Drawing.Point(3, 24);
            _alertsPictureBox.Name = "_alertsPictureBox";
            _alertsPictureBox.Size = new System.Drawing.Size(27, 28);
            _alertsPictureBox.TabIndex = 7;
            _alertsPictureBox.TabStop = false;
            // 
            // _outcomeTextBox
            // 
            _outcomeTextBox.BackColor = System.Drawing.Color.Black;
            _outcomeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _outcomeTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _outcomeTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _outcomeTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _outcomeTextBox.Location = new System.Drawing.Point(711, 24);
            _outcomeTextBox.Name = "_OutcomeTextBox";
            _outcomeTextBox.ReadOnly = true;
            _outcomeTextBox.Size = new System.Drawing.Size(219, 32);
            _outcomeTextBox.TabIndex = 5;
            _outcomeTextBox.Text = "COMPLIANCE";
            _outcomeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // _outcomeTextBoxLabel
            // 
            _outcomeTextBoxLabel.AutoSize = true;
            _outcomeTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _outcomeTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _outcomeTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _outcomeTextBoxLabel.Location = new System.Drawing.Point(711, 4);
            _outcomeTextBoxLabel.Name = "_OutcomeTextBoxLabel";
            _outcomeTextBoxLabel.Size = new System.Drawing.Size(219, 17);
            _outcomeTextBoxLabel.TabIndex = 4;
            _outcomeTextBoxLabel.Text = "OUTCOME";
            _outcomeTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            // 
            // MeasurementsHeader
            // 
            BackColor = System.Drawing.Color.Black;
            Controls.Add(_outcomeLayout);
            Name = "MeasurementsHeader";
            Size = new System.Drawing.Size(968, 61);
            _outcomeLayout.ResumeLayout(false);
            _outcomeLayout.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)_outcomePictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)_alertsPictureBox).EndInit();
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel _outcomeLayout;
        private System.Windows.Forms.TextBox _thermalTransientVoltageTextBox;
        private System.Windows.Forms.Label _thermalTransientVoltageTextBoxLabel;
        private System.Windows.Forms.TextBox _finalResistanceTextBox;
        private System.Windows.Forms.TextBox _initialResistanceTextBox;
        private System.Windows.Forms.Label _finalResistanceTextBoxLabel;
        private System.Windows.Forms.Label _initialResistanceTextBoxLabel;
        private System.Windows.Forms.PictureBox _outcomePictureBox;
        private System.Windows.Forms.PictureBox _alertsPictureBox;
        private System.Windows.Forms.TextBox _outcomeTextBox;
        private System.Windows.Forms.Label _outcomeTextBoxLabel;
    }
}
