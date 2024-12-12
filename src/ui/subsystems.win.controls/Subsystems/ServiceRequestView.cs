using System;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using cc.isr.Std.SplitExtensions;

namespace cc.isr.VI.SubsystemsWinControls;

/// <summary> A ServiceRequest view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class ServiceRequestView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "CodeQuality", "IDE0079:Remove unnecessary suppression", Justification = "<Pending>" )]
    public ServiceRequestView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this._serviceRequestFlagsComboBox.ComboBox.DataSource = null;
        this._serviceRequestFlagsComboBox.ComboBox.Items.Clear();
#pragma warning disable CA2263
        this._serviceRequestFlagsComboBox.ComboBox.DataSource = Enum.GetNames( typeof( Pith.ServiceRequests ) ).ToList();
#pragma warning restore CA2263
        this._serviceRequestMaskNumeric.NumericUpDown.DecimalPlaces = 0;
        this._serviceRequestMaskNumeric.NumericUpDown.Minimum = 0m;
        this._serviceRequestMaskNumeric.NumericUpDown.Maximum = 255m;
        this._serviceRequestMaskNumeric.NumericUpDown.Hexadecimal = true;
        this.InitializingComponents = false;
    }

    /// <summary> Creates a new <see cref="ServiceRequestView"/> </summary>
    /// <returns> A <see cref="ServiceRequestView"/>. </returns>
    public static ServiceRequestView Create()
    {
        ServiceRequestView? view = null;
        try
        {
            view = new ServiceRequestView();
            return view;
        }
        catch
        {
            view?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the <see cref="Control" />
    /// and its child controls and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources;
    /// <c>false</c> to release only unmanaged
    /// resources when called from the runtime
    /// finalize. </param>
    protected override void Dispose( bool disposing )
    {
        try
        {
            if ( !this.IsDisposed && disposing )
            {
                this.InitializingComponents = true;
                // make sure the device is unbound in case the form is closed without closing the device.
                this.AssignDeviceThis( null );
                if ( this.components is not null )
                {
                    this.components?.Dispose();
                    this.components = null;
                }
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " public members "

    /// <summary> Adds service request mask. </summary>
    public void AddServiceRequestMask()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} adding service request mask";
            Pith.ServiceRequests selectedFlag = ( Pith.ServiceRequests ) ( int ) (this._serviceRequestFlagsComboBox.SelectedItem ?? 0);
            this._serviceRequestMaskNumeric.Value = ( int ) this._serviceRequestMaskNumeric.Value | ( int ) selectedFlag;
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Removes the service request mask. </summary>
    private void RemoveServiceRequestMask()
    {
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device?.ResourceNameCaption} removing a service request mask";
            Pith.ServiceRequests selectedFlag = ( Pith.ServiceRequests ) ( int ) (this._serviceRequestFlagsComboBox.SelectedItem ?? 0);
            this._serviceRequestMaskNumeric.Value = ( int ) this._serviceRequestMaskNumeric.Value & ~( int ) selectedFlag;
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.Cursor = Cursors.Default;
        }
    }

    /// <summary> Toggle end of settling request enabled. </summary>
    private void ToggleEndOfSettlingRequestEnabled()
    {
        if ( this.InitializingComponents || this.Device is null ) return;
        string activity = string.Empty;
        try
        {
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            activity = $"{this.Device.ResourceNameCaption} enabling end of settling time request";
            this.Cursor = Cursors.WaitCursor;
            this.InfoProvider?.Clear();
            this.Device.StatusSubsystemBase?.ToggleEndOfScanService( this._serviceRequestEnabledMenuItem.Checked, this._usingNegativeTransitionsMenuItem.Checked, ( Pith.ServiceRequests ) ( int ) this._serviceRequestMaskNumeric.Value );
        }
        catch ( Exception ex )
        {
            if ( this.Device?.Session is not null )
                this.Device.Session.StatusPrompt = $"failed {activity}";
            activity = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
            _ = this.InfoProvider?.Annunciate( this._subsystemToolStrip, cc.isr.WinControls.InfoProviderLevel.Error, activity );
        }
        finally
        {
            this.ReadStatusRegister();
            this.Cursor = Cursors.Default;
        }
    }

    #endregion

    #region " device "

    /// <summary> The device. </summary>

    /// <summary> Gets the device. </summary>
    /// <value> The device. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    public VisaSessionBase? Device { get; private set; }

    /// <summary> Assigns the device and binds the relevant subsystem values. </summary>
    /// <param name="value"> The value. </param>
    [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
    private void AssignDeviceThis( VisaSessionBase? value )
    {
        if ( this.Device is not null )
        {
            this.Device = null;
        }

        this.Device = value;
        if ( value is not null )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogVerbose( $"{value.ResourceNameCaption} assigned to {nameof( ServiceRequestView ).SplitWords()}" );
        }
    }

    /// <summary> Assigns a device. </summary>
    /// <param name="value"> True to show or False to hide the control. </param>
    public void AssignDevice( VisaSessionBase? value )
    {
        this.AssignDeviceThis( value );
    }

    /// <summary> Reads the status register and lets the session process the status byte. </summary>
    protected void ReadStatusRegister()
    {
        if ( this.Device is null || this.Device.Session is null ) return;
        string activity = $"{this.Device.ResourceNameCaption} reading service request";
        try
        {
            this.Device.Session.ApplyStatusByte( this.Device.Session.ReadStatusByte() );
        }
        catch ( Exception ex )
        {
            _ = cc.isr.VI.SessionLogger.Instance.LogException( ex, activity );
        }
    }

    #endregion

    #region " control event handlers: srq "

    /// <summary> Event handler. Called by ServiceRequestMaskAddButton for click events. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ServiceRequestMaskAddButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.AddServiceRequestMask();
    }

    /// <summary>
    /// Event handler. Called by ServiceRequestMaskRemoveButton for click events.
    /// </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ServiceRequestMaskRemoveButton_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.RemoveServiceRequestMask();
    }

    /// <summary> Toggle service request menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void ToggleServiceRequestMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this.ToggleEndOfSettlingRequestEnabled();
    }

    /// <summary> Hexadecimal tool strip menu item click. </summary>
    /// <param name="sender"> Source of the event. </param>
    /// <param name="e">      Event information. </param>
    private void HexadecimalToolStripMenuItem_Click( object? sender, EventArgs e )
    {
        if ( this.InitializingComponents || sender is null || e is null ) return;
        this._serviceRequestMaskNumeric.NumericUpDown.Hexadecimal = this._hexadecimalToolStripMenuItem.Checked;
    }

    #endregion
}
