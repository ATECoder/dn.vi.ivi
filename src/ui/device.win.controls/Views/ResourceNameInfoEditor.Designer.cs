using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls
{
    public partial class ResourceNameInfoEditor
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
            AddResourceButton = new Button();
            ResourceNamesListBox = new ListBox();
            ResourceNameTextBox = new TextBox();
            ResourceNameTextBoxLabel = new Label();
            TestResourceButton = new Button();
            MessagesTextBox = new TextBox();
            LayoutPanel = new TableLayoutPanel();
            ResourcePanel = new Panel();
            BackupButton = new Button();
            RestoreButton = new Button();
            RemoveButton = new Button();
            ResourceFolderLabel = new Label();
            LayoutPanel.SuspendLayout();
            ResourcePanel.SuspendLayout();
            SuspendLayout();
            // 
            // AddResourceButton
            // 
            AddResourceButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AddResourceButton.Location = new System.Drawing.Point(242, 3);
            AddResourceButton.Name = "AddResourceButton";
            AddResourceButton.Size = new System.Drawing.Size(64, 25);
            AddResourceButton.TabIndex = 4;
            AddResourceButton.Text = "&Add";
            AddResourceButton.UseVisualStyleBackColor = true;
            AddResourceButton.Click += new EventHandler( AddResourceButton_Click );
            // 
            // ResourceNamesListBox
            // 
            ResourceNamesListBox.Dock = DockStyle.Fill;
            ResourceNamesListBox.FormattingEnabled = true;
            ResourceNamesListBox.ItemHeight = 17;
            ResourceNamesListBox.Location = new System.Drawing.Point(6, 67);
            ResourceNamesListBox.Name = "ResourceNamesListBox";
            ResourceNamesListBox.Size = new System.Drawing.Size(440, 236);
            ResourceNamesListBox.TabIndex = 4;
            ResourceNamesListBox.SelectedValueChanged += new EventHandler( ResourceNamesListBox_SelectedValueChanged );
            // 
            // ResourceNameTextBox
            // 
            ResourceNameTextBox.Dock = DockStyle.Bottom;
            ResourceNameTextBox.Location = new System.Drawing.Point(0, 30);
            ResourceNameTextBox.Name = "ResourceNameTextBox";
            ResourceNameTextBox.Size = new System.Drawing.Size(440, 25);
            ResourceNameTextBox.TabIndex = 1;
            // 
            // ResourceNameTextBoxLabel
            // 
            ResourceNameTextBoxLabel.AutoSize = true;
            ResourceNameTextBoxLabel.Location = new System.Drawing.Point(3, 11);
            ResourceNameTextBoxLabel.Name = "ResourceNameTextBoxLabel";
            ResourceNameTextBoxLabel.Size = new System.Drawing.Size(65, 17);
            ResourceNameTextBoxLabel.TabIndex = 0;
            ResourceNameTextBoxLabel.Text = "Resource:";
            // 
            // TestResourceButton
            // 
            TestResourceButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            TestResourceButton.Location = new System.Drawing.Point(110, 3);
            TestResourceButton.Name = "TestResourceButton";
            TestResourceButton.Size = new System.Drawing.Size(64, 25);
            TestResourceButton.TabIndex = 2;
            TestResourceButton.Text = "&Test";
            TestResourceButton.UseVisualStyleBackColor = true;
            TestResourceButton.Click += new EventHandler( TestResourceButton_Click );
            // 
            // MessagesTextBox
            // 
            MessagesTextBox.Dock = DockStyle.Top;
            MessagesTextBox.Location = new System.Drawing.Point(6, 309);
            MessagesTextBox.Multiline = true;
            MessagesTextBox.Name = "MessagesTextBox";
            MessagesTextBox.ReadOnly = true;
            MessagesTextBox.ScrollBars = ScrollBars.Both;
            MessagesTextBox.Size = new System.Drawing.Size(440, 123);
            MessagesTextBox.TabIndex = 5;
            MessagesTextBox.Text = "<messages>";
            // 
            // LayoutPanel
            // 
            LayoutPanel.ColumnCount = 3;
            LayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 3.0f));
            LayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100.0f));
            LayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 3.0f));
            LayoutPanel.Controls.Add(ResourcePanel, 1, 1);
            LayoutPanel.Controls.Add(MessagesTextBox, 1, 3);
            LayoutPanel.Controls.Add(ResourceNamesListBox, 1, 2);
            LayoutPanel.Controls.Add(ResourceFolderLabel, 1, 4);
            LayoutPanel.Dock = DockStyle.Fill;
            LayoutPanel.Location = new System.Drawing.Point(0, 0);
            LayoutPanel.Name = "LayoutPanel";
            LayoutPanel.RowCount = 6;
            LayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 3.0f));
            LayoutPanel.RowStyles.Add(new RowStyle());
            LayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100.0f));
            LayoutPanel.RowStyles.Add(new RowStyle());
            LayoutPanel.RowStyles.Add(new RowStyle());
            LayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 3.0f));
            LayoutPanel.Size = new System.Drawing.Size(452, 455);
            LayoutPanel.TabIndex = 6;
            // 
            // ResourcePanel
            // 
            ResourcePanel.Controls.Add(BackupButton);
            ResourcePanel.Controls.Add(RestoreButton);
            ResourcePanel.Controls.Add(RemoveButton);
            ResourcePanel.Controls.Add(AddResourceButton);
            ResourcePanel.Controls.Add(TestResourceButton);
            ResourcePanel.Controls.Add(ResourceNameTextBoxLabel);
            ResourcePanel.Controls.Add(ResourceNameTextBox);
            ResourcePanel.Dock = DockStyle.Top;
            ResourcePanel.Location = new System.Drawing.Point(6, 6);
            ResourcePanel.Name = "ResourcePanel";
            ResourcePanel.Size = new System.Drawing.Size(440, 55);
            ResourcePanel.TabIndex = 7;
            // 
            // BackupButton
            // 
            BackupButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            BackupButton.Location = new System.Drawing.Point(308, 3);
            BackupButton.Name = "BackupButton";
            BackupButton.Size = new System.Drawing.Size(64, 25);
            BackupButton.TabIndex = 6;
            BackupButton.Text = "Backup";
            BackupButton.UseVisualStyleBackColor = true;
            BackupButton.Click += new EventHandler( BackupButton_Click );
            // 
            // RestoreButton
            // 
            RestoreButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RestoreButton.Location = new System.Drawing.Point(374, 3);
            RestoreButton.Name = "RestoreButton";
            RestoreButton.Size = new System.Drawing.Size(64, 25);
            RestoreButton.TabIndex = 5;
            RestoreButton.Text = "Restore";
            RestoreButton.UseVisualStyleBackColor = true;
            RestoreButton.Click += new EventHandler( RestoreButton_Click );
            // 
            // RemoveButton
            // 
            RemoveButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RemoveButton.Location = new System.Drawing.Point(176, 3);
            RemoveButton.Name = "RemoveButton";
            RemoveButton.Size = new System.Drawing.Size(64, 25);
            RemoveButton.TabIndex = 3;
            RemoveButton.Text = "&Remove";
            RemoveButton.UseVisualStyleBackColor = true;
            RemoveButton.Click += new EventHandler( RemoveButton_Click );
            // 
            // ResourceFolderLabel
            // 
            ResourceFolderLabel.AutoSize = true;
            ResourceFolderLabel.Location = new System.Drawing.Point(6, 435);
            ResourceFolderLabel.Name = "ResourceFolderLabel";
            ResourceFolderLabel.Size = new System.Drawing.Size(146, 17);
            ResourceFolderLabel.TabIndex = 8;
            ResourceFolderLabel.Text = "location of resource file";
            // 
            // ResourceNameInfoEditor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7.0f, 17.0f);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(LayoutPanel);
            Name = "ResourceNameInfoEditor";
            Size = new System.Drawing.Size(452, 455);
            LayoutPanel.ResumeLayout(false);
            LayoutPanel.PerformLayout();
            ResourcePanel.ResumeLayout(false);
            ResourcePanel.PerformLayout();
            ResumeLayout(false);
        }

        private Button AddResourceButton;
        private TextBox ResourceNameTextBox;
        private Label ResourceNameTextBoxLabel;
        private Button TestResourceButton;
        private ListBox ResourceNamesListBox;
        private TextBox MessagesTextBox;
        private Panel ResourcePanel;
        private Button RemoveButton;
        private TableLayoutPanel LayoutPanel;
        private Button RestoreButton;
        private Button BackupButton;
        private Label ResourceFolderLabel;
    }
}
