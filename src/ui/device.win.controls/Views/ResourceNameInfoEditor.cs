using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls;

/// <summary> Editor for resource name Information. </summary>
/// <remarks>
/// David, 2020-06-07. (c) 2020 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para>
/// </remarks>
public partial class ResourceNameInfoEditor : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    public ResourceNameInfoEditor() : base()
    {
        this.InitializingComponents = true;
        this.InitializeComponent();
        this.InitializingComponents = false;
        this.ResourceNameInformations = [];
        this.ResourceFolderLabel.Text = $"Folder: {this.ResourceNameInformations.DefaultFolderName}";
        this.ToolTip?.SetToolTip( this.ResourceNamesListBox, "Click to select a resource" );
        this.ToolTip?.SetToolTip( this.ResourceNameTextBox, "Enter or select a resource from the list" );
        this.ToolTip?.SetToolTip( this.TestResourceButton, "Tests is a session can open to an instrument using the selected resource" );
        this.ToolTip?.SetToolTip( this.AddResourceButton, "Adds a resource" );
        this.ToolTip?.SetToolTip( this.RemoveButton, "Removes a resource" );
        this.ToolTip?.SetToolTip( this.BackupButton, "Saves the resource list to a backup file" );
        this.ToolTip?.SetToolTip( this.RestoreButton, "Restores the resource list from the backup file" );
        this.AddResourceButton.Name = "AddResourceButton";
        this.ResourceNamesListBox.Name = "ResourceNamesListBox";
        this.TestResourceButton.Name = "TestResourceButton";
        this.BackupButton.Name = "BackupButton";
        this.RestoreButton.Name = "RestoreButton";
        this.RemoveButton.Name = "RemoveButton";
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Control" />
    /// and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( !this.IsDisposed && disposing )
            {
                this.InitializingComponents = true;
                if ( this.components is not null )
                    this.components?.Dispose();
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " form events "

    /// <summary> Handles the <see cref="System.Windows.Forms.UserControl.Load" /> event. </summary>
    /// <param name="e"> An <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnLoad( EventArgs e )
    {
        try
        {
            this.ResourceNameInformations.ReadResources();
            this.ResourceNamesListBox.DataSource = this.ResourceNameInformations.ResourceNames;
        }
        finally
        {
            base.OnLoad( e );
        }
    }

    /// <summary> Tests resource button click. </summary>
    /// <remarks> David, 2020-06-07. </remarks>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void TestResourceButton_Click( object? sender, EventArgs e )
    {
        string resourceName = this.ResourceNameTextBox.Text;
        if ( string.IsNullOrEmpty( resourceName ) )
        {
            this.MessagesTextBox.Text = "Empty resource name. Please enter a resource name in the text box";
        }
        else
        {
            using Pith.SessionBase rm = SessionFactory.Instance.Factory.Session();
            (bool success, string details) = rm.CanCreateSession( resourceName, TimeSpan.FromMilliseconds( 250d ) );
            this.MessagesTextBox.Text = success ? $"Success! Resource {resourceName} found" : $@"Failure! Resource {resourceName} not found
{details}";
        }
    }

    /// <summary> Referesh list. </summary>
    private void RefereshList()
    {
        this.ResourceNamesListBox.DataSource = null;
        this.ResourceNamesListBox.Items.Clear();
        this.ResourceNamesListBox.DataSource = this.ResourceNameInformations.ResourceNames;
    }

    /// <summary> Removes the button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void RemoveButton_Click( object? sender, EventArgs e )
    {
        string resourceName = this.ResourceNameTextBox.Text;
        if ( this.ResourceNameInformations.Contains( resourceName ) )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.ResourceNameInformations.Remove( resourceName );
                this.ResourceNameInformations.WriteResources();
                this.RefereshList();
                this.MessagesTextBox.Text = $"{resourceName} was removed";
            }
            catch ( Exception ex )
            {
                this.MessagesTextBox.Text = ex.ToString();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }

    /// <summary> Adds a resource button click to 'e'. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void AddResourceButton_Click( object? sender, EventArgs e )
    {
        string resourceName = this.ResourceNameTextBox.Text;
        if ( !this.ResourceNameInformations.Contains( resourceName ) )
        {
            try
            {
                this.Cursor = Cursors.WaitCursor;
                this.ResourceNameInformations.Add( resourceName );
                this.ResourceNameInformations.WriteResources();
                this.RefereshList();
                this.MessagesTextBox.Text = $"{resourceName} was added";
            }
            catch ( Exception ex )
            {
                this.MessagesTextBox.Text = ex.ToString();
            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
    }

    /// <summary> Resource names list box selected value changed. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ResourceNamesListBox_SelectedValueChanged( object? sender, EventArgs e )
    {
        if ( !this.InitializingComponents )
        {
            this.ResourceNameTextBox.Text = this.ResourceNamesListBox.SelectedItem?.ToString();
        }
    }

    /// <summary> Restore button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void RestoreButton_Click( object? sender, EventArgs e )
    {
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.ResourceNameInformations.RestoreResources();
            this.ResourceNameInformations.WriteResources();
            this.RefereshList();
            this.MessagesTextBox.Text = $"Resources restored from backup {this.ResourceNameInformations.BackupFullFileName}";
        }
        catch ( Exception ex )
        {
            this.MessagesTextBox.Text = ex.ToString();
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Backup button click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void BackupButton_Click( object? sender, EventArgs e )
    {
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.ResourceNameInformations.BackupResources();
            this.MessagesTextBox.Text = $"Resources backed up to {this.ResourceNameInformations.BackupFullFileName}";
        }
        catch ( Exception ex )
        {
            this.MessagesTextBox.Text = ex.ToString();
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " resource name info "

    /// <summary> Gets or sets the resource name information collection. </summary>
    /// <value> The resource name information collection. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    public Pith.ResourceNameInfoCollection ResourceNameInformations { get; private set; }

    #endregion

}
