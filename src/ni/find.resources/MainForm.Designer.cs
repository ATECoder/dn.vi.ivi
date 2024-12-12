using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NI.FindResources;

public partial class MainForm
{
    private readonly TreeNode _gpibTreeNode;
    private readonly TreeNode _vxiTreeNode;
    private readonly TreeNode _serialTreeNode;
    private readonly TreeNode _pxiTreeNode;
    private readonly TreeNode _tcpIpTreeNode;
    private readonly TreeNode _uSBTreeNode;

    /// <summary>
    /// Required designer variable.
    /// </summary>
    private readonly System.ComponentModel.Container _components = null;

    private System.Windows.Forms.Button _useCustomStringButton;
    private System.Windows.Forms.Label _filterStringLabel;
    private System.Windows.Forms.Button _findResourcesButton;
    private System.Windows.Forms.Button _clearButton;
    private System.Windows.Forms.Label _availableResourcesLabel;
    private System.Windows.Forms.ListBox _filterStringsListBox;
    private System.Windows.Forms.TreeView _resourceTreeView;

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        this._availableResourcesLabel = new System.Windows.Forms.Label();
        this._resourceTreeView = new System.Windows.Forms.TreeView();
        this._findResourcesButton = new System.Windows.Forms.Button();
        this._filterStringsListBox = new System.Windows.Forms.ListBox();
        this._filterStringLabel = new System.Windows.Forms.Label();
        this._clearButton = new System.Windows.Forms.Button();
        this._useCustomStringButton = new System.Windows.Forms.Button();
        this.SuspendLayout();
        //
        // availableResourcesLabel
        //
        this._availableResourcesLabel.Location = new System.Drawing.Point( 16, 213 );
        this._availableResourcesLabel.Name = "availableResourcesLabel";
        this._availableResourcesLabel.Size = new System.Drawing.Size( 152, 16 );
        this._availableResourcesLabel.TabIndex = 0;
        this._availableResourcesLabel.Text = "Available Resources Found:";
        //
        // resourceTreeView
        //
        this._resourceTreeView.Anchor = (( System.Windows.Forms.AnchorStyles ) ((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
        | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this._resourceTreeView.Location = new System.Drawing.Point( 16, 232 );
        this._resourceTreeView.Name = "resourceTreeView";
        this._resourceTreeView.Size = new System.Drawing.Size( 248, 136 );
        this._resourceTreeView.TabIndex = 5;
        //
        // findResourcesButton
        //
        this._findResourcesButton.Location = new System.Drawing.Point( 16, 168 );
        this._findResourcesButton.Name = "findResourcesButton";
        this._findResourcesButton.Size = new System.Drawing.Size( 130, 23 );
        this._findResourcesButton.TabIndex = 8;
        this._findResourcesButton.Text = "Find Resources";
        this._findResourcesButton.Click += new System.EventHandler( this.FindResourcesButton_Click );
        //
        // filterStringsListBox
        //
        this._filterStringsListBox.Anchor = (( System.Windows.Forms.AnchorStyles ) (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
        | System.Windows.Forms.AnchorStyles.Right)));
        this._filterStringsListBox.Location = new System.Drawing.Point( 16, 40 );
        this._filterStringsListBox.Name = "filterStringsListBox";
        this._filterStringsListBox.Size = new System.Drawing.Size( 248, 121 );
        this._filterStringsListBox.TabIndex = 9;
        //
        // filterStringLabel
        //
        this._filterStringLabel.Location = new System.Drawing.Point( 16, 24 );
        this._filterStringLabel.Name = "filterStringLabel";
        this._filterStringLabel.Size = new System.Drawing.Size( 72, 16 );
        this._filterStringLabel.TabIndex = 10;
        this._filterStringLabel.Text = "Filter String:";
        //
        // clearButton
        //
        this._clearButton.Anchor = (( System.Windows.Forms.AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._clearButton.Location = new System.Drawing.Point( 152, 168 );
        this._clearButton.Name = "clearButton";
        this._clearButton.Size = new System.Drawing.Size( 112, 24 );
        this._clearButton.TabIndex = 11;
        this._clearButton.Text = "Clear";
        this._clearButton.Click += new System.EventHandler( this.ClearButton_Click );
        //
        // useCustomStringButton
        //
        this._useCustomStringButton.Anchor = (( System.Windows.Forms.AnchorStyles ) ((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
        this._useCustomStringButton.Location = new System.Drawing.Point( 152, 8 );
        this._useCustomStringButton.Name = "useCustomStringButton";
        this._useCustomStringButton.Size = new System.Drawing.Size( 112, 24 );
        this._useCustomStringButton.TabIndex = 12;
        this._useCustomStringButton.Text = "Use Custom String";
        this._useCustomStringButton.Click += new System.EventHandler( this.UseCustomStringButton_Click );
        //
        // MainForm
        //
        this.AutoScaleBaseSize = new System.Drawing.Size( 5, 13 );
        this.ClientSize = new System.Drawing.Size( 280, 373 );
        this.Controls.Add( this._useCustomStringButton );
        this.Controls.Add( this._clearButton );
        this.Controls.Add( this._filterStringLabel );
        this.Controls.Add( this._filterStringsListBox );
        this.Controls.Add( this._findResourcesButton );
        this.Controls.Add( this._resourceTreeView );
        this.Controls.Add( this._availableResourcesLabel );
        this.MaximizeBox = false;
        this.MinimumSize = new System.Drawing.Size( 288, 400 );
        this.Name = "MainForm";
        this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
        this.Text = "Available Resources List";
        this.ResumeLayout( false );

    }

}
