using cc.isr.VI.Pith.ExceptionExtensions;

namespace cc.isr.VI.Pith;

/// <summary> A Dummy GPIB interface session. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-21 </para>
/// </remarks>
[CLSCompliant( false )]
public class DummyGpibInterfaceSession : InterfaceSessionBase
{
    #region " constructor "

    /// <summary> Constructor. </summary>
    public DummyGpibInterfaceSession() : base()
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
                    this.CloseSession();
                }
                catch ( Exception ex )
                {
                    Debug.Assert( !Debugger.IsAttached, "Failed discarding enabled events.", $"Failed discarding enabled events. {ex.BuildMessage()}" );
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
    public override bool IsDummy { get; } = true;

    #endregion

    #region " GPIB interface "

    /// <summary> Sends the interface clear. </summary>
    public override void SendInterfaceClear()
    {
    }

    /// <summary> Gets the type of the hardware interface. </summary>
    /// <value> The type of the hardware interface. </value>
    public override HardwareInterfaceType HardwareInterfaceType => HardwareInterfaceType.Gpib;

    /// <summary> Gets the hardware interface number. </summary>
    /// <value> The hardware interface number. </value>
    public override int HardwareInterfaceNumber => 0;

    /// <summary> Returns all instruments to some default state. </summary>
    public override void ClearDevices()
    {
    }

    /// <summary> Clears the specified device. </summary>
    /// <param name="gpibAddress"> The instrument address. </param>
    public override void SelectiveDeviceClear( int gpibAddress )
    {
    }

    /// <summary> Clears the specified device. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    public override void SelectiveDeviceClear( string resourceName )
    {
    }

    /// <summary> Clears the interface. </summary>
    public override void ClearInterface()
    {
    }

    #endregion
}
