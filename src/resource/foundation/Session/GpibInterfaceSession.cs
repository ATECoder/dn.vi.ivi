using System.Diagnostics;

namespace cc.isr.VI.Foundation;

/// <summary> A gpib interface session. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-21 </para>
/// </remarks>
[CLSCompliant( false )]
public class GpibInterfaceSession : Pith.InterfaceSessionBase
{
    #region " constructor "

    /// <summary> Constructor. </summary>
    public GpibInterfaceSession() : base()
    {
    }

    #region " disposable support"

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    [DebuggerNonUserCode()]
    protected override void Dispose( bool disposing )
    {
        if ( this.IsDisposed ) return;
        try
        {
            if ( disposing )
            {
                try
                {
                    this.CloseSessionThis();
                }
                catch ( Exception ex )
                {
                    Debug.Assert( !Debugger.IsAttached, "Failed discarding enabled events.", $"Failed discarding enabled events. {ex}" );
                }
            }
        }
        finally
        {
            base.Dispose( disposing );
        }
    }

    #endregion

    #endregion

    #region " session "

    /// <summary> Gets the sentinel indicating whether this is a dummy session. </summary>
    /// <value> The dummy sentinel. </value>
    public override bool IsDummy { get; } = false;

    /// <summary> Gets the gpib interface. </summary>
    /// <value> The gpib interface. </value>
    private Ivi.Visa.IGpibInterfaceSession? GpibInterface { get; set; }

    /// <summary> Opens a session. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="timeout">      The timeout. </param>
    public override void OpenSession( string resourceName, TimeSpan timeout )
    {
        _ = this.OpenSessionThis( resourceName, timeout );
        base.OpenSession( resourceName, timeout );
    }

    /// <summary> Opens a session. </summary>
    /// <remarks> David, 2020-04-10. </remarks>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="timeout">      The timeout. </param>
    /// <returns> The Ivi.Visa.ResourceOpenStatus. </returns>
    private Ivi.Visa.ResourceOpenStatus OpenSessionThis( string? resourceName, TimeSpan timeout )
    {
        if ( resourceName == null ) throw new ArgumentNullException( nameof( resourceName ) );
        this.GpibInterface = Ivi.Visa.GlobalResourceManager.Open( resourceName, Ivi.Visa.AccessModes.None, ( int ) timeout.TotalMilliseconds,
            out Ivi.Visa.ResourceOpenStatus openStatus ) as Ivi.Visa.IGpibInterfaceSession;
        return openStatus;
    }

    /// <summary> Opens a session. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    public override void OpenSession( string resourceName )
    {
        _ = this.OpenSessionThis( resourceName, this.OpenTimeout );
        base.OpenSession( resourceName );
    }

    /// <summary> Closes the session. </summary>
    public override void CloseSession()
    {
        this.CloseSessionThis();
        base.CloseSession();
    }

    /// <summary> Closes the session. </summary>
    private void CloseSessionThis()
    {
        if ( this.GpibInterface is not null )
        {
            this.GpibInterface.DiscardEvents( Ivi.Visa.EventType.AllEnabled );
            this.GpibInterface.Dispose();
        }
    }

    #endregion

    #region " gpib interface "

    /// <summary> Sends the interface clear. </summary>
    public override void SendInterfaceClear()
    {
        this.GpibInterface?.SendInterfaceClear();
    }

    /// <summary> Gets the type of the hardware interface. </summary>
    /// <value> The type of the hardware interface. </value>
    public override Pith.HardwareInterfaceType HardwareInterfaceType => Pith.HardwareInterfaceType.Gpib;

    /// <summary> Gets the hardware interface number. </summary>
    /// <value> The hardware interface number. </value>
    public override int HardwareInterfaceNumber => this.GpibInterface?.HardwareInterfaceNumber ?? 0;

    /// <summary> Returns all instruments to some default state. </summary>
    public override void ClearDevices()
    {
        this.GpibInterface?.ClearDevices();
    }

    /// <summary> Clears the specified device. </summary>
    /// <param name="gpibAddress"> The instrument address. </param>
    public override void SelectiveDeviceClear( int gpibAddress )
    {
        this.GpibInterface?.SelectiveDeviceClear( gpibAddress );
    }

    /// <summary> Clears the specified device. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    public override void SelectiveDeviceClear( string resourceName )
    {
        this.GpibInterface?.SelectiveDeviceClear( resourceName );
    }

    /// <summary> Clears the interface. </summary>
    public override void ClearInterface()
    {
        this.GpibInterface?.ClearInterface();
    }

    #endregion
}
