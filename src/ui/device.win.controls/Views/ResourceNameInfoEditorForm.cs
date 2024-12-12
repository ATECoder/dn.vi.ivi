using System;
using System.Windows.Forms;

namespace cc.isr.VI.DeviceWinControls;

/// <summary> Form for editing the resource name Information. </summary>
/// <remarks>
/// David, 2020-06-07. (c) 2020 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para>
/// </remarks>
public partial class ResourceNameInfoEditorForm : System.Windows.Forms.Form
{
    #region " construction and clean up "

    /// <summary> Default constructor. </summary>
    public ResourceNameInfoEditorForm() : base()
    {
        this.InitializeComponent();
        this.Name = "Resource.Editor.Form";
        this.Text = "Resource Names Editor";
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Control" />
    /// and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( !this.IsDisposed && disposing )
            {
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " form events "

    /// <summary>
    /// Called upon receiving the <see cref="System.Windows.Forms.Form.Load" /> event.
    /// </summary>
    /// <param name="e"> An <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnLoad( EventArgs e )
    {
        try
        {
        }
        catch ( Exception )
        {
        }
        finally
        {
            base.OnLoad( e );
        }
    }

    /// <summary> Gets or sets the await the resource name validation task enabled. </summary>
    /// <value> The await the resource name validation task enabled. </value>
    protected bool AwaitResourceNameValidationTaskEnabled { get; set; }

    /// <summary>
    /// Called upon receiving the <see cref="System.Windows.Forms.Form.Shown" /> event.
    /// </summary>
    /// <param name="e"> A <see cref="EventArgs" /> that contains the event data. </param>
    protected override void OnShown( EventArgs e )
    {
        try
        {
            this.Cursor = Cursors.WaitCursor;
            base.OnShown( e );
        }
        catch ( Exception )
        {
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

}
