namespace cc.isr.VI.Pith;

/// <summary> A Dummy Resource Manager. </summary>
/// <remarks>
/// (c) 2013 Integrated Scientific Resources, Inc. All rights reserved. <para>
/// Licensed under The MIT License. </para><para>
/// David, 2013-09-10, 3.0.5001.x. </para>
/// </remarks>
public class DummyResourceManager : IDisposable
{
    #region " construction "

    /// <summary>
    /// Constructor that prevents a default instance of this class from being created.
    /// </summary>
    public DummyResourceManager() : base()
    {
    }

    #region "idisposable support"

    /// <summary> True to disposed value. </summary>
    private bool _disposedValue; // To detect redundant calls



    /// <summary>
    /// Releases the unmanaged resources used by the object and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing"> true to release both managed and unmanaged resources; false to
    /// release only unmanaged resources. </param>
    protected virtual void Dispose( bool disposing )
    {
        try
        {
            if ( !this._disposedValue && disposing )
            {
            }
        }
        finally
        {
            this._disposedValue = true;
        }
    }

    /// <summary> Finalizes this object. </summary>
    /// <remarks>
    /// David, 2015-11-23: Override Finalize() only if Dispose(disposing As Boolean) above has code
    /// to free unmanaged resources.
    /// </remarks>
    ~DummyResourceManager()
    {
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        this.Dispose( false );
    }

    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
    /// resources.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        this.Dispose( true );
        // uncommented the following line because Finalize() is overridden above.
        GC.SuppressFinalize( this );
    }
    #endregion

    #endregion

    #region " parse resources "

    /// <summary> Parse resource. </summary>
    /// <param name="resourceName"> Name of the resource. </param>
    /// <returns> <see cref="ResourceNameInfo"/>. </returns>
    public ResourceNameInfo ParseResource( string resourceName )
    {
        return new ResourceNameInfo( resourceName );
    }

    #endregion

    #region " find resources "

    /// <summary> Lists all resources. </summary>
    /// <returns> List of all resources. </returns>
    public IEnumerable<string> FindResources()
    {
        return [];
    }

    /// <summary> Tries to find resources. </summary>
    /// <param name="resources"> [in,out] The resources. </param>
    /// <returns>
    /// <c>true</c> if resources were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of the
    /// <paramref name="resources"/>.
    /// </returns>
    public bool TryFindResources( ref IEnumerable<string> resources )
    {
        resources = [];
        return true;
    }

    /// <summary> Tries to find resources. </summary>
    /// <param name="filter">    A pattern specifying the search. </param>
    /// <param name="resources"> [in,out] The resources. </param>
    /// <returns>
    /// <c>true</c> if resources were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of the
    /// <paramref name="resources"/>.
    /// </returns>
    public bool TryFindResources( string filter, ref IEnumerable<string> resources )
    {
        resources = [filter];
        return true;
    }

    /// <summary> Name of the resource. </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage( "Code Quality", "IDE0052:Remove unread private members", Justification = "<Pending>" )]
    private string? _resourceName;

    /// <summary> Returns true if the specified resource exists. </summary>
    /// <param name="resourceName"> The resource name. </param>
    /// <returns> <c>true</c> if the resource was located; Otherwise, <c>false</c>. </returns>
    public bool Exists( string resourceName )
    {
        this._resourceName = resourceName;
        return true;
    }

    /// <summary> Searches for the interface. </summary>
    /// <param name="resourceName"> The interface resource name. </param>
    /// <returns> <c>true</c> if the interface was located; Otherwise, <c>false</c>. </returns>
    public bool InterfaceExists( string resourceName )
    {
        this._resourceName = resourceName;
        return true;
    }

    /// <summary> Searches for all interfaces. </summary>
    /// <returns> The found interface resource names. </returns>
    public IEnumerable<string> FindInterfaces()
    {
        return [];
    }

    /// <summary> Tries to find interfaces. </summary>
    /// <param name="resources"> [in,out] The resources. </param>
    /// <returns>
    /// <c>true</c> if interfaces were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of the
    /// <paramref name="resources"/>.
    /// </returns>
    public bool TryFindInterfaces( ref IEnumerable<string> resources )
    {
        resources = [];
        return true;
    }

    /// <summary> Searches for the interfaces. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found interface resource names. </returns>
    public IEnumerable<string> FindInterfaces( HardwareInterfaceType interfaceType )
    {
        return [ResourceNamesManager.BuildInterfaceResourceName( interfaceType, 0 )];
    }

    /// <summary> Tries to find interfaces. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <param name="resources">     [in,out] The resources. </param>
    /// <returns>
    /// <c>true</c> if interfaces were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of the
    /// <paramref name="resources"/>.
    /// </returns>
    public bool TryFindInterfaces( HardwareInterfaceType interfaceType, ref IEnumerable<string> resources )
    {
        resources = [ResourceNamesManager.BuildInterfaceResourceName( interfaceType, 0 )];
        return true;
    }

    /// <summary> Searches for the instrument. </summary>
    /// <param name="resourceName"> The instrument resource name. </param>
    /// <returns> <c>true</c> if the instrument was located; Otherwise, <c>false</c>. </returns>
    public bool FindInstrument( string resourceName )
    {
        this._resourceName = resourceName;
        return true;
    }

    /// <summary> Searches for instruments. </summary>
    /// <returns> The found instrument resource names. </returns>
    public IEnumerable<string> FindInstruments()
    {
        return [];
    }

    /// <summary> Tries to find instruments. </summary>
    /// <param name="resources"> [in,out] The resources. </param>
    /// <returns>
    /// <c>true</c> if instruments were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of the
    /// <paramref name="resources"/>.
    /// </returns>
    public bool TryFindInstruments( ref IEnumerable<string> resources )
    {
        resources = [];
        return true;
    }

    /// <summary> Searches for instruments. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <returns> The found instrument resource names. </returns>
    public IEnumerable<string> FindInstruments( HardwareInterfaceType interfaceType )
    {
        return [ interfaceType == HardwareInterfaceType.Gpib
                                                    ? ResourceNamesManager.BuildGpibInstrumentResource(0, 0)
                                                    : ResourceNamesManager.BuildUsbInstrumentResource( 0, 0, 1, 1 ) ];
    }

    /// <summary> Tries to find instruments. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <param name="resources">     [in,out] The resources. </param>
    /// <returns>
    /// <c>true</c> if instruments were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of the
    /// <paramref name="resources"/>.
    /// </returns>
    public bool TryFindInstruments( HardwareInterfaceType interfaceType, ref IEnumerable<string> resources )
    {
        resources = [ interfaceType == HardwareInterfaceType.Gpib
                                                    ? ResourceNamesManager.BuildGpibInstrumentResource(0, 0)
                                                    : ResourceNamesManager.BuildUsbInstrumentResource( 0, 0, 1, 1 ) ];
        return true;
    }

    /// <summary> Searches for instruments. </summary>
    /// <param name="interfaceType"> Type of the interface. </param>
    /// <param name="boardNumber">   The board number. </param>
    /// <returns> The found instrument resource names. </returns>
    public IEnumerable<string> FindInstruments( HardwareInterfaceType interfaceType, int boardNumber )
    {
        return [$"{ResourceNamesManager.InterfaceBaseName[( int ) interfaceType]}{boardNumber}"];
    }

    /// <summary> Tries to find instruments. </summary>
    /// <param name="interfaceType">   Type of the interface. </param>
    /// <param name="interfaceNumber"> The interface number (e.g., board or port number). </param>
    /// <param name="resources">       [in,out] The resources. </param>
    /// <returns>
    /// <c>true</c> if instruments were located or false if failed or no instrument resources were
    /// located. If exception occurred, the exception details are returned in the first element of the
    /// <paramref name="resources"/>.
    /// </returns>
    public bool TryFindInstruments( HardwareInterfaceType interfaceType, int interfaceNumber, ref IEnumerable<string> resources )
    {
        resources = [$"{ResourceNamesManager.InterfaceBaseName[( int ) interfaceType]}{interfaceNumber}"];
        return true;
    }

    #endregion
}
