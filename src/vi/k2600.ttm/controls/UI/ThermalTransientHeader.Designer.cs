using System.Diagnostics;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls
{
    public partial class ThermalTransientHeader
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
                    if (disposing)
                    {
                        components?.Dispose();
                    }
                }
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
            _outcomeLayout = new System.Windows.Forms.TableLayoutPanel();
            _asymptoteTextBox = new System.Windows.Forms.TextBox();
            _thermalTransientVoltageTextBoxLabel = new System.Windows.Forms.Label();
            _estimatedVoltageTextBox = new System.Windows.Forms.TextBox();
            _estimatedVoltageTextBoxLabel = new System.Windows.Forms.Label();
            _timeConstantTextBoxLabel = new System.Windows.Forms.Label();
            _timeConstantTextBox = new System.Windows.Forms.TextBox();
            _correlationCoefficientTextBoxLabel = new System.Windows.Forms.Label();
            _correlationCoefficientTextBox = new System.Windows.Forms.TextBox();
            _standardErrorTextBox = new System.Windows.Forms.TextBox();
            _iterationsCountTextBox = new System.Windows.Forms.TextBox();
            _outcomeTextBox = new System.Windows.Forms.TextBox();
            _standardErrorTextBoxLabel = new System.Windows.Forms.Label();
            _iterationsCountTextBoxLabel = new System.Windows.Forms.Label();
            _outcomeTextBoxLabel = new System.Windows.Forms.Label();
            _outcomeLayout.SuspendLayout();
            SuspendLayout();
            //
            // _outcomeLayout
            //
            _outcomeLayout.BackColor = System.Drawing.Color.Transparent;
            _outcomeLayout.ColumnCount = 7;
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667f));
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667f));
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667f));
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667f));
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667f));
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 16.66667f));
            _outcomeLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            _outcomeLayout.Controls.Add(_asymptoteTextBox, 1, 2);
            _outcomeLayout.Controls.Add(_thermalTransientVoltageTextBoxLabel, 1, 1);
            _outcomeLayout.Controls.Add(_estimatedVoltageTextBox, 2, 2);
            _outcomeLayout.Controls.Add(_estimatedVoltageTextBoxLabel, 2, 1);
            _outcomeLayout.Controls.Add(_timeConstantTextBoxLabel, 0, 1);
            _outcomeLayout.Controls.Add(_timeConstantTextBox, 0, 2);
            _outcomeLayout.Controls.Add(_correlationCoefficientTextBoxLabel, 3, 1);
            _outcomeLayout.Controls.Add(_correlationCoefficientTextBox, 3, 2);
            _outcomeLayout.Controls.Add(_standardErrorTextBox, 4, 2);
            _outcomeLayout.Controls.Add(_iterationsCountTextBox, 5, 2);
            _outcomeLayout.Controls.Add(_outcomeTextBox, 6, 2);
            _outcomeLayout.Controls.Add(_standardErrorTextBoxLabel, 4, 1);
            _outcomeLayout.Controls.Add(_iterationsCountTextBoxLabel, 5, 1);
            _outcomeLayout.Controls.Add(_outcomeTextBoxLabel, 6, 1);
            _outcomeLayout.Dock = System.Windows.Forms.DockStyle.Top;
            _outcomeLayout.ForeColor = System.Drawing.SystemColors.ButtonFace;
            _outcomeLayout.Location = new System.Drawing.Point(0, 0);
            _outcomeLayout.Name = "_OutcomeLayout";
            _outcomeLayout.RowCount = 3;
            _outcomeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0f));
            _outcomeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _outcomeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle());
            _outcomeLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0f));
            _outcomeLayout.Size = new System.Drawing.Size(968, 65);
            _outcomeLayout.TabIndex = 1;
            //
            // _asymptoteTextBox
            //
            _asymptoteTextBox.BackColor = System.Drawing.Color.Black;
            _asymptoteTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _asymptoteTextBox.CausesValidation = false;
            _asymptoteTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _asymptoteTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _asymptoteTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _asymptoteTextBox.Location = new System.Drawing.Point(137, 23);
            _asymptoteTextBox.Name = "_asymptoteTextBox";
            _asymptoteTextBox.ReadOnly = true;
            _asymptoteTextBox.Size = new System.Drawing.Size(128, 32);
            _asymptoteTextBox.TabIndex = 3;
            _asymptoteTextBox.Text = "0.0";
            _asymptoteTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_asymptoteTextBox, "Asymptotic voltage");
            //
            // _thermalTransientVoltageTextBoxLabel
            //
            _thermalTransientVoltageTextBoxLabel.AutoSize = true;
            _thermalTransientVoltageTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _thermalTransientVoltageTextBoxLabel.CausesValidation = false;
            _thermalTransientVoltageTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _thermalTransientVoltageTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _thermalTransientVoltageTextBoxLabel.Location = new System.Drawing.Point(137, 3);
            _thermalTransientVoltageTextBoxLabel.Name = "_ThermalTransientVoltageTextBoxLabel";
            _thermalTransientVoltageTextBoxLabel.Size = new System.Drawing.Size(128, 17);
            _thermalTransientVoltageTextBoxLabel.TabIndex = 2;
            _thermalTransientVoltageTextBoxLabel.Text = "V ∞ [mV]";
            _thermalTransientVoltageTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //
            // _estimatedVoltageTextBox
            //
            _estimatedVoltageTextBox.BackColor = System.Drawing.Color.Black;
            _estimatedVoltageTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _estimatedVoltageTextBox.CausesValidation = false;
            _estimatedVoltageTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _estimatedVoltageTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _estimatedVoltageTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _estimatedVoltageTextBox.Location = new System.Drawing.Point(271, 23);
            _estimatedVoltageTextBox.Name = "_EstimatedVoltageTextBox";
            _estimatedVoltageTextBox.ReadOnly = true;
            _estimatedVoltageTextBox.Size = new System.Drawing.Size(128, 32);
            _estimatedVoltageTextBox.TabIndex = 5;
            _estimatedVoltageTextBox.Text = "0.0";
            _estimatedVoltageTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_estimatedVoltageTextBox, "Voltage at the end of the pulse");
            //
            // _estimatedVoltageTextBoxLabel
            //
            _estimatedVoltageTextBoxLabel.AutoSize = true;
            _estimatedVoltageTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _estimatedVoltageTextBoxLabel.CausesValidation = false;
            _estimatedVoltageTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _estimatedVoltageTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _estimatedVoltageTextBoxLabel.Location = new System.Drawing.Point(271, 3);
            _estimatedVoltageTextBoxLabel.Name = "_EstimatedVoltageTextBoxLabel";
            _estimatedVoltageTextBoxLabel.Size = new System.Drawing.Size(128, 17);
            _estimatedVoltageTextBoxLabel.TabIndex = 4;
            _estimatedVoltageTextBoxLabel.Text = "V [mV]";
            _estimatedVoltageTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //
            // _timeConstantTextBoxLabel
            //
            _timeConstantTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _timeConstantTextBoxLabel.CausesValidation = false;
            _timeConstantTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _timeConstantTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _timeConstantTextBoxLabel.Location = new System.Drawing.Point(3, 3);
            _timeConstantTextBoxLabel.Name = "_TimeConstantTextBoxLabel";
            _timeConstantTextBoxLabel.Size = new System.Drawing.Size(128, 17);
            _timeConstantTextBoxLabel.TabIndex = 0;
            _timeConstantTextBoxLabel.Text = "TC [ms]";
            _timeConstantTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //
            // _timeConstantTextBox
            //
            _timeConstantTextBox.BackColor = System.Drawing.Color.Black;
            _timeConstantTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _timeConstantTextBox.CausesValidation = false;
            _timeConstantTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _timeConstantTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _timeConstantTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _timeConstantTextBox.Location = new System.Drawing.Point(3, 23);
            _timeConstantTextBox.Name = "_TimeConstantTextBox";
            _timeConstantTextBox.ReadOnly = true;
            _timeConstantTextBox.Size = new System.Drawing.Size(128, 32);
            _timeConstantTextBox.TabIndex = 1;
            _timeConstantTextBox.Text = "0.00";
            _timeConstantTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_timeConstantTextBox, "Time constant");
            //
            // _correlationCoefficientTextBoxLabel
            //
            _correlationCoefficientTextBoxLabel.AutoSize = true;
            _correlationCoefficientTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _correlationCoefficientTextBoxLabel.CausesValidation = false;
            _correlationCoefficientTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _correlationCoefficientTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _correlationCoefficientTextBoxLabel.Location = new System.Drawing.Point(405, 3);
            _correlationCoefficientTextBoxLabel.Name = "_CorrelationCoefficientTextBoxLabel";
            _correlationCoefficientTextBoxLabel.Size = new System.Drawing.Size(128, 17);
            _correlationCoefficientTextBoxLabel.TabIndex = 4;
            _correlationCoefficientTextBoxLabel.Text = "R";
            _correlationCoefficientTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //
            // _correlationCoefficientTextBox
            //
            _correlationCoefficientTextBox.BackColor = System.Drawing.Color.Black;
            _correlationCoefficientTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _correlationCoefficientTextBox.CausesValidation = false;
            _correlationCoefficientTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _correlationCoefficientTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _correlationCoefficientTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _correlationCoefficientTextBox.Location = new System.Drawing.Point(405, 23);
            _correlationCoefficientTextBox.Name = "_CorrelationCoefficientTextBox";
            _correlationCoefficientTextBox.ReadOnly = true;
            _correlationCoefficientTextBox.Size = new System.Drawing.Size(128, 32);
            _correlationCoefficientTextBox.TabIndex = 8;
            _correlationCoefficientTextBox.Text = "0.0000";
            _correlationCoefficientTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_correlationCoefficientTextBox, "Correlation coefficient.");
            //
            // _standardErrorTextBox
            //
            _standardErrorTextBox.BackColor = System.Drawing.Color.Black;
            _standardErrorTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _standardErrorTextBox.CausesValidation = false;
            _standardErrorTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _standardErrorTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _standardErrorTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _standardErrorTextBox.Location = new System.Drawing.Point(539, 23);
            _standardErrorTextBox.Name = "_StandardErrorTextBox";
            _standardErrorTextBox.ReadOnly = true;
            _standardErrorTextBox.Size = new System.Drawing.Size(128, 32);
            _standardErrorTextBox.TabIndex = 8;
            _standardErrorTextBox.Text = "0.00";
            _standardErrorTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_standardErrorTextBox, "Standard Error");
            //
            // _iterationsCountTextBox
            //
            _iterationsCountTextBox.BackColor = System.Drawing.Color.Black;
            _iterationsCountTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _iterationsCountTextBox.CausesValidation = false;
            _iterationsCountTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _iterationsCountTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _iterationsCountTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _iterationsCountTextBox.Location = new System.Drawing.Point(673, 23);
            _iterationsCountTextBox.Name = "_IterationsCountTextBox";
            _iterationsCountTextBox.ReadOnly = true;
            _iterationsCountTextBox.Size = new System.Drawing.Size(128, 32);
            _iterationsCountTextBox.TabIndex = 8;
            _iterationsCountTextBox.Text = "0";
            _iterationsCountTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            ToolTip.SetToolTip(_iterationsCountTextBox, "Number of iterations required to estimate the best model");
            //
            // _outcomeTextBox
            //
            _outcomeTextBox.BackColor = System.Drawing.Color.Black;
            _outcomeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            _outcomeTextBox.CausesValidation = false;
            _outcomeTextBox.Dock = System.Windows.Forms.DockStyle.Bottom;
            _outcomeTextBox.Font = new System.Drawing.Font("Segoe UI", 18.0f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            _outcomeTextBox.ForeColor = System.Drawing.Color.Aquamarine;
            _outcomeTextBox.Location = new System.Drawing.Point(807, 23);
            _outcomeTextBox.Name = "_OutcomeTextBox";
            _outcomeTextBox.ReadOnly = true;
            _outcomeTextBox.Size = new System.Drawing.Size(158, 32);
            _outcomeTextBox.TabIndex = 8;
            _outcomeTextBox.Text = "OPTIMIZED";
            _outcomeTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            //
            // _standardErrorTextBoxLabel
            //
            _standardErrorTextBoxLabel.AutoSize = true;
            _standardErrorTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _standardErrorTextBoxLabel.CausesValidation = false;
            _standardErrorTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _standardErrorTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _standardErrorTextBoxLabel.Location = new System.Drawing.Point(539, 3);
            _standardErrorTextBoxLabel.Name = "_StandardErrorTextBoxLabel";
            _standardErrorTextBoxLabel.Size = new System.Drawing.Size(128, 17);
            _standardErrorTextBoxLabel.TabIndex = 4;
            _standardErrorTextBoxLabel.Text = "σ [mV]";
            _standardErrorTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            ToolTip.SetToolTip(_standardErrorTextBoxLabel, "Standard Error");
            //
            // _iterationsCountTextBoxLabel
            //
            _iterationsCountTextBoxLabel.AutoSize = true;
            _iterationsCountTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _iterationsCountTextBoxLabel.CausesValidation = false;
            _iterationsCountTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _iterationsCountTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _iterationsCountTextBoxLabel.Location = new System.Drawing.Point(673, 3);
            _iterationsCountTextBoxLabel.Name = "_IterationsCountTextBoxLabel";
            _iterationsCountTextBoxLabel.Size = new System.Drawing.Size(128, 17);
            _iterationsCountTextBoxLabel.TabIndex = 4;
            _iterationsCountTextBoxLabel.Text = "N";
            _iterationsCountTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //
            // _outcomeTextBoxLabel
            //
            _outcomeTextBoxLabel.AutoSize = true;
            _outcomeTextBoxLabel.BackColor = System.Drawing.Color.Black;
            _outcomeTextBoxLabel.CausesValidation = false;
            _outcomeTextBoxLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            _outcomeTextBoxLabel.ForeColor = System.Drawing.Color.Yellow;
            _outcomeTextBoxLabel.Location = new System.Drawing.Point(807, 3);
            _outcomeTextBoxLabel.Name = "_OutcomeTextBoxLabel";
            _outcomeTextBoxLabel.Size = new System.Drawing.Size(158, 17);
            _outcomeTextBoxLabel.TabIndex = 4;
            _outcomeTextBoxLabel.Text = "MeasurementOutcome";
            _outcomeTextBoxLabel.TextAlign = System.Drawing.ContentAlignment.BottomCenter;
            //
            // ThermalTransientHeader
            //
            BackColor = System.Drawing.Color.Black;
            Controls.Add(_outcomeLayout);
            Name = "ThermalTransientHeader";
            Size = new System.Drawing.Size(968, 59);
            _outcomeLayout.ResumeLayout(false);
            _outcomeLayout.PerformLayout();
            ResumeLayout(false);
        }

        private System.Windows.Forms.TableLayoutPanel _outcomeLayout;
        private System.Windows.Forms.TextBox _asymptoteTextBox;
        private System.Windows.Forms.Label _thermalTransientVoltageTextBoxLabel;
        private System.Windows.Forms.TextBox _estimatedVoltageTextBox;
        private System.Windows.Forms.TextBox _timeConstantTextBox;
        private System.Windows.Forms.Label _estimatedVoltageTextBoxLabel;
        private System.Windows.Forms.Label _timeConstantTextBoxLabel;
        private System.Windows.Forms.Label _correlationCoefficientTextBoxLabel;
        private System.Windows.Forms.TextBox _correlationCoefficientTextBox;
        private System.Windows.Forms.TextBox _standardErrorTextBox;
        private System.Windows.Forms.TextBox _iterationsCountTextBox;
        private System.Windows.Forms.TextBox _outcomeTextBox;
        private System.Windows.Forms.Label _standardErrorTextBoxLabel;
        private System.Windows.Forms.Label _iterationsCountTextBoxLabel;
        private System.Windows.Forms.Label _outcomeTextBoxLabel;
    }
}
