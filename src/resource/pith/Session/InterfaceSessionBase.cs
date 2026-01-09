namespace cc.isr.VI.Pith;

/// <summary> An interface session base class. </summary>
/// <remarks>
/// (c) 2015 Integrated Scientific Resources, Inc. All rights reserved.<para>
/// Licensed under The MIT License.</para><para>
/// David, 2015-11-24 </para>
/// </remarks>
[CLSCompliant( false )]
public abstract class InterfaceSessionBase : CommunityToolkit.Mvvm.ComponentModel.ObservableObject, IDisposable
{
    #region " construction and cleanup "

    /// <summary> Specialized constructor for use only by derived class. </summary>
    protected InterfaceSessionBase() : base()
    {
    }

    #region " disposable support "

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    /// resources.
    /// </summary>
    /// <remarks>
    /// Do not make this method Overridable (virtual) because a derived class should not be able to
    /// override this method.
    /// </remarks>
    public void Dispose()
    {
        this.Dispose( true );
        // Take this object off the finalization(Queue) and prevent finalization code
        // from executing a second time.
        GC.SuppressFinalize( this );
    }

    /// <summary> Gets or sets the disposed status. </summary>
    /// <value> The is disposed. </value>
    protected bool IsDisposed { get; private set; }

    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    protected virtual void Dispose( bool disposing )
    {
        if ( this.IsDisposed ) return;
        try
        {
            if ( disposing )
            {
            }
        }
        finally
        {
            this.IsDisposed = true;
        }
    }

    /// <summary> Finalizes this object. </summary>
    /// <remarks>
    /// Overrides should Dispose(disposing As Boolean) has code to free unmanaged resources.
    /// </remarks>
    ~InterfaceSessionBase()
    {
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        this.Dispose( false );
    }

    #endregion

    #endregion

    /// <summary> Gets or sets the interface open timeout. </summary>
    /// <value> The interface open timeout. </value>
    public TimeSpan OpenTimeout { get; set; } = TimeSpan.FromMilliseconds( 3000 );

    /// <summary> Gets or sets name of the resource. </summary>
    /// <value> The name of the resource. </value>
    public string? ResourceName { get; private set; }

    /// <summary> Gets or sets the sentinel indicating whether this is a dummy session. </summary>
    /// <value> The dummy sentinel. </value>
    public abstract bool IsDummy { get; }

    /// <summary> Gets or sets the is open. </summary>
    /// <value> The is open. </value>
    public bool IsOpen
    {
        get;
        protected set => _ = base.SetProperty( ref field, value );
    }

    /// <summary> Gets or sets the state of the resource open. </summary>
    /// <value> The resource open state. </value>
    public ResourceOpenState ResourceOpenState
    {
        get;

        protected set
        {
            field = value;
            this.IsOpen = value != ResourceOpenState.Unknown;
        }
    }

    /// <summary> Closes the <see cref="InterfaceSessionBase">Interface Session</see>. </summary>
    public virtual void CloseSession()
    {
        this.IsOpen = false;
    }

    /// <summary> Opens a <see cref="InterfaceSessionBase">Interface Session</see>. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <param name="timeout">      The timeout. </param>
    public virtual void OpenSession( string resourceName, TimeSpan timeout )
    {
        this.ResourceName = resourceName;
        this.IsOpen = true;
    }

    /// <summary> Opens a <see cref="InterfaceSessionBase">Interface Session</see>. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    public virtual void OpenSession( string resourceName )
    {
        this.ResourceName = resourceName;
        this.IsOpen = true;
    }

    /// <summary> Sends the interface clear. </summary>
    public abstract void SendInterfaceClear();

    /// <summary> Returns all instruments to some default state. </summary>
    public abstract void ClearDevices();

    /// <summary> Clears the specified device. </summary>
    /// <param name="gpibAddress"> The instrument address. </param>
    public abstract void SelectiveDeviceClear( int gpibAddress );

    /// <summary> Clears the specified device. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    public abstract void SelectiveDeviceClear( string resourceName );

    /// <summary> Clears the interface. </summary>
    public abstract void ClearInterface();

    /// <summary> Gets or sets the type of the hardware interface. </summary>
    /// <value> The type of the hardware interface. </value>
    public abstract HardwareInterfaceType HardwareInterfaceType { get; }

    /// <summary> Gets or sets the hardware interface number. </summary>
    /// <value> The hardware interface number. </value>
    public abstract int HardwareInterfaceNumber { get; }
}
