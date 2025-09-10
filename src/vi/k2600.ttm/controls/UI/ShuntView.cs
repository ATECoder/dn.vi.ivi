using System;
using System.ComponentModel;

namespace cc.isr.VI.Tsp.K2600.Ttm.Controls;

/// <summary> A shunt view. </summary>
/// <remarks>
/// (c) 2018 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2018-12-31 </para>
/// </remarks>
public partial class ShuntView : cc.isr.WinControls.ModelViewBase
{
    #region " construction and cleanup "

    /// <summary> Default constructor. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    public ShuntView() : base()
    {
        this.InitializingComponents = true;
        // This call is required by the Windows Form Designer.
        this.InitializeComponent();
        this.InitializingComponents = false;
    }

    /// <summary> Creates a new <see cref="ShuntView"/> </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <returns> A <see cref="ShuntView"/>. </returns>
    public static ShuntView Create()
    {
        ShuntView? view = null;
        try
        {
            view = new ShuntView();
            return view;
        }
        catch
        {
            view?.Dispose();
            throw;
        }
    }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="disposing"> <c>true</c> to release both managed and unmanaged resources;
    ///                                                   <c>false</c> to release only unmanaged
    ///                                                   resources when called from the runtime
    ///                                                   finalize. </param>
    protected override void Dispose( bool disposing )
    {
        if ( this.IsDisposed )
            return;
        try
        {
            if ( disposing )
            {
                this.InitializingComponents = true;
                // make sure the device is unbound in case the form is closed without closing the device.
                this.AssignDeviceThis( null );
                this.components?.Dispose();
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #region " device "

    /// <summary> Gets the device. </summary>
    /// <value> The device. </value>
    [DesignerSerializationVisibility( DesignerSerializationVisibility.Hidden )]
    [Browsable( false )]
    [CLSCompliant( false )]
    public K2600Device? Device { get; private set; }

    /// <summary> Assigns the device and binds the relevant subsystem values. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> The value. </param>
    [System.Runtime.CompilerServices.MethodImpl( System.Runtime.CompilerServices.MethodImplOptions.Synchronized )]
    private void AssignDeviceThis( K2600Device? value )
    {
        if ( this.Device is not null )
            this.Device = null;
        this.Device = value;
    }

    /// <summary> Assigns a device. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    /// <param name="value"> True to show or False to hide the control. </param>
    public void AssignDevice( K2600Device value )
    {
        this.AssignDeviceThis( value );
    }

    /// <summary> Reads the status register and lets the session process the status byte. </summary>
    /// <remarks> David, 2020-10-12. </remarks>
    protected void ReadStatusRegister()
    {
        if ( this.Device is null ) throw new InvalidOperationException( $"{nameof( ShuntView )}.{nameof( ShuntView.Device )} is null." );
        if ( this.Device.Session is null ) throw new InvalidOperationException( $"{nameof( ShuntView )}.{nameof( ShuntView.Device )}.{nameof( ShuntView.Device.Session )} is null." );

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
}
